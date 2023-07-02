using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.HistoryInOut.Dto
{
    public class HistoryAssetSelectOutputDto
    {
        public long Id { get; set; }
        public string AssetName { get; set; }
        public string SeriNumber { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffestiveDateTo { get; set; }
        public string Status { get; set; }
    }
}
