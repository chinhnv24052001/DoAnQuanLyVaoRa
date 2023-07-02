using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.ImportExcel.ImportWorkerReport.Dto
{
    public class WorkerRequestImportDto
    {
        public string EmployeesName { get; set; }   
        public string IdentityCard { get; set; }    
       public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
