using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Master.AssetGroup.Dto
{
    public class GetAssetGroupInputDto : PagedAndSortedInputDto
    {
        public string Filter { get; set; }

        public List<string> Permissions { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }
    }
}
