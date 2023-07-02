using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;
using tmss.ImportExcel.ImportEmployeesLearnedSafety;
using tmss.ImportExcel.ImportMstAsset.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using tmss.Master.EmployeesLearnedSafety;
using tmss.Master.Vender;
using Abp.UI;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text.RegularExpressions;
using Castle.Core.Logging;
using tmss.Master.AssetGroup;
using tmss.Master.Asset;

namespace tmss.ImportExcel.ImportMstAsset
{
    public class MstAssetImportFromExxcel : NpoiExcelImporterBase<MstAssetImportDto>, IMstAssetImport
    {
        private readonly IRepository<MstAssetGroup, long> _assetGroupRepository;
        private readonly IRepository<MstAsset, long> _assetRepository;
        private IWebHostEnvironment _env;
        private IEmployeesLearnedSafetyAppService _iEmployeesLearnedSafetyAppService;
        public ILogger _logger { get; set; }

        public MstAssetImportFromExxcel(IRepository<MstVender, long> venderRepository,
            IWebHostEnvironment env,
            IEmployeesLearnedSafetyAppService iEmployeesLearnedSafetyAppService,
            IRepository<MstAssetGroup, long> assetGroupRepository,
            IRepository<MstAsset, long> assetRepository,
            IRepository<MstEmployeesLearnedSafety, long> employeesLearnedSafetyRepository)
        {
            _env = env;
            _iEmployeesLearnedSafetyAppService = iEmployeesLearnedSafetyAppService;
            _assetGroupRepository = assetGroupRepository;
            _assetRepository = assetRepository;
        }

        public async Task<List<MstAssetImportDto>> GetAssetFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstAssetImportDto> rowList = new List<MstAssetImportDto>();
                ISheet sheet;
                using (var stream = new MemoryStream(fileBytes))
                {
                    stream.Position = 0;
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format

                        for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        {
                            sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                            rowList.AddRange(ReadDataFromExcel(sheet));
                        }
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        {
                            sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                            rowList.AddRange(ReadDataFromExcel(sheet));
                        }
                    }
                }
                return rowList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }

        private List<MstAssetImportDto> ReadDataFromExcel(ISheet sheet)
        {
            MstAssetImportDto importData = new MstAssetImportDto();
            List<MstAssetImportDto> rowList = new List<MstAssetImportDto>();

            var countRow = 1;
            do
            {
                countRow++;
            } while (!(sheet.GetRow(countRow) == null || string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(countRow).GetCell(0)))));

            for (int g = 1; g < countRow - 1; g++)
            {
                importData = new MstAssetImportDto();
                for (int h = 0; h < 3; h++)
                {
                    switch (h)
                    {
                        case 1:
                            importData.AssetName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            if (importData.AssetName == "")
                            {
                                importData.Validate = L("AssetInvalid") + ", "; 
                            }
                            break;
                        case 2:
                            importData.AssetGroupName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))))
                            {
                                var _assetGroup = _assetGroupRepository.GetAll().Where(p => p.AssetGroupName == Convert.ToString(sheet.GetRow(g + 1).GetCell(h))).FirstOrDefault();
                                if (_assetGroup != null)
                                {
                                    importData.AssetGroupId = _assetGroup.Id;
                                }
                                else
                                {
                                    importData.AssetGroupId = 0;
                                    importData.Validate +=  L("AssetGroupInvalid");
                                }
                            }
                            else
                            {
                                importData.AssetGroupId = 0;
                                importData.Validate +=  L("AssetGroupRequired");
                            }
                            break;
                    }
                }
                rowList.Add(importData);
            }

            return rowList;
        }

    }
}
