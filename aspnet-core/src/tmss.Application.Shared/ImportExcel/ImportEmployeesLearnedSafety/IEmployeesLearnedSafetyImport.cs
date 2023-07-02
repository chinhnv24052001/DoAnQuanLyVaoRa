using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;

namespace tmss.ImportExcel.ImportEmployeesLearnedSafety
{
    public interface IEmployeesLearnedSafetyImport: ITransientDependency
    {
        Task<List<EmployeesLearnedSafetyImportDto>> GetEmployeesLearnedSafetyFromExcel(byte[] fileBytes, string fileName);
        Task<List<long>> SetmployeesLearnedSafetyFromExcel(byte[] fileBytes, string fileName);
        Task deleteFile(string folder);
    }
}
