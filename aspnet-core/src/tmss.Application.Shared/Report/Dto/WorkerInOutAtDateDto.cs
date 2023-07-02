﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Report.Dto
{
    public class WorkerInOutAtDateDto
    {
        public long Id { get; set; }
        public int Stt { get; set; }
        public string WorkerName { get; set; }
        public string IdentityCard { get; set; }
        public string RequestCode { get; set; }
        public string RegisterRepresentativePhone { get; set; }
        public DateTime? StarDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
