using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportMstAsset.Dto;
using tmss.Master.Asset.Dto;
using tmss.Master.AssetGroup;
using tmss.Master.AssetGroup.Dto;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Asset
{
    public class AssetAppService : tmssAppServiceBase, IAssetAppService
    {

        private readonly IRepository<MstAsset, long> _assetRepository;
        private readonly IRepository<MstAssetGroup, long> _assetGroupRepository;
        public AssetAppService(IRepository<MstAsset, long> assetRepository, IRepository<MstAssetGroup, long> assetGroupRepository)
        {
            _assetRepository = assetRepository;
            _assetGroupRepository = assetGroupRepository;
        }
        public async Task DeleteById(int id)
        {
            await _assetRepository.DeleteAsync(id);
        }

        public async Task<List<AssetGroupSelectOutputDto>> GetAssetGroupForEdit()
        {
            var listAssetGroup = _assetGroupRepository.GetAll()
                .Select(p => new AssetGroupSelectOutputDto
                {
                    AssetGroupName = p.AssetGroupName,
                    Id = p.Id,
                });
            return listAssetGroup.ToList();
        }

        public async Task<PagedResultDto<AssetSelectOutputDto>> LoadAll(AssetInputDto input)
        {
            var assetList = from asset in _assetRepository.GetAll().AsNoTracking()
                            .Where(e => string.IsNullOrWhiteSpace(input.Filter) || e.AssetName.Contains(input.Filter))
                            join assetGroup in _assetGroupRepository.GetAll().AsNoTracking()
                            .Where(e => input.AssetGroupId == 0 || e.Id == input.AssetGroupId)
                            on asset.AssetGroupId equals assetGroup.Id
                            select new AssetSelectOutputDto
                            {
                                Id = asset.Id,
                                AssetName = asset.AssetName,
                                AssetGroupId = assetGroup.Id,
                                AssetGroupName = assetGroup.AssetGroupName
                            };
            var result = assetList.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetCount = assetList.Count();
            return new PagedResultDto<AssetSelectOutputDto>(
                assetCount,
                result.ToList()
                );
        }

        public async Task<AssetLoadForEditDto> LoadById(long id)
        {
            var asset = _assetRepository.GetAll()
                .Select(p => new AssetLoadForEditDto { Id = p.Id, AssetName = p.AssetName, AssetGroupId = p.AssetGroupId })
                .FirstOrDefault(p => p.Id == id);
            return asset;
        }

        public async Task Save(AssetSaveDto input)
        {
            if (await _assetRepository.FirstOrDefaultAsync(input.Id) == null)
            {
                MstAsset mstAsset = new MstAsset();
                mstAsset.Id = input.Id;
                mstAsset.AssetName = input.AssetName;
                mstAsset.AssetGroupId = input.AssetGroupId;
                mstAsset.CreationTime = DateTime.Now;
                mstAsset.CreatorUserId = AbpSession.UserId;
                await _assetRepository.InsertAsync(mstAsset);
            }
            else
            {
                MstAsset mstAsset = await _assetRepository.FirstOrDefaultAsync(input.Id);
                mstAsset.AssetName = input.AssetName;
                mstAsset.AssetGroupId = input.AssetGroupId;
                mstAsset.LastModificationTime = DateTime.Now;
                mstAsset.LastModifierUserId = AbpSession.UserId;
                await _assetRepository.UpdateAsync(mstAsset);
            }
        }

        public async Task<List<MstAssetImportDto>> SaveAllImport(List<MstAssetImportDto> input)
        {
            MstAsset mstAsset = new MstAsset();
            List<MstAssetImportDto> listAssetErr = new List<MstAssetImportDto>();
            foreach (var item in input)
            {
                if(item.Validate == null)
                {
                    mstAsset = new MstAsset();
                    mstAsset.AssetName = item.AssetName;
                    mstAsset.AssetGroupId = item.AssetGroupId;
                    mstAsset.CreationTime = DateTime.Now;
                    mstAsset.CreatorUserId = AbpSession.UserId;
                    await _assetRepository.InsertAsync(mstAsset);
                }
                else
                {
                    listAssetErr.Add(item);
                }
            }
            return listAssetErr;
        }

        public async Task<List<AssetLoadForEditDto>> GetAssetDropDown()
        {
            var listAsset = _assetRepository.GetAll()
                     .Select(p => new AssetLoadForEditDto
                     {
                         AssetName = p.AssetName,
                         Id = p.Id,
                     });
            return listAsset.ToList();
        }
    }
}
