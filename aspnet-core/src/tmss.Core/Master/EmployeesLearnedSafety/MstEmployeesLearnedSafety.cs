using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.EmployeesLearnedSafety
{
    [Table("MstEmployeesLearnedSafety")]
    public class MstEmployeesLearnedSafety: FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 500;
        public const int MaxIdentityCardLength = 15;
        [Required]
        [MaxLength(MaxNameLength)]
        public string EmployeesName { get; set; }
        [Required]
        [MaxLength(MaxIdentityCardLength)]
        public string IdentityCard { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public long VenderId { get; set; }
        public string? Address { get; set; }
        [Required]
        public long CourceId { get; set; }
        public string PersonInCharge { get; set; }
        public string FilePath { get; set; }
        public string Image { get; set; }
        public bool? IsLearnedSafety { get; set; }
        public DateTime EffecttivateDate { get; set; }

    }
}