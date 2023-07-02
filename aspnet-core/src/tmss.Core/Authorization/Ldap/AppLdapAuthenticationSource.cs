using Abp;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using tmss.Authorization.Roles;
using tmss.Authorization.Users;
using tmss.MultiTenancy;

namespace tmss.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        private readonly ILdapSettings _settings;
        private readonly IAbpZeroLdapModuleConfig _ldapModuleConfig;
        public ISettingManager _settingManager;
        private UserManager _userManager;
        private readonly RoleManager _roleManager;
        public ILogger _logger { get; set; }

        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig, SettingManager settingManager, UserManager userManager, RoleManager roleManager)
            : base(settings, ldapModuleConfig)
        {
            _settings = settings;
            _ldapModuleConfig = ldapModuleConfig;
            _settingManager = settingManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task<bool> TryAuthenticateAsync(string userNameOrEmailAddress, string plainPassword,
            Tenant tenant)
        {
            if (!_ldapModuleConfig.IsEnabled || !(await _settings.GetIsEnabled(tenant?.Id)))
            {
                return false;
            }

            await _settingManager.ChangeSettingForTenantAsync(tenant.Id, LdapSettingNames.UserName, userNameOrEmailAddress);
            await _settingManager.ChangeSettingForTenantAsync(tenant.Id, LdapSettingNames.Password, plainPassword);

            using (var principalContext = await CreatePrincipalContext(tenant, userNameOrEmailAddress))
            {
                return ValidateCredentials(principalContext, userNameOrEmailAddress, plainPassword);
            }
        }

        private async Task<bool> validateAccountLdap(Tenant tenant, string userNameOrEmailAddress, string plainPassword)
        {
            try
            {
                string domainLdap = ConvertToNullIfEmpty(await _settings.GetDomain(tenant?.Id));
                if (!string.IsNullOrEmpty(userNameOrEmailAddress))
                {
                    userNameOrEmailAddress = userNameOrEmailAddress.Split("@")[0];
                }

                var entry = new DirectoryEntry("LDAP://192.168.2.1", userNameOrEmailAddress, plainPassword);
                var searcher = new DirectorySearcher(entry);
                searcher.Filter = string.Format("(&(objectCategory=person)(sAMAccountName={0}))", userNameOrEmailAddress);
                SearchResultCollection results = searcher.FindAll();
                if (results != null) {
                    _logger.Info("Done");
                    return true;
                } else
                {
                    _logger.Info("false");
                    return false;
                }
            } catch (Exception ex)
            {
                _logger.Error(ex.Message);
                return false;
            }
        }

        public override async Task<User> CreateUserAsync(string userNameOrEmailAddress, Tenant tenant)
        {
            await CheckIsEnabled(tenant);

            var user = await base.CreateUserAsync(userNameOrEmailAddress, tenant);

            using (var principalContext = await CreatePrincipalContext(tenant, user))
            {
                var userPrincipal = FindUserPrincipalByIdentity(principalContext, userNameOrEmailAddress);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + userNameOrEmailAddress);
                }

                UpdateUserFromPrincipal(user, userPrincipal);

                user.IsEmailConfirmed = true;
                user.IsActive = true;
                user.AccountManager = GetManagerOfUser(principalContext, userPrincipal, tmssConsts.PROPERTY_LDAP_MANAGER);
                user.FullNameManager = GetFullNameManagerOfUser(principalContext, userPrincipal, tmssConsts.PROPERTY_LDAP_MANAGER);

                user.Roles = new Collection<UserRole>();
                var roleRequest = await _roleManager.GetRoleByNameAsync("Request");
                var roleSharedDrictory = await _roleManager.GetRoleByNameAsync("SharedDriectory");
                var roleReport = await _roleManager.GetRoleByNameAsync("Report");

                user.Roles.Add(new UserRole(tmssConsts.TENANT_ID_DEFAULT, user.Id, roleRequest.Id));
                user.Roles.Add(new UserRole(tmssConsts.TENANT_ID_DEFAULT, user.Id, roleSharedDrictory.Id));
                user.Roles.Add(new UserRole(tmssConsts.TENANT_ID_DEFAULT, user.Id, roleReport.Id));

                return user;
            }
        }

        public override async Task UpdateUserAsync(User user, Tenant tenant)
        {
            await CheckIsEnabled(tenant);

            await base.UpdateUserAsync(user, tenant);

            using (var principalContext = await CreatePrincipalContext(tenant, user))
            {
                var userPrincipal = FindUserPrincipalByIdentity(principalContext, user.UserName);
                var userPrincipalManager = GetManagerOfUser(principalContext, userPrincipal, tmssConsts.PROPERTY_LDAP_MANAGER);

                if (userPrincipal == null)
                {
                    throw new AbpException("Unknown LDAP user: " + user.UserName);
                }

                UpdateUserFromPrincipal(user, userPrincipal);
                user.AccountManager = GetManagerOfUser(principalContext, userPrincipal, tmssConsts.PROPERTY_LDAP_MANAGER);
                user.FullNameManager = GetFullNameManagerOfUser(principalContext, userPrincipal, tmssConsts.PROPERTY_LDAP_MANAGER);
                 
            }
        }

        //public async Task UpdateUserPermissions()
        //{
        //    var user = await UserManager.GetUserByIdAsync(input.Id);
        //    var grantedPermissions =
        //        PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
        //    await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        //}

        protected virtual UserPrincipal FindUserPrincipalByIdentity(
            PrincipalContext principalContext,
            string userNameOrEmailAddress)
        {
            var userPrincipal =
                UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, userNameOrEmailAddress) ??
                UserPrincipal.FindByIdentity(principalContext, IdentityType.UserPrincipalName, userNameOrEmailAddress);

            return userPrincipal;
        }

        protected virtual void UpdateUserFromPrincipal(User user, UserPrincipal userPrincipal)
        {
            user.UserName = GetUsernameFromUserPrincipal(userPrincipal);

            user.Name = userPrincipal.GivenName;
            user.Surname = userPrincipal.Surname;
            user.EmailAddress = userPrincipal.EmailAddress;
            user.Division = GetValueByProperties(userPrincipal, tmssConsts.PROPERTY_LDAP_DIVISION);
            user.Department = GetValueByProperties(userPrincipal, tmssConsts.PROPERTY_LDAP_DEPARTMENT);

            if (userPrincipal.Enabled.HasValue)
            {
                user.IsActive = userPrincipal.Enabled.Value;
            }
        }

        protected virtual string GetUsernameFromUserPrincipal(UserPrincipal userPrincipal)
        {
            return userPrincipal.SamAccountName.IsNullOrEmpty()
                ? userPrincipal.UserPrincipalName
                : userPrincipal.SamAccountName;
        }

        private string GetValueByProperties(UserPrincipal userPrincipal,string property)
        {
            string value = "";
            DirectoryEntry dirEntry = (DirectoryEntry)userPrincipal.GetUnderlyingObject();
            if (dirEntry.Properties[property].Value != null)
            {
                value = dirEntry.Properties[property].Value.ToString();
            }
            return value;
        }

        protected string GetManagerOfUser(
            PrincipalContext principalContext,
            UserPrincipal userPrincipal,
            string property)
        {
            string manager = "";
            if (!string.IsNullOrEmpty(GetValueByProperties(userPrincipal, property)))
            {
                var userPrincipalManger =
                    UserPrincipal.FindByIdentity(principalContext, IdentityType.DistinguishedName, GetValueByProperties(userPrincipal, property));
                if (userPrincipalManger != null)
                {
                    manager = GetUsernameFromUserPrincipal(userPrincipalManger);
                }
            }
            return manager;
        }

        protected virtual string GetFullNameFromUserPrincipal(UserPrincipal userPrincipal)
        {
            return userPrincipal.Name.IsNullOrEmpty()
                ? userPrincipal.DisplayName
                : userPrincipal.Name;
        }

        protected string GetFullNameManagerOfUser(
            PrincipalContext principalContext,
            UserPrincipal userPrincipal,
            string property)
        {
            string fullNameManager = "";
            if (!string.IsNullOrEmpty(GetValueByProperties(userPrincipal, property)))
            {
                var userPrincipalManger =
                    UserPrincipal.FindByIdentity(principalContext, IdentityType.DistinguishedName, GetValueByProperties(userPrincipal, property));
                if (userPrincipalManger != null)
                {
                    fullNameManager = GetFullNameFromUserPrincipal(userPrincipalManger);
                }
            }
            return fullNameManager;
        }
    }
}