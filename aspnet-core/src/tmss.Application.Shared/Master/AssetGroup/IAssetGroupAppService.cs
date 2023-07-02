using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.AssetGroup.Dto;

namespace tmss.Master.AssetGroup
{
    public interface IAssetGroupAppService : IApplicationService
    {
        Task DeleteById(long id);
        Task<AssetGroupSelectOutputDto> LoadById(long id);
        Task Save(AssetGroupSaveDto assetGroupSave);
        Task<PagedResultDto<AssetGroupSelectOutputDto>> LoadAll(GetAssetGroupInputDto input);
    }
}
