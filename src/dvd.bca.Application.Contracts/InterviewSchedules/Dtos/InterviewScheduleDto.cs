using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.InterviewSchedules.Dtos
{
    public class InterviewScheduleDto : FullAuditedEntityDto<Guid>
    {
        public Guid ApplicationId { get; set; }

        public int RoundNumber { get; set; }

        public InterviewType InterviewType { get; set; }

        public DateTime ScheduledTime { get; set; }

        public int DurationMinutes { get; set; }

        public string? Location { get; set; }

        public string? MeetingLink { get; set; }

        public string? ContactPerson { get; set; }

        public string? Note { get; set; }

        public InterviewStatus Status { get; set; }

        public Guid? CreatedByUserId { get; set; }
    }
}
