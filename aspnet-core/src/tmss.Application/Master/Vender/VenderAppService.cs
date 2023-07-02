using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Vender
{
    public class VenderAppService : tmssAppServiceBase, IVenderAppService
    {
        private readonly IRepository<MstVender, long> _venderRepository;
        public VenderAppService(IRepository<MstVender, long> venderRepository)
        {
            _venderRepository = venderRepository;
        }

        public async Task<int> CountVender()
        {
            var count =await _venderRepository.CountAsync();
            return count;
        }

        public async Task DeleteById(long id)
        {
            await _venderRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<VenderSelectOutputDto>> LoadAll(VenderInputDto input)
        {
            var listVender = from vender in _venderRepository.GetAll()
                             .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), p => p.VenderName.Contains(input.Filter))
                             select new VenderSelectOutputDto()
                             {
                                 Id = vender.Id,
                                 VenderName = vender.VenderName,
                                 Address = vender.Address,
                                 PhoneNumber = vender.PhoneNumber
                             };
            var result = listVender.Skip(input.SkipCount).Take(input.MaxResultCount);
            var venderCount = listVender.Count();
            return new PagedResultDto<VenderSelectOutputDto>
                (venderCount,
                result.ToList()
                );
        }

        public async Task<VenderSelectOutputDto> LoadById(long id)
        {
            var vender = _venderRepository.GetAll()
               .Select(p => new VenderSelectOutputDto { Id =p.Id, VenderName=p.VenderName, Address=p.Address, PhoneNumber=p.PhoneNumber })
               .FirstOrDefault(p => p.Id == id);
            return vender;
        }

        public async Task Save(VenderSaveDto input)
        {
            if (await _venderRepository.FirstOrDefaultAsync(input.Id) == null)
            {
                MstVender mstVender = new MstVender();
                mstVender.Id = input.Id;
                mstVender.VenderName = input.VenderName;
                mstVender.Address = input.Address;  
                mstVender.PhoneNumber = input.PhoneNumber;
                mstVender.CreationTime = DateTime.Now;
                mstVender.CreatorUserId = AbpSession.UserId;
                await _venderRepository.InsertAsync(mstVender);
            }
            else
            {
                MstVender mstVender = await _venderRepository.FirstOrDefaultAsync(input.Id);
                mstVender.VenderName = input.VenderName;
                mstVender.Address = input.Address;
                mstVender.PhoneNumber = input.PhoneNumber;
                mstVender.LastModificationTime = DateTime.Now;
                mstVender.LastModifierUserId = AbpSession.UserId;
                await _venderRepository.UpdateAsync(mstVender);
            }
        }
    }
}
