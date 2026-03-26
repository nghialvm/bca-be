using dvd.bca.Candidates;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Results;
using dvd.bca.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace dvd.bca.Service.Candidates
{
    public class CurrentCandidateResolver : ApplicationService, ICurrentCandidateResolver
    {
        private const string CandidateRoleName = "candidate";

        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;
        private readonly IRepository<CandidateDocument, Guid> _candidateDocumentRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IdentityUserManager _userManager;

        public CurrentCandidateResolver(
            IRepository<Candidate, Guid> candidateRepository,
            IRepository<CandidateDocument, Guid> candidateDocumentRepository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<Employee, Guid> employeeRepository,
            IdentityUserManager userManager)
        {
            _candidateRepository = candidateRepository;
            _candidateDocumentRepository = candidateDocumentRepository;
            _applicationRepository = applicationRepository;
            _employeeRepository = employeeRepository;
            _userManager = userManager;
        }

        public async Task<Guid?> FindCandidateIdForCurrentUserAsync()
        {
            if (CurrentUser.Id == null || !IsCurrentUserCandidate())
            {
                return null;
            }

            await EnsureCurrentCandidateProfileAsync();
            return CurrentUser.Id.Value;
        }

        public async Task<Guid> GetRequiredCandidateIdForCurrentUserAsync()
        {
            var candidateId = await FindCandidateIdForCurrentUserAsync();
            if (!candidateId.HasValue)
            {
                throw new UserFriendlyException("Current user is not a candidate.");
            }

            return candidateId.Value;
        }

        public async Task<Guid> NormalizeCandidateIdAsync(Guid candidateId)
        {
            if (!IsCurrentUserCandidate())
            {
                return candidateId;
            }

            return await GetRequiredCandidateIdForCurrentUserAsync();
        }

        private async Task EnsureCurrentCandidateProfileAsync()
        {
            var userId = CurrentUser.Id!.Value;
            var candidate = await _candidateRepository.FindAsync(userId);
            if (candidate != null)
            {
                return;
            }

            var user = await _userManager.GetByIdAsync(userId);
            var existingCandidate = await FindExistingCandidateAsync(user);

            if (existingCandidate == null)
            {
                await _candidateRepository.InsertAsync(CreateCandidateFromUser(user), autoSave: true);
                return;
            }

            if (existingCandidate.Id == userId)
            {
                return;
            }

            await MigrateCandidateAsync(existingCandidate, user);
        }

        private async Task<Candidate?> FindExistingCandidateAsync(IdentityUser user)
        {
            var queryable = await _candidateRepository.GetQueryableAsync();

            if (!user.Email.IsNullOrWhiteSpace())
            {
                var email = user.Email.Trim().ToLowerInvariant();
                var byEmail = await AsyncExecuter.FirstOrDefaultAsync(
                    queryable.Where(x => x.Email != null && x.Email.ToLower() == email)
                );

                if (byEmail != null)
                {
                    return byEmail;
                }
            }

            return null;
        }

        private async Task MigrateCandidateAsync(Candidate sourceCandidate, IdentityUser user)
        {
            var originalCandidateCode = sourceCandidate.CandidateCode;
            sourceCandidate.CandidateCode = BuildMigratedCandidateCode(sourceCandidate.Id);
            await _candidateRepository.UpdateAsync(sourceCandidate, autoSave: true);

            var targetCandidate = new Candidate(user.Id)
            {
                CandidateCode = originalCandidateCode,
                CandidateType = sourceCandidate.CandidateType,
                EmployeeId = sourceCandidate.EmployeeId,
                FullName = string.IsNullOrWhiteSpace(sourceCandidate.FullName)
                    ? BuildFullName(user)
                    : sourceCandidate.FullName,
                DateOfBirth = sourceCandidate.DateOfBirth,
                Gender = sourceCandidate.Gender,
                PhoneNumber = string.IsNullOrWhiteSpace(sourceCandidate.PhoneNumber)
                    ? user.PhoneNumber
                    : sourceCandidate.PhoneNumber,
                Email = string.IsNullOrWhiteSpace(sourceCandidate.Email)
                    ? user.Email
                    : sourceCandidate.Email,
                Address = sourceCandidate.Address,
                IdentityNumber = sourceCandidate.IdentityNumber,
                CurrentCompany = sourceCandidate.CurrentCompany,
                CurrentPosition = sourceCandidate.CurrentPosition,
                YearsOfExperience = sourceCandidate.YearsOfExperience,
                HighestEducation = sourceCandidate.HighestEducation,
                UniversityName = sourceCandidate.UniversityName,
                Major = sourceCandidate.Major,
                Status = sourceCandidate.Status,
                Source = sourceCandidate.Source,
                Note = sourceCandidate.Note
            };

            await _candidateRepository.InsertAsync(targetCandidate, autoSave: true);
            await ReassignCandidateReferencesAsync(sourceCandidate.Id, user.Id);
            await _candidateRepository.DeleteAsync(sourceCandidate, autoSave: true);
        }

        private async Task ReassignCandidateReferencesAsync(Guid sourceCandidateId, Guid targetCandidateId)
        {
            var documents = await _candidateDocumentRepository.GetListAsync(x => x.CandidateId == sourceCandidateId);
            foreach (var document in documents)
            {
                document.CandidateId = targetCandidateId;
                await _candidateDocumentRepository.UpdateAsync(document, autoSave: true);
            }

            var applications = await _applicationRepository.GetListAsync(x => x.CandidateId == sourceCandidateId);
            foreach (var application in applications)
            {
                application.CandidateId = targetCandidateId;
                await _applicationRepository.UpdateAsync(application, autoSave: true);
            }

            var employees = await _employeeRepository.GetListAsync(x => x.CandidateId == sourceCandidateId);
            foreach (var employee in employees)
            {
                employee.CandidateId = targetCandidateId;
                await _employeeRepository.UpdateAsync(employee, autoSave: true);
            }
        }

        private Candidate CreateCandidateFromUser(IdentityUser user)
        {
            return new Candidate(user.Id)
            {
                CandidateCode = BuildCandidateCode(user.Id),
                CandidateType = CandidateType.External,
                FullName = BuildFullName(user),
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Status = CandidateStatus.Active,
                Source = "CandidateAccount"
            };
        }

        private static string BuildCandidateCode(Guid userId)
        {
            return $"CAND-{userId:N}";
        }

        private static string BuildMigratedCandidateCode(Guid candidateId)
        {
            return $"MIGRATED-{candidateId:N}";
        }

        private static string BuildFullName(IdentityUser user)
        {
            var fullName = string.Join(
                " ",
                new[] { user.Name, user.Surname }.Where(x => !string.IsNullOrWhiteSpace(x))
            ).Trim();

            return string.IsNullOrWhiteSpace(fullName)
                ? (user.UserName ?? user.Email ?? user.Id.ToString())
                : fullName;
        }

        private bool IsCurrentUserCandidate()
        {
            return CurrentUser.Roles?.Any(x =>
                string.Equals(x, CandidateRoleName, StringComparison.OrdinalIgnoreCase)) == true;
        }
    }
}
