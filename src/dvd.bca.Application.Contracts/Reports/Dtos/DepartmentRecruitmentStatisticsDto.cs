using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class DepartmentRecruitmentStatisticsDto
    {
        public Guid DepartmentId { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public int TotalRecruitmentRequests { get; set; }

        public int TotalApplications { get; set; }

        public int TotalOffers { get; set; }

        public int TotalHired { get; set; }
    }
}
