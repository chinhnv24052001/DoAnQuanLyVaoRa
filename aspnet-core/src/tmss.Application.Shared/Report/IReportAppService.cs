using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Dto;
using tmss.Report.Dto;

namespace tmss.Report
{
    public interface IReportAppService : IApplicationService
    {
        Task<FileDto> GetWorkerInOutAtDateToExcel(DateTimeInputDto input);
        Task<FileDto> GetAssetInOutAtSeriNumberToExcel(SeriNumberInputDto input);
        Task<FileDto> GetAssetOutOfDateToExcel(AssetOutOfDateInPutDto input);
        Task<PagedResultDto<WorkerInOutAtIdentity>> GetWorkerInOutAtDate(DateTimeInputDto input);
        Task<PagedResultDto<ListAssetInOutAtSeriNumberDto>> GetAssetInOutAtSeriNumber(SeriNumberInputDto input);
        Task<PagedResultDto<AssetOutOfDateSelectOutPutDto>> GetAssetOutOfDate(AssetOutOfDateInPutDto input);
    }
}
