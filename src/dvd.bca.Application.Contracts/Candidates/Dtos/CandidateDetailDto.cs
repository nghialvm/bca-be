using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Candidates.Dtos
{
    public class CandidateDetailDto : CandidateDto
    {
        public int? ApplicationCount { get; set; }

        public int? DocumentCount { get; set; }
    }
}