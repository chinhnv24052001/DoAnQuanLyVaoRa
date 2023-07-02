using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tmss.Master.AssetGroup
{
    [Table("MstAssetGroup")]
    public class MstAssetGroup : FullAuditedEntity<long>, IEntity<long>
    {
        public string AssetGroupName { get; set; }
    }
}
