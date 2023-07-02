using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.AssetInOutManament.Dto
{
    public class RequestAssetInOutInputDto: PagedAndSortedInputDto
    {
        public bool IsRequestAssetIn { get; set; }
        public long RequestId { get; set; }
        public string EmployeesName { get; set; }
        public string IdentityCard { get; set; }
        public string TagCode { get; set; }
        public string SeriNumber { get; set; }
        public long VenderId { get; set; }
        public string RequestCode { get; set; }
        public List<string> Permissions { get; set; }
        public int? Role { get; set; }
        public bool OnlyLockedUsers { get; set; }
    }
}
