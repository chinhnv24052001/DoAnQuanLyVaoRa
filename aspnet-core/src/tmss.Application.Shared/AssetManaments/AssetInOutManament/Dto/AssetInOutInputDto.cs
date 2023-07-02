using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class AssetInOutInputDto : PagedAndSortedInputDto
    {
        public bool IsAssetIn { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public string RequestCode { get; set; }
        public string VenderName { get; set; }
        public string UserRequest { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
