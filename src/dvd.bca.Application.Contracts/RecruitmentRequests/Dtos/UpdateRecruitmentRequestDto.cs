using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.RecruitmentRequests.Dtos
{
    public class UpdateRecruitmentRequestDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid PositionId { get; set; }

        [Range(1, int.MaxValue)]
        public int Headcount { get; set; }

        [Required]
        [StringLength(50)]
        public string EmploymentType { get; set; }

        [StringLength(255)]
        public string WorkLocation { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMax { get; set; }

        public string Description { get; set; }

        public string Requirement { get; set; }

        public string Benefit { get; set; }

        public DateTime? ApplicationDeadline { get; set; }
    }
}