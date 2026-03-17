using dvd.bca.InterviewEvaluations.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.InterviewEvaluations
{
    public interface IInterviewEvaluationAppService :
        ICrudAppService<
            InterviewEvaluationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateInterviewEvaluationDto,
            UpdateInterviewEvaluationDto>
    {
        Task<List<InterviewEvaluationDto>> GetListByInterviewScheduleIdAsync(Guid interviewScheduleId);

        Task<List<InterviewEvaluationDto>> GetListByApplicationIdAsync(Guid applicationId);

        Task<InterviewEvaluationDto> RecordEvaluationResultAsync(CreateInterviewEvaluationDto input);

        Task<InterviewEvaluationDto> ChangeEvaluationResultAsync(Guid id, UpdateInterviewEvaluationDto input);
    }
}
