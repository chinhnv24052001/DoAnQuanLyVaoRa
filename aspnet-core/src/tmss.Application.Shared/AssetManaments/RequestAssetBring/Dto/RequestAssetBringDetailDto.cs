using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class RequestAssetBringDetailDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public String DateRequest { get; set; }
        public string DateManageApproval { get; set; }
        public string ManageApprovalName { get; set; }
        public string ManageApproval { get; set; }
        public long ManageApprovalId { get; set; }
        public string TemManageApprovalName { get; set; }
        public string DateAdminApproval { get; set; }
        public string AdminApprovalName { get; set; }
        public long? AdminApprovalId { get; set; }
        public string Department { get; set; }
        public string RequestCode { get; set; }
        public long? Status { get; set; }
        public long? VenderId { get; set; }
        public string VenderName { get; set; }
        public long? CreateByUserId { get; set; }
        public string UserName { get; set; }
        public bool IsManagerApproved { get; set; }
        public bool IsADMApproved { get; set; }
        public bool IsTemManagerApproved { get; set; }
        public string ReasonForRefusal { get; set; }
        public long? TypeRequest { get; set; }
        public bool TemManageIntervent { get; set; }
        public string LiveMonitorName { get; set; }
        public string LiveMonitorDepartment { get; set; }
        public string LiveMonitorPhoneNumber { get; set; }
        public string WhereToBring { get; set; }
        public string TradeUnionOrganization { get; set; }
        public string DepartmentClient { get; set; }
        public string PersonInChargeOfSubName { get; set; }
        public string PersonInChangeOfSubPhone { get; set; }
        public string AdmApprovalNameWairting { get; set; }
        public string AdmApprovalMessage { get; set; }
        public string ManageApprovalMessage { get; set; }
        public List<AioAssetDetailDto> listAssetDto { get; set; }
        public List<AioEmployeesDetailDto> listEmployeesDto { get; set; }
    }
}
