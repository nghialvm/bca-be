using dvd.bca.Candidates;
using dvd.bca.Candidates.Dtos;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace dvd.bca.Service.Candidates
{
    [Authorize(bcaPermissions.Recruitment.Candidates.Default)]
    public class CandidateAppService
        : CrudAppService<
            Candidate,
            CandidateDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateCandidateDto,
            UpdateCandidateDto>,
          ICandidateAppService
    {
        private const string CandidateRoleName = "candidate";

        private readonly IdentityUserManager _userManager;

        protected override string GetPolicyName { get; set; } = null;
        protected override string GetListPolicyName { get; set; } = null;
        protected override string CreatePolicyName { get; set; } = null;
        protected override string UpdatePolicyName { get; set; } = null;
        protected override string DeletePolicyName { get; set; } = null;

        public CandidateAppService(
            IRepository<Candidate, Guid> repository,
            IdentityUserManager userManager)
            : base(repository)
        {
            _userManager = userManager;
            GetPolicyName = bcaPermissions.Recruitment.Candidates.Default;
            GetListPolicyName = bcaPermissions.Recruitment.Candidates.Default;
            CreatePolicyName = bcaPermissions.Recruitment.Candidates.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.Candidates.Update;
            DeletePolicyName = bcaPermissions.Recruitment.Candidates.Delete;
        }

        public override async Task<CandidateDto> CreateAsync(CreateCandidateDto input)
        {
            await CheckCreatePolicyAsync();

            try
            {
                await EnsureCandidateUserAsync(input.UserId);

                var candidateByUserId = await Repository.FirstOrDefaultAsync(x => x.Id == input.UserId);
                if (candidateByUserId != null)
                {
                    throw new UserFriendlyException($"Candidate profile for user '{input.UserId}' already exists.");
                }

                var existedCandidate = await Repository.FirstOrDefaultAsync(x => x.CandidateCode == input.CandidateCode);
                if (existedCandidate != null)
                {
                    throw new UserFriendlyException($"Candidate code '{input.CandidateCode}' already exists.");
                }

                var entity = MapToEntity(input);

                await Repository.InsertAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<CandidateDto> UpdateAsync(Guid id, UpdateCandidateDto input)
        {
            await CheckUpdatePolicyAsync();

            try
            {
                var entity = await GetEntityByIdAsync(id);

                var duplicatedCandidate = await Repository.FirstOrDefaultAsync(x => x.CandidateCode == input.CandidateCode && x.Id != id);
                if (duplicatedCandidate != null)
                {
                    throw new UserFriendlyException($"Candidate code '{input.CandidateCode}' already exists.");
                }

                MapToEntity(input, entity);

                await Repository.UpdateAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            await CheckDeletePolicyAsync();

            try
            {
                var entity = await Repository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new UserFriendlyException($"Candidate with id '{id}' was not found.");
                }

                await Repository.DeleteAsync(entity, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<CandidateDto> GetAsync(Guid id)
        {
            await CheckGetPolicyAsync();

            try
            {
                var entity = await GetEntityByIdAsync(id);

                return MapToGetOutputDto(entity);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<PagedResultDto<CandidateDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            await CheckGetListPolicyAsync();

            try
            {
                var queryable = await Repository.GetQueryableAsync();

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    ApplyPaging(
                        ApplySorting(queryable, input),
                        input
                    )
                );

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<CandidateDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected override IQueryable<Candidate> ApplySorting(IQueryable<Candidate> query, PagedAndSortedResultRequestDto input)
        {
            return query.OrderByDescending(x => x.CreationTime);
        }

        protected override Candidate MapToEntity(CreateCandidateDto createInput)
        {
            var entity = new Candidate(createInput.UserId);
            ObjectMapper.Map(createInput, entity);
            return entity;
        }

        protected override void MapToEntity(UpdateCandidateDto updateInput, Candidate entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override CandidateDto MapToGetOutputDto(Candidate entity)
        {
            return ObjectMapper.Map<Candidate, CandidateDto>(entity);
        }

        private async Task EnsureCandidateUserAsync(Guid userId)
        {
            var user = await _userManager.GetByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Any(x => string.Equals(x, CandidateRoleName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new UserFriendlyException($"User '{userId}' does not have role '{CandidateRoleName}'.");
            }
        }
    }
}
