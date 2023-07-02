using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.EmployeesLearnedSafety.Dto;

namespace tmss.Master.EmployeesLearnedSafety
{
    public interface IEmployeesLearnedSafetyAppService : IApplicationService
    {
        Task<PagedResultDto<EmployeesLearnedSafetySelectOutputDto>> LoadAllByCourceId(EmployeesLearnedSafetyInputDto input);
        Task Save(EmployeesLearnedSafetySaveDto input);
        Task DeleteById(int[] listId);
        Task<EmployeesLearnedSafetyLoadForEditDto> LoadById(long id);
        Task<bool> UpdateFilePath(long id, string image);
        Task<bool> SetEmployeesLearnedSafety(List<long> listId);
    }
}
