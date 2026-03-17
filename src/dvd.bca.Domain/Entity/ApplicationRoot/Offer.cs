using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("Offers")]
    public class Offer : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public Guid ApplicationId { get; set; }

        [Required]
        [Column("Salary", TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Column("StartDate", TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column("ProbationMonths", TypeName = "int")]
        public int? ProbationMonths { get; set; }

        [StringLength(255)]
        [Column("WorkLocation", TypeName = "nvarchar(255)")]
        public string? WorkLocation { get; set; }

        [Column("Benefit", TypeName = "nvarchar(max)")]
        public string? Benefit { get; set; }

        [Column("Note", TypeName = "nvarchar(1000)")]
        public string? Note { get; set; }

        [Required]
        [Column("Status", TypeName = "int")]
        public OfferStatus Status { get; set; } = OfferStatus.Draft;

        [Column("SentTime", TypeName = "datetime2")]
        public DateTime? SentTime { get; set; }

        [Column("ExpiredTime", TypeName = "datetime2")]
        public DateTime? ExpiredTime { get; set; }

        // Navigation
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        public ICollection<CandidateResponse> CandidateResponses { get; set; } = new List<CandidateResponse>();
    }
}
