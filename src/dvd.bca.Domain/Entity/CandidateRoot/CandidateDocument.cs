using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.CandidateRoot
{
    [Table("CandidateDocuments")]
    public class CandidateDocument : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("CandidateId", TypeName = "uniqueidentifier")]
        public Guid CandidateId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("DocumentType", TypeName = "nvarchar(100)")]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("FileName", TypeName = "nvarchar(255)")]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Column("FilePath", TypeName = "varchar(500)")]
        public string FilePath { get; set; } = string.Empty;

        [Column("FileSize", TypeName = "bigint")]
        public long FileSize { get; set; }

        [StringLength(100)]
        [Column("ContentType", TypeName = "varchar(100)")]
        public string? ContentType { get; set; }

        [Column("Description", TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }

        // Navigation
        [ForeignKey(nameof(CandidateId))]
        public virtual Candidate Candidate { get; set; }
    }
}
