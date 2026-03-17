using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("InterviewEvaluations")]
    public class InterviewEvaluation : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("InterviewScheduleId", TypeName = "uniqueidentifier")]
        public Guid InterviewScheduleId { get; set; }

        [Required]
        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public Guid ApplicationId { get; set; }

        [Required]
        [Column("EvaluatorUserId", TypeName = "uniqueidentifier")]
        public Guid EvaluatorUserId { get; set; }

        [Column("OverallScore", TypeName = "decimal(18,2)")]
        public decimal? OverallScore { get; set; }

        [Required]
        [Column("Result", TypeName = "int")]
        public InterviewResult Result { get; set; }

        [Column("Comment", TypeName = "nvarchar(2000)")]
        public string? Comment { get; set; }

        // Navigation
        [ForeignKey(nameof(InterviewScheduleId))]
        public virtual InterviewSchedule InterviewSchedule { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
    }
}
