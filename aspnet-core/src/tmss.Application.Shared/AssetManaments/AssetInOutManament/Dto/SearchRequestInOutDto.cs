using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class SearchRequestInOutDto
    {
        public bool IsRequestAssetIn { get; set; }
        public long VenderId { get; set; }
        public string RequestCode { get; set; }
        public int SkipCount { get; set; }  
        public int MaxResultCount { get; set; }
    }
}
