using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments.HistoryInOut.Dto;

namespace tmss.AssetManaments.HistoryInOut
{
    public interface IHistoryInOurAppService: IApplicationService
    {
        Task<PagedResultDto<HistoryAssetDetailSelectOutputDto>> LoadAllHistoryAssetDetail(HistoryAssetDetailInputDto input);
        Task<PagedResultDto<HistoryWorkerDetailSelectOutputDto>> LoadAllHistoryWorkerDetail(HistoryWorkerDetailInputDto input);
    }
}
