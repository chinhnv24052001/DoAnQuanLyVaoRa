using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.EmployeesLearnedSafety.Dto;
using tmss.Master.Vender;

namespace tmss.Master.EmployeesLearnedSafety
{
    public class EmployeesLearnedSafetyAppService : tmssAppServiceBase, IEmployeesLearnedSafetyAppService
    {
        private readonly IRepository<MstEmployeesLearnedSafety, long> _employeesLearnedSafetyRepository;
        private readonly IRepository<MstVender, long> _venderRepository;
        private IWebHostEnvironment _env;
        public EmployeesLearnedSafetyAppService(IRepository<MstEmployeesLearnedSafety, long> employeesLearnedSafetyRepository, IRepository<MstVender, long> venderRepository,
            IWebHostEnvironment env)
        {
            _employeesLearnedSafetyRepository = employeesLearnedSafetyRepository;
            _venderRepository = venderRepository;
            _env = env;
        }

        public async Task DeleteById(int[] listId)
        {
            foreach (var i in listId)
            {
                await _employeesLearnedSafetyRepository.DeleteAsync(i);
            }
        }

        public async Task<PagedResultDto<EmployeesLearnedSafetySelectOutputDto>> LoadAllByCourceId(EmployeesLearnedSafetyInputDto input)
        {
            var x = _env.ContentRootPath;
            //var listEmployeesLearnedSafety = from employee in _employeesLearnedSafetyRepository.GetAll().AsNoTracking()
            //                                  .Where(e => (string.IsNullOrWhiteSpace(input.EmployeesName) || e.EmployeesName.Contains(input.EmployeesName))
            //                                 && (string.IsNullOrWhiteSpace(input.PersonInCharge) || e.PersonInCharge.Contains(input.PersonInCharge))
            //                                 && (string.IsNullOrWhiteSpace(input.IdentityCard) || e.IdentityCard.Contains(input.IdentityCard))
            //                                 && e.CourceId == input.CourceId )
            //                                 join vender in _venderRepository.GetAll().AsNoTracking()
            //                                 on employee.Vender equals vender.Id
            //                                 select new EmployeesLearnedSafetySelectOutputDto
            //                                 {
            //                                     Id = employee.Id,
            //                                     EmployeesName = employee.EmployeesName,
            //                                     CourceId = employee.Id,
            //                                     IdentityCard = employee.IdentityCard,
            //                                     Gender = employee.Gender ? L("Man") : L("Woman"),
            //                                     PhoneNumber = employee.PhoneNumber,
            //                                     Address=employee.Address,
            //                                     VenderName=vender.VenderName,
            //                                     PersonInCharge=employee.PersonInCharge,
            //                                     ViewImage = employee.Image
            //                                 };

            var listEmployeesLearnedSafety = from employee in _employeesLearnedSafetyRepository.GetAll().AsNoTracking()
                                              .Where(e => (string.IsNullOrWhiteSpace(input.EmployeesName) || e.EmployeesName.Contains(input.EmployeesName))
                                             && (string.IsNullOrWhiteSpace(input.PersonInCharge) || e.PersonInCharge.Contains(input.PersonInCharge))
                                             && (string.IsNullOrWhiteSpace(input.IdentityCard) || e.IdentityCard.Contains(input.IdentityCard)))
                                             join vender in _venderRepository.GetAll().AsNoTracking()
                                             on employee.VenderId equals vender.Id
                                             select new EmployeesLearnedSafetySelectOutputDto
                                             {
                                                 Id = employee.Id,
                                                 EmployeesName = employee.EmployeesName,
                                                 //CourceId = employee.Id,
                                                 IdentityCard = employee.IdentityCard,
                                                 Gender = employee.Gender ? L("Man") : L("Woman"),
                                                 PhoneNumber = employee.PhoneNumber,
                                                 Address = employee.Address,
                                                 VenderName = vender.VenderName,
                                                 PersonInCharge = employee.PersonInCharge,
                                                 ViewImage = employee.Image,
                                                 LearnedSafetyST = employee.IsLearnedSafety.Value ? L("LearnedSafety") : L("DontLearnedSafety"),
                                                 IsLearnedSafety = employee.IsLearnedSafety.Value,
                                                 EffecttivateDate = employee.EffecttivateDate
                                             };

            var result = listEmployeesLearnedSafety.Skip(input.SkipCount).Take(input.MaxResultCount);
            var totalCount = listEmployeesLearnedSafety.Count();
            return new PagedResultDto<EmployeesLearnedSafetySelectOutputDto>
            (totalCount,
            await result.ToListAsync()
            );
        }

