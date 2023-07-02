using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.AssetManaments
{
    [Table("AioRequestPeopleInOut")]
    public class AioRequestPeopleInOut : FullAuditedEntity<long>, IEntity<long>
    {
        public long RequestPeopleId { get; set; }
        public DateTime? InDateTime { get; set; }
        public DateTime? OutDateTime { get; set; }
        public long? NoteInId { get; set; }
        public long? NoteOutId { get; set; }
    }
}
