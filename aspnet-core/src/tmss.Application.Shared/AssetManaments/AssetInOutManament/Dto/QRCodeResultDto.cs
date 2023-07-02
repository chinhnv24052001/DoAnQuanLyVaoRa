using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class QRCodeResultDto
    {
        public long RequestId { get; set; }
        public string FirstItemString { get; set; }
        public bool IsIn { get; set; }
        public string Title {get; set; }
        public string Description { get; set; }
        public string DateRequest { get; set; }
        public string RequestCode { get; set; }
        public string VenderName { get; set; }
        public long? TypeRequest { get; set; }
        public string LiveMonitorName { get; set; }
        public string LiveMonitorPhoneNumber { get; set; }
        public string LiveMonitorDepartment { get; set; }
        public string WhereToBring { get; set; }
        public string DepartmentClient { get; set; }
        public string TradeUnionOrganization { get; set; }
        public bool FirtStatusInOut { get; set; }
        public int StatusQRScanner { get; set; }
        public string PersonInChargeOfSubName { get; set; }
        public string PersonInChangeOfSubPhone { get; set; }
        public AssetInfoScannerDto AssetInfo { get; set; }
        public EmployeesInfoScannerDto EmployeesInfo { get; set; }
    }
}
