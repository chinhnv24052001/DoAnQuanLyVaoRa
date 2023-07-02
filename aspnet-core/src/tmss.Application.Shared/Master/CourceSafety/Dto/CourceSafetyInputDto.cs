using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Master.CourceSafety.Dto
{
    public class CourceSafetyInputDto : PagedAndSortedInputDto
    {
        public string CourceName { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}


