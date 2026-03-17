using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.InterviewEvaluations.Dtos
{
    public class UpdateInterviewEvaluationDto
    {
        [Required]
        public Guid InterviewScheduleId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid EvaluatorUserId { get; set; }

        [Range(0, 100, ErrorMessage = "OverallScore must be between 0 and 100.")]
        public decimal? OverallScore { get; set; }

        [Required]
        public InterviewResult Result { get; set; }

        [StringLength(2000)]
        public string? Comment { get; set; }
    }
}
