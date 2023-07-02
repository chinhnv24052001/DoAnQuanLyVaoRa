using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.Vender.Dto
{
    public class VenderSaveDto
    {
        public long Id { get; set; }
        public string VenderName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
