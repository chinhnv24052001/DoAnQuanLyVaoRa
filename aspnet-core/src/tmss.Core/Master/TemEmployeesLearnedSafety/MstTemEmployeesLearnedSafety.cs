using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.TemEmployeesLearnedSafety
{   [Table("MstTemEmployeesLearnedSafety")]
    public class MstTemEmployeesLearnedSafety : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 500;
        public const int MaxIdentityCardLength = 15;
        public long? Seq { get; set; }
        [Required]
        [MaxLength(MaxNameLength)]
        public string EmployeesName { get; set; }
        [Required]
        [MaxLength(MaxIdentityCardLength)]
        public string IdentityCard { get; set; }
        public long CourceId { get; set; }
        public string? Validate { get; set; }
    }
}
