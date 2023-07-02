using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.HistoryInOut.Dto
{
    public class HistoryWorkerDetailSelectOutputDto
    {
        public long Id { get; set; }
        public string WorkerName { get; set; }
        public string IdentityCard { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set; }
        public string GuardNameCheckIn { get; set; }
        public string GuardNameCheckOut { get; set; }
        public string NoteCheckOut { get; set; }
        public string NoteCheckIn { get; set; }
    }
}
