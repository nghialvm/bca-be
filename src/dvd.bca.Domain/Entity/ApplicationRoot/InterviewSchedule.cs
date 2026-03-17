using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("InterviewSchedules")]
    public class InterviewSchedule : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public Guid ApplicationId { get; set; }

        [Column("RoundNumber", TypeName = "int")]
        public int RoundNumber { get; set; }

        [Required]
        [Column("InterviewType", TypeName = "int")]
        public InterviewType InterviewType { get; set; }

        [Required]
        [Column("ScheduledTime", TypeName = "datetime2")]
        public DateTime ScheduledTime { get; set; }

        [Column("DurationMinutes", TypeName = "int")]
        public int DurationMinutes { get; set; }

        [StringLength(255)]
        [Column("Location", TypeName = "nvarchar(255)")]
        public string? Location { get; set; }

        [StringLength(500)]
        [Column("MeetingLink", TypeName = "varchar(500)")]
        public string? MeetingLink { get; set; }

        [StringLength(255)]
        [Column("ContactPerson", TypeName = "nvarchar(255)")]
        public string? ContactPerson { get; set; }

        [Column("Note", TypeName = "nvarchar(1000)")]
        public string? Note { get; set; }

        [Required]
        [Column("Status", TypeName = "int")]
        public InterviewStatus Status { get; set; }

        [Column("CreatedByUserId", TypeName = "uniqueidentifier")]
        public Guid? CreatedByUserId { get; set; }

        // Navigation
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }

        public virtual ICollection<InterviewEvaluation> InterviewEvaluations { get; set; } = new List<InterviewEvaluation>();
    }
}
