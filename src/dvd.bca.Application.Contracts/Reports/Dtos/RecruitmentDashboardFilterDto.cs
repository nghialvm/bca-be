using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class RecruitmentDashboardFilterDto
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? JobPositionId { get; set; }
    }
}
