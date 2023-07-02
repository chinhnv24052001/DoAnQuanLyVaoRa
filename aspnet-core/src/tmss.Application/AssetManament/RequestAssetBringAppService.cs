using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.POIFS.Crypt.Dsig;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments;
using tmss.AssetManaments.Dto;
using tmss.AssetManaments.RequestAssetBring.Dto;
using tmss.Authorization.Roles;
using tmss.Authorization.Users;
using tmss.Email;
using tmss.Master;
using tmss.Master.Asset;
using tmss.Master.Asset.Dto;
using tmss.Master.CourceSafety;
using tmss.Master.EmployeesLearnedSafety;
using tmss.Master.EmployeesLearnedSafety.Dto;
using tmss.Master.Vender;
using tmss.Master.Vender.Dto;
using tmss.Migrations;
using tmss.SendEmail;
using tmss.SQL;
using File = System.IO.File;

namespace tmss.AssetManament
{
    public class RequestAssetBringAppService : tmssAppServiceBase, IRequestAssetBringAppService
    {

        private readonly IRepository<AioRequest, long> _amRequestAssetBringRepository;
        private readonly IRepository<AioRequestAsset, long> _amAssetRepository;
        private readonly IRepository<AioRequestPeople, long> _amEmployeesRepository;
        private readonly IRepository<MstAsset, long> _mstAssetRepository;
        private readonly IRepository<MstVender, long> _mstVenderRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<MstStatus, long> _statusRequestRepository;
        private readonly IRepository<MstTemplateEmail, long> _mstTemplateEmailRepository;
        private readonly IRepository<AioRequestLog, long> _aioRequestActionLogRepository;
        private readonly IDapperRepository<MstAsset, long> _aioSearchRequestRepository;
        private readonly IRepository<MstCourceSafety, long> _courceSafetyRepository;
        private readonly IRepository<MstEmployeesLearnedSafety, long> _employeesLearnedSafetyRepository;
        private readonly ISqlHelper _sqlHelper;
        private readonly AssetInOutManamentAppService _assetInOutManamentAppService;
        private readonly IRepository<AioRequestAssetInOut, long> _aioAssetCheckInOut;
        private readonly IRepository<AioRequestPeopleInOut, long> _aioPeopleCheckInOut;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role, int> _roleRepository;
        private readonly ISendEmailRequest _iSendEmailRequest;

        private IWebHostEnvironment _env;
        public RequestAssetBringAppService(
            IRepository<AioRequest, long> amRequestAssetBringRepository,
            IRepository<AioRequestAsset, long> amAssetRepository,
            IRepository<AioRequestPeople, long> amEmployeesRepository,
            IRepository<MstAsset, long> mstAssetRepository,
            IRepository<MstVender, long> mstVenderRepository,
            IRepository<User, long> userRepository,
            IRepository<MstStatus, long> statusRequestRepository,
            IRepository<AioRequestLog, long> aioRequestActionLogRepository,
            IDapperRepository<MstAsset, long> aioSearchRequestRepository,
            IRepository<MstTemplateEmail, long> mstTemplateEmailRepository,
            IRepository<MstCourceSafety, long> courceSafetyRepository,
            IRepository<MstEmployeesLearnedSafety, long> employeesLearnedSafetyRepository,
            ISqlHelper sqlHelper,
            IRepository<AioRequestAssetInOut, long> aioAssetCheckInOut,
            IRepository<AioRequestPeopleInOut, long> aioPeopleCheckInOut,
            AssetInOutManamentAppService assetInOutManamentAppService,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role, int> roleRepository,
             IWebHostEnvironment env,
             ISendEmailRequest iSendEmailRequest
            )
        {
            _amRequestAssetBringRepository = amRequestAssetBringRepository;
            _amAssetRepository = amAssetRepository;
            _amEmployeesRepository = amEmployeesRepository;
            _mstAssetRepository = mstAssetRepository;
            _mstVenderRepository = mstVenderRepository;
            _userRepository = userRepository;
            _statusRequestRepository = statusRequestRepository;
            _aioRequestActionLogRepository = aioRequestActionLogRepository;
            _aioSearchRequestRepository = aioSearchRequestRepository;
            _mstTemplateEmailRepository = mstTemplateEmailRepository;
            _courceSafetyRepository = courceSafetyRepository;
            _employeesLearnedSafetyRepository = employeesLearnedSafetyRepository;
            _sqlHelper = sqlHelper;
            _assetInOutManamentAppService = assetInOutManamentAppService;
            _aioAssetCheckInOut = aioAssetCheckInOut;
            _aioPeopleCheckInOut = aioPeopleCheckInOut;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _env = env;
            _iSendEmailRequest = iSendEmailRequest;
        }

        public async Task DeleteById(long id)
        {
            var listAsset = await _amAssetRepository.GetAll().AsNoTracking().Where(e => e.RequestId == id).ToListAsync();
            foreach (AioRequestAsset item in listAsset)
            {
                await _amAssetRepository.DeleteAsync(item.Id);
            }
            var listEmployees = await _amEmployeesRepository.GetAll().AsNoTracking().Where(e => e.RequestId == id).ToListAsync();
            foreach (AioRequestPeople item in listEmployees)
            {
                await _amEmployeesRepository.DeleteAsync(item.Id);
            }
            await _amRequestAssetBringRepository.DeleteAsync(id);
        }

        public async Task<List<AssetSelectOutputDto>> GetAssetForEdit()
        {
            var listAsset = _mstAssetRepository.GetAll()
                      .Select(p => new AssetSelectOutputDto
                      {
                          AssetName = p.AssetName,
                          Id = p.Id,
                      });
            return listAsset.ToList();
        }

