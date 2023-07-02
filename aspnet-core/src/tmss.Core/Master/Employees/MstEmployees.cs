using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.Employees
{
    [Table("MstEmployees")]
    public class MstEmployees : FullAuditedEntity<long>, IEntity<long>
    {
        public const int MaxNameLength = 500;
        public const int MaxAddressLength = 1000;
        public const int MaxPhoneLength = 15;
        [Required]
        [MaxLength(MaxNameLength)]
        public string EmployeesName { get; set; }
        [MaxLength(MaxAddressLength)]
        public string Address { get; set; }
        [MaxLength(MaxPhoneLength)]
        public string PhoneNumber { get; set; }
        [Required]
        public long VenderId { get; set; }
        [Required]
        [MaxLength(MaxPhoneLength)]
        public string IdentityCard { get; set; }
    }
}
