using dvd.bca.Employees.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.Employees
{
    public interface IEmployeeAppService : ICrudAppService<
        EmployeeDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateEmployeeDto,
        UpdateEmployeeDto>
    {
        Task<EmployeeDto> GetByCandidateIdAsync(Guid candidateId);
    }
}