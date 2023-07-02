using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Text;
using tmss.AssetManaments.RequestAssetBring.Dto;

namespace tmss.ImportExcel.ImportRequest.Dto
{
    public class RequestImportDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string TypeRequestKey { get; set; }
        public long? TypeRequest { get; set; }
        public DateTime? DateRequest { get; set; }
        public string DateManageApproval { get; set; }
        public long? UserId { get; set; }
        public string ManageApproval { get; set; }
        public string UserName { get; set; }
        public long? VenderId { get; set; }
        public string VenderAddress { get; set; }
        public string VenderPhoneNumber { get; set; }
        public string PersonInChargeOfSubName { get; set; }
        public string PersonInChangeOfSubPhone { get; set; }
        public string DateAdminApproval { get; set; }
        public long? AdminApprovalId { get; set; }
        public string RequestCode { get; set; }
        public long? Status { get; set; }
        public int StatusDraft { get; set; }
        public bool TemManageIntervent { get; set; }
        public List<AioAssetDto> AssetList { get; set; }
        public List<AioEmployeesDto> WorkersList { get; set; }
        public List<ErrorImportDto> ErrorImportList { get; set; }
        public string LiveMonitorName { get; set; }
        public string LiveMonitorDepartment { get; set; }
        public string LiveMonitorPhoneNumber { get; set; }
        public string LiveMonitorPosition { get; set; }
        public string WhereToBring { get; set; }
        public string TradeUnionOrganization { get; set; }
        public string DepartmentClient { get; set; }
        public bool StatusWarning { get; set; }
        public string  WarningMessage { get; set; } 
        
    }
}
