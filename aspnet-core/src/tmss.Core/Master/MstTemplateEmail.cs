using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master
{
    public class MstTemplateEmail : FullAuditedEntity<long>, IEntity<long>
    {
        public string TemplateCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
