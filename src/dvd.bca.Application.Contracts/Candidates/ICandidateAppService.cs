using dvd.bca.Candidates.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace dvd.bca.Candidates
{
    public interface ICandidateAppService
        : ICrudAppService<
            CandidateDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateCandidateDto,
            UpdateCandidateDto>
    {
    }
}