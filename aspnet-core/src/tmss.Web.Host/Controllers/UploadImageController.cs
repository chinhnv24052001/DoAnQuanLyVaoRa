using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using tmss.Authorization;
using tmss.Master.EmployeesLearnedSafety;

namespace tmss.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UploadImageController : UploadFileControllerBase
    {
        public UploadImageController(IEmployeesLearnedSafetyAppService employeesLearnedSafetyAppService) : base(employeesLearnedSafetyAppService)
        {

        }
    }
}
