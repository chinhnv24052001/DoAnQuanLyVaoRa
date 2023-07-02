using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.Asset.Dto
{
    public class AssetSaveDto
    {
        public long Id { get; set; }
        public string AssetName { get; set; }
        public long AssetGroupId { get; set; }
        public string TagCode { get; set; }
    }
}
