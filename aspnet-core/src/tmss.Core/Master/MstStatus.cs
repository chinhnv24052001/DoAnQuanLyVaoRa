using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master
{
    public class MstStatus : FullAuditedEntity<long>, IEntity<long>
    {
        [MaxLength(50)]
        public string Key { get; set; }
        [MaxLength(500)]
        public string Name { get; set; }
    }
}
