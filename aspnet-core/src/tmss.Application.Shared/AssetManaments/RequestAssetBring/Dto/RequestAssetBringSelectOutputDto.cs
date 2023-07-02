using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using tmss.AssetManaments.RequestAssetBring.Dto;

namespace tmss.AssetManaments.Dto
{
    public class RequestAssetBringSelectOutputDto : EntityDto<long>
    {
        public long Id { get; set; }
        public string RequestCode { get; set; }
        public string Status { get; set; }
        public string KeyStatus { get; set; }
        public DateTime DateRequest { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DateManageApproval { get; set; }
        public string ManageApprovalName { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public DateTime? DateAdminApproval { get; set; }
        public long? AdminApprovalId { get; set; }
        public bool IsApproval { get; set; }
        public long TotalCount { get; set; }
        public string LiveMonitorName { get; set; }
        public string LiveMonitorDepartment { get; set; }
        public string LiveMonitorPhoneNumber { get; set; }
        public string WhereToBring { get; set; }
        public string TypeNameRequest { get; set; }
        public long CreatorUserId { get; set; }
    }
}

