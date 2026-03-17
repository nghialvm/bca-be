using dvd.bca.Entity.Recruitment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace dvd.bca.Entity.ApplicationRoot

{
    [Table("Departments")]
    public class Department : FullAuditedEntity<Guid>
    {
        [Required]
        [StringLength(50)]
        [Column("Code", TypeName = "varchar(50)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("Name", TypeName = "nvarchar(255)")]
        public string Name { get; set; } = string.Empty;

        [Column("ManagerUserId", TypeName = "uniqueidentifier")]
        public Guid? ManagerUserId { get; set; }

        [Column("Description", TypeName = "nvarchar(1000)")]
        public string? Description { get; set; }

        [Column("IsActive", TypeName = "bit")]
        public bool IsActive { get; set; } = true;
        public virtual ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();
        public virtual ICollection<RecruitmentRequest> RecruitmentRequests { get; set; } = new List<RecruitmentRequest>();
    }
}
