using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.Asset.Dto;
using tmss.Master.AssetGroup.Dto;

namespace tmss.Master.Asset
{
    public interface IAssetAppService : IApplicationService
    {
        Task<PagedResultDto<AssetSelectOutputDto>> LoadAll(AssetInputDto input);
        Task Save(AssetSaveDto input);
        Task DeleteById(int id);
        Task<List<AssetGroupSelectOutputDto>> GetAssetGroupForEdit();
        Task<AssetLoadForEditDto> LoadById(long id);
    }
}
