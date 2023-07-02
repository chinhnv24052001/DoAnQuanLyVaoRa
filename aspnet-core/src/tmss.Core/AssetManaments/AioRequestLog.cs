using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.AssetManaments
{
    public class AioRequestLog : FullAuditedEntity<long>, IEntity<long>
    {
        public long StatusId { get; set; }
        public long RequestId { get; set; }
        public string ReasonRefusal { get; set; }

    }
}
