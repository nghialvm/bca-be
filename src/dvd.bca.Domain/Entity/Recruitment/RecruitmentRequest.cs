using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.Recruitment
{
    [Table("RecruitmentRequests")]
    public class RecruitmentRequest : FullAuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(50)]
        [Column("RequestCode", TypeName = "varchar(50)")]
        public string RequestCode { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("Title", TypeName = "nvarchar(255)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("DepartmentId", TypeName = "uniqueidentifier")]
        public Guid DepartmentId { get; set; }

        [Required]
        [Column("PositionId", TypeName = "uniqueidentifier")]
        public Guid PositionId { get; set; }

        [Column("Headcount", TypeName = "int")]
        public int Headcount { get; set; }

        [Required]
        [StringLength(100)]
        [Column("EmploymentType", TypeName = "nvarchar(100)")]
        public string EmploymentType { get; set; } = string.Empty;

        [StringLength(255)]
        [Column("WorkLocation", TypeName = "nvarchar(255)")]
        public string? WorkLocation { get; set; }

        [Column("SalaryMin", TypeName = "decimal(18,2)")]
        public decimal? SalaryMin { get; set; }

        [Column("SalaryMax", TypeName = "decimal(18,2)")]
        public decimal? SalaryMax { get; set; }

        [Column("Description", TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Column("Requirement", TypeName = "nvarchar(max)")]
        public string? Requirement { get; set; }

        [Column("Benefit", TypeName = "nvarchar(max)")]
        public string? Benefit { get; set; }

        [Column("ApplicationDeadline", TypeName = "datetime2")]
        public DateTime? ApplicationDeadline { get; set; }

        [Column("Status", TypeName = "int")]
        public RecruitmentRequestStatus Status { get; set; } = RecruitmentRequestStatus.Draft;

        [Column("CreatedByUserId", TypeName = "uniqueidentifier")]
        public Guid? CreatedByUserId { get; set; }

        [Column("ApprovedByUserId", TypeName = "uniqueidentifier")]
        public Guid? ApprovedByUserId { get; set; }

        [Column("ApprovedTime", TypeName = "datetime2")]
        public DateTime? ApprovedTime { get; set; }

        [Column("RejectReason", TypeName = "nvarchar(1000)")]
        public string? RejectReason { get; set; }

        [Column("PublishedTime", TypeName = "datetime2")]
        public DateTime? PublishedTime { get; set; }

        [Column("ClosedTime", TypeName = "datetime2")]
        public DateTime? ClosedTime { get; set; }


        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(PositionId))]
        public virtual JobPosition JobPosition { get; set; }

        // navigator
        public virtual ICollection<RecruitmentApproval> RecruitmentApprovals { get; set; } = new List<RecruitmentApproval>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
