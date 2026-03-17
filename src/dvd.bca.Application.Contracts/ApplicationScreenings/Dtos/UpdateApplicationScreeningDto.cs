using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.ApplicationScreenings.Dtos
{
    public class UpdateApplicationScreeningDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public Guid ScreenedByUserId { get; set; }

        [Required]
        public DateTime ScreeningTime { get; set; }

        [Required]
        public ScreeningResult Result { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        [Range(typeof(decimal), "0", "9999999999999999")]
        public decimal? Score { get; set; }

        public string? CriteriaSummary { get; set; }
    }
}