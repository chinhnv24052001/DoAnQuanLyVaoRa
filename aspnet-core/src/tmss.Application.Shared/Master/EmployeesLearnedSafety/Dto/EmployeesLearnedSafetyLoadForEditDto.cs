using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.EmployeesLearnedSafety.Dto
{
    public class EmployeesLearnedSafetyLoadForEditDto
    {
        public long Id { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long CourceId { get; set; }
        public long VenderId { get; set; }
        public string PersonInCharge { get; set; }
        
    }
}