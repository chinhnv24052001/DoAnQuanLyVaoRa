using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Report.Dto;

namespace tmss.Report.ExportToFile
{
    public interface IExportToExcel
    {
        FileDto ExportListWorkerInOutAtDateToFile(List< WorkerInOutAtIdentity> workerInOutAtDateDto);
        FileDto ExportListWorkerInOutAtIdentityToFile(List<WorkerInOutAtDateDto> workerInOutAtDateDto);

        FileDto ExportListAssetInOutAtSeriNumberToFile(List<ListAssetInOutAtSeriNumberDto> assetInOutAtSeriNumberDto);
        FileDto ExportListAssetOutOfDateToFile(List<AssetOutOfDateSelectOutPutDto> assetInOutAtSeriNumberDto);
    }
}
