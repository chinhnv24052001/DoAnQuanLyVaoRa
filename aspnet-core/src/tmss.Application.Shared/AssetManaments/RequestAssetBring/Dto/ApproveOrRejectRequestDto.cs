using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class ApproveOrRejectRequestDto
    {
        public long Id { get; set; }
        public string ReasonRefusal { get; set; }
        public string Type { get; set; }
    }
}
