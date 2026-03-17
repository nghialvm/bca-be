using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("CandidateResponses")]
    public class CandidateResponse : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public Guid ApplicationId { get; set; }

        [Column("OfferId", TypeName = "uniqueidentifier")]
        public Guid? OfferId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("ResponseType", TypeName = "nvarchar(100)")]
        public string ResponseType { get; set; } = string.Empty;

        [Required]
        [Column("ResponseTime", TypeName = "datetime2")]
        public DateTime ResponseTime { get; set; }

        [Column("Note", TypeName = "nvarchar(1000)")]
        public string? Note { get; set; }

        // Navigation
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }

        [ForeignKey(nameof(OfferId))]
        public virtual Offer Offer { get; set; }    
    }
}
