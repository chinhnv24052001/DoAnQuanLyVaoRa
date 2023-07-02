using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Report.Dto
{
    public class ListAssetInOutAtSeriNumberDto
    {
        public long Id { get; set; }
        public int Stt { get; set; }
        public string TagCode { get; set; }
        public string AssetName { get; set; }
        public string PersonSignAsset { get; set; }
        public string RequestCode { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set; }
    }
}