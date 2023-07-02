﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using tmss.Authorization.Users.Dto;
using tmss.Storage;
using Abp.BackgroundJobs;
using tmss.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;
using tmss.Authorization.Users.Importing;
using tmss.ImportExcel.ImportEmployeesLearnedSafety;

namespace tmss.Web.Controllers
{
    public abstract class UsersControllerBase : tmssControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
       // protected readonly IEmployeesLearnedSafetyImport _mstEmployeesLearnedSafetyImport;

        protected UsersControllerBase(
            IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager)
           // IEmployeesLearnedSafetyImport mstEmployeesLearnedSafetyImport)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
           //_mstEmployeesLearnedSafetyImport = mstEmployeesLearnedSafetyImport;
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        public async Task<JsonResult> ImportFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes, $"{DateTime.UtcNow} import from excel file.");

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        //[HttpPost]
        ////  [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        //public async Task<JsonResult> ImportEmployeesFromExcel()
        //{
        //    try
        //    {
        //        var file = Request.Form.Files.First();

        //        if (file == null)
        //        {
        //            throw new UserFriendlyException(L("File_Empty_Error"));
        //        }

        //        if (file.Length > 1048576 * 100) //100 MB
        //        {
        //            throw new UserFriendlyException(L("File_SizeLimit_Error"));
        //        }

        //        byte[] fileBytes;
        //        using (var stream = file.OpenReadStream())
        //        {
        //            fileBytes = stream.GetAllBytes();
        //        }

        //        var report = await _mstEmployeesLearnedSafetyImport.GetEmployeesLearnedSafetyFromExcel(fileBytes, file.FileName);

        //        return Json(new AjaxResponse(new { report }));
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
        //    }
        //}
    }
}