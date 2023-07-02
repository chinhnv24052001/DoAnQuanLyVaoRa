using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class AssetInOutSelectOutputDto
    {
        public long Id { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public string TagCode { get; set; }
        public int Total { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffestiveDateTo { get; set; }
        public DateTime? InDateTime { get; set; }   
        public DateTime? OutDateTime { get; set; }
        //public string Status { get; set; }
        //public DateTime? DateIn { get; set; }
        //public Boolean IsOut { get; set; }
        //public DateTime? DateOut { get; set; }
        //public Boolean IsIn { get; set; }
    }
}
