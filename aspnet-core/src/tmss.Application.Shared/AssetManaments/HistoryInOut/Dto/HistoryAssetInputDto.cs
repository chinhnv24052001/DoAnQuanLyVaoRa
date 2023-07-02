using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.HistoryInOut.Dto
{
    public class HistoryAssetInputDto: PagedAndSortedInputDto
    {
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffestiveDateTo { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
