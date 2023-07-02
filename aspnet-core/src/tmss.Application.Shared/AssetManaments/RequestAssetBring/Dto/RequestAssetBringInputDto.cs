using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.AssetManaments.Dto
{
    public class RequestAssetBringInputDto : PagedInputDto
    {
        public string Title { get; set; }
        public string RequestCode { get; set; }
        public DateTime? DateRequest { get; set; }
        public long TypeRequestId { get; set; }

        public string TabKey { get; set; }
        public string Status { get; set; }

        public RequestAssetBringInputDto()
        {
            SkipCount = 1;
        }
    }
}
