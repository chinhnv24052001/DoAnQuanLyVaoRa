using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.RequestAssetBring.Dto
{
    public class SearchRequestDto : PagedInputDto
    {
        public string TypeRequest { get; set; }
        public long UserId { get; set; }
        public string UserManager { get; set; }
        public string Status { get; set; }
        public string Title { get; set; }
        public string RequestCode { get; set; }
        public DateTime? DateRequest { get; set; }
        public long TypeRequestId { get; set; }
        public string Role { get; set; }

    }
}
