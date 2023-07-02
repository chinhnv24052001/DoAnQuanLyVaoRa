using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using System.Linq;
using System.Threading.Tasks;
using tmss.AssetManaments;
using tmss.AssetManaments.AssetInOutManament;
using tmss.AssetManaments.AssetInOutManament.Dto;
using tmss.Master;
using tmss.Master.Asset;
using tmss.Master.Vender;
using BarcodeLib;
using System.IO;
using tmss.SQL;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using tmss.Master.TemEmployeesLearnedSafety;
using tmss.Master.EmployeesLearnedSafety;
using tmss.AssetManaments.RequestAssetBring.Dto;
using tmss.Master.Employees;
using Microsoft.AspNetCore.Hosting;

namespace tmss.AssetManament
{
    public class AssetInOutManamentAppService : tmssAppServiceBase, IAssetInOutManamentAppService
    {
        private readonly IRepository<AioRequest, long> _amRequestAssetBringRepository;
        private readonly IRepository<AioRequestAsset, long> _amAssetRepository;
        private readonly IRepository<AioRequestPeople, long> _amEmployeesRepository;
        private readonly IRepository<MstAsset, long> _mstAssetRepository;
        private readonly IRepository<MstVender, long> _mstVenderRepository;
        private readonly IRepository<AioRequestAssetInOut, long> _aioAssetCheckInOut;
        private readonly IRepository<AioRequestPeopleInOut, long> _aioPeopleCheckInOut;
        private readonly IDapperRepository<MstAsset, long> _aioSearchRequestRepository;
        private readonly IRepository<MstEmployeesLearnedSafety, long> _mstEmployeesLearnedSafetyRepository;
        private IWebHostEnvironment _env;

        private readonly IRepository<MstStatus, long> _statusRequestRepository;
        private readonly ISqlHelper _sqlHelper;
        public AssetInOutManamentAppService(IRepository<AioRequest, long> amRequestAssetBringRepository,
            IRepository<AioRequestAsset, long> amAssetRepository,
            IRepository<AioRequestPeople, long> amEmployeesRepository,
            IRepository<MstAsset, long> mstAssetRepository,
            IRepository<MstVender, long> mstVenderRepository,
            IDapperRepository<MstAsset, long> aioSearchRequestRepository,
            IRepository<AioRequestAssetInOut, long> aioAssetCheckInOut,
            IRepository<AioRequestPeopleInOut, long> aioPeopleCheckInOut,
            IRepository<MstStatus, long> statusRequestRepository,
            IRepository<MstEmployeesLearnedSafety, long> mstEmployeesLearnedSafetyRepository,
            ISqlHelper sqlHelper,
            IWebHostEnvironment env)
        {
            _amRequestAssetBringRepository = amRequestAssetBringRepository;
            _amAssetRepository = amAssetRepository;
            _amEmployeesRepository = amEmployeesRepository;
            _mstAssetRepository = mstAssetRepository;
            _mstVenderRepository = mstVenderRepository;
            _aioSearchRequestRepository = aioSearchRequestRepository;
            _aioAssetCheckInOut = aioAssetCheckInOut;
            _aioPeopleCheckInOut = aioPeopleCheckInOut;
            _statusRequestRepository = statusRequestRepository;
            _mstEmployeesLearnedSafetyRepository=mstEmployeesLearnedSafetyRepository;
            _sqlHelper = sqlHelper;
            _env = env;
        }

