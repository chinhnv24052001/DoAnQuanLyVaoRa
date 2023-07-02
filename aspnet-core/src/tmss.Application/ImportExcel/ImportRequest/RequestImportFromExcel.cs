using Abp.Domain.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportAssetImport.Dto;
using tmss.ImportExcel.ImportAssetImport;
using tmss.Master.Asset;
using tmss.ImportExcel.ImportRequest.Dto;
using tmss.AssetManaments.RequestAssetBring.Dto;
using tmss.Master.Vender;
using tmss.Migrations;
using System.Globalization;
using static NPOI.HSSF.Util.HSSFColor;
using tmss.ImportExcel.ImportEmployeesLearnedSafety;
using tmss.AssetManaments;
using tmss.Authorization.Users;
using Abp.Runtime.Session;

namespace tmss.ImportExcel.ImportRequest
{
    public class RequestImportFromExcel : NpoiExcelImporterBase<AssetRequestImportDto>, IRequestImport
    {
        private readonly IRepository<MstAsset, long> _assetRepository;
        private readonly IRepository<MstVender, long> _venderRepository;
        private readonly IRequestAssetBringAppService _iRequestAssetBringAppService;
        private readonly IRepository<User, long> _userRepository;

        public RequestImportFromExcel(IRepository<MstAsset, long> assetRepository, IRepository<MstVender, long> venderRepository, IRequestAssetBringAppService iRequestAssetBringAppService, IRepository<User, long> userRepository)
        {
            _assetRepository = assetRepository;
            _venderRepository = venderRepository;
            _iRequestAssetBringAppService = iRequestAssetBringAppService;
            _userRepository = userRepository;
        }
        public async Task<RequestImportDto> GetClientRequestFromExcel(byte[] fileBytes, string fileName, int typeRequest, long userId)
        {
            try
            {
                RequestImportDto rowList = new RequestImportDto();
                ISheet sheet;
                using (var stream = new MemoryStream(fileBytes))
                {
                    stream.Position = 0;
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                        sheet = xssfwb.GetSheetAt(0); //get first sheet from workbook 
                        rowList = await ReadDataFromExcel(sheet, xssfwb.GetSheetName(0), typeRequest, userId);
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        sheet = xssfwb.GetSheetAt(0); //get first sheet from workbook 
                        rowList = await ReadDataFromExcel(sheet, xssfwb.GetSheetName(0), typeRequest, userId);
                    }
                }
                return rowList;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }

