using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.Candidates.Dtos
{
    public class UpdateCandidateDto
    {
        [Required]
        [StringLength(50)]
        public string CandidateCode { get; set; }

        [Required]
        public CandidateType CandidateType { get; set; }

        public Guid? EmployeeId { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(20)]
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(50)]
        public string IdentityNumber { get; set; }

        [StringLength(255)]
        public string CurrentCompany { get; set; }

        [StringLength(255)]
        public string CurrentPosition { get; set; }

        [Range(0, 100)]
        public int? YearsOfExperience { get; set; }

        [StringLength(255)]
        public string HighestEducation { get; set; }

        [StringLength(255)]
        public string UniversityName { get; set; }

        [StringLength(255)]
        public string Major { get; set; }

        [Required]
        public CandidateStatus Status { get; set; }

        [StringLength(100)]
        public string Source { get; set; }

        public string Note { get; set; }
    }
}