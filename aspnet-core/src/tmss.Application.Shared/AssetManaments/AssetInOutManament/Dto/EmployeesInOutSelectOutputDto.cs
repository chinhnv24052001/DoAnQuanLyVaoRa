using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class EmployeesInOutSelectOutputDto
    {
        public long Id { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public DateTime EffectiveDateFrom { get; set; }
        public DateTime EffestiveDateTo { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set;}
        public string Target { get; set; }
        public string Company { get; set; }
        //public string Status { get; set; }
        //public DateTime? DateIn { get; set; }
        //public Boolean IsOut { get; set; }
        //public DateTime? DateOut { get; set; }
        //public Boolean IsIn { get; set; }
    }
}
