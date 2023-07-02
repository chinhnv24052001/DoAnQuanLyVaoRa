using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.AssetGroup.Dto;
using tmss.Master.CourceSafety.Dto;

namespace tmss.Master.CourceSafety
{
    public class CourceSafetyAppService : tmssAppServiceBase, ICourceSafetyAppService
    {
        private readonly IRepository<MstCourceSafety, long> _courceSafetyRepository;
        public CourceSafetyAppService(IRepository<MstCourceSafety, long> courceSafetyRepository)
        {
            _courceSafetyRepository = courceSafetyRepository;
        }

        public async Task<int> CountCource()
        {
            return await _courceSafetyRepository.CountAsync();
        }

        public async Task DeleteById(int id)
        {
           await _courceSafetyRepository.DeleteAsync(id);
        }

        public async Task<List<CourceSafetySaveDto>> GetCourseForAddEmployees()
        {
            var listCourse = _courceSafetyRepository.GetAll().Where(p=> DateTime.Compare(p.EffectiveDateEnd, DateTime.Now)  >= 0 )
                .Select(p => new CourceSafetySaveDto
                {
                    CourceName = p.CourceName,
                    Id = p.Id,
                });
            return listCourse.ToList();
        }

        public async Task<PagedResultDto<CourceSafetySelectOutputDto>> LoadAll(CourceSafetyInputDto input)
        {
            var listCourceSafety = from courceSafety in _courceSafetyRepository.GetAll().AsNoTracking()
                                   .Where(e => string.IsNullOrWhiteSpace(input.CourceName) || e.CourceName.Contains(input.CourceName))
                                   select new CourceSafetySelectOutputDto()
                                   {
                                       Id = courceSafety.Id,
                                       CourceName = courceSafety.CourceName,
                                       Description = courceSafety.Description,
                                       EffectiveDateStart = courceSafety.EffectiveDateStart,
                                       EffectiveDateEnd = courceSafety.EffectiveDateEnd,
                                   };
            var result = listCourceSafety.Skip(input.SkipCount).Take(input.MaxResultCount);
            var totalCount = listCourceSafety.Count();
            return new PagedResultDto<CourceSafetySelectOutputDto>
            (totalCount,
            await result.ToListAsync() 
            );
        }

        public async Task<CourceSafetyLoadForEditDto> LoadById(long id)
        {
            var courceSafety = _courceSafetyRepository.GetAll()
                .Select(e => new CourceSafetyLoadForEditDto
                {
                    Id=e.Id,
                    Description=e.Description,
                    CourceName = e.CourceName,
                    EffectiveDateStart = e.EffectiveDateStart,
                    EffectiveDateEnd = e.EffectiveDateEnd,
                }).FirstOrDefault(e=>e.Id==id);
            return courceSafety;
        }

        public async Task Save(CourceSafetySaveDto input)
        {
            if (await _courceSafetyRepository.FirstOrDefaultAsync(input.Id) == null)
            {
                MstCourceSafety mstCourceSafety = new MstCourceSafety();
                mstCourceSafety.Id = input.Id;
                mstCourceSafety.CourceName = input.CourceName;
                mstCourceSafety.Description = input.Description;
                mstCourceSafety.EffectiveDateStart = input.EffectiveDateStart;
                mstCourceSafety.EffectiveDateEnd = input.EffectiveDateEnd;
                mstCourceSafety.CreationTime = DateTime.Now;
                mstCourceSafety.CreatorUserId = AbpSession.UserId;
                await _courceSafetyRepository.InsertAsync(mstCourceSafety);
            }
            else
            {
                MstCourceSafety mstCourceSafety = await _courceSafetyRepository.FirstOrDefaultAsync(input.Id);
                mstCourceSafety.CourceName = input.CourceName;
                mstCourceSafety.Description = input.Description;
                mstCourceSafety.EffectiveDateStart = input.EffectiveDateStart;
                mstCourceSafety.EffectiveDateEnd = input.EffectiveDateEnd;
                mstCourceSafety.LastModificationTime = DateTime.Now;
                mstCourceSafety.LastModifierUserId = AbpSession.UserId;
                await _courceSafetyRepository.UpdateAsync(mstCourceSafety);
            }
        }
    }
}