        public async Task<long> CheckInOutAsset(long input, long noteId, string statusInOut)
        {
            AioRequestAsset assetInOut = await _amAssetRepository.FirstOrDefaultAsync(input);
            if (assetInOut != null)
            {
                long statusGuardWait = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_GUARD)).Id;
                var statusIn = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_BROUGHT_IN));
                var statusOut = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_BROUGHT_OUT));

                if (assetInOut.Status == statusGuardWait)
                {
                    AioRequestAssetInOut aioAssetCheckInOut = new AioRequestAssetInOut();
                    if (statusInOut == "IN")
                    {
                        assetInOut.Status = statusIn.Id;
                        assetInOut.IsIn = 1;
                        aioAssetCheckInOut.InDateTime = DateTime.Now;
                    }
                    else
                    {
                        assetInOut.Status = statusOut.Id;
                        assetInOut.IsIn = 2;
                        aioAssetCheckInOut.OutDateTime = DateTime.Now;
                    }
                    aioAssetCheckInOut.RequestAssetId = assetInOut.Id;
                    aioAssetCheckInOut.NoteInId = noteId;
                    aioAssetCheckInOut.CreationTime = DateTime.Now;
                    aioAssetCheckInOut.CreatorUserId = AbpSession.UserId;
                    await _aioAssetCheckInOut.InsertAsync(aioAssetCheckInOut);
                    await _amAssetRepository.UpdateAsync(assetInOut);
                }
                else if (assetInOut.Status == statusOut.Id)
                {
                    assetInOut.Status = statusIn.Id;
                    assetInOut.IsIn = 1;
                    AioRequestAssetInOut aioAssetCheckInOut = new AioRequestAssetInOut();
                    aioAssetCheckInOut.RequestAssetId = assetInOut.Id;
                    aioAssetCheckInOut.InDateTime = DateTime.Now;
                    aioAssetCheckInOut.NoteInId = noteId;
                    aioAssetCheckInOut.CreationTime = DateTime.Now;
                    aioAssetCheckInOut.CreatorUserId = AbpSession.UserId;
                    await _aioAssetCheckInOut.InsertAsync(aioAssetCheckInOut);
                    await _amAssetRepository.UpdateAsync(assetInOut);
                }
                else
                {
                    assetInOut.Status = statusOut.Id;
                    assetInOut.IsIn = 2;
                    AioRequestAssetInOut aioAssetCheckInOut = _aioAssetCheckInOut.GetAll().
                    Where(p => p.RequestAssetId == assetInOut.Id).OrderByDescending(e => e.Id)
                   .FirstOrDefault();
                    aioAssetCheckInOut.NoteOutId = noteId;
                    aioAssetCheckInOut.LastModificationTime = DateTime.Now;
                    aioAssetCheckInOut.OutDateTime = DateTime.Now;
                    aioAssetCheckInOut.LastModifierUserId = AbpSession.UserId;
                    await _aioAssetCheckInOut.UpdateAsync(aioAssetCheckInOut);
                    await _amAssetRepository.UpdateAsync(assetInOut);
                }
            }
            return assetInOut.RequestId;
        }

        public async Task<long> CheckInOutEmployees(long input, long noteId, string statusInOut)
        {
            AioRequestPeople employeesInOut = await _amEmployeesRepository.FirstOrDefaultAsync(input);
            if (employeesInOut != null)
            {
                long statusGuardWait = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WAIT_GUARD)).Id;
                long statusIn = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_GOT_IN)).Id;
                long statusOut = _statusRequestRepository.FirstOrDefault(p => p.Key.Equals(AppConsts.STATUS_WENT_OUT)).Id;

                if (employeesInOut.Status == statusGuardWait)
                {
                    AioRequestPeopleInOut aioPeopleCheckInOut = new AioRequestPeopleInOut();
                    if (statusInOut == "IN")
                    {
                        employeesInOut.Status = statusIn;
                        employeesInOut.IsIn = true;
                        aioPeopleCheckInOut.InDateTime = DateTime.Now;
                    }
                    else
                    {
                        employeesInOut.Status = statusOut;
                        employeesInOut.IsIn = false;
                        aioPeopleCheckInOut.OutDateTime = DateTime.Now;
                    }

                    aioPeopleCheckInOut.RequestPeopleId = employeesInOut.Id;
                    aioPeopleCheckInOut.NoteInId = noteId;
                    aioPeopleCheckInOut.CreationTime = DateTime.Now;
                    aioPeopleCheckInOut.CreatorUserId = AbpSession.UserId;
                    await _aioPeopleCheckInOut.InsertAsync(aioPeopleCheckInOut);
                    await _amEmployeesRepository.UpdateAsync(employeesInOut);
                }
                else if ( employeesInOut.Status == statusOut)
                {
                    employeesInOut.Status = statusIn;
                    employeesInOut.IsIn = true;
                    AioRequestPeopleInOut aioPeopleCheckInOut = new AioRequestPeopleInOut();
                    aioPeopleCheckInOut.RequestPeopleId = employeesInOut.Id;
                    aioPeopleCheckInOut.NoteInId = noteId;
                    aioPeopleCheckInOut.CreationTime = DateTime.Now;
                    aioPeopleCheckInOut.InDateTime = DateTime.Now;
                    aioPeopleCheckInOut.CreatorUserId = AbpSession.UserId;
                    await _aioPeopleCheckInOut.InsertAsync(aioPeopleCheckInOut);
                    await _amEmployeesRepository.UpdateAsync(employeesInOut);
                }
                else
                {
                    employeesInOut.Status = statusOut;
                    employeesInOut.IsIn = false;
                    AioRequestPeopleInOut aioPeopleCheckInOut = _aioPeopleCheckInOut.GetAll().
                    Where(p => p.RequestPeopleId == employeesInOut.Id).OrderByDescending(e => e.Id)
                   .FirstOrDefault();
                    aioPeopleCheckInOut.OutDateTime = DateTime.Now;
                    aioPeopleCheckInOut.NoteOutId = noteId;
                    aioPeopleCheckInOut.LastModificationTime = DateTime.Now;
                    aioPeopleCheckInOut.LastModifierUserId = AbpSession.UserId;
                    await _aioPeopleCheckInOut.UpdateAsync(aioPeopleCheckInOut);
                    await _amEmployeesRepository.UpdateAsync(employeesInOut);
                }
            }
            return employeesInOut.RequestId;
        }

        //set in-out all request
        public async Task CheckInOutRequest(long input, bool IsRequestAssetIn, string StringInOut)
        {
            RequestAssetInOutInputDto requestAssetInOutInputDto = new RequestAssetInOutInputDto();
            requestAssetInOutInputDto.IsRequestAssetIn = IsRequestAssetIn;
            requestAssetInOutInputDto.RequestId = input;

            var listAssetInOut = await LoadAllAssetsInOutToCheck(requestAssetInOutInputDto);
            foreach (var asset in listAssetInOut)
            {
               await CheckInOutAsset(asset.Id, 0, StringInOut);
            }

            
            var listEmployeesInOut = await LoadAllEmployeesInOutCheck(requestAssetInOutInputDto);
            foreach (var employees in listEmployeesInOut)
            {
               await CheckInOutEmployees(employees.Id, 0, StringInOut);
            }
        }

        //getAll to check 
        public async Task<List<AssetInOutSelectOutputDto>> LoadAllAssetsInOutToCheck(RequestAssetInOutInputDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_IO @IsAssetIn, @SeriNumber, @RequestId, @TagCode";

            var assetInOut = await _aioSearchRequestRepository.QueryAsync<AssetInOutSelectOutputDto>(_sql, new
            {
                @IsAssetIn = input.IsRequestAssetIn,
                @SeriNumber = input.SeriNumber,
                @RequestId = input.RequestId,
                @TagCode = input.TagCode
            });

            var result = assetInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            return result.ToList();
        }

        public async Task<List<EmployeesInOutSelectOutputDto>> LoadAllEmployeesInOutCheck(RequestAssetInOutInputDto input)
        {
            string _sql = "EXEC P_SEARCH_EMPLOYEES_IO @IsEmployeesIn, @EmployeesName, @IdentityCard, @RequestId";

            var employeesInOut = await _aioSearchRequestRepository.QueryAsync<EmployeesInOutSelectOutputDto>(_sql, new
            {
                @IsEmployeesIn = input.IsRequestAssetIn,
                @EmployeesName = input.EmployeesName,
                @IdentityCard = input.IdentityCard,
                @RequestId = input.RequestId
            });

            var result = employeesInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            return result.ToList();
        }
        
        //Create BarCode Asset and Employees =>QR Code
        public async Task<byte[]> CreateAssetBarCode(int teamItem, long input)
        {
            AioRequestAsset requestAsset;
            AioRequestPeople requestEmployees;
            string requestAssetIdToString;
            string changeId;
            string barCodeString;
            if (teamItem==1)
            {
                 requestAsset = await _amAssetRepository.FirstOrDefaultAsync(p => p.Id == input); //_amAssetRepository.GetAll().Where(p=>p.AssetId == input).FirstOrDefault();
                 requestAssetIdToString = requestAsset.RequestId.ToString();
                barCodeString = "a";
                changeId = input.ToString();
            }
            else
            {
                requestEmployees = await _amEmployeesRepository.FirstOrDefaultAsync(p => p.Id == input); //_amAssetRepository.GetAll().Where(p=>p.AssetId == input).FirstOrDefault();
                 requestAssetIdToString = requestEmployees.RequestId.ToString();
                barCodeString = "e";
                 changeId = input.ToString();
            }

            do
            {
                changeId = "0" + changeId;
            }
            while (changeId.Length < 7);

            do
            {
                requestAssetIdToString = "0" + requestAssetIdToString;
            }
            while (requestAssetIdToString.Length < 7);

             barCodeString += "-" + requestAssetIdToString + "-"+ changeId;


            //BarcodeLib.Barcode code128 = new Barcode();
            //Image imBarcode = code128.Encode(BarcodeLib.TYPE.CODE128, barCodeString);

            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(barCodeString, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(10);

            MemoryStream ms = new MemoryStream();
            //imBarcode.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            QrBitmap.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }

        //Get info barcode =>QR Code
        public async Task<string> GetInfoBarCode(int teamItem, long input)
        {
            AioRequestAsset requestAsset;
            AioRequestPeople requestEmployees;
            string barCodeString;       
            if (teamItem == 1)
            {
                requestAsset = await _amAssetRepository.FirstOrDefaultAsync(p => p.Id == input); //_amAssetRepository.GetAll().Where(p=>p.AssetId == input).FirstOrDefault();
                var mstAsset = await _mstAssetRepository.FirstOrDefaultAsync(p => p.Id == requestAsset.AssetId);
                if(requestAsset.SeriNumber!=null)
                {
                    barCodeString = mstAsset.AssetName + "-" + requestAsset.SeriNumber;
                }
                else barCodeString = mstAsset.AssetName;
            }
            else
            {
                requestEmployees = await _amEmployeesRepository.FirstOrDefaultAsync(p => p.Id == input); //_amAssetRepository.GetAll().Where(p=>p.AssetId == input).FirstOrDefault();
                barCodeString = requestEmployees.EmployeesName+"-"+requestEmployees.IdentityCard;
            }

            return barCodeString;
        }

        //Read BarCode
        public async Task<QRCodeResultDto> ReadQRCode(string input)
        {
            QRCodeResultDto qRCodeResultDto = new QRCodeResultDto();
            QRCodeResultDto qRCodeResultDtoResultFalse = new QRCodeResultDto();
            qRCodeResultDtoResultFalse.StatusQRScanner = 3;

            string stringCut;
            stringCut = input.Substring(2);
            string[] arrListStr = stringCut.Split('-');
            if(arrListStr.Length !=2 )
            {
                return qRCodeResultDtoResultFalse;
            }    
            else
            {
                var stringRequestId = arrListStr[0];
                var stringId = arrListStr[1];
                qRCodeResultDto.RequestId = (long)Convert.ToDouble(stringRequestId);
                qRCodeResultDto.FirstItemString = input.Substring(0, 1);
                long Id = (long)Convert.ToDouble(stringId);

                var requestAssetBring = await _amRequestAssetBringRepository.GetAll()
                   .Select(e => new RequestAssetBringDetailDto
                   {
                       Id = e.Id,
                       Title = e.Title,
                       Description = e.Description,
                       DateRequest = e.DateRequest.ToString("dd/MM/yyyy"),
                       RequestCode = e.RequestCode,
                       VenderId = e.VenderId,
                       TypeRequest = e.TypeRequest,
                       LiveMonitorName = e.LiveMonitorName,
                       LiveMonitorPhoneNumber = e.LiveMonitorPhoneNumber,
                       LiveMonitorDepartment = e.LiveMonitorDepartment,
                       DepartmentClient = e.DepartmentClient,
                       TradeUnionOrganization = e.TradeUnionOrganization,
                       WhereToBring = e.WhereToBring,
                       Status = e.Status,
                       PersonInChargeOfSubName = e.PersonInChargeOfSubName,
                       PersonInChangeOfSubPhone = e.PersonInChangeOfSubPhone,
                   })
                   .FirstOrDefaultAsync(p => p.Id == qRCodeResultDto.RequestId);

                if (requestAssetBring == null)
                {
                    return qRCodeResultDtoResultFalse;
                }
                else
                {
                    qRCodeResultDto.Title = requestAssetBring.Title;
                    qRCodeResultDto.RequestId = requestAssetBring.Id;
                    qRCodeResultDto.Description = requestAssetBring.Description;
                    qRCodeResultDto.DateRequest = requestAssetBring.DateRequest;
                    qRCodeResultDto.RequestCode = requestAssetBring.RequestCode;
                    qRCodeResultDto.TypeRequest = requestAssetBring.TypeRequest;
                    qRCodeResultDto.LiveMonitorName = requestAssetBring.LiveMonitorName;
                    qRCodeResultDto.LiveMonitorPhoneNumber = requestAssetBring.LiveMonitorPhoneNumber;
                    qRCodeResultDto.LiveMonitorDepartment = requestAssetBring.LiveMonitorDepartment;
                    qRCodeResultDto.WhereToBring = requestAssetBring.WhereToBring;
                    qRCodeResultDto.DepartmentClient = requestAssetBring.DepartmentClient;
                    qRCodeResultDto.TradeUnionOrganization = requestAssetBring.TradeUnionOrganization;
                    qRCodeResultDto.PersonInChargeOfSubName = requestAssetBring.PersonInChargeOfSubName;
                    qRCodeResultDto.PersonInChangeOfSubPhone = requestAssetBring.PersonInChangeOfSubPhone;

                    if (requestAssetBring.TypeRequest != 1 && requestAssetBring.TypeRequest != 4 )
                    {
                        qRCodeResultDto.VenderName = _mstVenderRepository.GetAll().Where(p => p.Id == requestAssetBring.VenderId).FirstOrDefault().VenderName;
                    }

                    if(qRCodeResultDto.FirstItemString != "a" && qRCodeResultDto.FirstItemString != "e")
                    {
                        return qRCodeResultDtoResultFalse;
                    }
                    else
                    {
                        if (qRCodeResultDto.FirstItemString == "a")
                        {
                            var assetInfo = (from assetIO in _amAssetRepository.GetAll().AsNoTracking()
                                             join asset in _mstAssetRepository.GetAll().AsNoTracking()
                                             on assetIO.AssetId equals asset.Id
                                             select new AssetInfoScannerDto()
                                             {
                                                 AssetIOId = assetIO.Id,
                                                 AssetName = asset.AssetName,
                                                 SeriNumber = assetIO.SeriNumber,
                                                 TagCode = assetIO.TagCode,
                                                 Total = assetIO.Total,
                                                 DateStart = assetIO.DateStart.ToString("dd/MM/yyyy"),
                                                 DateEnd = assetIO.DateEnd != null ? assetIO.DateEnd.Value.ToString("dd/MM/yyyy") : "",
                                                 DateEffect = assetIO.DateEnd,
                                                 AssetImage = assetIO.AssetImage,
                                                 AviationIsBack = assetIO.AviationIsBack,
                                                 AviationIsBackString = assetIO.AviationIsBack.Value ? L("AviationIsBack") : ""
                                             }).FirstOrDefault(e => e.AssetIOId == Id);
                            
                            if (assetInfo == null)
                            {
                                return qRCodeResultDtoResultFalse;
                            }
                            else
                            {
                                qRCodeResultDto.StatusQRScanner = 1;

                            if(assetInfo.DateEffect != null)
                                {
                                    if (assetInfo.DateEffect.Value.Year < DateTime.Now.Year)
                                    {
                                        assetInfo.CheckEffectDate = false;
                                    }
                                    else if (assetInfo.DateEffect.Value.Year == DateTime.Now.Year)
                                    {
                                        if (assetInfo.DateEffect.Value.Month < DateTime.Now.Month)
                                        {
                                            assetInfo.CheckEffectDate = false;
                                        }
                                        else if (assetInfo.DateEffect.Value.Month == DateTime.Now.Month)
                                        {
                                            if (assetInfo.DateEffect.Value.Day < DateTime.Now.Day)
                                            {
                                                assetInfo.CheckEffectDate = false;
                                            }
                                            else
                                            {
                                                assetInfo.CheckEffectDate = true;
                                            }
                                        }
                                        else
                                        {
                                            assetInfo.CheckEffectDate = true;
                                        }
                                    }
                                    else
                                    {
                                        assetInfo.CheckEffectDate = true;
                                    }
                                }
                            else
                                {
                                    assetInfo.CheckEffectDate = false;
                                }

                                AioRequestAssetInOut aioAssetCheckInOut = _aioAssetCheckInOut.GetAll().
                                    Where(p => p.RequestAssetId == assetInfo.AssetIOId).OrderByDescending(e => e.Id)
                                   .FirstOrDefault();
                                if (aioAssetCheckInOut != null)
                                {
                                    assetInfo.DateIn = aioAssetCheckInOut.InDateTime != null ? aioAssetCheckInOut.InDateTime.Value.ToString("dd/MM/yyyy") : "";
                                    assetInfo.DateOut = aioAssetCheckInOut.OutDateTime != null ? aioAssetCheckInOut.OutDateTime.Value.ToString("dd/MM/yyyy") : "";
                                    if ((aioAssetCheckInOut.InDateTime == null && aioAssetCheckInOut.OutDateTime == null) || (aioAssetCheckInOut.InDateTime != null && aioAssetCheckInOut.OutDateTime != null))
                                    {
                                        qRCodeResultDto.IsIn = true;
                                        if(aioAssetCheckInOut.InDateTime == null && aioAssetCheckInOut.OutDateTime == null)
                                        {
                                            qRCodeResultDto.FirtStatusInOut = true;
                                        }
                                    }
                                    else qRCodeResultDto.IsIn = false;
                                }

                                qRCodeResultDto.AssetInfo = assetInfo;
                            }
                        }
                        else
                        {
                            var employeesInfo = _amEmployeesRepository.GetAll()
                                             .Select(e => new EmployeesInfoScannerDto
                                             {
                                                 EmployeesIOId = e.Id,
                                                 IdentityCard = e.IdentityCard,
                                                 EmployeesName = e.EmployeesName,
                                                 DateStart = e.DateStart.ToString("dd/MM/yyyy"),
                                                 DateEnd = e.DateEnd.ToString("dd/MM/yyyy"),
                                                 DateEffect = e.DateEnd,
                                                 Target = e.Target,
                                                 Company = e.Company

                                             }).FirstOrDefault(e => e.EmployeesIOId == Id);
                            if(employeesInfo == null)
                            {
                                return qRCodeResultDtoResultFalse;
                            }
                            else
                            {
                                qRCodeResultDto.StatusQRScanner = 1;
                                var employeesLearned = await _mstEmployeesLearnedSafetyRepository.FirstOrDefaultAsync(p => p.IdentityCard == employeesInfo.IdentityCard && p.Image != null);
                                if (employeesLearned != null)
                                {
                                    //employeesInfo.EmployeesImage = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(_env.ContentRootPath, employeesLearned.FilePath)));
                                    employeesInfo.EmployeesImage = employeesLearned.Image;
                                }
                                else employeesInfo.EmployeesImage = "";

                                //Check han vao ra
                                if (employeesInfo.DateEffect.Year < DateTime.Now.Year)
                                {
                                    employeesInfo.CheckEffectDate = false;
                                }
                                else if (employeesInfo.DateEffect.Year == DateTime.Now.Year)
                                {
                                    if (employeesInfo.DateEffect.Month < DateTime.Now.Month)
                                    {
                                        employeesInfo.CheckEffectDate = false;
                                    }
                                    else if (employeesInfo.DateEffect.Month == DateTime.Now.Month)
                                    {
                                        if (employeesInfo.DateEffect.Day < DateTime.Now.Day)
                                        {
                                            employeesInfo.CheckEffectDate = false;
                                        }
                                        else
                                        {
                                            employeesInfo.CheckEffectDate = true;
                                        }
                                    }
                                    else
                                    {
                                        employeesInfo.CheckEffectDate = true;
                                    }
                                }
                                else
                                {
                                    employeesInfo.CheckEffectDate = true;
                                }

                                var employeesLearnedSafety = _mstEmployeesLearnedSafetyRepository.GetAll().FirstOrDefault(e => e.IdentityCard == employeesInfo.IdentityCard);
                                if (employeesLearnedSafety != null)
                                {
                                    employeesInfo.PhoneNumber = employeesLearnedSafety.PhoneNumber;
                                    employeesInfo.Address = employeesLearnedSafety.Address;
                                }
                                AioRequestPeopleInOut aioPeopleCheckInOut = _aioPeopleCheckInOut.GetAll().
                                 Where(p => p.RequestPeopleId == employeesInfo.EmployeesIOId).OrderByDescending(e => e.Id)
                                .FirstOrDefault();
                                if (aioPeopleCheckInOut != null)
                                {
                                    employeesInfo.DateIn = aioPeopleCheckInOut.InDateTime != null ? aioPeopleCheckInOut.InDateTime.Value.ToString("dd/MM/yyyy") : "";
                                    employeesInfo.DateOut = aioPeopleCheckInOut.OutDateTime != null ? aioPeopleCheckInOut.OutDateTime.Value.ToString("dd/MM/yyyy") : "";
                                    if ((aioPeopleCheckInOut.InDateTime == null && aioPeopleCheckInOut.OutDateTime == null) || (aioPeopleCheckInOut.InDateTime != null && aioPeopleCheckInOut.OutDateTime != null))
                                    {
                                        qRCodeResultDto.IsIn = true;
                                    }
                                    else qRCodeResultDto.IsIn = false;
                                }

                                qRCodeResultDto.EmployeesInfo = employeesInfo;
                            }
                        }
                    }

                    var mstStatus = await _statusRequestRepository.FirstOrDefaultAsync(requestAssetBring.Status.Value);
                    if (mstStatus.Key != AppConsts.STATUS_ADM_APPROVE)
                    {
                        qRCodeResultDto.StatusQRScanner = 2;
                        //return qRCodeResultDtoResultFalse;
                    }
                }
            }
            

            return qRCodeResultDto;
        }

        //Check any when approval request in out
        public async Task<bool> CheckAssetOrEmployeesInRequestInOut(long id)
        {
            bool checkEffective=true;
            var listAsset = _amAssetRepository.GetAll().Where(p => p.RequestId == id);
            var listEmployees = _amEmployeesRepository.GetAll().Where(p => p.RequestId == id);
            if(listAsset!=null)
            {
                //checkEffective= listAsset.Any(e => CheckEffectiveDateAssetInOut(e.Id));
                foreach(var item in listAsset)
                {
                    if (item.DateEnd != null)
                    {
                        if (item.DateEnd.Value.Year < DateTime.Now.Year)
                        {
                            checkEffective = false;
                        }
                        else if (item.DateEnd.Value.Year == DateTime.Now.Year)
                        {
                            if (item.DateEnd.Value.Month < DateTime.Now.Month)
                            {
                                checkEffective = false;
                            }
                            else if (item.DateEnd.Value.Month == DateTime.Now.Month)
                            {
                                if (item.DateEnd.Value.Day < DateTime.Now.Day)
                                {
                                    checkEffective = false;
                                }
                                else
                                {
                                    checkEffective = true;
                                }
                            }
                            else
                            {
                                checkEffective = true;
                            }
                        }
                        else
                        {
                            checkEffective = true;
                        }

                        if (checkEffective == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        checkEffective = false;
                    }
                   
                }
            }
            else
            {
                //checkEffective = listEmployees.Any(e => CheckEffectiveDateEmployeesInOut(e.Id));
                foreach (var item in listEmployees)
                {
                    if (item.DateEnd.Year < DateTime.Now.Year)
                    {
                        checkEffective = false;
                    }
                    else if (item.DateEnd.Year == DateTime.Now.Year)
                    {
                        if (item.DateEnd.Month < DateTime.Now.Month)
                        {
                            checkEffective = false;
                        }
                        else if (item.DateEnd.Month == DateTime.Now.Month)
                        {
                            if (item.DateEnd.Day < DateTime.Now.Day)
                            {
                                checkEffective = false;
                            }
                            else
                            {
                                checkEffective = true;
                            }
                        }
                        else
                        {
                            checkEffective = true;
                        }
                    }
                    else
                    {
                        checkEffective = true;
                    }

                    if (checkEffective == false)
                    {
                        break;
                    }
                }
            }
            return checkEffective;
        }


        //Check effective Asset
        public async Task<bool> CheckEffectiveDateAssetInOut(long id)
        {
            bool checkEffective;
            var assetInfo = (from assetIO in _amAssetRepository.GetAll().AsNoTracking()
                             join asset in _mstAssetRepository.GetAll().AsNoTracking()
                             on assetIO.AssetId equals asset.Id
                             select new AssetInfoScannerDto()
                             {
                                 AssetIOId = assetIO.Id,
                                 AssetName = asset.AssetName,
                                 SeriNumber = assetIO.SeriNumber,
                                 TagCode = assetIO.TagCode,
                                 Total = assetIO.Total,
                                 DateStart = assetIO.DateStart.ToString("dd/MM/yyyy"),
                                 DateEnd = assetIO.DateEnd != null ?  assetIO.DateEnd.Value.ToString("dd/MM/yyyy") : "",
                                 DateEffect = assetIO.DateEnd,
                                 AssetImage = assetIO.AssetImage
                             }).FirstOrDefault(e => e.AssetIOId == id);
            if (assetInfo == null)
            {
                checkEffective= false;
            }
            else
            {
                //if (assetInfo.DateEffect.Year <= DateTime.Now.Year || assetInfo.DateEffect.Month <= DateTime.Now.Month || assetInfo.DateEffect.Day <= DateTime.Now.Day)
                //{
                //    checkEffective = false;
                //}
                //else checkEffective = true;

                if(assetInfo.DateEffect != null)
                {
                    if (assetInfo.DateEffect.Value.Year < DateTime.Now.Year)
                    {
                        checkEffective = false;
                    }
                    else if (assetInfo.DateEffect.Value.Year == DateTime.Now.Year)
                    {
                        if (assetInfo.DateEffect.Value.Month < DateTime.Now.Month)
                        {
                            checkEffective = false;
                        }
                        else if (assetInfo.DateEffect.Value.Month == DateTime.Now.Month)
                        {
                            if (assetInfo.DateEffect.Value.Day < DateTime.Now.Day)
                            {
                                checkEffective = false;
                            }
                            else
                            {
                                checkEffective = true;
                            }
                        }
                        else
                        {
                            checkEffective = true;
                        }
                    }
                    else
                    {
                        checkEffective = true;
                    }
                }
                else
                {
                    checkEffective = false;
                }
            }
            return checkEffective;
        }

        //Check effective Employees
        public async Task<bool> CheckEffectiveDateEmployeesInOut(long id)
        {
            bool checkEffective;
            var employeesInfo = _amEmployeesRepository.GetAll()
                                            .Select(e => new EmployeesInfoScannerDto
                                            {
                                                EmployeesIOId = e.Id,
                                                IdentityCard = e.IdentityCard,
                                                EmployeesName = e.EmployeesName,
                                                DateStart = e.DateStart.ToString("dd/MM/yyyy"),
                                                DateEnd = e.DateEnd.ToString("dd/MM/yyyy"),
                                                DateEffect = e.DateEnd,
                                                Target = e.Target,
                                                Company = e.Company

                                            }).FirstOrDefault(e => e.EmployeesIOId == id);
            if (employeesInfo == null)
            {
                checkEffective = false;
            }
            else
            {
                if (employeesInfo.DateEffect.Year < DateTime.Now.Year)
                {
                    checkEffective = false;
                }
                else if (employeesInfo.DateEffect.Year == DateTime.Now.Year)
                {
                    if (employeesInfo.DateEffect.Month < DateTime.Now.Month)
                    {
                        checkEffective = false;
                    }
                    else if (employeesInfo.DateEffect.Month == DateTime.Now.Month)
                    {
                        if (employeesInfo.DateEffect.Day < DateTime.Now.Day)
                        {
                            checkEffective = false;
                        }
                        else
                        {
                            checkEffective = true;
                        }
                    }
                    else
                    {
                        checkEffective = true;
                    }
                }
                else
                {
                    checkEffective = true;
                }

            }
            return checkEffective;
        }

        public async Task<PagedResultDto<AssetInOutSelectOutputDto>> LoadAllAssetInOut(RequestAssetInOutInputDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_IO @IsAssetIn, @SeriNumber, @RequestId, @TagCode";

            var assetInOut = await _aioSearchRequestRepository.QueryAsync<AssetInOutSelectOutputDto>(_sql, new
            {
                @IsAssetIn = input.IsRequestAssetIn,
                @SeriNumber = input.SeriNumber,
                @RequestId = input.RequestId,
                @TagCode=input.TagCode
            });

            var result = assetInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetCount = assetInOut.Count();
            return new PagedResultDto<AssetInOutSelectOutputDto>(
                assetCount,
                result.ToList());
        }

       
        public async Task<PagedResultDto<EmployeesInOutSelectOutputDto>> LoadAllEmployeesInOut(RequestAssetInOutInputDto input)
        {
            string _sql = "EXEC P_SEARCH_EMPLOYEES_IO @IsEmployeesIn, @EmployeesName, @IdentityCard, @RequestId";

            var employeesInOut = await _aioSearchRequestRepository.QueryAsync<EmployeesInOutSelectOutputDto>(_sql, new
            {
                @IsEmployeesIn = input.IsRequestAssetIn,
                @EmployeesName = input.EmployeesName,
                @IdentityCard = input.IdentityCard,
                @RequestId = input.RequestId
            });

            var result = employeesInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var employeesCount = employeesInOut.Count();
            return new PagedResultDto<EmployeesInOutSelectOutputDto>(
                employeesCount,
                result.ToList());
        }
        
        
        public async Task<PagedResultDto<RequestAssetInOutSelectOutputDto>> LoadAllRequestAssetInOut(RequestAssetInOutInputDto input)
        {
            string _sql = "EXEC P_SEARCH_REQUEST_IO @IsRequestAssetIn, @VenderId, @RequestCode, @RequestId, @EmployeesName, @IdentityCard, @TagCode, @SeriNumber";

            var requestInOut = await _aioSearchRequestRepository.QueryAsync<RequestAssetInOutSelectOutputDto>(_sql, new
            {
                @IsRequestAssetIn = input.IsRequestAssetIn,
                @VenderId = input.VenderId,
                @RequestCode = input.RequestCode,
                @RequestId = input.RequestId,
                @EmployeesName= input.EmployeesName,
                @IdentityCard = input.IdentityCard,
                @TagCode = input.TagCode,
                @SeriNumber = input.SeriNumber
            });

            var result = requestInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var requestInOutCount = requestInOut.Count();
            return new PagedResultDto<RequestAssetInOutSelectOutputDto>(
                requestInOutCount,
                result.ToList());
        }
    }
}
 