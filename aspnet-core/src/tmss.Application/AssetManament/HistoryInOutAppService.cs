using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments;
using tmss.AssetManaments.HistoryInOut;
using tmss.AssetManaments.HistoryInOut.Dto;
using tmss.Master.Asset;

namespace tmss.AssetManament
{
    public class HistoryInOutAppService : tmssAppServiceBase, IHistoryInOurAppService
    {
        private readonly IRepository<AioRequestAsset, long> _amAssetRepository;
        private readonly IRepository<AioRequestPeople, long> _amEmployeesRepository;
        private readonly IRepository<MstAsset, long> _mstAssetRepository;
        private readonly IRepository<AioRequestAssetInOut, long> _aioAssetCheckInOut;
        private readonly IRepository<AioRequestPeopleInOut, long> _aioPeopleCheckInOut;
        private readonly IDapperRepository<MstAsset, long> _aioSearchRequestRepository;
        public HistoryInOutAppService(IRepository<AioRequest, long> amRequestAssetBringRepository,
            IRepository<AioRequestAsset, long> amAssetRepository,
            IRepository<AioRequestPeople, long> amEmployeesRepository,
            IRepository<MstAsset, long> mstAssetRepository,
            IRepository<AioRequestAssetInOut, long> aioAssetCheckInOut,
            IDapperRepository<MstAsset, long> aioSearchRequestRepository,
            IRepository<AioRequestPeopleInOut, long> aioPeopleCheckInOut)
        {
            _amAssetRepository = amAssetRepository;
            _amEmployeesRepository = amEmployeesRepository;
            _mstAssetRepository = mstAssetRepository;
            _aioAssetCheckInOut = aioAssetCheckInOut;
            _aioPeopleCheckInOut = aioPeopleCheckInOut;
            _aioSearchRequestRepository = aioSearchRequestRepository;
        }


        public async Task<PagedResultDto<HistoryWorkerDetailSelectOutputDto>> LoadAllHistoryWorkerDetail(HistoryWorkerDetailInputDto input)
        {
            string _sql = "EXEC P_SEARCH_WORKER_DETAIL_IO_HISTORY @RequestId, @WorkerIOId";      

            var workerDetailInOutHistory = await _aioSearchRequestRepository.QueryAsync<HistoryWorkerDetailSelectOutputDto>(_sql, new
            {
                @RequestId = input.RequestId,
                @WorkerIOId = input.WorkerIOId
            });

            var result = workerDetailInOutHistory.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetCount = workerDetailInOutHistory.Count();
            return new PagedResultDto<HistoryWorkerDetailSelectOutputDto>(
                assetCount,
                result.ToList());
        }

        public async Task<PagedResultDto<HistoryAssetDetailSelectOutputDto>> LoadAllHistoryAssetDetail(HistoryAssetDetailInputDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_DETAIL_IO_HISTORY @RequestId, @AssetIOId";

            var assetDetailInOutHistory = await _aioSearchRequestRepository.QueryAsync<HistoryAssetDetailSelectOutputDto>(_sql, new
            {
                @RequestId = input.RequestId,
                @AssetIOId = input.AssetIOId
            });

            var result = assetDetailInOutHistory.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetCount = assetDetailInOutHistory.Count();
            return new PagedResultDto<HistoryAssetDetailSelectOutputDto>(
                assetCount,
                result.ToList());
        }
    }
}
