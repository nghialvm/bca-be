using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Reports.Dtos
{
    public class EmployeeStatisticsDto
    {
        public int TotalHiredEmployees { get; set; }

        public int InternalCandidatesHired { get; set; }

        public int ExternalCandidatesHired { get; set; }
    }
}
