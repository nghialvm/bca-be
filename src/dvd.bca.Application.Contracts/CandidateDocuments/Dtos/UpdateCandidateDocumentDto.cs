using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.CandidateDocuments.Dtos
{
    public class UpdateCandidateDocumentDto
    {
        [Required]
        public Guid CandidateId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentType { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        [Range(0, long.MaxValue)]
        public long FileSize { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}
