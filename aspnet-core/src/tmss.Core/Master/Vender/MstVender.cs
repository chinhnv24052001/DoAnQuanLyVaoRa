using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.Vender
{
    [Table("MstVender")]
    public class MstVender : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 500;
        public const int MaxAddressLength = 1000;
        public const int MaxPhoneLength = 15;
        [Required]
        [MaxLength(MaxNameLength)]
        public string VenderName { get; set; }
        [MaxLength(MaxNameLength)]
        public string Address { get; set; }
        [MaxLength(MaxNameLength)]
        public string PhoneNumber { get; set; }
    }
}
