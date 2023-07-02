using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments;
using tmss.AssetManaments.AssetInOutManament;

namespace tmss.SendEmail
{
    public class SendEmailRequest : tmssServiceBase, ISendEmailRequest, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly IRepository<AioRequestAsset, long> _amAssetRepository;
        private readonly IRepository<AioRequestPeople, long> _amEmployeesRepository;
        private readonly IAssetInOutManamentAppService _assetInOutManamentAppService;

        public SendEmailRequest(IEmailSender emailSender,
            IRepository<AioRequestAsset, long> amAssetRepository,
            IRepository<AioRequestPeople, long> amEmployeesRepository,
            IAssetInOutManamentAppService assetInOutManamentAppService)
        {
            _emailSender = emailSender;
            _amAssetRepository= amAssetRepository;
            _amEmployeesRepository= amEmployeesRepository;
            _assetInOutManamentAppService = assetInOutManamentAppService;
        }
         
        public async Task SendEmail(string p_RequestId, string p_Requestor_Account, string p_Requestor_FullName, string p_Approver_FullName, string p_To_Emaill, string p_TitleRequest, string p_Title, string p_BodyEmail, string p_division)
        {
            try
            {
                string subject = p_Title;
                subject = subject.Replace("(Approver_FullName)", p_Approver_FullName);
                subject = subject.Replace("(Requestor_FullName)", p_Requestor_FullName);
                StringBuilder body = new StringBuilder();
                body.Append(p_BodyEmail);

                //emailRequest.Body.from = tmssConsts.EMAIL_NOREPLY;
                ////emailRequest.Body.to = "ap-einvoice@toyotavn.com.vn";
                ////emailRequest.Body.from = p_FromUserName + "@toyotavn.com.vn";
                //emailRequest.Body.to = p_To_Emaill + tmssConsts.EMAIL_SUFFIX;
                //emailRequest.Body.cc = "";
                //emailRequest.Body.subject = subject;
                string s_body = body.ToString();

                s_body = s_body.Replace("(Approver_FullName)", p_Approver_FullName);
                s_body = s_body.Replace("(Requestor_FullName)", p_Requestor_FullName);
                s_body = s_body.Replace("(RequestId)", p_RequestId);
                s_body = s_body.Replace("(TitleRequest)", p_TitleRequest);
                s_body = s_body.Replace("Account_Requestor", p_Requestor_Account);
                s_body = s_body.Replace("(Division)", p_division);

                await _emailSender.SendAsync(new MailMessage
                {
                    To = { p_To_Emaill + tmssConsts.EMAIL_SUFFIX },
                    Subject = subject,
                    Body = s_body,
                    IsBodyHtml = true
                });
            } catch (Exception ex)
            {
                Logger.Error("Send Email: " + ex.Message, ex);
            }
        }

        public async Task SendEmailManufacture(long p_RequestId,  string p_To_Emaill)
        {
            try
            {
                var folderName = Path.Combine("wwwroot", "TempFile1");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var index = 1;
                List<string> listAttachments = new List<string>();
                MailMessage mail = new MailMessage
                {
                    Subject = "Send QR code to supplier.",
                    Body = "List supplier code",
                    IsBodyHtml = false,
                    To = { p_To_Emaill }
                    //From = new MailAddress("fromaddress@gmail.com")
                };

                //Xoa tat ca cac tem file
                await deleteFile(pathToSave);

                var listAsset = await _amAssetRepository.GetAll().AsNoTracking().Where(e => e.RequestId == p_RequestId).ToListAsync();
                foreach (AioRequestAsset item in listAsset)
                {
                    
                    string fileName = item.Id.ToString() + DateTime.Now.Millisecond.ToString() + "_" + index.ToString() + ".png";
                    byte[] bytes = await _assetInOutManamentAppService.CreateAssetBarCode(1, item.Id);
                    string fullPath = await GenarateFile(fileName, bytes);
                    mail.Attachments.Add(new Attachment(fullPath));
                    index++;
                }

                var listEmployees = await _amEmployeesRepository.GetAll().AsNoTracking().Where(e => e.RequestId == p_RequestId).ToListAsync();
                foreach (AioRequestPeople item in listEmployees)
                {
                    string fileName = item.Id.ToString() + DateTime.Now.Millisecond.ToString() + "_" + index.ToString() + ".png";
                    byte[] bytes = await _assetInOutManamentAppService.CreateAssetBarCode(0, item.Id);
                    string fullPath = await GenarateFile(fileName, bytes);
                    mail.Attachments.Add(new Attachment(fullPath));
                    index++;
                }

                _emailSender.SendAsync(mail);
            }
            catch (Exception ex)
            {
                Logger.Error("Send Email: " + ex.Message, ex);
            }
        }

        //delete tem file.
        public async Task deleteFile(string folder)
        {
            try
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folder);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Delete file: " + ex.Message, ex);
            }
        }

        //genarate file 
        public async Task<string> GenarateFile(string fileName, byte[] byteFile)
        {
            var folderName = Path.Combine("wwwroot", "TempFile1");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, fileName);
            if (byteFile != null)
            {
                if (Directory.Exists(folderName) == false)
                {
                    Directory.CreateDirectory(folderName);
                }
                if (fullPath != null && fullPath.Length > 0)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    FileStream file = File.Create(fullPath);
                    file.Write(byteFile, 0, byteFile.Length);
                    file.Close();
                }
            }
            else
            {
                fullPath = "";
            }
            return fullPath;
        }
    }
}
