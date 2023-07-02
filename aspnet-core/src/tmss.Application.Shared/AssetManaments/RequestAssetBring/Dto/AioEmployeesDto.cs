using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class AioEmployeesDto
    {
        public long Id { get; set; }
        public long VenderId { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public int IdentityVal { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public string Target { get; set; }
        public string Company { get; set; }
    }
}
