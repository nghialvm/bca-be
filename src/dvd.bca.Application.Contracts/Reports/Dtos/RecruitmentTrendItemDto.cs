using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class RecruitmentTrendItemDto
    {
        public string Period { get; set; } = string.Empty;

        public int TotalApplications { get; set; }

        public int TotalOffers { get; set; }

        public int TotalHired { get; set; }
    }
}
