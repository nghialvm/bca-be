using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.CandidateResponses.Dtos
{
    public class GetCandidateResponseListInput : PagedAndSortedResultRequestDto
    {
        public Guid? ApplicationId { get; set; }

        public Guid? OfferId { get; set; }

        public CandidateResponseType? ResponseType { get; set; }

        public CandidateResponseChannel? ResponseChannel { get; set; }

        public DateTime? ResponseTimeFrom { get; set; }

        public DateTime? ResponseTimeTo { get; set; }
    }
}
