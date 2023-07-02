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
    [Table("AioRequest")]
    public class AioRequest : FullAuditedEntity<long>, IEntity<long>
    {
        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        [Required]
        public DateTime DateRequest { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public DateTime? DateManageApproval { get; set; }
        public DateTime? DateTemManageApproval { get; set; }
        public string ManageApproval { get; set; }
        public string Adm { get; set; }
        public long? VenderId { get; set; }
        public string TradeUnionOrganization { get; set; }
        public string DepartmentClient { get; set; }
        public DateTime? DateAdminApproval { get; set; }
        public long? AdminApprovalId { get; set; }
        public long? TypeRequest { get; set; }
        [StringLength(500)]
        public string RequestCode { get; set; }
        public long? Status { get; set; }
        public int StatusDraft { get; set; }
        public string TemManagerApproval { get; set; }
        public bool TemManageIntervent { get; set; }
        public string LiveMonitorName { get; set; }
        public string LiveMonitorDepartment { get; set;  }
        public string LiveMonitorPhoneNumber { get; set; }
        public string WhereToBring { get; set; }
        public string PersonInChargeOfSubName { get; set; }
        public string PersonInChangeOfSubPhone { get; set; }
        public string AdmApprovalMessage { get; set; }
        public string ManageApprovalMessage { get; set; }

    }
}
