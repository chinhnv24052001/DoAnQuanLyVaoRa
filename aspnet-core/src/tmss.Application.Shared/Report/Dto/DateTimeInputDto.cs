using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Report.Dto
{
    public class DateTimeInputDto : PagedAndSortedInputDto
    {
        public DateTime? DateExport { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
