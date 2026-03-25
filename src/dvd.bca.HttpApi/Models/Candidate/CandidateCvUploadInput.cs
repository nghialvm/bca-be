using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace dvd.bca.Models.Candidate
{
    public class CandidateCvUploadInput
    {
        [Required]
        public Guid CandidateId { get; set; }

        [Required]
        public IFormFile File { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
