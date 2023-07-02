using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;
using tmss.Master.EmployeesLearnedSafety;
using tmss.Master.TemEmployeesLearnedSafety.Dto;
using tmss.TemEmployeesLearnedSafety;

namespace tmss.Master.TemEmployeesLearnedSafety
{
    public class TemEmployeesLearnedSafetyAppService : tmssAppServiceBase, ITemEmployeesLearnedSafety
    {
        private readonly IRepository<MstTemEmployeesLearnedSafety, long> _temEmployeesLearnedSafetyRepository;
        private readonly IRepository<MstEmployeesLearnedSafety, long> _employeesLearnedSafetyRepository;
        public TemEmployeesLearnedSafetyAppService(IRepository<MstTemEmployeesLearnedSafety, long> temEmployeesLearnedSafetyRepository,
            IRepository<MstEmployeesLearnedSafety, long> employeesLearnedSafetyRepository)
        {
            _temEmployeesLearnedSafetyRepository = temEmployeesLearnedSafetyRepository;
            _employeesLearnedSafetyRepository = employeesLearnedSafetyRepository;
        }

        public async Task DeleteAll()
        {
            List<MstTemEmployeesLearnedSafety> mstTemEmployeesLearnedSafetyDelete = await _temEmployeesLearnedSafetyRepository.GetAll().AsNoTracking().Where(e => e.CreatorUserId == AbpSession.UserId).ToListAsync();
            foreach (MstTemEmployeesLearnedSafety mstTem in mstTemEmployeesLearnedSafetyDelete)
            {
                await _temEmployeesLearnedSafetyRepository.DeleteAsync(mstTem.Id);
            }
        }

        public async Task<PagedResultDto<TemEmployeesLearnedSafetySelectOutputDto>> LoadAll(TemEmployeesLearnedSafetyInputDto input)
        {
            var listTemEmployeesLearned = from temEmployees in _temEmployeesLearnedSafetyRepository.GetAll().AsNoTracking()
                                          .Where(e => (string.IsNullOrWhiteSpace(input.EmployeesName) || e.EmployeesName.Contains(input.EmployeesName))
                                          && e.CreatorUserId== AbpSession.UserId)
                                          select new TemEmployeesLearnedSafetySelectOutputDto
                                          {
                                              Id=temEmployees.Id,
                                              EmployeesName=temEmployees.EmployeesName,
                                              CourceId=temEmployees.CourceId,
                                              IdentityCard=temEmployees.IdentityCard,
                                              Validate=temEmployees.Validate,
                                              Seq=temEmployees.Seq
                                          };
            var result = listTemEmployeesLearned.Skip(input.SkipCount).Take(input.MaxResultCount);
            var totalCount = listTemEmployeesLearned.Count();
            return new PagedResultDto<TemEmployeesLearnedSafetySelectOutputDto>
            (totalCount,
            await result.ToListAsync()
            );
        }

        public async Task<List<EmployeesLearnedSafetyImportDto>> SaveAllImport(List<EmployeesLearnedSafetyImportDto> input, long courceId)
        {
            MstEmployeesLearnedSafety mstEmployeesLearnedSafety = new MstEmployeesLearnedSafety();
            List<EmployeesLearnedSafetyImportDto> listEmployeesVal = new List<EmployeesLearnedSafetyImportDto>();
            if (input.Count!=0)
            {
                foreach (EmployeesLearnedSafetyImportDto employeesLearnedSafetyImport in input)
                {
                    if(employeesLearnedSafetyImport.Validate!=null)
                    {
                        listEmployeesVal.Add(employeesLearnedSafetyImport);
                    }
                   else
                    {
                        mstEmployeesLearnedSafety = new MstEmployeesLearnedSafety();
                        mstEmployeesLearnedSafety.Id = 0;
                        mstEmployeesLearnedSafety.EmployeesName = employeesLearnedSafetyImport.EmployeesName;
                        mstEmployeesLearnedSafety.PhoneNumber = employeesLearnedSafetyImport.PhoneNumber;
                        mstEmployeesLearnedSafety.Address = employeesLearnedSafetyImport.Address;
                        mstEmployeesLearnedSafety.Gender = (employeesLearnedSafetyImport.Gender == "Nam") ? true : false;
                        mstEmployeesLearnedSafety.CourceId = courceId;
                        mstEmployeesLearnedSafety.IdentityCard = employeesLearnedSafetyImport.IdentityCard;
                        mstEmployeesLearnedSafety.VenderId = employeesLearnedSafetyImport.VenderId;
                        mstEmployeesLearnedSafety.PersonInCharge = employeesLearnedSafetyImport.PersonInCharge;
                        mstEmployeesLearnedSafety.Image = employeesLearnedSafetyImport.Image;
                        mstEmployeesLearnedSafety.CreationTime = DateTime.Now;
                        mstEmployeesLearnedSafety.CreatorUserId = AbpSession.UserId;
                        await _employeesLearnedSafetyRepository.InsertAsync(mstEmployeesLearnedSafety);
                    }
                }
            }
            return listEmployeesVal;
        }
    }
}
