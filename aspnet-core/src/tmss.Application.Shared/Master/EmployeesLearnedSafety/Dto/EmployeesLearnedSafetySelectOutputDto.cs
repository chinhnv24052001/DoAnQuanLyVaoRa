using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.EmployeesLearnedSafety.Dto
{
    public class EmployeesLearnedSafetySelectOutputDto
    {
        public long Id { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long CourceId { get; set; }
        public string CourseName { get; set; }
        public string VenderName { get; set; }
        public string PersonInCharge { get; set; }
        public string ViewImage { get; set; }
        public DateTime? EffecttivateDate { get; set; }
        public string LearnedSafetyST { get; set; } 
        public bool IsLearnedSafety { get; set; } 

    }
}
