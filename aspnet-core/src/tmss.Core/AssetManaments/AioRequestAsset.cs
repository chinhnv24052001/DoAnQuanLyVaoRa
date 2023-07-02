using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.AssetManaments
{
    [Table("AioRequestAsset")]
    public class AioRequestAsset : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        public long AssetId { get; set; }
        public string SeriNumber { get; set; }
        [Required]
        public string TagCode { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        [Required]
        public long RequestId { get; set; }
        public long Status { get; set; }
        public int IsIn { get; set; }
        public int Total { get; set; }
        public string FilePath { get; set; }
        public byte[] AssetImage { get; set; }
        public bool? AviationIsBack { get; set; }
    }
}
