using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class AioAssetDto
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public int Total { get; set; }
        public byte[] AssetImage { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string FilePath { get; set; }
        public bool? AviationIsBack { get; set; }
    }
}
