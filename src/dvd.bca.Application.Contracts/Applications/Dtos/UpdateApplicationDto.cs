using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.Applications.Dtos
{
    public class UpdateApplicationDto
    {
        [Required]
        [StringLength(50)]
        public string ApplicationCode { get; set; } = string.Empty;

        [Required]
        public Guid RecruitmentRequestId { get; set; }

        [Required]
        public Guid CandidateId { get; set; }

        [Required]
        public DateTime AppliedTime { get; set; }

        [Required]
        [EnumDataType(typeof(ApplicationStatus))]
        public ApplicationStatus Status { get; set; }

        public Guid? CVFileId { get; set; }

        [StringLength(500)]
        public string? SubmittedCvUrl { get; set; }

        [StringLength(100)]
        public string? Source { get; set; }

        public string? Note { get; set; }

        [StringLength(100)]
        public string? FinalResult { get; set; }
    }
}
