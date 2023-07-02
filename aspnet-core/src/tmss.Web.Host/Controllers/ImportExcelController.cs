using Abp.AspNetCore.Mvc.Authorization;
using tmss.Authorization;
using tmss.ImportExcel.ImportAssetImport;
using tmss.ImportExcel.ImportEmployeesLearnedSafety;
using tmss.ImportExcel.ImportMstAsset;
using tmss.ImportExcel.ImportRequest;
using tmss.ImportExcel.ImportWorkerReport;

namespace tmss.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class ImportExcelController : ImportExcelControllerBase
    {
        public ImportExcelController(IEmployeesLearnedSafetyImport _mstEmployeesLearnedSafetyImport,
            IAssetRequestImport _assetRequestImport,
            IWorkerRequestImport _workerRequestImport,
            IRequestImport _requestImport,
            IMstAssetImport _mstAssetImport)
            : base(_mstEmployeesLearnedSafetyImport, _assetRequestImport, _workerRequestImport, _requestImport, _mstAssetImport)
        {
        }
    }
}
