using dvd.bca.Candidates;
using dvd.bca.Candidates.Dtos;
using dvd.bca.Entity.CandidateRoot;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.Candidates
{
    [Authorize]
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
        protected override string GetPolicyName { get; set; } = null;
        protected override string GetListPolicyName { get; set; } = null;
        protected override string CreatePolicyName { get; set; } = null;
        protected override string UpdatePolicyName { get; set; } = null;
        protected override string DeletePolicyName { get; set; } = null;

        public CandidateAppService(IRepository<Candidate, Guid> repository)
            : base(repository)
        {
        }

        public override async Task<CandidateDto> CreateAsync(CreateCandidateDto input)
        {
            await CheckCreatePolicyAsync();

            try
            {
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
            return ObjectMapper.Map<CreateCandidateDto, Candidate>(createInput);
        }

        protected override void MapToEntity(UpdateCandidateDto updateInput, Candidate entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override CandidateDto MapToGetOutputDto(Candidate entity)
        {
            return ObjectMapper.Map<Candidate, CandidateDto>(entity);
        }
    }
}