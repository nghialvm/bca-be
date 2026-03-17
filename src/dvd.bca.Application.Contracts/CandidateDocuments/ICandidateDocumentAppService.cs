using dvd.bca.CandidateDocuments.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.CandidateDocuments
{
    public interface ICandidateDocumentAppService :
        ICrudAppService<
            CandidateDocumentDto,
            Guid,
            GetCandidateDocumentListInput,
            CreateCandidateDocumentDto,
            UpdateCandidateDocumentDto>
    {
        Task<PagedResultDto<CandidateDocumentDto>> GetListByCandidateIdAsync(Guid candidateId);
    }
}
