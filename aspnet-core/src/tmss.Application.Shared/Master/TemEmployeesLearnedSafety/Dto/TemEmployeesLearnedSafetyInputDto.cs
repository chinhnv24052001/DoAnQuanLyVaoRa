using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Master.TemEmployeesLearnedSafety.Dto
{
    public class TemEmployeesLearnedSafetyInputDto : PagedAndSortedInputDto
    {
        public string EmployeesName { get; set; }
        public string CourceCode { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}

