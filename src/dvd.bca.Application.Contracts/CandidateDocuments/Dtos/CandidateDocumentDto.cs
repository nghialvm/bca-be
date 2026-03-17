using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.CandidateDocuments.Dtos
{
    public class CandidateDocumentDto : EntityDto<Guid>
    {
        public Guid CandidateId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public long FileSize { get; set; }

        public string ContentType { get; set; }

        public string Description { get; set; }
    }
}
