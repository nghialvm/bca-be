using dvd.bca.Entity.ApplicationRoot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.JobPositions.Dtos
{
    public class UpdateJobPositionDto
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
