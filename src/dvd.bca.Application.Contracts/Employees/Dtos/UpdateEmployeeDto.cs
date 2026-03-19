using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.Employees.Dtos
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(50)]
        public string EmployeeCode { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        public Guid CandidateId { get; set; }

        public Guid? ApplicationId { get; set; }

        public Guid? OfferId { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid JobPositionId { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        public EmployeeStatus Status { get; set; }

        [Required]
        public EmployeeSourceType SourceType { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string WorkEmail { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }
    }
}
