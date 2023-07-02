using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportWorkerReport;
using tmss.ImportExcel.ImportWorkerReport.Dto;

namespace tmss.ImportExcel.ImportWorkerRequest
{
    public class WorkerRequestImportFromExcel : NpoiExcelImporterBase<WorkerRequestImportDto>, IWorkerRequestImport
    {
        public async Task<List<WorkerRequestImportDto>> GetWorkerRequestFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<WorkerRequestImportDto> rowList = new List<WorkerRequestImportDto>();
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
                            rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index)));
                        }
                    }
                    else
                    {
                        HSSFWorkbook xssfwb = new HSSFWorkbook(stream); //This will read 2003 Excel fromat
                        for (var index = 0; index < xssfwb.NumberOfSheets; index++)
                        {
                            sheet = xssfwb.GetSheetAt(index); //get first sheet from workbook 
                            rowList.AddRange(ReadDataFromExcel(sheet, xssfwb.GetSheetName(index)));
                        }
                    }
                }
                return rowList;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(00, "File tải lên không hợp lệ");
            }
        }


        private List<WorkerRequestImportDto> ReadDataFromExcel(ISheet sheet, string sheetName)
        {
            int countRow = sheet.LastRowNum;

            WorkerRequestImportDto importData = new WorkerRequestImportDto();
            List<WorkerRequestImportDto> rowList = new List<WorkerRequestImportDto>();

            for (int g = 1; g < countRow; g++)
            {
                importData = new WorkerRequestImportDto();
                for (int h = 0; h < 5; h++)
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
                            importData.DateStart = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : new DateTime();
                            break;
                        case 4:
                            importData.DateEnd = !string.IsNullOrWhiteSpace(Convert.ToString(sheet.GetRow(g + 1).GetCell(h))) ? DateTime.ParseExact(Convert.ToString(sheet.GetRow(g + 1).GetCell(h)), "d/M/yyyy", null) : new DateTime();
                            break;
                    }
                }

                rowList.Add(importData);
            }

            return rowList;
        }
    }
}
