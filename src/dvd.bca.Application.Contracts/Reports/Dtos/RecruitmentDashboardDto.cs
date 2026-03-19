using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class RecruitmentDashboardDto
    {
        public int TotalRecruitmentRequests { get; set; }

        public int TotalPublishedRecruitmentRequests { get; set; }

        public int TotalApplications { get; set; }

        public int TotalCandidates { get; set; }

        public int TotalScreenedApplications { get; set; }

        public int TotalInterviewScheduled { get; set; }

        public int TotalInterviewPassed { get; set; }

        public int TotalOffers { get; set; }

        public int TotalOfferAccepted { get; set; }

        public int TotalHiredEmployees { get; set; }

        public decimal HiringRate { get; set; }

        public decimal OfferAcceptanceRate { get; set; }
    }
}
