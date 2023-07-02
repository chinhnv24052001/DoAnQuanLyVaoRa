using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;
using tmss.Master.Asset;
using tmss.Master.Vender;
using Castle.Core.Logging;
using tmss.Master.EmployeesLearnedSafety;

namespace tmss.ImportExcel.ImportEmployeesLearnedSafety
{
    public class EmployeesLearnedSafetyImportFromExcel : NpoiExcelImporterBase<EmployeesLearnedSafetyImportDto>, IEmployeesLearnedSafetyImport
    {
        private readonly IRepository<MstVender, long> _venderRepository;
        private readonly IRepository<MstEmployeesLearnedSafety, long> _employeesLearnedSafetyRepository;
        private IWebHostEnvironment _env;
        private IEmployeesLearnedSafetyAppService _iEmployeesLearnedSafetyAppService;
        public ILogger _logger { get; set; }
        public EmployeesLearnedSafetyImportFromExcel(IRepository<MstVender, long> venderRepository, IWebHostEnvironment env, IEmployeesLearnedSafetyAppService iEmployeesLearnedSafetyAppService, IRepository<MstEmployeesLearnedSafety, long> employeesLearnedSafetyRepository)
        {
            _venderRepository = venderRepository;
            _env = env;
            _iEmployeesLearnedSafetyAppService = iEmployeesLearnedSafetyAppService;
            _employeesLearnedSafetyRepository = employeesLearnedSafetyRepository;
        }
        public async Task<List<EmployeesLearnedSafetyImportDto>> GetEmployeesLearnedSafetyFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<EmployeesLearnedSafetyImportDto> rowList = new List<EmployeesLearnedSafetyImportDto>();
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
                            rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));
                        }
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        {
                            sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                            rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));
                        }
                    }
                    // if (rowList.Any()) await _mstCrmCbsTargetAppService.CheckValidateData(rowList);
                }
                return rowList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }

        private List<EmployeesLearnedSafetyImportDto> ReadDataFromExcel(ISheet sheet, string sheetName, string fileName, List<XSSFPictureData> allPics)
        {
            //int countRow = sheet.LastRowNum;
            //string pattern = @"[0-9]{12,15}";
            string phone_regex = @"(09|03|07|08|05)+([0-9]{8})";
            EmployeesLearnedSafetyImportDto importData = new EmployeesLearnedSafetyImportDto();
            List<EmployeesLearnedSafetyImportDto> rowList = new List<EmployeesLearnedSafetyImportDto>();

            var countRow = 1;
            do
            {
                countRow++;
            } while (!(sheet.GetRow(countRow) == null || string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(countRow).GetCell(0)))));

            for (int g = 1; g < countRow-1; g++)
            {
                importData = new EmployeesLearnedSafetyImportDto();
                for (int h = 0; h < 9; h++)
                {
                    switch (h)
                    {
                        case 1:
                            importData.EmployeesName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 2:
                            importData.IdentityCard = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 3:
                            importData.Gender = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 4:
                            importData.PhoneNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 5:
                            importData.Address = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 6:
                            importData.VenderName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))))
                            {
                                var _vender = _venderRepository.GetAll().Where(p => p.VenderName == Convert.ToString(sheet.GetRow(g + 1).GetCell(h))).FirstOrDefault();
                                if (_vender != null)
                                {
                                    importData.VenderId = _vender.Id;
                                }
                                else importData.VenderId = 0;
                            }
                            else importData.VenderId = 0;
                            break;
                        case 7:
                            importData.PersonInCharge = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 8:
                            int idx = getIndexImageFromExcel(fileName, (g + 2));
                            importData.Image = idx > 0 ? Convert.ToBase64String(allPics[idx - 1].Data) : "";
                            break;
                    }
                }

                //Match identityVal = Regex.Match(importData.IdentityCard, pattern);
                Match phoneVal = Regex.Match(importData.PhoneNumber, phone_regex);
                if (importData.EmployeesName == "")
                {
                    importData.Validate = L("EmployeesNameRequired");

                    if (importData.IdentityCard == "" || importData.IdentityCard == null)
                    {
                        importData.Validate += ", " + L("_employeesIdentityVal");
                    }
                    if (!phoneVal.Success)
                    {
                        importData.Validate += ", " + L("_phonenumberVal");
                    }
                    if (importData.VenderId == 0)
                    {
                        importData.Validate += ", " + L("_venderVal");
                    }
                    if (importData.PersonInCharge == "")
                    {
                        importData.Validate += ", " + L("_personInChargeVal");
                    }
                }
                else if (importData.IdentityCard == "" || importData.IdentityCard == null)
                {
                    importData.Validate = L("EmployeesIdentityVal");
                    if (!phoneVal.Success)
                    {
                        importData.Validate += ", " + L("_phonenumberVal");
                    }
                    if (importData.VenderId == 0)
                    {
                        importData.Validate += ", " + L("_venderVal");
                    }
                    if (importData.PersonInCharge == "")
                    {
                        importData.Validate += ", " + L("_personInChargeVal");
                    }
                }
                else if (!phoneVal.Success)
                {
                    importData.Validate = L("PhonenumberVal");
                    if (importData.VenderId == 0)
                    {
                        importData.Validate += ", " + L("_venderVal");
                    }
                    if (importData.PersonInCharge == "")
                    {
                        importData.Validate += ", " + L("_personInChargeVal");
                    }
                }
                else if (importData.VenderId == 0)
                {
                    importData.Validate = L("VenderVal");
                    if (importData.PersonInCharge == "")
                    {
                        importData.Validate += ", " + L("_personInChargeVal");
                    }
                }
                else if (importData.PersonInCharge == "")
                {
                    importData.Validate += ", " + L("PersonInChargeVal");
                }
                rowList.Add(importData);
            }

            return rowList;
        }

        private int getIndexImageFromExcel(string fileName, int row)
        {
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            int indexImg = 0;
            var folderName = Path.Combine("wwwroot", "TempFile");
            var pathToSave = Path.Combine(_env.ContentRootPath, folderName);
            var fullPath = Path.Combine(pathToSave, fileName);
            var excelApp = new Application();                                                                          
            excelApp.Workbooks.Add();

            Workbook workbook = excelApp.Workbooks.Open(fullPath, Type.Missing, Type.Missing,
                              Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                              Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                              Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
            Worksheet worksheet = workbook.Worksheets["Sheet1"];
            foreach (var pic in worksheet.Pictures())
            {
                int startCol = pic.TopLeftCell.Column;
                int startRow = pic.TopLeftCell.Row;
                int endCol = pic.BottomRightCell.Column;
                int endRow = pic.BottomRightCell.Row;
                double leftPic = pic.Left;
                double topPic = pic.Top;
                double heightPic = pic.Height;
                double bottomPic = pic.Height + pic.Top;
                var cell = worksheet.Cells[row, 9];
                double leftCell = cell.Left;
                double topCell = cell.Top;
                double heightCell = cell.Height;
                double bottomCell = cell.Height + cell.Top; 
                if (startRow == row && endRow == row)
                {
                    indexImg = pic.Index;
                } else if (startRow < row && endRow == row)
                {
                    double differenceTop = topCell - topPic;
                    if (heightPic - differenceTop > differenceTop)
                    {
                        indexImg = pic.Index;
                    } else
                    {

                    }
                } else if (startRow == row && endRow > row)
                {
                    double differenceBottom = bottomPic - bottomCell;
                    if (heightPic - differenceBottom > differenceBottom)
                    {
                        indexImg = pic.Index;
                    } else
                    {

                    }
                } else if ((startRow + 1) == row && (endRow - 1 == row))
                {
                    double differenceTop = topCell - topPic;
                    double differenceBottom = bottomPic - bottomCell;
                    if (heightPic - differenceBottom > differenceTop && heightPic - differenceBottom > differenceBottom)
                    {
                        indexImg = pic.Index;
                    } else
                    {

                    }
                }
                
            }
            
            return indexImg;
        }

        public async Task deleteFile(string folder)
        {
            try
            {
                //File.Delete(Path.Combine(folder, fileName));
                System.IO.DirectoryInfo di = new DirectoryInfo(folder);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }

        }

        public async Task<List<long>> SetmployeesLearnedSafetyFromExcel(byte[] fileBytes, string fileName)
        {

            try
            {
                List<EmployeesLearnedSafetyImportDto> rowList = new List<EmployeesLearnedSafetyImportDto>();
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
                            rowList.AddRange(ReadDataSetEmployeesFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));

                        }
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        {
                            sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                            rowList.AddRange(ReadDataSetEmployeesFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));
                        }
                    }
                }
                List<long> list = new List<long>();
                foreach (var e in rowList)
                {
                    var employees = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(p => p.IdentityCard == e.IdentityCard);
                    if(employees!=null)
                    {
                        list.Add(employees.Id);
                    }
                }
                
                return list;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }


        private List<EmployeesLearnedSafetyImportDto> ReadDataSetEmployeesFromExcel(ISheet sheet, string sheetName, string fileName, List<XSSFPictureData> allPics)
        {
            int countRow1 = sheet.LastRowNum;
            EmployeesLearnedSafetyImportDto importData = new EmployeesLearnedSafetyImportDto();
            List<EmployeesLearnedSafetyImportDto> rowList = new List<EmployeesLearnedSafetyImportDto>();

            for (int g = 1; g < countRow1; g++)
            {
                importData = new EmployeesLearnedSafetyImportDto();
                for (int h = 0; h < 3; h++)
                {
                    switch (h)
                    {
                        case 1:
                            importData.EmployeesName = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                        case 2:
                            importData.IdentityCard = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            break;
                    }
                }
                rowList.Add(importData);
            }

            return rowList;
        }
    }
}



