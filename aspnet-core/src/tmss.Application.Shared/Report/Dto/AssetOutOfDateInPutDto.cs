using System;
using System.Collections.Generic;
using System.Text;
using tmss.Dto;

namespace tmss.Report.Dto
{
    public class AssetOutOfDateInPutDto: PagedInputDto
    {
        public long AssetId { get; set; }
        public string TagCode { get; set; }
        public string SeriNumber { get; set; }
        public string RequestCode { get; set; }
    }
}


