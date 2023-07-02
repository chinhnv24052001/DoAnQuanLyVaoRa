using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.DataCodes
{
    [Table("DataCode")]
    public class DataCode : IEntity<long>
    {   [Key]
        public long Id { get; set; }
        public string Value { get; set; }
        public string Value_Code { get; set; }
        public bool IsTransient()
        {
            return false;
        }
    }
}
