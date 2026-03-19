using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class RecruitmentFunnelDto
    {
        public int TotalApplications { get; set; }

        public int ScreeningPassed { get; set; }

        public int InterviewScheduled { get; set; }

        public int InterviewPassed { get; set; }

        public int Offered { get; set; }

        public int Hired { get; set; }
    }
}
