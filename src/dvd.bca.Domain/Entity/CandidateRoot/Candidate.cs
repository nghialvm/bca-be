using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.CandidateRoot
{
    [Table("Candidates")]
    public class Candidate : FullAuditedAggregateRoot<Guid>
    {
        protected Candidate()
        {
        }

        public Candidate(Guid id)
            : base(id)
        {
        }

        [Required]
        [StringLength(50)]
        [Column("CandidateCode", TypeName = "varchar(50)")]
        public string CandidateCode { get; set; } = string.Empty;

        [Column("CandidateType")]
        public CandidateType? CandidateType { get; set; }

        [Column("EmployeeId", TypeName = "uniqueidentifier")]
        public Guid? EmployeeId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("FullName", TypeName = "nvarchar(255)")]
        public string FullName { get; set; } = string.Empty;

        [Column("DateOfBirth", TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [Column("Gender")]
        public Gender? Gender { get; set; }

        [StringLength(20)]
        [Column("PhoneNumber", TypeName = "varchar(20)")]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        [Column("Email", TypeName = "varchar(255)")]
        public string? Email { get; set; }

        [StringLength(500)]
        [Column("Address", TypeName = "nvarchar(500)")]
        public string? Address { get; set; }

        [StringLength(50)]
        [Column("IdentityNumber", TypeName = "varchar(50)")]
        public string? IdentityNumber { get; set; }

        [StringLength(255)]
        [Column("CurrentCompany", TypeName = "nvarchar(255)")]
        public string? CurrentCompany { get; set; }

        [StringLength(255)]
        [Column("CurrentPosition", TypeName = "nvarchar(255)")]
        public string? CurrentPosition { get; set; }

        [Column("YearsOfExperience", TypeName = "int")]
        public int? YearsOfExperience { get; set; }

        [StringLength(255)]
        [Column("HighestEducation", TypeName = "nvarchar(255)")]
        public string? HighestEducation { get; set; }

        [StringLength(255)]
        [Column("UniversityName", TypeName = "nvarchar(255)")]
        public string? UniversityName { get; set; }

        [StringLength(255)]
        [Column("Major", TypeName = "nvarchar(255)")]
        public string? Major { get; set; }

        [Column("Status")]
        public CandidateStatus? Status { get; set; }

        [StringLength(100)]
        [Column("Source", TypeName = "nvarchar(100)")]
        public string? Source { get; set; }

        [Column("Note", TypeName = "nvarchar(max)")]
        public string? Note { get; set; }

        // Navigation
        public virtual ICollection<CandidateDocument> CandidateDocuments { get; set; } = new List<CandidateDocument>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
