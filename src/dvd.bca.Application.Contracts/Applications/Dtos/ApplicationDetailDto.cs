using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Applications.Dtos
{
    public class ApplicationDetailDto : ApplicationDto
    {
        public string? CandidateCode { get; set; }
        public string? CandidateName { get; set; }
        public string? RecruitmentRequestCode { get; set; }
        public string? RecruitmentRequestTitle { get; set; }
    }
}
