using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.CourceSafety.Dto;

namespace tmss.Master.CourceSafety
{
    public interface ICourceSafetyAppService : IApplicationService
    {
        Task<PagedResultDto<CourceSafetySelectOutputDto>> LoadAll(CourceSafetyInputDto input);
        Task<int> CountCource();
        Task Save(CourceSafetySaveDto input);
        Task DeleteById(int id);
        Task<CourceSafetyLoadForEditDto> LoadById(long id);
    }
}
