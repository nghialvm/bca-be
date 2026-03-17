using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.Offers.Dtos
{
    public class CreateOfferDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        [Range(0.01, 9999999999999999.99, ErrorMessage = "Salary must be greater than 0.")]
        public decimal Salary { get; set; }

        public DateTime? StartDate { get; set; }

        [Range(0, 24, ErrorMessage = "ProbationMonths must be between 0 and 24.")]
        public int? ProbationMonths { get; set; }

        [StringLength(255)]
        public string? WorkLocation { get; set; }

        public string? Benefit { get; set; }

        [StringLength(1000)]
        public string? Note { get; set; }

        public DateTime? SentTime { get; set; }

        public DateTime? ExpiredTime { get; set; }
    }
}
