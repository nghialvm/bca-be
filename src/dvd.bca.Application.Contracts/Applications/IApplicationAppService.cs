using dvd.bca.Applications.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.Applications
{
    public interface IApplicationAppService
        : ICrudAppService<
            ApplicationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateApplicationDto,
            UpdateApplicationDto>
    {
        Task<PagedResultDto<ApplicationDto>> GetListByRecruitmentRequestIdAsync(
            Guid recruitmentRequestId,
            PagedAndSortedResultRequestDto input);

        Task<PagedResultDto<ApplicationDto>> GetListByCandidateIdAsync(
            Guid candidateId,
            PagedAndSortedResultRequestDto input);
    }
}
