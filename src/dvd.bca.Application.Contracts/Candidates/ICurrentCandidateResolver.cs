using System;
using System.Threading.Tasks;

namespace dvd.bca.Candidates
{
    public interface ICurrentCandidateResolver
    {
        Task<Guid?> FindCandidateIdForCurrentUserAsync();

        Task<Guid> GetRequiredCandidateIdForCurrentUserAsync();

        Task<Guid> NormalizeCandidateIdAsync(Guid candidateId);
    }
}
