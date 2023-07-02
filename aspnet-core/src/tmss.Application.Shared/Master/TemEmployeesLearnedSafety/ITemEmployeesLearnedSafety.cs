using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;
using tmss.Master.TemEmployeesLearnedSafety.Dto;

namespace tmss.TemEmployeesLearnedSafety
{
    public interface ITemEmployeesLearnedSafety : IApplicationService
    {
        Task<PagedResultDto<TemEmployeesLearnedSafetySelectOutputDto>> LoadAll(TemEmployeesLearnedSafetyInputDto input);
        Task<List<EmployeesLearnedSafetyImportDto>> SaveAllImport(List<EmployeesLearnedSafetyImportDto> input, long courceId);
        Task DeleteAll();
    }
}
