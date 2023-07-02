using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class AioAssetDetailDto
    {
        public long Id { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public int Total { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string FilePath { get; set; }
        public byte[] AssetImage { get; set; }
        public bool? AviationIsBack { get; set; }
        public string AviationIsBackString { get; set; }
    }
}
