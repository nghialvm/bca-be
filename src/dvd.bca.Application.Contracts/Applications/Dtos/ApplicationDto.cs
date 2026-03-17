using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.Applications.Dtos
{
    public class ApplicationDto : EntityDto<Guid>
    {
        public string ApplicationCode { get; set; }

        public Guid RecruitmentRequestId { get; set; }

        public Guid CandidateId { get; set; }

        public DateTime AppliedTime { get; set; }

        public ApplicationStatus Status { get; set; }

        public Guid? CVFileId { get; set; }

        public string? SubmittedCvUrl { get; set; }

        public string? Source { get; set; }

        public string? Note { get; set; }

        public string? FinalResult { get; set; }
    }
}
