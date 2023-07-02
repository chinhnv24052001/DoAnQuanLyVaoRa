using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Email
{
    public class EmailCommon
    {
        public static string SendEmail(string p_RequestId, string p_Requestor_Account, string p_Requestor_FullName, string p_Approver_FullName, string p_To_Emaill, string p_TitleRequest,
           string p_Title, string p_BodyEmail, string p_division)
        {
            try
            {
                //MTemplateEmail m_EMAIL_TEMPLATE = _bussinessContext.M_EMAIL_TEMPLATE.Find(p_EmailCode);
                string subject = p_Title;
                subject = subject.Replace("(Approver_FullName)", p_Approver_FullName);
                subject = subject.Replace("(Requestor_FullName)", p_Requestor_FullName);
                StringBuilder body = new StringBuilder();
                body.Append(p_BodyEmail);
                EmailService.EmailServiceSoapClient emailService = new EmailService.EmailServiceSoapClient(EmailService.EmailServiceSoapClient.EndpointConfiguration.EmailServiceSoap);
                EmailService.SendEmailRequestBody EmailBody = new EmailService.SendEmailRequestBody();
                EmailService.SendEmailRequest emailRequest = new EmailService.SendEmailRequest(EmailBody);
                
                emailRequest.Body.from = tmssConsts.EMAIL_NOREPLY;
                //emailRequest.Body.to = "ap-einvoice@toyotavn.com.vn";
                //emailRequest.Body.from = p_FromUserName + "@toyotavn.com.vn";
                emailRequest.Body.to = p_To_Emaill + tmssConsts.EMAIL_SUFFIX;
                emailRequest.Body.cc = "";
                emailRequest.Body.subject = subject;
                
                string s_body = body.ToString();

                s_body = s_body.Replace("(Approver_FullName)", p_Approver_FullName);
                s_body = s_body.Replace("(Requestor_FullName)", p_Requestor_FullName);
                s_body = s_body.Replace("(RequestId)", p_RequestId);
                s_body = s_body.Replace("(TitleRequest)", p_TitleRequest);
                s_body = s_body.Replace("Account_Requestor", p_Requestor_Account);
                s_body = s_body.Replace("(Division)", p_division);
                emailRequest.Body.body = s_body;
                
                emailService.SendEmailAsync(emailRequest);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
    }
}
