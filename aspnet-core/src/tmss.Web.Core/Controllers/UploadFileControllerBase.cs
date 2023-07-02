using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tmss.Master.EmployeesLearnedSafety;

namespace tmss.Web.Controllers
{
    public class UploadFileControllerBase : tmssControllerBase
    {
        protected readonly IEmployeesLearnedSafetyAppService _employeesLearnedSafetyAppService;

        protected UploadFileControllerBase(
               IEmployeesLearnedSafetyAppService employeesLearnedSafetyAppService)
        {
            _employeesLearnedSafetyAppService = employeesLearnedSafetyAppService;
        }

        public async Task<JsonResult> UploadImageEmployeesLearnedSafety(long id)
        {
            bool result = false;
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 5) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                if (file.Length > 0)
                {
                    string strImgBase64 = "";
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo(stream);
                        var fileBytes = stream.ToArray();
                        strImgBase64 = Convert.ToBase64String(fileBytes);
                    }
                    result = await _employeesLearnedSafetyAppService.UpdateFilePath(id, strImgBase64);
                    
                }
            }
            catch (UserFriendlyException ex)
            {
                Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
            return Json(new AjaxResponse(new { result }));
        }
    }
}
