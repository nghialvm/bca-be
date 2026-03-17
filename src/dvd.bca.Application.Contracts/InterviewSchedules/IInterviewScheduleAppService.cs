using dvd.bca.InterviewSchedules.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.InterviewSchedules
{
    public interface IInterviewScheduleAppService
        : ICrudAppService<
            InterviewScheduleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateInterviewScheduleDto,
            UpdateInterviewScheduleDto>
    {
        Task<PagedResultDto<InterviewScheduleDto>> GetListByApplicationIdAsync(Guid applicationId);
    }
}