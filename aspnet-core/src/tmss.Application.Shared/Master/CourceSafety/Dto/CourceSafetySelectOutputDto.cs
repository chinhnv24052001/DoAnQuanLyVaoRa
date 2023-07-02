using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.CourceSafety.Dto
{
    public class CourceSafetySelectOutputDto
    {
        public long Id { get; set; }
        public string CourceName { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDateStart { get; set; }
        public DateTime EffectiveDateEnd { get; set; }
    }
}
