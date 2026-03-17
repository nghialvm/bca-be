using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("ApplicationScreenings")]
    public class ApplicationScreening : FullAuditedEntity<Guid>
    {
        [Required]
        [Column("ApplicationId", TypeName = "uniqueidentifier")]
        public Guid ApplicationId { get; set; }

        [Required]
        [Column("ScreenedByUserId", TypeName = "uniqueidentifier")]
        public Guid ScreenedByUserId { get; set; }

        [Required]
        [Column("ScreeningTime", TypeName = "datetime2")]
        public DateTime ScreeningTime { get; set; }

        [Required]
        [Column("Result", TypeName = "int")]
        public ScreeningResult Result { get; set; }

        [Column("Comment", TypeName = "nvarchar(1000)")]
        public string? Comment { get; set; }

        [Column("Score", TypeName = "decimal(18,2)")]
        public decimal? Score { get; set; }

        [Column("CriteriaSummary", TypeName = "nvarchar(max)")]
        public string? CriteriaSummary { get; set; }

        // Navigation
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
    }
}
