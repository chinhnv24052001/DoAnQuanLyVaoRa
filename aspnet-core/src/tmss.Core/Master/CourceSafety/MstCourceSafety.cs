using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.CourceSafety
{   [Table("MstCourceSafety")]
    public class MstCourceSafety : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxCourceNameLength = 500;
        public const int MaxDescriptionLength = 1000;
        [Required]
        [MaxLength(MaxCourceNameLength)]
        public string CourceName { get; set; }
        [Required]
        public DateTime EffectiveDateStart { get; set; }
        [Required]
        public DateTime EffectiveDateEnd { get; set; }
        [MaxLength(MaxDescriptionLength)]
        public string? Description { get; set; }
    }
}

