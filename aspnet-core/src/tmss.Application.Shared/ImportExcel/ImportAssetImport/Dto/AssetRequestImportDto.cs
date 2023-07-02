using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.ImportExcel.ImportAssetImport.Dto
{
    public class AssetRequestImportDto
    {
        public long AssetId { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public int Total { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string assetImage { get; set; }
        public bool? AviationIsBack { get; set; }
    }
}
