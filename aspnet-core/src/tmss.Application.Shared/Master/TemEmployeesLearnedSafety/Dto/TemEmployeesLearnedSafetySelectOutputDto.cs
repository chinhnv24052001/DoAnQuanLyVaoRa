using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.TemEmployeesLearnedSafety.Dto
{
    public class TemEmployeesLearnedSafetySelectOutputDto
    {
        public long Id { get; set; }
        public long? Seq { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public long CourceId { get; set; }
        public string Validate { get; set; }
        public string VenderName { get; set; }
    }
}
