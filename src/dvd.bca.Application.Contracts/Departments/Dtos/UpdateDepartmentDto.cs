using dvd.bca.Entity.ApplicationRoot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.AutoMapper;

namespace dvd.bca.Departments.Dtos
{
    public class UpdateDepartmentDto
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public Guid? ManagerUserId { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