        public async Task<List<VenderSelectOutputDto>> GetVenderForEdit()
        {
            var listVender = _mstVenderRepository.GetAll()
                     .Select(p => new VenderSelectOutputDto
                     {
                         VenderName = p.VenderName,
                         Id = p.Id,
                     });
            return listVender.ToList();
        }

        public async Task<string> GetUserName()
        {
            var userLogin = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            return userLogin.Name;
        }

        public async Task<List<StatusRequestDto>> GetStatusRequest()
        {
            var statusRequest = _statusRequestRepository.GetAll()
                .Select(e => new StatusRequestDto
                {
                    Id = e.Id,
                    StatusRequestName = e.Name
                });

            return statusRequest.ToList();
        }

        public async Task<PagedResultDto<RequestAssetBringSelectOutputDto>> LoadAll(RequestAssetBringInputDto input)
        {
            long userId = 0;
            string userName = "";
            string typeRequest = "";
            string _role = "";

            var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            var userADM = await (from us in _userRepository.GetAll().AsNoTracking()
                                 join userRole in _userRoleRepository.GetAll().AsNoTracking()
                                 on us.Id equals userRole.UserId
                                 join role in _roleRepository.GetAll().AsNoTracking()
                                 on userRole.RoleId equals role.Id
                                 where us.Id == user.Id
                                 select new UserRequestDto()
                                 {
                                     Id = us.Id,
                                     UserName = us.UserName,
                                     RoleName = role.DisplayName
                                 }
                        ).FirstOrDefaultAsync(p => p.RoleName == AppConsts.ROLE_ADM);
            if (userADM != null)
            {
                _role = userADM.RoleName;
            }

            switch (input.TabKey)
            {
                case AppConsts.U_WAITTINGME:
                    userName =  user.UserName;
                    typeRequest = AppConsts.R_TYPE_MENU_WAITTING;
                    break;
                case AppConsts.U_APPROVEDBYME:
                    userName = user.UserName;
                    typeRequest = AppConsts.R_TYPE_MENU_APPROVED;
                    break;
                case AppConsts.U_REJECTEDBYME:
                    userName = user.UserName;
                    typeRequest = AppConsts.R_TYPE_MENU_RERECTED;
                    break;
                case AppConsts.U_OTHER_REQUEST:
                    userName = user.UserName;
                    typeRequest = AppConsts.R_TYPE_MENU_OTHER;
                    break;

                case AppConsts.M_WATTING:
                    userId = user.Id;
                    typeRequest = AppConsts.R_TYPE_MENU_WAITTING;
                    break;
                case AppConsts.M_APPROVED:
                    userId = user.Id;
                    typeRequest = AppConsts.R_TYPE_MENU_APPROVED;
                    break;
                case AppConsts.M_REJECTED:
                    userId = user.Id;
                    typeRequest = AppConsts.R_TYPE_MENU_RERECTED;
                    break;
                case AppConsts.M_OTHER_REQUEST:
                    userId = user.Id;
                    typeRequest = AppConsts.R_TYPE_MENU_OTHER;
                    break;

                case AppConsts.IS_DRAFT:
                    userId = user.Id;
                    typeRequest = AppConsts.R_TYPE_MENU_DRAFT;
                    break;
               
            }

            //string _sql = "EXEC P_SEARCH_REQUEST @TypeRequest ,@UserId, @UserManager, @Title, @Status, @RequestCode, @DateRequest";

            SearchRequestDto searchRequestDto = new SearchRequestDto();
            searchRequestDto.UserId = userId;
            searchRequestDto.TypeRequest = typeRequest;
            searchRequestDto.UserManager = userName;
            searchRequestDto.DateRequest = input.DateRequest;
            searchRequestDto.RequestCode = input.RequestCode;
            searchRequestDto.Title = input.Title;
            searchRequestDto.Status = input.Status;
            searchRequestDto.SkipCount = input.SkipCount;
            searchRequestDto.MaxResultCount = input.MaxResultCount;
            searchRequestDto.TypeRequestId = input.TypeRequestId;
            searchRequestDto.Role = _role;

            //List<RequestAssetBringSelectOutputDto> requestAssetBringSelectOutputDtos = await _sqlHelper.ExecuteStoredProcedure<SearchRequestDto, RequestAssetBringSelectOutputDto>(searchRequestDto, "P_SEARCH_REQUEST");

            //var requestAsset = await _aioSearchRequestRepository.QueryAsync<RequestAssetBringSelectOutputDto>(_sql, new
            //{
            //    @TypeRequest = typeRequest,
            //    @UserId = userId,
            //    @UserManager = userName,
            //    @Title = input.Title,
            //    @Status = input.Status,
            //    @RequestCode = input.RequestCode,
            //    @DateRequest = input.DateRequest,
            //});
            //var resultQuery = from request in requestAsset
            //                  select new RequestAssetBringSelectOutputDto
            //                  {
            //                      Id = request.Id,
            //                      Title = request.Title,
            //                      Description = request.Description,
            //                      DateRequest = request.DateRequest,
            //                      DateManageApproval = request.DateManageApproval,
            //                      ManageApprovalId = request.ManageApprovalId,
            //                      DateAdminApproval = request.DateAdminApproval,
            //                      AdminApprovalId = request.AdminApprovalId,
            //                      RequestCode = request.RequestCode,
            //                      Status = request.Status,
            //                      KeyStatus = request.KeyStatus
            //                  };

            //var result = requestAsset.Skip(input.SkipCount).Take(input.MaxResultCount);
            //var requestAssetCount = requestAsset.Count();
            //int TotalCount = 0;
            //if (requestAssetBringSelectOutputDtos != null && requestAssetBringSelectOutputDtos.Count > 0)
            //    TotalCount = (int)requestAssetBringSelectOutputDtos[0].TotalCount;
            return await _sqlHelper.ExecuteStoredProcedureWithPaging<SearchRequestDto, RequestAssetBringSelectOutputDto>(searchRequestDto, "P_SEARCH_REQUEST");
        }