        private async Task<RequestImportDto> ReadDataFromExcel(ISheet sheet, string sheetName, int typeRequet, long userId)
        {
            int countRow = sheet.LastRowNum;

            RequestImportDto importData = new RequestImportDto();
            List<AioAssetDto> listAsset = new List<AioAssetDto>();
            List<AioEmployeesDto> listEmployees = new List<AioEmployeesDto>();
            AioAssetDto rowAsset = new AioAssetDto();
            AioEmployeesDto rowEmployees = new AioEmployeesDto();
            var user = await _userRepository.FirstOrDefaultAsync(userId);
            //var typeRequet = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(3).GetCell(2))) ? Convert.ToString(sheet.GetRow(3).GetCell(2)) == AppConsts.INTERNAL_RQ1? 1: Convert.ToString(sheet.GetRow(3).GetCell(2)) == AppConsts.VENDER_EMPLOYEES_RQ2 ? 2 : Convert.ToString(sheet.GetRow(3).GetCell(2)) == AppConsts.VENDER_ASSET_RQ3 ? 3: Convert.ToString(sheet.GetRow(3).GetCell(2)) == AppConsts.CLIENT_RQ4 ? 4 : 0 : 0;
            if (typeRequet == 1)
            {
                importData.TypeRequest = typeRequet;
                importData.Title = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(4).GetCell(2))) ? Convert.ToString(sheet.GetRow(4).GetCell(2)) : "";
                importData.DateRequest = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(5).GetCell(2))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(5).GetCell(2)), "d/M/yyyy", null) : new DateTime();
                importData.Description = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(6).GetCell(2))) ? Convert.ToString(sheet.GetRow(6).GetCell(2)) : "";
                importData.UserName = user.UserName;
                importData.Department = user.Department;
                for (int g = 9; g < countRow; g++)
                {
                    rowAsset = new AioAssetDto();
                    for (int h = 0; h < 7; h++)
                    {
                        switch (h)
                        {
                            case 1:
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))))
                                {
                                    var _asset = _assetRepository.GetAll().Where(p => p.AssetName == Convert.ToString(sheet.GetRow(g + 1).GetCell(h))).FirstOrDefault();
                                    if (_asset != null)
                                    {
                                        rowAsset.AssetId = _asset.Id;
                                    }
                                    else rowAsset.AssetId = 0;
                                }
                                else rowAsset.AssetId = 0;
                                break;
                            case 2:
                                rowAsset.SeriNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 3:
                                rowAsset.TagCode = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 4:
                                rowAsset.Total = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Int32.Parse((Convert.ToString(sheet.GetRow(g + 1).GetCell(h)))) : 0;
                                break;
                            case 5:
                                rowAsset.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/MM/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                            case 6:
                                rowAsset.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/MM/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                        }
                    }
                    listAsset.Add(rowAsset);
                }
                importData.AssetList = listAsset;
            }
            else if (typeRequet == 2)
            {
                importData.TypeRequest = typeRequet;
                bool IsEnterVender=false;
                for (int i = 2; i < 5; i++)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(5).GetCell(i))))
                    {
                        IsEnterVender = true;
                        var _vender = _venderRepository.GetAll().Where(p => p.VenderName == Convert.ToString(sheet.GetRow(5).GetCell(2))).FirstOrDefault();
                        if (_vender != null)
                        {
                            importData.VenderId = _vender.Id;
                            importData.VenderAddress = _vender.Address;
                            importData.VenderPhoneNumber = _vender.PhoneNumber;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(6).GetCell(i))))
                    {
                        importData.PersonInChargeOfSubName = Convert.ToString(sheet.GetRow(6).GetCell(i));
                        var x = Convert.ToString(sheet.GetRow(6).GetCell(2));
                    }

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(7).GetCell(i))))
                    {
                        importData.Title = Convert.ToString(sheet.GetRow(7).GetCell(i));
                    }

                    //if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(8).GetCell(i))))
                    //{
                    //    importData.DateRequest = DateTime.ParseExact(Convert.ToString(sheet.GetRow(8).GetCell(2)), "dd/MM/yyyy", null);
                    //}

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(9).GetCell(i))))
                    {
                        importData.WhereToBring = Convert.ToString(sheet.GetRow(9).GetCell(i));
                    }
                }

                importData.PersonInChangeOfSubPhone = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(6).GetCell(6))) ? Convert.ToString(sheet.GetRow(6).GetCell(6)) : "";
                importData.LiveMonitorName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(10).GetCell(2))) ? Convert.ToString(sheet.GetRow(10).GetCell(2)) : "";
                importData.LiveMonitorPosition = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(10).GetCell(4))) ? Convert.ToString(sheet.GetRow(10).GetCell(4)) : "";

                importData.LiveMonitorPhoneNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(11).GetCell(4))) ? Convert.ToString(sheet.GetRow(11).GetCell(4)) : "";
                importData.LiveMonitorDepartment = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(11).GetCell(2))) ? Convert.ToString(sheet.GetRow(11).GetCell(2)) : "";
                importData.UserName = user.UserName;
                importData.Department = user.Department;
                var date = new DateTime();

                //Check StatusWanning
                importData.StatusWarning = true;
                List<ErrorImportDto> listErr =new List<ErrorImportDto>();
                ErrorImportDto err = new ErrorImportDto();
                if (importData.Title == "" || importData.Title == null)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_TITLE_REQUEST;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.VenderId == 0)
                {
                    err = new ErrorImportDto();
                    if (IsEnterVender)
                    {
                        err.ColumnErr = AppConsts.COL_ERR_VENDER_NAME;
                        err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    }
                    else
                    {
                        err.ColumnErr = AppConsts.COL_ERR_VENDER_NAME;
                        err.MesageErr = AppConsts.INPUT_IS_INCORECT;
                    }
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.PersonInChargeOfSubName == "" || importData.PersonInChargeOfSubName == null)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_PERSION_INCHANGE_OF_SUB;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " person in charge of sub,";
                    importData.StatusWarning = false;
                }
                if (importData.LiveMonitorName == "" || importData.LiveMonitorName == null )
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_LIVE_MONITOR_NAME;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " live monitor name,";
                    importData.StatusWarning = false;
                }
                if (importData.LiveMonitorDepartment == "" || importData.LiveMonitorDepartment == null)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_LIVE_MONITOR_DEPARTMENT;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " live monitor department,";
                    importData.StatusWarning = false;
                }
                if (importData.WhereToBring == "" || importData.WhereToBring == null)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_WHERE_TO_WORK;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " where to work";
                    importData.StatusWarning = false;
                }

                var startEmployeesRow = 12;
                do
                {
                    startEmployeesRow++;
                } while (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(startEmployeesRow).GetCell(1))));

                for (int g = 13; g < startEmployeesRow - 1; g++)
                {
                    var valListEmployees = true;
                    rowEmployees = new AioEmployeesDto();
                    for (int h = 0; h < 5; h++)
                    {

                        switch (h)
                        {
                            case 1:
                                rowEmployees.EmployeesName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 2:
                                rowEmployees.IdentityCard = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                var checkEmployees = await _iRequestAssetBringAppService.CheckWorkerLearnedSafety(rowEmployees.IdentityCard);
                                if (checkEmployees == false)
                                {
                                    rowEmployees.IdentityVal = 1;
                                }
                                break;
                            case 3:
                                rowEmployees.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                            case 4:
                                rowEmployees.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                        }
                    }
                    if (rowEmployees.EmployeesName == "" || rowEmployees.IdentityCard == "" || rowEmployees.DateStart == date || rowEmployees.DateEnd == date || rowEmployees.IdentityVal == 1)
                    {
                        valListEmployees = false;
                        //importData.StatusWarning = false;
                    }
                    if (!valListEmployees)
                    {
                        err = new ErrorImportDto();
                        err.ColumnErr = AppConsts.COL_ERR_LIST_WORKER;
                        err.MesageErr = "Dòng "+(g-12).ToString();
                        listErr.Add(err);
                        importData.StatusWarning = false;
                        //importData.WarningMessage += " list employees";
                    }
                    listEmployees.Add(rowEmployees);
                }
               
                importData.WorkersList = listEmployees;
                importData.ErrorImportList = listErr;
            }
            else if (typeRequet == 3)
            {
                importData.TypeRequest = typeRequet;

                importData.Title = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(21).GetCell(5))) ? Convert.ToString(sheet.GetRow(21).GetCell(5)) : "";
                bool IsEnterVender = false;
                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(8).GetCell(6))))
                {
                    IsEnterVender = true;
                    var _vender = _venderRepository.GetAll().Where(p => p.VenderName == Convert.ToString(sheet.GetRow(8).GetCell(6))).FirstOrDefault();
                    if (_vender != null)
                    {
                        importData.VenderId = _vender.Id;
                    }
                    else importData.VenderId = 0;
                }
                else importData.VenderId = 0;

                //for (int i = 14; i < 24; i++)
                //{
                //    var x = Convert.ToString(sheet.GetRow(9).GetCell(i));

                //}
                //importData.DateRequest = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(10).GetCell(8))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(10).GetCell(8)), "dd/MM/yyyy", null) : new DateTime();
                importData.LiveMonitorName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(9).GetCell(6))) ? Convert.ToString(sheet.GetRow(9).GetCell(6)) : "";
                importData.LiveMonitorPhoneNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(9).GetCell(18))) ? Convert.ToString(sheet.GetRow(9).GetCell(18)) : "";
                importData.PersonInChargeOfSubName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(7).GetCell(6))) ? Convert.ToString(sheet.GetRow(7).GetCell(6)) : "";
                importData.PersonInChangeOfSubPhone = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(7).GetCell(18))) ? Convert.ToString(sheet.GetRow(7).GetCell(18)) : "";

                importData.LiveMonitorDepartment = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(10).GetCell(6))) ? Convert.ToString(sheet.GetRow(10).GetCell(6)) : "";
                importData.WhereToBring = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(20).GetCell(5))) ? Convert.ToString(sheet.GetRow(20).GetCell(5)) : "";
                importData.Description = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(11).GetCell(2))) ? Convert.ToString(sheet.GetRow(11).GetCell(2)) : "";
                var DateFrom = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(22).GetCell(15))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(22).GetCell(15)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                var DateTo = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(22).GetCell(21))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(22).GetCell(21)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                importData.UserName = user.UserName;
                importData.Department = user.Department;

                var date = new DateTime();
                //Check StatusWanning
                List<ErrorImportDto> listErr = new List<ErrorImportDto>();
                ErrorImportDto err = new ErrorImportDto();
                importData.StatusWarning = true;
                if (importData.Title == "" || importData.Title ==null)
                {
                    //importData.WarningMessage += " purpose,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_TITLE_REQUEST;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.VenderId == 0)
                {
                    err = new ErrorImportDto();
                    if (IsEnterVender)
                    {
                        err.ColumnErr = AppConsts.COL_ERR_VENDER_NAME;
                        err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    }
                    else
                    {
                        err.ColumnErr = AppConsts.COL_ERR_VENDER_NAME;
                        err.MesageErr = AppConsts.INPUT_IS_INCORECT;
                    }
                    listErr.Add(err);
                    //importData.WarningMessage += " vender,";
                    importData.StatusWarning = false;
                }
                if (importData.LiveMonitorName == "" || importData.LiveMonitorName == null)
                {
                    //importData.WarningMessage += " live monitor name,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_LIVE_MONITOR_NAME;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.LiveMonitorDepartment == "" || importData.LiveMonitorDepartment== null)
                {
                    //importData.WarningMessage += " live monitor department,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_LIVE_MONITOR_DEPARTMENT;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.WhereToBring == "" || importData.WhereToBring== null)
                {
                    //importData.WarningMessage += " where to bring,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_WHERE_TO_BRING;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (DateFrom == date)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_START_DATE;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " start date,";
                    importData.StatusWarning = false;
                }
                if (DateTo == date)
                {
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_END_DATE;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " end date,";
                    importData.StatusWarning = false;
                }

                var startEmployeesRow = 12;
                do
                {
                    startEmployeesRow++;
                } while (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(startEmployeesRow).GetCell(6))));

                for (int g = 13; g < startEmployeesRow - 1; g++)
                {
                    rowAsset = new AioAssetDto();
                    var valAsset = true;
                    for (int h = 0; h <= 17; h++)
                    {
                        switch (h)
                        {
                            case 6:
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))))
                                {
                                    var _asset = _assetRepository.GetAll().Where(p => p.AssetName == Convert.ToString(sheet.GetRow(g + 1).GetCell(h))).FirstOrDefault();
                                    if (_asset != null)
                                    {
                                        rowAsset.AssetId = _asset.Id;
                                    }
                                    else rowAsset.AssetId = 0;
                                }
                                else rowAsset.AssetId = 0;
                                break;
                            case 11:
                                rowAsset.TagCode = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 17:
                                rowAsset.Total = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Int32.Parse((Convert.ToString(sheet.GetRow(g + 1).GetCell(h)))) : 0;
                                break;
                                //case 4:
                                //    rowAsset.Total = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Int32.Parse((Convert.ToString(sheet.GetRow(g + 1).GetCell(h)))) : 0;
                                //    break;
                                //case 5:
                                //    rowAsset.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : new DateTime();
                                //    break;
                                //case 6:
                                //    rowAsset.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : new DateTime();
                                //    break;
                        }
                        rowAsset.DateStart = DateFrom;
                        rowAsset.DateEnd = DateTo;
                       
                    }
                    if (rowAsset.AssetId == 0 || rowAsset.TagCode == "" || rowAsset.Total==0)
                    {
                        valAsset = false;
                        //importData.StatusWarning = false;
                    }
                    if (!valAsset)
                    {
                        err = new ErrorImportDto();
                        err.ColumnErr = AppConsts.COL_ERR_LIST_ASSET;
                        err.MesageErr = "Dòng " + (g - 12).ToString();
                        listErr.Add(err);
                        importData.StatusWarning = false;
                        //importData.WarningMessage += " list employees";
                    }
                    listAsset.Add(rowAsset);
                    importData.ErrorImportList = listErr;
                }
                //if (!valAsset)
                //{
                //    importData.StatusWarning = false;
                //    importData.WarningMessage += " list asset";
                //}
                importData.AssetList = listAsset;
            }

            else if (typeRequet == 4)
            {
                importData.TypeRequest = typeRequet;
                importData.Title = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(9).GetCell(1))) ? Convert.ToString(sheet.GetRow(9).GetCell(1)) : "";
                //importData.DateRequest = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(9).GetCell(1))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(9).GetCell(1)), "dd/MM/yyyy", null) : new DateTime();
                importData.TradeUnionOrganization = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(6).GetCell(1))) ? Convert.ToString(sheet.GetRow(6).GetCell(1)) : "";
                importData.DepartmentClient = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(7).GetCell(1))) ? Convert.ToString(sheet.GetRow(7).GetCell(1)) : "";
                importData.Description = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(10).GetCell(1))) ? Convert.ToString(sheet.GetRow(10).GetCell(1)) : "";

                importData.PersonInChargeOfSubName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(8).GetCell(1))) ? Convert.ToString(sheet.GetRow(8).GetCell(1)) : "";
                importData.PersonInChangeOfSubPhone = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(8).GetCell(3))) ? Convert.ToString(sheet.GetRow(8).GetCell(3)) : "";

                importData.UserName = user.UserName;
                importData.Department = user.Department;

                if (importData.Title == "" || importData.TradeUnionOrganization == "" || importData.DepartmentClient == "")
                {
                    importData.StatusWarning = false;
                }
                else importData.StatusWarning = true;

                //check val
                var date = new DateTime();
                List<ErrorImportDto> listErr = new List<ErrorImportDto>();
                ErrorImportDto err = new ErrorImportDto();
                importData.StatusWarning = true;
                if (importData.Title == "" || importData.Title == null)
                {
                    //importData.WarningMessage += " purpose,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_TITLE_REQUEST;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.TradeUnionOrganization == "" || importData.TradeUnionOrganization== null)
                {
                    //importData.WarningMessage += " trade union organization,";
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_TRADE_UNION_ORGANAGATION;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    importData.StatusWarning = false;
                }
                if (importData.DepartmentClient == "" || importData.DepartmentClient == null)
                {
                    
                    err = new ErrorImportDto();
                    err.ColumnErr = AppConsts.COL_ERR_DEPARTMENT_CLIENT;
                    err.MesageErr = AppConsts.YOU_HAVE_NOT_ENTERED_THE_VALUE;
                    listErr.Add(err);
                    //importData.WarningMessage += " department client,";
                    importData.StatusWarning = false;
                }

                var startClientRow = 13;
                do
                {
                    startClientRow++;
                } while (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(startClientRow).GetCell(0))));

                
                for (int g = 13; g < startClientRow-1; g++)
                {
                    var valListEmployees = true;
                    rowEmployees = new AioEmployeesDto();
                    for (int h = 0; h < 4; h++)
                    {
                        switch (h)
                        {
                            case 0:
                                rowEmployees.EmployeesName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 1:
                                rowEmployees.IdentityCard = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 2:
                                rowEmployees.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                            case 3:
                                rowEmployees.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", CultureInfo.InvariantCulture) : new DateTime();
                                break;
                        }
                    }
                    if(rowEmployees.EmployeesName=="" || rowEmployees.IdentityCard=="" || rowEmployees.DateStart== date || rowEmployees.DateEnd== date)
                    {
                        valListEmployees = false;

                    }
                    if (!valListEmployees)
                    {
                        err = new ErrorImportDto();
                        err.ColumnErr = AppConsts.COL_ERR_LIST_CLIENT;
                        err.MesageErr = "Dòng " + (g - 12).ToString();
                        listErr.Add(err);
                        importData.StatusWarning = false;
                        //importData.WarningMessage += " list employees";
                    }
                    listEmployees.Add(rowEmployees);
                }
                //if(!valListEmployees)
                //{
                //    importData.StatusWarning = false;
                //    importData.WarningMessage += " list client";
                //}
                importData.WorkersList = listEmployees;
                importData.ErrorImportList = listErr;
            }
            else
            {
                return importData;
            }

            return importData;
        }
    }
}
