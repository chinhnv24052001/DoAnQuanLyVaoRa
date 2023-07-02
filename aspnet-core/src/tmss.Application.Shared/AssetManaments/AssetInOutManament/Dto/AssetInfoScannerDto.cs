using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class AssetInfoScannerDto
    {
        public long AssetIOId { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public int Total { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public DateTime? DateEffect { get; set; }
        public bool CheckEffectDate { get; set; }
        public string DateIn { get; set; }
        public string DateOut { get; set; }
        public byte[] AssetImage { get; set; }
        public bool? AviationIsBack { get; set; }
        public string AviationIsBackString { get; set; }
    }
}