        public async Task<long> GetUserLoginId ()
        {
            return (long)AbpSession.UserId;
        }

        public async Task<RequestAssetBringSaveDto> LoadUserForCreate()
        {
            var requestAsset = new RequestAssetBringSaveDto();
            var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            requestAsset.Department = user.Department;
            requestAsset.UserName = user.Surname + " " + user.Name;
            return requestAsset;
        }

        public async Task<RequestAssetBringSaveDto> LoadById(long id)
        {
            var requestAsset = _amRequestAssetBringRepository.GetAll()
               .Select(p => new RequestAssetBringSaveDto
               {
                   Id = p.Id,
                   Title = p.Title,
                   TypeRequest = p.TypeRequest,
                   Description = p.Description,
                   DateRequest = p.DateRequest,
                   DateManageApproval = p.DateManageApproval != null ? p.DateManageApproval.Value.ToString("dd/MM/yyyy") : "",
                   DateAdminApproval = p.DateAdminApproval != null ? p.DateAdminApproval.Value.ToString("dd/MM/yyyy") : "",
                   RequestCode = p.RequestCode,
                   UserId = p.CreatorUserId,
                   VenderId = p.VenderId,
                   Status = p.Status,
                   StatusDraft = p.StatusDraft,
                   LiveMonitorName = p.LiveMonitorName,
                   LiveMonitorDepartment = p.LiveMonitorDepartment,
                   LiveMonitorPhoneNumber = p.LiveMonitorPhoneNumber,
                   WhereToBring = p.WhereToBring,
                   PersonInChangeOfSubPhone = p.PersonInChangeOfSubPhone,
                   PersonInChargeOfSubName=p.PersonInChargeOfSubName
               })
               .FirstOrDefault(p => p.Id == id);

            var user = await _userRepository.FirstOrDefaultAsync((long)requestAsset.UserId);
            requestAsset.Department = user.Department;
            requestAsset.UserName = user.Surname + " " + user.Name;
            if (requestAsset.Id != 0)
            {
                var listAsset = from asset in _amAssetRepository.GetAll().AsNoTracking()
                            .Where(e => e.RequestId == requestAsset.Id)
                                select new AioAssetDto
                                {
                                    Id = asset.Id,
                                    AssetId = asset.AssetId,
                                    SeriNumber = asset.SeriNumber,
                                    TagCode = asset.TagCode,
                                    Total=asset.Total,
                                    DateStart = asset.DateStart,
                                    DateEnd = asset.DateEnd,
                                    AssetImage=asset.AssetImage,
                                    FilePath = asset.FilePath
                                };
                List<AioAssetDto> listAssets = listAsset.ToList();
                requestAsset.AssetList = listAssets;

                var listEmployees = from employees in _amEmployeesRepository.GetAll().AsNoTracking()
                            .Where(e => e.RequestId == requestAsset.Id)
                                    select new AioEmployeesDto
                                    {
                                        Id = employees.Id,
                                        EmployeesName = employees.EmployeesName,
                                        DateStart = employees.DateStart,
                                        DateEnd = employees.DateEnd,
                                        IdentityCard = employees.IdentityCard,
                                        Target= employees.Target,
                                        Company=employees.Company
                                    };
                List<AioEmployeesDto> listEmployeess = listEmployees.ToList();
                requestAsset.WorkersList = listEmployeess;
            }
            return requestAsset;
        }

        //create or update
        public async Task<bool> CheckWorkerLearnedSafety(string input)
        {
            bool CheckEmployed = false;
            if(input == "")
            {
                return CheckEmployed;
            }

            var employees = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(p => p.IdentityCard.Equals(input));

            if(employees!=null)

            {
                CheckEmployed = DateTime.Compare(employees.EffecttivateDate, DateTime.Now) > 0 ? true : false;
            }
            else
            {
                CheckEmployed = false;
            }

            return CheckEmployed;
        }

