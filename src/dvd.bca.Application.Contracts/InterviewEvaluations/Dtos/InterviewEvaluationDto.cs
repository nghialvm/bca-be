using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.InterviewEvaluations.Dtos
{
    public class InterviewEvaluationDto : FullAuditedEntityDto<Guid>
    {
        public Guid InterviewScheduleId { get; set; }

        public Guid ApplicationId { get; set; }

        public Guid EvaluatorUserId { get; set; }

        public decimal? OverallScore { get; set; }

        public InterviewResult Result { get; set; }

        public string? Comment { get; set; }
    }
}
