﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.Master.Employees.Dto
{
    public class EmployeesSelectOutputDto
    {
        public long Id { get; set; }
        public string EmployeesName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string VenderName { get; set; }
    }
}
