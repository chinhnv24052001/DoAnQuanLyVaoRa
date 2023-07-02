using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class RequestAssetInOutSelectOutputDto
    {
        public long Id { get; set; }
        public string RequestCode { get; set; }
        public string TitleRequest { get; set; }
        public string TypeRequest { get; set; }
        public long TypeRequestId { get; set; }
        public string Department { get; set; }
        public string VenderName { get; set; }
        public DateTime? EffectiveDate { get; set; }    
    }
}
