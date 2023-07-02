using Abp.Domain.Repositories;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Office.Interop.Excel;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportAssetImport;
using tmss.ImportExcel.ImportAssetImport.Dto;
using tmss.Master.Asset;

namespace tmss.ImportExcel.ImportAssetRequest
{
    public class AssetRequestImportFromExcel : NpoiExcelImporterBase<AssetRequestImportDto>, IAssetRequestImport
    {
        private readonly IRepository<MstAsset, long> _assetRepository;
        private IWebHostEnvironment _env;
        public ILogger _logger { get; set; }
        public AssetRequestImportFromExcel(IWebHostEnvironment env, IRepository<MstAsset, long> assetRepository)
        {
            _assetRepository = assetRepository;
            _env=env;
        }
        public async Task<List<AssetRequestImportDto>> GetAssetRequestFromExcel(byte[] fileBytes, string fileName, string check)
        {
            try
            {
                List<AssetRequestImportDto> rowList = new List<AssetRequestImportDto>();
                ISheet sheet;
                using (var stream = new MemoryStream(fileBytes))
                {
                    stream.Position = 0;
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format
                        sheet = xssfwb.GetSheetAt(0); //get first sheet from workbook 
                        rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(0), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures(), check));

                        //for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        //{
                        //    sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                        //    rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));
                        //}
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        sheet = xssfwb.GetSheetAt(0); //get first sheet from workbook 
                        rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(0), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures(), check));

                        //for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        //{
                        //    sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                        //    rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index), fileName, (List<XSSFPictureData>)xssfwb.GetAllPictures()));
                        //}
                    }
                }

               
                return rowList;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }

        private  List<AssetRequestImportDto> ReadDataFromExcel(ISheet sheet, string sheetName, string fileName, List<XSSFPictureData> allPics, string check)
        {
            AssetRequestImportDto importData = new AssetRequestImportDto();
            List<AssetRequestImportDto> rowList = new List<AssetRequestImportDto>();

            var countRow = 1;
            do
            {
                countRow++;
            } while (!(sheet.GetRow(countRow) == null || string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(countRow).GetCell(0)))));

            if(check == "IN")
            {
                for (int g = 1; g < countRow - 1; g++)
                {
                    importData = new AssetRequestImportDto();
                    for (int h = 0; h < 8; h++)
                    {
                        switch (h)
                        {
                            case 1:
                                if (!string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))))
                                {
                                    var _asset = _assetRepository.GetAll().Where(p => p.AssetName == Convert.ToString(sheet.GetRow(g + 1).GetCell(h))).FirstOrDefault();
                                    if (_asset != null)
                                    {
                                        importData.AssetId = _asset.Id;
                                    }
                                    else importData.AssetId = 0;
                                }
                                else importData.AssetId = 0;
                                break;
                            //case 2:
                            //    importData.SeriNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            //    break;
                            case 2:
                                importData.TagCode = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 3:
                                importData.Total = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Int32.Parse((Convert.ToString(sheet.GetRow(g + 1).GetCell(h)))) : 0;
                                break;
                            case 4:
                                importData.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : null;
                                break;
                            case 5:
                                importData.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : null;
                                break;
                            case 6:
                                importData.AviationIsBack = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) == "0" ? true : false : false;
                                break;
                            case 7:
                                //int idx = getIndexImageFromExcel(fileName, (g + 2));
                                //importData.assetImage = idx > 0 ? Convert.ToBase64String(allPics[idx - 1].Data) : "";
                                importData.assetImage = "";
                                break;
                        }
                    }

                    rowList.Add(importData);
                }
            }
            else
            {
                for (int g = 1; g < countRow - 1; g++)
                {
                    importData = new AssetRequestImportDto();
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
                                        importData.AssetId = _asset.Id;
                                    }
                                    else importData.AssetId = 0;
                                }
                                else importData.AssetId = 0;
                                break;
                            //case 2:
                            //    importData.SeriNumber = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                            //    break;
                            case 2:
                                importData.TagCode = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Convert.ToString(sheet.GetRow(g + 1).GetCell(h)) : "";
                                break;
                            case 3:
                                importData.Total = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? Int32.Parse((Convert.ToString(sheet.GetRow(g + 1).GetCell(h)))) : 0;
                                break;
                            case 4:
                                importData.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : null;
                                break;
                            case 5:
                                importData.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : null;
                                break;
                            case 6:
                                //int idx = getIndexImageFromExcel(fileName, (g + 2));
                                //importData.assetImage = idx > 0 ? Convert.ToBase64String(allPics[idx - 1].Data) : "";
                                importData.assetImage = "";
                                break;
                        }
                    }

                    rowList.Add(importData);
                }
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
                }
                else if (startRow < row && endRow == row)
                {
                    double differenceTop = topCell - topPic;
                    if (heightPic - differenceTop > differenceTop)
                    {
                        indexImg = pic.Index;
                    }
                    else
                    {

                    }
                }
                else if (startRow == row && endRow > row)
                {
                    double differenceBottom = bottomPic - bottomCell;
                    if (heightPic - differenceBottom > differenceBottom)
                    {
                        indexImg = pic.Index;
                    }
                    else
                    {

                    }
                }
                else if ((startRow + 1) == row && (endRow - 1 == row))
                {
                    double differenceTop = topCell - topPic;
                    double differenceBottom = bottomPic - bottomCell;
                    if (heightPic - differenceBottom > differenceTop && heightPic - differenceBottom > differenceBottom)
                    {
                        indexImg = pic.Index;
                    }
                    else
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
    }
}
