using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot
{
    [Table("Applications")]
    public class Application : FullAuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(50)]
        [Column("ApplicationCode", TypeName = "varchar(50)")]
        public string ApplicationCode { get; set; } = string.Empty;

        [Required]
        [Column("RecruitmentRequestId", TypeName = "uniqueidentifier")]
        public Guid RecruitmentRequestId { get; set; }

        [Required]
        [Column("CandidateId", TypeName = "uniqueidentifier")]
        public Guid CandidateId { get; set; }

        [Column("AppliedTime", TypeName = "datetime2")]
        public DateTime AppliedTime { get; set; }

        [Column("Status", TypeName = "int")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        [Column("CVFileId", TypeName = "uniqueidentifier")]
        public Guid? CVFileId { get; set; }

        [StringLength(500)]
        [Column("SubmittedCvUrl", TypeName = "nvarchar(500)")]
        public string? SubmittedCvUrl { get; set; }

        [StringLength(100)]
        [Column("Source", TypeName = "nvarchar(100)")]
        public string? Source { get; set; }

        [Column("Note", TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        [StringLength(100)]
        [Column("FinalResult", TypeName = "nvarchar(100)")]
        public string? FinalResult { get; set; }

        // Navigation
        [ForeignKey(nameof(RecruitmentRequestId))]
        public virtual RecruitmentRequest RecruitmentRequest { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public virtual Candidate Candidate { get; set; }

        public virtual ICollection<ApplicationScreening> ApplicationScreenings { get; set; } = new List<ApplicationScreening>();
        public virtual ICollection<InterviewSchedule> InterviewSchedules { get; set; } = new List<InterviewSchedule>();
        public virtual ICollection<InterviewEvaluation> InterviewEvaluations { get; set; } = new List<InterviewEvaluation>();
        public virtual ICollection<CandidateResponse> CandidateResponses { get; set; } = new List<CandidateResponse>();

        public virtual Offer Offer { get; set; }
    }
}
