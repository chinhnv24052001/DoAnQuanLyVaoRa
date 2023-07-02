using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master
{
    public class MstVenderAccount : FullAuditedEntity<long>, IEntity<long>
    {
        public string VenderAccountName { get; set; }

    }
}