        private async Task Update(RequestAssetBringSaveDto input)
        {
            AioRequest amRequestAssetBring = await _amRequestAssetBringRepository.FirstOrDefaultAsync(input.Id);
            var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            long statusCreateId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_CREATE_REQUEST)).Id;
            long statusManagerApprovalId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_MANAGER_APPROVE)).Id;
            long statusWaitTemManager = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_TEM_MANAGER)).Id;
            if(amRequestAssetBring.DateManageApproval != null)
            {
                input.Status = statusManagerApprovalId;
            }
            else
            {
                input.Status = statusCreateId;
            }
            input.TemManageIntervent = false;
            //quan ly phu duyet
            //foreach (AioEmployeesDto amEmployeesDto in input.WorkersList)
            //{
            //    if (await CheckWorkerLearnedSafety(amEmployeesDto.IdentityCard) == false)
            //    {
            //        input.Status = statusWaitTemManager;
            //        input.TemManageIntervent = true;
            //        break;
            //    }
            //}
            input.UserId = user.Id;
            input.ManageApproval = user.AccountManager;
            input.DateManageApproval = null;
            input.DateAdminApproval = null;
            //amRequestAssetBring.Adm = "adm";
            amRequestAssetBring.TemManagerApproval = "haonx2";
            if (input.Description == null)
            {
                input.Description = "";
            }
            amRequestAssetBring.AdmApprovalMessage = "";
            amRequestAssetBring.ManageApprovalMessage = "";
            amRequestAssetBring = ObjectMapper.Map(input, amRequestAssetBring);
            amRequestAssetBring.DateRequest = DateTime.Now;
            await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
            foreach (AioAssetDto amAssetDto in input.AssetList)
            {
                if (await _amAssetRepository.FirstOrDefaultAsync(amAssetDto.Id) == null)
                {
                    AioRequestAsset amAsset = new AioRequestAsset();
                    amAssetDto.AviationIsBack = amAssetDto.AviationIsBack == true ? true : false;
                    amAsset = ObjectMapper.Map<AioRequestAsset>(amAssetDto);
                    amAsset.RequestId = amRequestAssetBring.Id;
                    await _amAssetRepository.InsertAsync(amAsset);
                }
                else
                {
                    AioRequestAsset amAsset = await _amAssetRepository.FirstOrDefaultAsync(amAssetDto.Id);
                    amAssetDto.AviationIsBack = amAssetDto.AviationIsBack == true ? true : false;
                    if (amAssetDto.AviationIsBack == true)
                    {
                        amAssetDto.DateEnd = null;
                    }
                    amAsset = ObjectMapper.Map(amAssetDto, amAsset);
                    amAsset.RequestId = amRequestAssetBring.Id;
                    await _amAssetRepository.UpdateAsync(amAsset);
                }
            }
            foreach (AioEmployeesDto amEmployeesDto in input.WorkersList)
            {
                if (await _amEmployeesRepository.FirstOrDefaultAsync(amEmployeesDto.Id) == null)
                {
                    AioRequestPeople amEmployees = new AioRequestPeople();
                    amEmployees = ObjectMapper.Map<AioRequestPeople>(amEmployeesDto);
                    amEmployees.RequestId = amRequestAssetBring.Id;
                    await _amEmployeesRepository.InsertAsync(amEmployees);
                }
                else
                {
                    AioRequestPeople amEmployees = await _amEmployeesRepository.FirstOrDefaultAsync(amEmployeesDto.Id);
                    amEmployees = ObjectMapper.Map(amEmployeesDto, amEmployees);
                    amEmployees.RequestId = amRequestAssetBring.Id;
                    await _amEmployeesRepository.UpdateAsync(amEmployees);
                }

            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        private async Task Create(RequestAssetBringSaveDto input)
        {
            var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            input.UserId = user.Id;
            input.ManageApproval = user.AccountManager;
            if (input.Description == null)
            {
                input.Description = "";
            }
            AioRequest amRequestAssetBring = new AioRequest();
            AioRequest aioRequestAssetBring = new AioRequest();
            AioRequestAsset amAsset = new AioRequestAsset();
            AioRequestPeople amEmployees = new AioRequestPeople();
            amRequestAssetBring = ObjectMapper.Map<AioRequest>(input);
            amRequestAssetBring.RequestCode = DateTime.Now.ToString("yymmssfff");
            //amRequestAssetBring.Adm = "adm";
            amRequestAssetBring.TemManagerApproval = "haonx2";
            long statusCreateId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_CREATE_REQUEST)).Id;
            long statusCreateDraftId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_CREATE_DRAFT)).Id;
            long statusWaitTemManager = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_TEM_MANAGER)).Id;

            amRequestAssetBring.TemManageIntervent = false;

            //check quan ly phu
            //foreach (AioEmployeesDto amEmployeesDto in input.WorkersList)
            //{
            //    if (await CheckWorkerLearnedSafety(amEmployeesDto.IdentityCard) == false)
            //    {
            //        amRequestAssetBring.Status = statusWaitTemManager;
            //        amRequestAssetBring.TemManageIntervent = true;
            //        break;
            //    }
            //}

            //Luu nhap sau cung
            if (amRequestAssetBring.StatusDraft == 1)
            {
                amRequestAssetBring.Status = statusCreateDraftId;
            }
            else amRequestAssetBring.Status = statusCreateId;

            amRequestAssetBring.DateRequest = DateTime.Now;
            amRequestAssetBring.AdmApprovalMessage = "";
            amRequestAssetBring.ManageApprovalMessage = "";

            await _amRequestAssetBringRepository.InsertAsync(amRequestAssetBring);
            await CurrentUnitOfWork.SaveChangesAsync();
            long statusWaitGuardId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_GUARD)).Id;
            foreach (AioAssetDto amAssetDto in input.AssetList)
            {
                amAssetDto.AviationIsBack = amAssetDto.AviationIsBack == true ? true : false;
                if(amAssetDto.AviationIsBack == true)
                {
                    amAssetDto.DateEnd = null;
                }
                amAsset = ObjectMapper.Map<AioRequestAsset>(amAssetDto);
                amAsset.RequestId = amRequestAssetBring.Id;
                await _amAssetRepository.InsertAsync(amAsset);
            }

            foreach (AioEmployeesDto amEmployeesDto in input.WorkersList)
            {
                amEmployees = ObjectMapper.Map<AioRequestPeople>(amEmployeesDto);
                amEmployees.RequestId = amRequestAssetBring.Id;
                await _amEmployeesRepository.InsertAsync(amEmployees);
            }
            await CurrentUnitOfWork.SaveChangesAsync();

            var userADM = await (from us in _userRepository.GetAll().AsNoTracking()
                                 join userRole in _userRoleRepository.GetAll().AsNoTracking()
                                 on us.Id equals userRole.UserId
                                 join role in _roleRepository.GetAll().AsNoTracking()
                                 on userRole.RoleId equals role.Id
                                 where us.Id == user.Id
                                 select new UserRequestDto()
                                 {
                                     Id = us.Id,
                                     UserName = us.UserName,
                                     RoleName = role.DisplayName
                                 }
                        ).FirstOrDefaultAsync(p => p.RoleName == AppConsts.ROLE_ADM);
            if (userADM != null)
            {
                ApproveOrRejectRequestDto approveOrRejectRequestDto = new ApproveOrRejectRequestDto();
                approveOrRejectRequestDto.Id = amRequestAssetBring.Id;
                approveOrRejectRequestDto.Type = AppConsts.TYPE_APPROVED;
                var admApproval = await AdmApproveOrReject(approveOrRejectRequestDto);
            }

            //if (input.StatusDraft == 2)
            //{
            //    if (_statusRequestRepository.FirstOrDefault(p => p.Id == (long)amRequestAssetBring.Status).Key == AppConsts.STATUS_CREATE_REQUEST)
            //    {
            //        //Send email to manager
            //        MstTemplateEmail mstTemplateEmail = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("WAITTING")).FirstOrDefault();
            //        User requestor = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            //        await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), requestor.UserName, (requestor.Name + " " + requestor.Surname), requestor.FullNameManager, requestor.AccountManager,amRequestAssetBring.Title, mstTemplateEmail.Title, mstTemplateEmail.Message, requestor.Division);
            //    }
            //}
        }

        //email Manufac
        public async Task SendEmailToManufacture(long requestId, string email)
        {
            await _iSendEmailRequest.SendEmailManufacture(requestId, email);
        }

        public async Task<List<AioEmployeesDto>> LoadEmployeesByCourseId(long id)
        {
            var listEmployeesLearnedSf = from em in _employeesLearnedSafetyRepository.GetAll().AsNoTracking()
                                         join course in _courceSafetyRepository.GetAll().AsNoTracking() 
                                         on em.CourceId equals course.Id
                                         where course.Id == id
                                         select new AioEmployeesDto
                                         {
                                             EmployeesName = em.EmployeesName,
                                             IdentityCard=em.IdentityCard,
                                         };
            return listEmployeesLearnedSf.ToList();
        }

        public async Task Save(RequestAssetBringSaveDto input)
        {
            if (await _amRequestAssetBringRepository.FirstOrDefaultAsync(input.Id) == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task SaveDraft(long id)
        {
            var request = await _amRequestAssetBringRepository.FirstOrDefaultAsync(id);
            if (request != null)
            {
                long statusCreateDraftId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_CREATE_DRAFT)).Id;
                    request.Status = statusCreateDraftId;
                request.StatusDraft = 1;
                await _amRequestAssetBringRepository.UpdateAsync(request);
            }
        }

        public async Task SendMailToCancelRequest(long id)
        {
            var request = await _amRequestAssetBringRepository.FirstOrDefaultAsync(id);
            long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_MANAGER_APPROVE)).Id; 

            //if(request!=null&& statusId!=null)
            //{
            //    if(request.Status==statusId)
            //    {
            //        //send email to adm
            //        MstTemplateEmail mstTemplateEmail = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("CANCEL_REQUEST")).FirstOrDefault();
            //        User requestor = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            //        var usersAdm = await UserManager.GetUsersInRoleAsync(AppConsts.ROLE_ADM);
            //        foreach (User userRoleAdm in usersAdm)
            //        {
            //            User userAdm = _userRepository.GetAll().Where(p => p.UserName == userRoleAdm.UserName).FirstOrDefault();
            //            EmailCommon.SendEmail(request.Id.ToString(), requestor.UserName, (requestor.Name + " " + requestor.Surname), (userRoleAdm.Name + " " + userRoleAdm.Surname), userRoleAdm.UserName, request.Title, mstTemplateEmail.Title, mstTemplateEmail.Message, requestor.Division);
            //        }
            //    }
            //    else
            //    {
            //        //Send email to manager
            //        MstTemplateEmail mstTemplateEmail = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("CANCEL_REQUEST")).FirstOrDefault();
            //        User requestor = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
            //        EmailCommon.SendEmail(request.Id.ToString(), requestor.UserName, (requestor.Name + " " + requestor.Surname), requestor.FullNameManager, requestor.AccountManager, request.Title, mstTemplateEmail.Title, mstTemplateEmail.Message, requestor.Division);
            //    }
            //}
        }

        //check request to cancel
        public async Task<bool> CheckRequestToCancel(long id)
        {
            var check = false;
            var request = await _amRequestAssetBringRepository.FirstOrDefaultAsync(id);
            if (request != null)
            {
                if (request.AdminApprovalId > 0)
                {
                    check = false;
                }
                else
                {
                    check = true;
                }
            }
            else check = false;
            return check;
        }

        public async Task<RequestAssetBringDetailDto> GetRequestAssetBringDetail(long id)
        {
            var x = await _amRequestAssetBringRepository.FirstOrDefaultAsync(id);
            var requestAssetBring = await _amRequestAssetBringRepository.GetAll()
                .Select(e => new RequestAssetBringDetailDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    DateRequest = e.DateRequest.ToString("dd/MM/yyyy"),
                    DateManageApproval = e.DateManageApproval != null ? e.DateManageApproval.Value.ToString("dd/MM/yyyy") : "",
                    DateAdminApproval = e.DateAdminApproval != null ? e.DateAdminApproval.Value.ToString("dd/MM/yyyy") : "",
                    //AdminApprovalName = _userRepository.GetAll().Where(p => p.Id == e.AdminApprovalId).FirstOrDefault().Name,
                    RequestCode = e.RequestCode,
                    ManageApproval = e.ManageApproval,
                    Status = e.Status,
                    VenderId = e.VenderId,
                    CreateByUserId = e.CreatorUserId,
                    TypeRequest = e.TypeRequest,
                    //AdminApprovalName = e.Adm,
                    AdminApprovalId = e.AdminApprovalId,
                    TemManageApprovalName = e.TemManagerApproval,
                    TemManageIntervent = e.TemManageIntervent,
                    LiveMonitorName = e.LiveMonitorName,
                    LiveMonitorPhoneNumber = e.LiveMonitorPhoneNumber,
                    LiveMonitorDepartment = e.LiveMonitorDepartment,
                    WhereToBring = e.WhereToBring,
                    TradeUnionOrganization= e.TradeUnionOrganization,
                    DepartmentClient = e.DepartmentClient,
                    PersonInChargeOfSubName = e.PersonInChargeOfSubName,
                    PersonInChangeOfSubPhone = e.PersonInChangeOfSubPhone,
                    ManageApprovalMessage = e.ManageApprovalMessage,
                    AdmApprovalMessage = e.AdmApprovalMessage
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            var user = _userRepository.GetAll().Where(p => p.Id == requestAssetBring.CreateByUserId).FirstOrDefault();
            requestAssetBring.UserName = user.Surname + " " + user.Name;
            requestAssetBring.Department = user.Department;
            
            requestAssetBring.ManageApprovalName = user.AccountManager;
            requestAssetBring.AdmApprovalNameWairting = "adm";

            //if(requestAssetBring.DateManageApproval!="")
            //{
            //    var managerApproval = await _userRepository.FirstOrDefaultAsync(p => p.UserName == requestAssetBring.ManageApproval);
            //    if(managerApproval!=null)
            //    {
            //        requestAssetBring.ManageApprovalName = managerApproval.Surname + " " + managerApproval.Name;
            //    }
            //}


            var admAproval = _userRepository.GetAll().Where(p => p.Id == requestAssetBring.AdminApprovalId).FirstOrDefault();
            if (admAproval != null)
            {
                requestAssetBring.AdminApprovalName = admAproval.Surname + " " + admAproval.Name;
                requestAssetBring.AdmApprovalNameWairting = admAproval.Surname + " " + admAproval.Name;
            }

            var userLogin = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();

            if (userLogin.UserName == requestAssetBring.ManageApproval)
            {
                requestAssetBring.IsManagerApproved = true;
            }
            else requestAssetBring.IsManagerApproved = false;

            var userADM = await (from us in _userRepository.GetAll().AsNoTracking()
                                 join userRole in _userRoleRepository.GetAll().AsNoTracking()
                                 on us.Id equals userRole.UserId
                                 join role in _roleRepository.GetAll().AsNoTracking()
                                 on userRole.RoleId equals role.Id
                                 where us.Id == userLogin.Id
                                 select new UserRequestDto()
                                 {
                                     Id = us.Id,
                                     UserName = us.UserName,
                                     RoleName = role.DisplayName
                                 }
                        ).FirstOrDefaultAsync(p => p.RoleName == AppConsts.ROLE_ADM);
            if (userADM!=null)
            {
                requestAssetBring.IsADMApproved = true;
            }
            else requestAssetBring.IsADMApproved = false;

            //if (userLogin.UserName == requestAssetBring.AdminApprovalName)
            //{
            //    requestAssetBring.IsADMApproved = true;
            //}
            //else requestAssetBring.IsADMApproved = false;

            if (requestAssetBring.Status == 6 || requestAssetBring.Status == 8 || requestAssetBring.Status == 23)
            {
                AioRequestLog aioRequestActionLog = _aioRequestActionLogRepository.GetAll()
               .Where(p => p.RequestId == id).OrderByDescending(e => e.Id).FirstOrDefault();

                if (aioRequestActionLog != null)
                {
                    requestAssetBring.ReasonForRefusal = aioRequestActionLog.ReasonRefusal;
                }
            }

            var asset = from amAsset in _amAssetRepository.GetAll().AsNoTracking()
                        join _asset in _mstAssetRepository.GetAll().AsNoTracking()
                        on amAsset.AssetId equals _asset.Id
                        where amAsset.RequestId == requestAssetBring.Id
                        select new AioAssetDetailDto
                        {
                            Id = amAsset.Id,
                            AssetName = _asset.AssetName,
                            SeriNumber = amAsset.SeriNumber,
                            TagCode = amAsset.TagCode,
                            Total = amAsset.Total,
                            DateStart = amAsset.DateStart,
                            DateEnd = amAsset.DateEnd,
                            AssetImage = amAsset.AssetImage,
                            AviationIsBack = amAsset.AviationIsBack,
                            AviationIsBackString = amAsset.AviationIsBack.Value ? L("AviationIsBack") : ""
                        };
            requestAssetBring.listAssetDto = asset.ToList();

            
            var listEmployees = await (from _employees in _amEmployeesRepository.GetAll().AsNoTracking()
                                       where _employees.RequestId == requestAssetBring.Id
                                       select new AioEmployeesDetailDto
                                       {
                                           Id = _employees.Id,
                                           EmployeesName = _employees.EmployeesName,
                                           IdentityCard = _employees.IdentityCard,
                                           DateStart = _employees.DateStart,
                                           DateEnd = _employees.DateEnd,
                                           Target=_employees.Target,
                                           Company=_employees.Company
                                       }).ToListAsync();

            foreach(var em in listEmployees)
            {
                var employeesLearned = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(p => p.IdentityCard == em.IdentityCard && p.Image != null);
                if (employeesLearned != null)
                {
                    em.EmployeesImage = employeesLearned.Image;
                }
                else em.EmployeesImage = "";
            }

            if (requestAssetBring.TypeRequest != 1 && requestAssetBring.TypeRequest!=4)
            {   //select vender for typrRequest=2
                requestAssetBring.VenderName = _mstVenderRepository.GetAll().Where(p => p.Id == requestAssetBring.VenderId).FirstOrDefault().VenderName;

            }

            //Validate worker learned Safety
            if (listEmployees != null)
            {
                if(requestAssetBring.TypeRequest != 4)
                {
                    foreach (var employees in listEmployees)
                    {
                        employees.CheckWorkerLearned = await CheckWorkerLearnedSafety(employees.IdentityCard);
                    }
                }
                else
                {
                    foreach (var employees in listEmployees)
                    {
                        employees.CheckWorkerLearned = true;
                    }
                } 
                    
            }

            requestAssetBring.listEmployeesDto = listEmployees;

            return requestAssetBring;
        }

        //tem_manager acctep or reject request
        public async Task<bool> TemManagerApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto)
        {
            if (approveOrRejectRequestDto.Id > 0)
            {
                AioRequest amRequestAssetBring = await _amRequestAssetBringRepository.FirstOrDefaultAsync(approveOrRejectRequestDto.Id);
                if (AppConsts.TYPE_APPROVED.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_CREATE_REQUEST)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;
                        amRequestAssetBring.DateTemManageApproval = DateTime.Now;
                        AioRequestLog aioRequestActionLog = new AioRequestLog(); 
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("APPROVALFORUSER")).FirstOrDefault();
                        User user = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //EmailCommon.SendEmail(approveOrRejectRequestDto.Id.ToString(), user.Surname+user.Name, user.EmailAddress, "PhamVanHieu",
                        //    "chinhc5ph@gmail.com", amRequestAssetBring.Title, "Test", "Test", mstTemplateEmailForUser.Message);

                        //Send email to manager
                        MstTemplateEmail mstTemplateEmail = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("WAITTING")).FirstOrDefault();
                        User requestor = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
                        //EmailCommon.SendEmail(approveOrRejectRequestDto.Id.ToString(), requestor.Surname+requestor.Name, requestor.EmailAddress, "PhamVanHieu",
                        //    "chinhc5ph@gmail.com", amRequestAssetBring.Title, "Test", "Test", mstTemplateEmail.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (AppConsts.TYPE_REJECT.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_TEM_MANAGER_REJECT)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;
                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        aioRequestActionLog.ReasonRefusal = approveOrRejectRequestDto.ReasonRefusal;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("REJECTINGFORUSER")).FirstOrDefault();
                        User user = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //EmailCommon.SendEmail(approveOrRejectRequestDto.Id.ToString(), user.Surname+user.Name, user.EmailAddress, "PhamVanHieu",
                        //    "chinhc5ph@gmail.com", amRequestAssetBring.Title, "Test", "Test", mstTemplateEmailForUser.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //manager acctep or reject request
        public async Task<bool> ManagerApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto)
        {
            if (approveOrRejectRequestDto.Id > 0)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
                AioRequest amRequestAssetBring = await _amRequestAssetBringRepository.FirstOrDefaultAsync(approveOrRejectRequestDto.Id);
                if (AppConsts.TYPE_APPROVED.Equals(approveOrRejectRequestDto.Type))
                {
                    if (amRequestAssetBring != null)
                    {
                        long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_MANAGER_APPROVE)).Id;
                        amRequestAssetBring.Status = statusId;
                        amRequestAssetBring.DateManageApproval = DateTime.Now;
                        //amRequestAssetBring.ManageApproval = user.Surname + " " + user.Name;
                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("APPROVALFORUSER")).FirstOrDefault();
                        //User userMail = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), userMail.UserName, userMail.FullNameManager, (userMail.Name + " " + userMail.Surname), userMail.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, userMail.Division);

                        ////send email to adm
                        //MstTemplateEmail mstTemplateEmail = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("WAITTING")).FirstOrDefault();
                        //User requestor = _userRepository.GetAll().Where(p => p.Id == AbpSession.UserId).FirstOrDefault();
                        //var usersAdm = await UserManager.GetUsersInRoleAsync(AppConsts.ROLE_ADM);
                        //foreach (User userRoleAdm in usersAdm)
                        //{
                        //    User userAdm = _userRepository.GetAll().Where(p => p.UserName == userRoleAdm.UserName).FirstOrDefault();
                        //    await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), requestor.UserName, (requestor.Name + " " + requestor.Surname), (userRoleAdm.Name + " " + userRoleAdm.Surname), userRoleAdm.UserName, amRequestAssetBring.Title, mstTemplateEmail.Title, mstTemplateEmail.Message, requestor.Division);
                        //}
                        //EmailCommon.SendEmail(approveOrRejectRequestDto.Id.ToString(), requestor.Surname+requestor.Name, requestor.EmailAddress, "PhamVanHieu",
                        //    "chinhc5ph@gmail.com", amRequestAssetBring.Title, "Test", "Test", mstTemplateEmail.Message);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (AppConsts.TYPE_REJECT.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_MANAGER_REJECT)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;

                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        aioRequestActionLog.ReasonRefusal = approveOrRejectRequestDto.ReasonRefusal;

                        //amRequestAssetBring.ManageApproval = user.Surname + " " + user.Name;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("REJECTINGFORUSER")).FirstOrDefault();
                        //User userMail = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), userMail.UserName, userMail.FullNameManager, (userMail.Name + " " + userMail.Surname), userMail.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, userMail.Division);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (AppConsts.TYPE_REQUEST_INFO.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_MANAGER_REQUEST_INFO)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;
                        amRequestAssetBring.ManageApprovalMessage = approveOrRejectRequestDto.ReasonRefusal;

                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        //amRequestAssetBring.ManageApproval = user.Surname + " " + user.Name;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("REQUEST_INFO")).FirstOrDefault();
                        //User userMail = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), userMail.UserName, userMail.FullNameManager, (userMail.Name + " " + userMail.Surname), userMail.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, userMail.Division);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AdmApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto)
        {
            if (approveOrRejectRequestDto.Id > 0)
            {
                AioRequest amRequestAssetBring = await _amRequestAssetBringRepository.FirstOrDefaultAsync(approveOrRejectRequestDto.Id);
                if (AppConsts.TYPE_APPROVED.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_ADM_APPROVE)).Id;
                    long statusGuardWait = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_GUARD)).Id;
                    if (amRequestAssetBring != null)
                    {
                        //Update AmRequestAssetBring
                        amRequestAssetBring.Status = statusId;
                        amRequestAssetBring.DateAdminApproval = DateTime.Now;
                        amRequestAssetBring.AdmApprovalMessage = approveOrRejectRequestDto.ReasonRefusal;
                        amRequestAssetBring.AdminApprovalId = AbpSession.UserId;
                        //Insert AioRequestActionLog
                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        //Update AsetIO
                        List<AioRequestAsset> assetIO = _amAssetRepository.GetAll().Where(p => p.RequestId == amRequestAssetBring.Id).ToList();
                        List<AioRequestPeople> employeesIO = _amEmployeesRepository.GetAll().Where(p => p.RequestId == amRequestAssetBring.Id).ToList();
                        if (assetIO != null)
                        {
                            foreach (AioRequestAsset asset in assetIO)
                            {
                                asset.Status = statusGuardWait;
                                await _amAssetRepository.UpdateAsync(asset);

                                //update in
                                AioRequestAssetInOut aioAssetCheckInOut = new AioRequestAssetInOut();
                                aioAssetCheckInOut.RequestAssetId = asset.Id;
                                aioAssetCheckInOut.CreationTime = DateTime.Now;
                                aioAssetCheckInOut.CreatorUserId = AbpSession.UserId;
                                await _aioAssetCheckInOut.InsertAsync(aioAssetCheckInOut);
                            }
                        }
                        if (employeesIO != null)
                        {
                            foreach (AioRequestPeople employees in employeesIO)
                            {
                                employees.Status = statusGuardWait;
                                await _amEmployeesRepository.UpdateAsync(employees);

                                //Update in
                                AioRequestPeopleInOut aioPeopleCheckInOut = new AioRequestPeopleInOut();
                                aioPeopleCheckInOut.RequestPeopleId = employees.Id;
                                aioPeopleCheckInOut.CreationTime = DateTime.Now;
                                aioPeopleCheckInOut.CreatorUserId = AbpSession.UserId;
                                await _aioPeopleCheckInOut.InsertAsync(aioPeopleCheckInOut);
                            }
                        }
                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();

                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("APPROVALFORUSER")).FirstOrDefault();
                        //User user = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), user.AccountManager, user.FullNameManager, (user.Name + " " + user.Surname), user.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, user.Division);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (AppConsts.TYPE_REJECT.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_ADM_REJECT)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;

                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        amRequestAssetBring.AdminApprovalId = AbpSession.UserId;
                        aioRequestActionLog.ReasonRefusal = approveOrRejectRequestDto.ReasonRefusal;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("REJECTINGFORUSER")).FirstOrDefault();
                        //User user = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), user.UserName, user.FullNameManager, (user.Name + " " + user.Surname), user.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, user.Division);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (AppConsts.TYPE_REQUEST_INFO.Equals(approveOrRejectRequestDto.Type))
                {
                    long statusId = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_ADM_REQUEST_INFO)).Id;
                    if (amRequestAssetBring != null)
                    {
                        amRequestAssetBring.Status = statusId;
                        amRequestAssetBring.AdmApprovalMessage = approveOrRejectRequestDto.ReasonRefusal;

                        AioRequestLog aioRequestActionLog = new AioRequestLog();
                        aioRequestActionLog.StatusId = statusId;
                        aioRequestActionLog.RequestId = amRequestAssetBring.Id;
                        amRequestAssetBring.AdminApprovalId = AbpSession.UserId;

                        await _amRequestAssetBringRepository.UpdateAsync(amRequestAssetBring);
                        await _aioRequestActionLogRepository.InsertAsync(aioRequestActionLog);
                        await CurrentUnitOfWork.SaveChangesAsync();
                        //send email to user
                        //MstTemplateEmail mstTemplateEmailForUser = _mstTemplateEmailRepository.GetAll().Where(p => p.TemplateCode.Equals("REQUEST_INFO")).FirstOrDefault();
                        //User user = _userRepository.GetAll().Where(p => p.Id == amRequestAssetBring.CreatorUserId).FirstOrDefault();
                        //await _iSendEmailRequest.SendEmail(amRequestAssetBring.Id.ToString(), user.UserName, user.FullNameManager, (user.Name + " " + user.Surname), user.UserName, amRequestAssetBring.Title, mstTemplateEmailForUser.Title, mstTemplateEmailForUser.Message, user.Division);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        //function orther

        public async Task<int> CountDraftRequest()
        {
            var user = await _userRepository.FirstOrDefaultAsync((long)AbpSession.UserId);
            var countDraftRequest = _amRequestAssetBringRepository.GetAll().Where(p => p.StatusDraft == 1 && p.CreatorUserId == user.Id).ToList().Count();
            return countDraftRequest;
        }

    }
}
