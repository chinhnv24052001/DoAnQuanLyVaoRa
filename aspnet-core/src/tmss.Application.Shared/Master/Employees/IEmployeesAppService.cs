using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.Employees.Dto;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Employees
{
    public interface IEmployeesAppService: IApplicationService
    {
        Task<PagedResultDto<EmployeesSelectOutputDto>> LoadAll(EmployeesInputDto input);
        Task Save(EmployessSaveDto input);
        Task DeleteById(long id);
        Task<List<GetVenderForEditEmployeesDto>> GetVenderForEdit();
        Task<EmployeesLoadByIdDto> LoadById(long id);
    }
}
