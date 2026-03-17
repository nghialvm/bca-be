using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.InterviewSchedules.Dtos
{
    public class CreateInterviewScheduleDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "RoundNumber must be greater than 0.")]
        public int RoundNumber { get; set; }

        [Required]
        [EnumDataType(typeof(InterviewType))]
        public InterviewType InterviewType { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        [Range(1, 1440, ErrorMessage = "DurationMinutes must be between 1 and 1440.")]
        public int DurationMinutes { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }

        [StringLength(500)]
        public string? MeetingLink { get; set; }

        [StringLength(255)]
        public string? ContactPerson { get; set; }

        [StringLength(1000)]
        public string? Note { get; set; }

        [Required]
        [EnumDataType(typeof(InterviewStatus))]
        public InterviewStatus Status { get; set; } = InterviewStatus.Pending;

        public Guid? CreatedByUserId { get; set; }
    }
}
