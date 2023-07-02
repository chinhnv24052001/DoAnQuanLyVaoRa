using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class EmployeesInOutInputDto : PagedAndSortedInputDto
    {
        public bool IsEmployeesIn { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string RequestCode { get; set; }
        public string UserRequest { get; set; }
        public string VenderName { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
