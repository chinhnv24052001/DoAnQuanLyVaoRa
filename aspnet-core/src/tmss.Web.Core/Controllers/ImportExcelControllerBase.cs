using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using tmss.DataExporting.Excel.NPOI;
using tmss.ImportExcel.ImportAssetImport;
using tmss.ImportExcel.ImportAssetImport.Dto;
using tmss.ImportExcel.ImportEmployeesLearnedSafety;
using tmss.ImportExcel.ImportMstAsset;
using tmss.ImportExcel.ImportRequest;
using tmss.ImportExcel.ImportWorkerReport;
using tmss.Storage;

namespace tmss.Web.Controllers
{
    public class ImportExcelControllerBase : tmssControllerBase
    {
        protected readonly IEmployeesLearnedSafetyImport _mstEmployeesLearnedSafetyImport;
        protected readonly IMstAssetImport _mstAssetImport;
        protected readonly IAssetRequestImport _assetRequestImport;
        protected readonly IWorkerRequestImport _workerRequestImport;
        protected readonly IRequestImport _requestImport;

        public ILogger _logger { get; set; }

        protected ImportExcelControllerBase(
           IEmployeesLearnedSafetyImport mstEmployeesLearnedSafetyImport,
           IAssetRequestImport assetRequestImport,
           IWorkerRequestImport workerRequestImport,
           IRequestImport requestImport,
           IMstAssetImport mstAssetImport)
        {
            _mstEmployeesLearnedSafetyImport = mstEmployeesLearnedSafetyImport;
            _assetRequestImport = assetRequestImport;
            _workerRequestImport = workerRequestImport;
            _requestImport = requestImport;
            _mstAssetImport = mstAssetImport;
        }

       // [HttpPost]
       //[AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create)] 
        public async Task<JsonResult> ImportEmployeesFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                //var check = Request.Form["check"];
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

                var folderName = Path.Combine("wwwroot", "TempFile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //Xoa tat ca cac file
                //await _assetRequestImport.deleteFile(pathToSave);
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                //var tenantId = AbpSession.TenantId;
                //var fileObject = new BinaryObject(tenantId, fileBytes, $"{DateTime.UtcNow} import from excel file.");

                //await BinaryObjectManager.SaveAsync(fileObject);

                var report = await _mstEmployeesLearnedSafetyImport.GetEmployeesLearnedSafetyFromExcel(fileBytes, fileName);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import worker
        public async Task<JsonResult> ImportWorkerRequestFromExcel()
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

                var report = await _workerRequestImport.GetWorkerRequestFromExcel(fileBytes, file.FileName);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import asset
        public async Task<JsonResult> ImportAssetRequestFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();
                var check = Request.Form["check"];

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

                var folderName = Path.Combine("wwwroot", "TempFile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //Xoa tat ca cac file
                //await _assetRequestImport.deleteFile(pathToSave);
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var report = await _assetRequestImport.GetAssetRequestFromExcel(fileBytes, fileName, check);
                
                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import request Vender(Employees)
        public async Task<JsonResult> ImportRequestVenderEmployeesFromExcel()
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

                var UserId = (long)AbpSession.UserId;
                var report = await _requestImport.GetClientRequestFromExcel(fileBytes, file.FileName, 2, UserId);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import request Vender(Asset)
        public async Task<JsonResult> ImportRequestVenderAssetFromExcel()
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

                var UserId = (long)AbpSession.UserId;
                var report = await _requestImport.GetClientRequestFromExcel(fileBytes, file.FileName, 3, UserId);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import request Vender(Asset)
        public async Task<JsonResult> ImportRequestClientFromExcel()
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
                var UserId = (long)AbpSession.UserId;
                var report = await _requestImport.GetClientRequestFromExcel(fileBytes, file.FileName, 4, UserId);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import request Vender(Asset)
        public async Task<JsonResult> SetEmployeesFromExcel()
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

                var folderName = Path.Combine("wwwroot", "TempFile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var data = await _mstEmployeesLearnedSafetyImport.SetmployeesLearnedSafetyFromExcel(fileBytes, fileName);

                return Json(new AjaxResponse(new { data }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        public async Task<JsonResult> SendMailToManufacture()
        {
            try
            {
                var file = Request.Form.Files.First();
                var email = Request.Form["email"];
                var requestId = Request.Form["requestId"];
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

                var folderName = Path.Combine("wwwroot", "TempFile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                //var tenantId = AbpSession.TenantId;
                //var fileObject = new BinaryObject(tenantId, fileBytes, $"{DateTime.UtcNow} import from excel file.");

                //await BinaryObjectManager.SaveAsync(fileObject);

                var report = await _mstEmployeesLearnedSafetyImport.GetEmployeesLearnedSafetyFromExcel(fileBytes, file.FileName);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //Import asset
        public async Task<JsonResult> ImportAssetFromExcel()
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

                var folderName = Path.Combine("wwwroot", "TempFile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.Millisecond.ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, fileName);
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var report = await _mstAssetImport.GetAssetFromExcel(fileBytes, fileName);

                return Json(new AjaxResponse(new { report }));
            }
            catch (UserFriendlyException ex)
            {
                _logger.Error(ex.Message);
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}
