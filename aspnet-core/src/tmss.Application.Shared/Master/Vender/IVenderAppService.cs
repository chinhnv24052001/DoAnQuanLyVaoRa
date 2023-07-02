using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Vender
{
    public interface IVenderAppService
    {
        Task DeleteById(long id);
        Task<VenderSelectOutputDto> LoadById(long id);
        Task Save(VenderSaveDto assetGroupSave);
        Task<PagedResultDto<VenderSelectOutputDto>> LoadAll(VenderInputDto input);
        Task<int> CountVender();
    }
}
