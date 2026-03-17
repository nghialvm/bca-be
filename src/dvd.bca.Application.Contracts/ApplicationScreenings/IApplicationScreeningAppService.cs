using dvd.bca.ApplicationScreenings.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.ApplicationScreenings
{
    public interface IApplicationScreeningAppService
        : ICrudAppService<
            ApplicationScreeningDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateApplicationScreeningDto,
            UpdateApplicationScreeningDto>
    {
        Task<List<ApplicationScreeningDto>> GetListByApplicationIdAsync(Guid applicationId);

        Task<ApplicationScreeningDto> RecordScreeningResultAsync(CreateApplicationScreeningDto input);

        Task<ApplicationScreeningDto> ChangeScreeningResultAsync(Guid id, UpdateApplicationScreeningDto input);
    }
}
