using dvd.bca.Departments.Dtos;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.Departments
{
    public interface IDepartmentAppService :
        ICrudAppService<
            DepartmentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateDepartmentDto,
            UpdateDepartmentDto>
    {
    }
}
