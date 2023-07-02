using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.Asset
{
    [Table("MstAsset")]
    public class MstAsset : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 500;
        public const int MaxTagCodeLength = 100;
        [Required]
        [MaxLength(MaxNameLength)]
        public string AssetName { get; set; }
        [Required]
        public long AssetGroupId { get; set; }
        
    }
}
