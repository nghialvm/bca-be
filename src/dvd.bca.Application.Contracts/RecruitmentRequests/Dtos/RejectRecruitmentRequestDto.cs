using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.RecruitmentRequests.Dtos
{
    public class RejectRecruitmentRequestDto
    {
        [Required]
        [StringLength(1000)]
        public string Reason { get; set; }
    }
}
