﻿using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tmss.AssetManaments;
using tmss.Authorization.Delegation;
using tmss.Authorization.Roles;
using tmss.Authorization.Users;
using tmss.Chat;
using tmss.DataCodes;
using tmss.Editions;
using tmss.Friendships;
using tmss.Master;
using tmss.Master.Asset;
using tmss.Master.AssetGroup;
using tmss.Master.CourceSafety;
using tmss.Master.Employees;
using tmss.Master.EmployeesLearnedSafety;
using tmss.Master.Note;
using tmss.Master.TemEmployeesLearnedSafety;
using tmss.Master.Vender;
using tmss.MultiTenancy;
using tmss.MultiTenancy.Accounting;
using tmss.MultiTenancy.Payments;
using tmss.Storage;

namespace tmss.EntityFrameworkCore
{
    public class tmssDbContext : AbpZeroDbContext<Tenant, Role, User, tmssDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<MstAssetGroup> MstAssetGroups { get; set; }
        public virtual DbSet<MstAsset> MstAssets { get; set; }
        public virtual DbSet<MstVender> MstVenders { get; set; }
        public virtual DbSet<MstEmployees> MstEmployeess { get; set; }
        public virtual DbSet<AioRequestPeople> AioEmployeess { get; set; }
        public virtual DbSet<AioRequestAsset> AioAssets { get; set; }
        public virtual DbSet<AioRequest> AioRequestAssetBrings { get; set; }
        public virtual DbSet<DataCode> DataCodes { get; set; }
        public virtual DbSet<MstEmployeesLearnedSafety> MstEmployeesLearnedSafetys { get; set; }
        public virtual DbSet<MstTemEmployeesLearnedSafety> TemEmployeesLearnedSafetys { get; set; }
        public virtual DbSet<MstCourceSafety> MstCourceSafetys { get; set; }
        public virtual DbSet<MstNote> MstNotes { get; set; }
        //public virtual DbSet<AioStatusRequest> AioStatusRequests { get; set; }
        public virtual DbSet<MstStatus> MstStatus { get; set; }
        public virtual DbSet<MstTemplateEmail> MstTemplateEmail { get; set; }
        public virtual DbSet<AioRequestLog> AioRequestActionLog { get; set; }
        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }
        public virtual DbSet<AioRequestPeopleInOut> AioPeopleCheckInOuts { get; set; }
        public virtual DbSet<AioRequestAssetInOut> AioAssetCheckInOuts { get; set; }
        public virtual DbSet<MstVenderAccount> MstVenderAccount { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public tmssDbContext(DbContextOptions<tmssDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
