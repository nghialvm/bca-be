using dvd.bca.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.Recruitment
{
    [Table("recruitmentApprovals")]
    public class RecruitmentApproval : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("recruitmentRequestId")]
        public Guid RecruitmentRequestId { get; set; }

        [Required]
        [Column("approverUserId")]
        public Guid ApproverUserId { get; set; }

        [Column("approvalAction")]
        public ApprovalAction ApprovalAction { get; set; }

        [Column("comment")]
        public string? Comment { get; set; }

        [Column("actionTime")]
        public DateTime ActionTime { get; set; }

        [Column("stepOrder")]
        public int StepOrder { get; set; }

        // Navigation
        [ForeignKey(nameof(RecruitmentRequestId))]
        public virtual RecruitmentRequest RecruitmentRequest { get; set; }
    }
}
