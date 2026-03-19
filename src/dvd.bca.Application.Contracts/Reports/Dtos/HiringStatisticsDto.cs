using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class HiringStatisticsDto
    {
        public int TotalApplications { get; set; }

        public int TotalOffersAccepted { get; set; }

        public int TotalHiredEmployees { get; set; }

        public decimal ApplicationToHireRate { get; set; }

        public decimal OfferAcceptedToHireRate { get; set; }
    }
}
