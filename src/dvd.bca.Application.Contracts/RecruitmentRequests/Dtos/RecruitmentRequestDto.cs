using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.RecruitmentRequests.Dtos
{
    public class RecruitmentRequestDto : FullAuditedEntityDto<Guid>
    {
        public string RequestCode { get; set; }
        public string Title { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid JobPositionId { get; set; }
        public int Headcount { get; set; }
        public string EmploymentType { get; set; }
        public string WorkLocation { get; set; }
        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
        public string Benefit { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public RecruitmentRequestStatus Status { get; set; }
        public Guid? CreatedByUserId { get; set; }
        public Guid? ApprovedByUserId { get; set; }
        public DateTime? ApprovedTime { get; set; }
        public string RejectReason { get; set; }
        public DateTime? PublishedTime { get; set; }
        public DateTime? ClosedTime { get; set; }
    }
}
