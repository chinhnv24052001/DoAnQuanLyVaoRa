using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.Note
{
    [Table("MstNote")]
    public class MstNote : FullAuditedEntity<long>, IEntity<long>
    {
        public string NoteText { get; set; } 

    }
}
