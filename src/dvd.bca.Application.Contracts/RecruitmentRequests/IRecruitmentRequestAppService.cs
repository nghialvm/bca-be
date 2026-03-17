using dvd.bca.RecruitmentRequests.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.RecruitmentRequests
{
    public interface IRecruitmentRequestAppService
        : ICrudAppService<
            RecruitmentRequestDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateRecruitmentRequestDto,
            UpdateRecruitmentRequestDto>
    {
        Task<RecruitmentRequestDto> SubmitForApprovalAsync(Guid id);
        Task<RecruitmentRequestDto> ApproveAsync(Guid id);
        Task<RecruitmentRequestDto> RejectAsync(Guid id, RejectRecruitmentRequestDto input);
        Task<RecruitmentRequestDto> PublishAsync(Guid id);
        Task<RecruitmentRequestDto> CloseAsync(Guid id, CloseRecruitmentRequestDto input);
    }
}
