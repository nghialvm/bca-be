using dvd.bca.Entity.Recruitment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("JobPositions")]
    public class JobPosition : FullAuditedEntity<Guid>
    {
        [Required]
        [StringLength(50)]
        [Column("Code", TypeName = "varchar(50)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("Name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("DepartmentId", TypeName = "uniqueidentifier")]
        public Guid DepartmentId { get; set; }

        [Column("Description", TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }

        [Column("IsActive", TypeName = "bit")]
        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public virtual ICollection<RecruitmentRequest> RecruitmentRequests { get; set; } = new List<RecruitmentRequest>();
    }
}
