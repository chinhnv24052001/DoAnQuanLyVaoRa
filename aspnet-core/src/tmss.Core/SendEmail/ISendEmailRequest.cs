using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.SendEmail
{
    public interface ISendEmailRequest
    {
        Task SendEmail(string p_RequestId, string p_Requestor_Account, string p_Requestor_FullName, string p_Approver_FullName, string p_To_Emaill, string p_TitleRequest,
           string p_Title, string p_BodyEmail, string p_division);

        Task SendEmailManufacture(long p_RequestId, string p_To_Emaill);
    }
}
