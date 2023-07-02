using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class EmployeesInfoScannerDto
    {
        public long EmployeesIOId { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public bool CheckEffectDate { get; set; }
        public DateTime DateEffect { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public string EmployeesImage { get; set; }
        public string Target { get; set; }
        public string Company { get; set; }
    }
}
