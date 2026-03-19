using dvd.bca.CandidateResponses.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace dvd.bca.CandidateResponses
{
    public interface ICandidateResponseAppService :
        ICrudAppService<
            CandidateResponseDto,
            Guid,
            GetCandidateResponseListInput,
            CreateCandidateResponseDto,
            UpdateCandidateResponseDto>
    {
        Task<List<CandidateResponseDto>> GetListByApplicationIdAsync(Guid applicationId);

        Task<List<CandidateResponseDto>> GetListByOfferIdAsync(Guid offerId);
    }
}