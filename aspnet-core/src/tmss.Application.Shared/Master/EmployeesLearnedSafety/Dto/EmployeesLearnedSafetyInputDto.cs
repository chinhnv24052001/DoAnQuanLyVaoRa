using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Master.EmployeesLearnedSafety.Dto
{
    public class EmployeesLearnedSafetyInputDto : PagedAndSortedInputDto
    {
        public string EmployeesName { get; set; }
        public long CourceId { get; set; }
        public string PersonInCharge { get; set; }
        public string IdentityCard { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
