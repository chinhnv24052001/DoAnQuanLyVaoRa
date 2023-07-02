using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Report.Dto;
using tmss.Storage;

namespace tmss.Report.ExportToFile
{
    public class ExportToExcel : NpoiExcelExporterBase, IExportToExcel
    {
        public ExportToExcel(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportListWorkerInOutAtDateToFile(List<WorkerInOutAtIdentity > workerInOutAtDateDto)
        {
            return CreateExcelPackage(
                "ListWorkerInOutAtDate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ListWorkerInOutAtDate");

                    AddHeader(
                        sheet,
                        L("STT"),
                        L("WorkerName"),
                        L("IdentityCard"),
                        L("InDateTime"),
                        L("OutDateTime")
                       
                    );

                    AddObjects(
                        sheet, 2, workerInOutAtDateDto,
                        _ => _.Stt, 
                        _ => _.WorkerName,
                        _ => _.IdentityCard,
                        _ => _.InDateTime,
                        _ => _.OutDateTime
                       
                        );

                    for (var i = 1; i <= workerInOutAtDateDto.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd hh:mm:ss");
                        var cell = sheet.GetRow(i).CreateCell(0);
                        cell.SetCellValue(i.ToString());
                    }
                    

                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
        
        public FileDto ExportListWorkerInOutAtIdentityToFile(List<WorkerInOutAtDateDto> workerInOutAtDateDto)
        {
            return CreateExcelPackage(
                "ListWorkerInOutAtDate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ListWorkerInOutAtDate");

                    AddHeader(
                        sheet,
                        L("STT"),
                        L("WorkerName"),
                        L("IdentityCard"),
                         L("StarDate"),
                        L("EndDate")
                    );

                    AddObjects(
                        sheet, 2, workerInOutAtDateDto,
                        _ => _.Stt, 
                        _ => _.WorkerName,
                        _ => _.IdentityCard,
                         _ => _.StarDate,
                        _ => _.EndDate
                        );

                    for (var i = 1; i <= workerInOutAtDateDto.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd hh:mm:ss");
                        var cell = sheet.GetRow(i).CreateCell(0);
                        cell.SetCellValue(i.ToString());
                    }
                    

                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }

        public FileDto ExportListAssetInOutAtSeriNumberToFile(List<ListAssetInOutAtSeriNumberDto> assetInOutAtSeriNumberDto)
        {
            return CreateExcelPackage(
                "ListAssetInOutAtSeriNumber.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ListAssetInOutAtSeriNumber");

                    AddHeader(
                        sheet,
                        L("STT"),
                        L("AssetName"),
                        L("TagCode"),
                        L("InDateTime"),
                        L("OutDateTime")
                    );

                    AddObjects(
                        sheet, 2, assetInOutAtSeriNumberDto,
                        _ => _.Stt,
                        _ => _.AssetName,
                        _ => _.TagCode,
                        _ => _.InDateTime,
                        _ => _.OutDateTime
                        );

                    for (var i = 1; i <= assetInOutAtSeriNumberDto.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd hh:mm:ss");
                        var cell = sheet.GetRow(i).CreateCell(0);
                        cell.SetCellValue(i.ToString());
                    }


                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        public FileDto ExportListAssetOutOfDateToFile(List<AssetOutOfDateSelectOutPutDto> assetInOutAtSeriNumberDto)
        {
            return CreateExcelPackage(
                "ListAssetOutOfDate.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ListAssetOutOfDate");

                    AddHeader(
                        sheet,
                        L("STT"),
                        L("AssetName"),
                        L("SeriNumber"),
                        L("TagCode"),
                        L("RequestCode"),
                        L("Total"),
                        L("StarDate"),
                        L("EndDate")
                    );

                    AddObjects(
                        sheet, 2, assetInOutAtSeriNumberDto,
                        _ => _.Stt,
                        _ => _.AssetName,
                        _ => _.SeriNumber,
                        _ => _.TagCode,
                        _ => _.RequestCode,
                        _ => _.Total,
                        _ => _.StarDate,
                        _ => _.EndDate
                        );

                    for (var i = 1; i <= assetInOutAtSeriNumberDto.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd hh:mm:ss");
                        SetCellDataFormat(sheet.GetRow(i).Cells[7], "yyyy-mm-dd hh:mm:ss");
                        var cell = sheet.GetRow(i).CreateCell(0);
                        cell.SetCellValue(i.ToString());
                    }


                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
