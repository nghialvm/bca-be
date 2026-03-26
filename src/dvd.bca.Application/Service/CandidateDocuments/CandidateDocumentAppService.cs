using dvd.bca.CandidateDocuments;
using dvd.bca.CandidateDocuments.Dtos;
using dvd.bca.Candidates;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.CandidateDocuments
{
    [Authorize(bcaPermissions.Recruitment.CandidateDocuments.Default)]
    public class CandidateDocumentAppService :
        CrudAppService<
            CandidateDocument,
            CandidateDocumentDto,
            Guid,
            GetCandidateDocumentListInput,
            CreateCandidateDocumentDto,
            UpdateCandidateDocumentDto>,
        ICandidateDocumentAppService
    {
        private readonly ICurrentCandidateResolver _currentCandidateResolver;
        private readonly IRepository<Candidate, Guid> _candidateRepository;

        public CandidateDocumentAppService(
            IRepository<CandidateDocument, Guid> repository,
            IRepository<Candidate, Guid> candidateRepository,
            ICurrentCandidateResolver currentCandidateResolver)
            : base(repository)
        {
            _currentCandidateResolver = currentCandidateResolver;
            _candidateRepository = candidateRepository;

            GetPolicyName = bcaPermissions.Recruitment.CandidateDocuments.Default;
            GetListPolicyName = bcaPermissions.Recruitment.CandidateDocuments.Default;
            CreatePolicyName = bcaPermissions.Recruitment.CandidateDocuments.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.CandidateDocuments.Update;
            DeletePolicyName = bcaPermissions.Recruitment.CandidateDocuments.Delete;
        }

        public override async Task<CandidateDocumentDto> CreateAsync(CreateCandidateDocumentDto input)
        {
            try
            {
                input.CandidateId = await _currentCandidateResolver.NormalizeCandidateIdAsync(input.CandidateId);
                await CheckCandidateExistsAsync(input.CandidateId);

                var entity = MapToEntity(input);

                entity = await Repository.InsertAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<CandidateDocumentDto> UpdateAsync(Guid id, UpdateCandidateDocumentDto input)
        {
            try
            {
                input.CandidateId = await _currentCandidateResolver.NormalizeCandidateIdAsync(input.CandidateId);
                await CheckCandidateExistsAsync(input.CandidateId);

                var entity = await Repository.GetAsync(id);

                MapToEntity(input, entity);

                entity = await Repository.UpdateAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
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
                    throw new UserFriendlyException($"CandidateDocument with id '{id}' was not found.");
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

        public override async Task<CandidateDocumentDto> GetAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);
                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<PagedResultDto<CandidateDocumentDto>> GetListAsync(GetCandidateDocumentListInput input)
        {
            try
            {
                await CheckGetListPolicyAsync();

                var queryable = await Repository.GetQueryableAsync();

                var query = queryable.AsQueryable();

                if (input.CandidateId.HasValue)
                {
                    query = query.Where(x => x.CandidateId == input.CandidateId.Value);
                }

                if (!string.IsNullOrWhiteSpace(input.DocumentType))
                {
                    query = query.Where(x => x.DocumentType.Contains(input.DocumentType));
                }

                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(x =>
                        x.FileName.Contains(input.Filter) ||
                        x.FilePath.Contains(input.Filter) ||
                        x.Description.Contains(input.Filter));
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                query = query.OrderBy(string.IsNullOrWhiteSpace(input.Sorting) ? "CreationTime desc" : input.Sorting)
                             .Skip(input.SkipCount)
                             .Take(input.MaxResultCount);

                var entities = await AsyncExecuter.ToListAsync(query);

                var items = entities.Select(MapToGetOutputDto).ToList();

                return new PagedResultDto<CandidateDocumentDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(bcaPermissions.Recruitment.CandidateDocuments.Default)]
        public async Task<PagedResultDto<CandidateDocumentDto>> GetListByCandidateIdAsync(Guid candidateId)
        {
            try
            {
                await CheckGetListPolicyAsync();

                var queryable = await Repository.GetQueryableAsync();

                var query = queryable
                    .Where(x => x.CandidateId == candidateId)
                    .OrderByDescending(x => x.CreationTime);

                var entities = await AsyncExecuter.ToListAsync(query);

                var items = entities.Select(MapToGetOutputDto).ToList();

                return new PagedResultDto<CandidateDocumentDto>(items.Count, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected override CandidateDocument MapToEntity(CreateCandidateDocumentDto createInput)
        {
            return ObjectMapper.Map<CreateCandidateDocumentDto, CandidateDocument>(createInput);
        }

        protected override void MapToEntity(UpdateCandidateDocumentDto updateInput, CandidateDocument entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override CandidateDocumentDto MapToGetOutputDto(CandidateDocument entity)
        {
            return ObjectMapper.Map<CandidateDocument, CandidateDocumentDto>(entity);
        }

        private async Task CheckCandidateExistsAsync(Guid candidateId)
        {
            var queryable = await _candidateRepository.GetQueryableAsync();

            var exists = await AsyncExecuter.AnyAsync(queryable, x => x.Id == candidateId);

            if (!exists)
            {
                throw new UserFriendlyException($"Candidate does not exist or deleted. CandidateId: {candidateId}");
            }
        }
    }
}
