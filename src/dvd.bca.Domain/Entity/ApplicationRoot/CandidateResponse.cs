using dvd.bca.Enums;
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
        [Column("ResponseType", TypeName = "int")]
        public CandidateResponseType ResponseType { get; set; }

        [Required]
        [Column("ResponseChannel", TypeName = "int")]
        public CandidateResponseChannel ResponseChannel { get; set; }

        [Required]
        [Column("ResponseTime", TypeName = "datetime2")]
        public DateTime ResponseTime { get; set; }

        [StringLength(2000)]
        [Column("ResponseContent", TypeName = "nvarchar(2000)")]
        public string? ResponseContent { get; set; }

        [StringLength(1000)]
        [Column("Note", TypeName = "nvarchar(1000)")]
        public string? Note { get; set; }

        public virtual Application Application { get; set; }
        public virtual Offer Offer { get; set; }
    }
}
