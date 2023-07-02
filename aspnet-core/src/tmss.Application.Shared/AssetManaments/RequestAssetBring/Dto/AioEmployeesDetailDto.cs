using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class AioEmployeesDetailDto
    {
        public long Id { get; set; }
        public bool CheckWorkerLearned { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string EmployeesImage { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Target { get; set; }
        public string Company { get; set; }
    }
}
