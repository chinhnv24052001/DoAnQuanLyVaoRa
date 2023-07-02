using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.HistoryInOut.Dto
{
    public class HistoryAssetDetailInputDto : PagedAndSortedInputDto
    {   
        public long RequestId { get; set; }
        public long AssetIOId { get; set; }   
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
