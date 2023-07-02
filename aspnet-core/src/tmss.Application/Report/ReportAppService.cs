using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments.AssetInOutManament.Dto;
using tmss.AssetManaments.Dto;
using tmss.AssetManaments.RequestAssetBring.Dto;
using tmss.Auditing.Exporting;
using tmss.Authorization.Users;
using tmss.Dto;
using tmss.Master.Asset;
using tmss.Report.Dto;
using tmss.Report.ExportToFile;
using tmss.SQL;

namespace tmss.Report
{
    public class ReportAppService : tmssAppServiceBase, IReportAppService
    {
        private readonly IDapperRepository<MstAsset, long> _aioSearchRequestRepository;
        private readonly IExportToExcel _exportToExcel;
        private readonly ISqlHelper _sqlHelper;
        public ReportAppService(
            IDapperRepository<MstAsset, long> aioSearchRequestRepository,
            IExportToExcel exportToExcel,
            ISqlHelper sqlHelper
         )
        {
            _aioSearchRequestRepository = aioSearchRequestRepository;
            _exportToExcel = exportToExcel;
            _sqlHelper = sqlHelper;
        }
        public async Task<FileDto> GetWorkerInOutAtDateToExcel(DateTimeInputDto input)
        {
            string _sql = "EXEC P_SEARCH_WORKER_IO_AT_DATE @Date";
            var workerInOut = await _aioSearchRequestRepository.QueryAsync<WorkerInOutAtIdentity>(_sql, new
            {
                @Date = input.DateExport
            });

            return _exportToExcel.ExportListWorkerInOutAtDateToFile(workerInOut.ToList());
        }

        public async Task<PagedResultDto<WorkerInOutAtIdentity>> GetWorkerInOutAtDate(DateTimeInputDto input)
        {
            string _sql = "EXEC P_SEARCH_WORKER_IO_AT_DATE @Date";
            var workerInOut = await _aioSearchRequestRepository.QueryAsync<WorkerInOutAtIdentity>(_sql, new
            {
                @Date = input.DateExport
            });
            var result = workerInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var workerInOutCount = workerInOut.Count();
            return new PagedResultDto<WorkerInOutAtIdentity>(
                workerInOutCount,
                result.ToList());
        }
        public async Task<PagedResultDto<WorkerInOutAtDateDto>> GetWorkerInOutAtIdentityCard(IdentityCardInputDto input)
        {
            string _sql = "EXEC P_SEARCH_WORKER_IO_AT_IDENTITYCARD @IdentityCard";

            var workerInOut = await _aioSearchRequestRepository.QueryAsync<WorkerInOutAtDateDto>(_sql, new
            {              
                
                @IdentityCard = input.IdentityCard               
 
            });

            var result = workerInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var workerInOutCount = workerInOut.Count();
            return new PagedResultDto<WorkerInOutAtDateDto>(
                workerInOutCount,
                result.ToList());
        }

        public async Task<FileDto> GetWorkerInOutAtIdentityCardToExcel(IdentityCardInputDto input)
        {
            string _sql = "EXEC P_SEARCH_WORKER_IO_AT_IDENTITYCARD @IdentityCard";
            var workerInOut = await _aioSearchRequestRepository.QueryAsync<WorkerInOutAtDateDto>(_sql, new
            {
                @IdentityCard = input.IdentityCard
            });

            return _exportToExcel.ExportListWorkerInOutAtIdentityToFile(workerInOut.ToList());
        }

        public async Task<PagedResultDto<ListAssetInOutAtSeriNumberDto>> GetAssetInOutAtSeriNumber(SeriNumberInputDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_IO_AT_SERINUMBER @SeriNumber, @DateInOut";
            var assetInOut = await _aioSearchRequestRepository.QueryAsync<ListAssetInOutAtSeriNumberDto>(_sql, new
            {
                @SeriNumber = input.SeriNumber,
                @DateInOut = input.DateInOut

            });
            var result = assetInOut.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetInOutCount = assetInOut.Count();
            return new PagedResultDto<ListAssetInOutAtSeriNumberDto>(
                assetInOutCount,
                result.ToList());
        }

        public async Task<FileDto> GetAssetInOutAtSeriNumberToExcel(SeriNumberInputDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_IO_AT_SERINUMBER @SeriNumber @DateInOut";
            var assetInOut = await _aioSearchRequestRepository.QueryAsync<ListAssetInOutAtSeriNumberDto>(_sql, new
            {
                @SeriNumber = input.SeriNumber,
                @DateInOut = input.DateInOut
            });

            return _exportToExcel.ExportListAssetInOutAtSeriNumberToFile(assetInOut.ToList());
        }

        public async Task<PagedResultDto<AssetOutOfDateSelectOutPutDto>> GetAssetOutOfDate (AssetOutOfDateInPutDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_OUT_OF_DATE @AssetId, @TagCode, @SeriNumber, @RequestCode";
            var assetOutOfDate = await _aioSearchRequestRepository.QueryAsync<AssetOutOfDateSelectOutPutDto>(_sql, new
            {
                @AssetId = input.AssetId,
                @TagCode =input.TagCode,
                @SeriNumber = input.SeriNumber,
                @RequestCode = input.RequestCode
            });
            var result = assetOutOfDate.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetOutOfDateCount = assetOutOfDate.Count();
            return new PagedResultDto<AssetOutOfDateSelectOutPutDto>(
                assetOutOfDateCount,
                result.ToList());
        }

        public async Task<FileDto> GetAssetOutOfDateToExcel(AssetOutOfDateInPutDto input)
        {
            string _sql = "EXEC P_SEARCH_ASSET_OUT_OF_DATE @AssetId, @TagCode, @SeriNumber, @RequestCode";
            var assetOutOfDate = await _aioSearchRequestRepository.QueryAsync<AssetOutOfDateSelectOutPutDto>(_sql, new
            {
                @AssetId = input.AssetId,
                @TagCode = input.TagCode,
                @SeriNumber = input.SeriNumber,
                @RequestCode = input.RequestCode
            });
            return _exportToExcel.ExportListAssetOutOfDateToFile(assetOutOfDate.ToList());
        }
    }
}
