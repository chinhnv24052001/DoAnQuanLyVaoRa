using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using tmss.Authorization.Users.Dto;
using tmss.Dto;

namespace tmss.Master.Employees.Dto
{
    public class EmployeesInputDto : PagedAndSortedInputDto
    {
        public long VenderId { get; set; }
        public string Filter { get; set; }

        public List<string> Permissions { get; set; }

        public int? Role { get; set; }

        public bool OnlyLockedUsers { get; set; }
    }
}
