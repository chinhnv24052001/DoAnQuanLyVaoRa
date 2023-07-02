using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto
{
    public class EmployeesLearnedSafetyImportDto
    {  
        public long Seq { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Validate { get; set; }
        public string VenderName { get; set; }
        public long VenderId { get; set; }
        public string PersonInCharge { get; set; }
        public DateTime WorkingDateStart { get; set; }
        public DateTime WorkingDateEnd { get; set; }
        public string Image { get; set; }
    }
}