        public async Task<bool> SetEmployeesLearnedSafety(List<long> listId)
        {
            foreach (var i in listId) 
            {
                var employees = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(i);
                if (employees != null)
                {
                    //var y = employees.EffecttivateDate;
                    employees.EffecttivateDate = DateTime.Now.AddMonths(6);
                    employees.IsLearnedSafety = true;
                    await _employeesLearnedSafetyRepository.UpdateAsync(employees);
                }
            }
           
            return true;
        }

        public async Task<bool> SetEmployeesNotLearnedSafety(List<long> listId)
        {
            foreach (var i in listId)
            {
                var employees = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(i);
                if (employees != null)
                {
                    //var y = employees.EffecttivateDate;
                    employees.EffecttivateDate = DateTime.Now;
                    employees.IsLearnedSafety = false;
                    await _employeesLearnedSafetyRepository.UpdateAsync(employees);
                }
            }

            return true;
        }

        public async Task<EmployeesLearnedSafetyLoadForEditDto> LoadById(long id)
        {
            var employeesLearnedSafety = _employeesLearnedSafetyRepository.GetAll()
                .Select(p => new EmployeesLearnedSafetyLoadForEditDto
                {
                    Id = p.Id,
                    EmployeesName = p.EmployeesName,
                    //CourceId = p.CourceId,
                    IdentityCard = p.IdentityCard,
                    Gender=p.Gender? 1:2,
                    PhoneNumber = p.PhoneNumber,
                    Address=p.Address, 
                    VenderId=p.VenderId,
                    PersonInCharge=p.PersonInCharge
                }).FirstOrDefault(p=> p.Id == id);
                return employeesLearnedSafety;
        }

        public async Task Save(EmployeesLearnedSafetySaveDto input)
        {
            if (await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(input.Id)==null)
            {
                MstEmployeesLearnedSafety mstEmployeesLearnedSafety = new MstEmployeesLearnedSafety();
                mstEmployeesLearnedSafety.Id=input.Id;
                mstEmployeesLearnedSafety.EmployeesName = input.EmployeesName;
                //mstEmployeesLearnedSafety.CourceId = input.CourceId;
                mstEmployeesLearnedSafety.CourceId = 0;
                mstEmployeesLearnedSafety.IdentityCard = input.IdentityCard;
                mstEmployeesLearnedSafety.Gender = input.Gender==1? true: false;
                mstEmployeesLearnedSafety.PhoneNumber = input.PhoneNumber;
                mstEmployeesLearnedSafety.Address = input.Address;
                mstEmployeesLearnedSafety.VenderId = input.VenderId;
                mstEmployeesLearnedSafety.PersonInCharge=input.PersonInCharge;
                mstEmployeesLearnedSafety.CreatorUserId = AbpSession.UserId; 
                mstEmployeesLearnedSafety.EffecttivateDate = DateTime.Now;  
                mstEmployeesLearnedSafety.CreationTime=DateTime.Now;
                mstEmployeesLearnedSafety.IsLearnedSafety=false;
                await _employeesLearnedSafetyRepository.InsertAsync(mstEmployeesLearnedSafety);
            }
            else
            {
                MstEmployeesLearnedSafety mstEmployeesLearnedSafety = await _employeesLearnedSafetyRepository.FirstOrDefaultAsync(input.Id);
                mstEmployeesLearnedSafety.EmployeesName = input.EmployeesName;
                //mstEmployeesLearnedSafety.CourceId = input.CourceId;
                mstEmployeesLearnedSafety.IdentityCard = input.IdentityCard;
                mstEmployeesLearnedSafety.Gender = input.Gender == 1 ? true : false;
                mstEmployeesLearnedSafety.PhoneNumber = input.PhoneNumber;
                mstEmployeesLearnedSafety.Address = input.Address;
                mstEmployeesLearnedSafety.VenderId = input.VenderId;
                mstEmployeesLearnedSafety.PersonInCharge = input.PersonInCharge;
                mstEmployeesLearnedSafety.LastModifierUserId = AbpSession.UserId;
                mstEmployeesLearnedSafety.LastModificationTime = DateTime.Now;
                await _employeesLearnedSafetyRepository.UpdateAsync(mstEmployeesLearnedSafety);
            }
        }

        public async Task<bool> UpdateFilePath(long id, string image)
        {
            MstEmployeesLearnedSafety mstEmployeesLearnedSafety = _employeesLearnedSafetyRepository.Get(id);
            if (mstEmployeesLearnedSafety != null)
            {
                mstEmployeesLearnedSafety.Image = image;

               await _employeesLearnedSafetyRepository.UpdateAsync(mstEmployeesLearnedSafety);
                return true;
            } else
            {
                return false;
            }
        }

    }
}


