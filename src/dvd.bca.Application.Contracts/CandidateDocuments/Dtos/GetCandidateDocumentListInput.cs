using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.CandidateDocuments.Dtos
{
    public class GetCandidateDocumentListInput : PagedAndSortedResultRequestDto
    {
        public Guid? CandidateId { get; set; }

        public string? DocumentType { get; set; }

        public string? Filter { get; set; }
    }
}
