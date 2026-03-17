using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.Candidates.Dtos
{
    public class CandidateDto : EntityDto<Guid>
    {
        public string CandidateCode { get; set; }

        public CandidateType CandidateType { get; set; }

        public Guid? EmployeeId { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string IdentityNumber { get; set; }

        public string CurrentCompany { get; set; }

        public string CurrentPosition { get; set; }

        public int? YearsOfExperience { get; set; }

        public string HighestEducation { get; set; }

        public string UniversityName { get; set; }

        public string Major { get; set; }

        public CandidateStatus Status { get; set; }

        public string Source { get; set; }

        public string Note { get; set; }
    }
}
