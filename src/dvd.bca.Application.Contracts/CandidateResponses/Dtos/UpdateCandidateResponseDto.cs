using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.CandidateResponses.Dtos
{
    public class UpdateCandidateResponseDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        public Guid? OfferId { get; set; }

        [Required]
        public CandidateResponseType ResponseType { get; set; }

        [Required]
        public CandidateResponseChannel ResponseChannel { get; set; }

        [Required]
        public DateTime ResponseTime { get; set; }

        [StringLength(2000)]
        public string ResponseContent { get; set; }

        [StringLength(1000)]
        public string Note { get; set; }
    }
}
