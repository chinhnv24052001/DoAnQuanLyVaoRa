using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.AssetGroup;
using tmss.Master.AssetGroup.Dto;

namespace tmss.Master.AssetGroup1
{
    public class AssetGroupAppService : tmssAppServiceBase, IAssetGroupAppService
    {
        private readonly IRepository<MstAssetGroup, long> _assetGroupAppService;

        public AssetGroupAppService(IRepository<MstAssetGroup, long> assetGroupAppService)
        {
            _assetGroupAppService = assetGroupAppService;
        }

        public async Task DeleteById(long id)
        {
            await _assetGroupAppService.DeleteAsync(id);
        }

        public async Task<PagedResultDto<AssetGroupSelectOutputDto>> LoadAll(GetAssetGroupInputDto input)
        {
            var listAssetGroup = from a in _assetGroupAppService.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), p => p.AssetGroupName.Contains(input.Filter))
                                 select new AssetGroupSelectOutputDto()
                                 {
                                     Id = a.Id,
                                     AssetGroupName = a.AssetGroupName
                                 };
            var result = listAssetGroup.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetGroupCount = listAssetGroup.Count();
            return new PagedResultDto<AssetGroupSelectOutputDto>
                (assetGroupCount,
                result.ToList()
                );
        }

        public async Task<AssetGroupSelectOutputDto> LoadById(long id)
        {
            var assetGroup = _assetGroupAppService.GetAll()
               .Select(p => new AssetGroupSelectOutputDto { Id = p.Id, AssetGroupName = p.AssetGroupName })
               .FirstOrDefault(p => p.Id == id);
            return assetGroup;      
        }

        public async Task Save(AssetGroupSaveDto assetGroupSave)
        {
            if (await _assetGroupAppService.FirstOrDefaultAsync(assetGroupSave.Id) == null)
            {
                MstAssetGroup mstAssetGroup = new MstAssetGroup();
                mstAssetGroup.Id = assetGroupSave.Id;
                mstAssetGroup.AssetGroupName = assetGroupSave.AssetGroupName;
                mstAssetGroup.CreationTime = DateTime.Now;
                mstAssetGroup.CreatorUserId = AbpSession.UserId;
                await _assetGroupAppService.InsertAsync(mstAssetGroup);
            }
            else
            {
                MstAssetGroup mstAssetGroup = await _assetGroupAppService.FirstOrDefaultAsync(assetGroupSave.Id);
                mstAssetGroup.AssetGroupName = assetGroupSave.AssetGroupName;
                mstAssetGroup.LastModificationTime = DateTime.Now;
                mstAssetGroup.LastModifierUserId = AbpSession.UserId;
                await _assetGroupAppService.UpdateAsync(mstAssetGroup);
            }
        }
    }
}
