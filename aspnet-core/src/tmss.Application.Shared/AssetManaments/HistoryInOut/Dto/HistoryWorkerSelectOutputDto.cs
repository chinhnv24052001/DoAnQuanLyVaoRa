using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.HistoryInOut.Dto
{
    public class HistoryWorkerSelectOutputDto
    {
        public long Id { get; set; }
        public string WorkerName { get; set; }
        public string IdentityCard { get; set; }
        public DateTime? EffectiveDateFrom { get; set; }
        public DateTime? EffestiveDateTo { get; set; }
        public string Status { get; set; }
    }
}
