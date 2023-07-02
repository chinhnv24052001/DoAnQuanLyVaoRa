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
    [Table("AioRequestPeople")]
    public class AioRequestPeople : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxName = 500;
        [Required]
        [MaxLength(MaxName)]
        public string EmployeesName { get; set; }
        [Required]
        public string IdentityCard { get; set; }
        [Required]
        public long RequestId { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        [Required]
        public DateTime DateEnd { get; set; }
        public string Target { get; set; }
        public string Company { get; set; }
        public long Status { get; set; }
        public bool IsIn { get; set; }
        public byte[] ImageEmployees { get; set; }
    }
}
