using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using dvd.bca.Enums;


namespace dvd.bca.Entity.Results
{
    [Table("Employees")]
    public class Employee : FullAuditedAggregateRoot<Guid>
    {
        [Required]
        [StringLength(50)]
        [Column("EmployeeCode", TypeName = "varchar(50)")]
        public virtual string EmployeeCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column("FullName", TypeName = "nvarchar(255)")]
        public virtual string FullName { get; set; }

        [Required]
        [Column("CandidateId", TypeName = "uniqueidentifier")]
        public virtual Guid CandidateId { get; set; }

        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public virtual Guid? ApplicationId { get; set; }

        [Column("OfferId", TypeName = "uniqueidentifier")]
        public virtual Guid? OfferId { get; set; }

        [Required]
        [Column("DepartmentId", TypeName = "uniqueidentifier")]
        public virtual Guid DepartmentId { get; set; }

        [Required]
        [Column("JobPositionId", TypeName = "uniqueidentifier")]
        public virtual Guid JobPositionId { get; set; }

        [Required]
        [Column("JoinDate", TypeName = "datetime2")]
        public virtual DateTime JoinDate { get; set; }

        [Required]
        [Column("Status", TypeName = "int")]
        public virtual EmployeeStatus Status { get; set; }

        [Required]
        [Column("SourceType", TypeName = "int")]
        public virtual EmployeeSourceType SourceType { get; set; }

        [StringLength(255)]
        [Column("WorkEmail", TypeName = "varchar(255)")]
        public virtual string WorkEmail { get; set; }

        [StringLength(1000)]
        [Column("Note", TypeName = "nvarchar(1000)")]
        public virtual string Note { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }

        [ForeignKey(nameof(OfferId))]
        public virtual Offer Offer { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(JobPositionId))]
        public virtual JobPosition JobPosition { get; set; }
    }
}