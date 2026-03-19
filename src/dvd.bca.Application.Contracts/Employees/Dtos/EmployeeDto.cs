using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.Employees.Dtos
{
    public class EmployeeDto : EntityDto<Guid>
    {
        public string EmployeeCode { get; set; }

        public string FullName { get; set; }

        public Guid CandidateId { get; set; }

        public Guid? ApplicationId { get; set; }

        public Guid? OfferId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobPositionId { get; set; }

        public DateTime JoinDate { get; set; }

        public EmployeeStatus Status { get; set; }

        public EmployeeSourceType SourceType { get; set; }

        public string WorkEmail { get; set; }

        public string Note { get; set; }
    }
}