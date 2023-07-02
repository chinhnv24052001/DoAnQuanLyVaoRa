using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Report.Dto
{
    public class AssetOutOfDateSelectOutPutDto
    {
        public long AssetRequestInOutId { get; set; }
        public int Stt { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public string RequestCode { get; set; }
        public int Total { get; set; }
        public DateTime? StarDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string PersonSignAsset { get; set; }
    }
}
