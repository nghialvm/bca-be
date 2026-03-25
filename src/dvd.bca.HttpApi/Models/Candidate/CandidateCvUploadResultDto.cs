using System;

namespace dvd.bca.Models.Candidate
{
    public class CandidateCvUploadResultDto
    {
        public Guid DocumentId { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = string.Empty;
    }
}
