using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tmss.Master.Employees.Dto;
using tmss.Master.Vender;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Employees
{
    public class EmployeesAppService : tmssAppServiceBase, IEmployeesAppService
    {
        private readonly IRepository<MstEmployees, long> _employeesRepository;
        private readonly IRepository<MstVender, long> _venderRepository;
        public EmployeesAppService(IRepository<MstEmployees, long> employeesRepository, IRepository<MstVender, long> venderRepository)
        {
            _employeesRepository = employeesRepository;
            _venderRepository = venderRepository;
        }
        public async Task DeleteById(long id)
        {
            await _employeesRepository.DeleteAsync(id);
        }

        public async Task<List<GetVenderForEditEmployeesDto>> GetVenderForEdit()
        {
            var listVender = _venderRepository.GetAll()
               .Select(p => new GetVenderForEditEmployeesDto
               {
                   Id = p.Id,
                   VenderName = p.VenderName
               });
            return listVender.ToList();
        }

        public async Task<PagedResultDto<EmployeesSelectOutputDto>> LoadAll(EmployeesInputDto input)
        {
            var listEmployees = from employee in _employeesRepository.GetAll()
                                 .Where(e => string.IsNullOrWhiteSpace(input.Filter) || e.EmployeesName.Contains(input.Filter))
                                join vender in _venderRepository.GetAll()
                                .Where(e => input.VenderId == 0 || e.Id == input.VenderId)
                                on employee.VenderId equals vender.Id
                                select new EmployeesSelectOutputDto
                                {
                                    Id = employee.Id,
                                    Address = employee.Address,
                                    EmployeesName = employee.EmployeesName,
                                    PhoneNumber = employee.PhoneNumber,
                                    VenderName = vender.VenderName
                                };
            var result = listEmployees.Skip(input.SkipCount).Take(input.MaxResultCount);
            var employCount = listEmployees.Count();
            return new PagedResultDto<EmployeesSelectOutputDto>
                (employCount,
                result.ToList()
                );
        }

        public async Task<EmployeesLoadByIdDto> LoadById(long id)
        {
            var employees = _employeesRepository.GetAll()
                .Select(p => new EmployeesLoadByIdDto { Id = p.Id, EmployeesName = p.EmployeesName, Address = p.Address, PhoneNumber = p.PhoneNumber, VenderId = p.VenderId })
                .FirstOrDefault(p => p.Id == id);          
            return employees;
        }

        public async Task Save(EmployessSaveDto input)
        {
            if (await _employeesRepository.FirstOrDefaultAsync(input.Id) == null)
            {
                MstEmployees mstEmployees = new MstEmployees();
                mstEmployees.Id = input.Id;
                mstEmployees.EmployeesName = input.EmployeesName;
                mstEmployees.Address = input.Address;
                mstEmployees.PhoneNumber = input.PhoneNumber;
                mstEmployees.VenderId = input.VenderId;
                mstEmployees.CreationTime = DateTime.Now;
                mstEmployees.CreatorUserId = AbpSession.UserId;
                await _employeesRepository.InsertAsync(mstEmployees);
            }
            else
            {
                MstEmployees mstEmployees = await _employeesRepository.FirstOrDefaultAsync(input.Id);
                mstEmployees.EmployeesName = input.EmployeesName;
                mstEmployees.Address = input.Address;
                mstEmployees.PhoneNumber = input.PhoneNumber;
                mstEmployees.VenderId = input.VenderId;
                mstEmployees.LastModificationTime = DateTime.Now;
                mstEmployees.LastModifierUserId = AbpSession.UserId;
                await _employeesRepository.UpdateAsync(mstEmployees);
            }
        }
    }
}
