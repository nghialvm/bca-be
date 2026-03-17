using dvd.bca.JobPositions.Dtos;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.JobPositions
{
    public interface IJobPositionAppService :
        ICrudAppService<
            JobPositionDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateJobPositionDto,
            UpdateJobPositionDto>
    {
    }
}
