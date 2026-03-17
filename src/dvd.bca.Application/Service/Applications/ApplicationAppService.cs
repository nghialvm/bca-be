using dvd.bca.Applications;
using dvd.bca.Applications.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.Applications
{
    [Authorize]
    public class ApplicationAppService
        : CrudAppService<
            Application,
            ApplicationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateApplicationDto,
            UpdateApplicationDto>,
          IApplicationAppService
    {
        private readonly IRepository<RecruitmentRequest, Guid> _recruitmentRequestRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;

        public ApplicationAppService(
            IRepository<Application, Guid> repository,
            IRepository<RecruitmentRequest, Guid> recruitmentRequestRepository,
            IRepository<Candidate, Guid> candidateRepository)
            : base(repository)
        {
            _recruitmentRequestRepository = recruitmentRequestRepository;
            _candidateRepository = candidateRepository;
        }

        public override async Task<ApplicationDto> CreateAsync(CreateApplicationDto input)
        {
            try
            {
                await CheckCreatePolicyAsync();

                await ValidateCreateAsync(input);

                var entity = MapToEntity(input);
                entity.Status = ApplicationStatus.Submitted;
                entity = await Repository.InsertAsync(entity, autoSave: true);

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

        public override async Task<ApplicationDto> UpdateAsync(Guid id, UpdateApplicationDto input)
        {
            try
            {
                await CheckUpdatePolicyAsync();

                var entity = await GetEntityByIdAsync(id);

                await ValidateUpdateAsync(id, input);

                MapToEntity(input, entity);

                entity = await Repository.UpdateAsync(entity, autoSave: true);

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

        public override async Task DeleteAsync(Guid id)
        {
            try
            {
                await CheckDeletePolicyAsync();

                var entity = await GetEntityByIdAsync(id);

                await Repository.DeleteAsync(entity, autoSave: true);
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

        public override async Task<ApplicationDto> GetAsync(Guid id)
        {
            try
            {
                await CheckGetPolicyAsync();

                var entity = await GetEntityByIdAsync(id);

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

        public override async Task<PagedResultDto<ApplicationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                await CheckGetListPolicyAsync();

                var queryable = await Repository.GetQueryableAsync();

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var sorting = input.Sorting.IsNullOrWhiteSpace()
                    ? nameof(Application.AppliedTime) + " desc"
                    : input.Sorting;

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy(sorting)
                        .PageBy(input)
                );

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<ApplicationDto>(totalCount, items);
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

        public async Task<PagedResultDto<ApplicationDto>> GetListByRecruitmentRequestIdAsync(
            Guid recruitmentRequestId,
            PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                queryable = queryable.Where(x => x.RecruitmentRequestId == recruitmentRequestId);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var sorting = input.Sorting.IsNullOrWhiteSpace()
                    ? nameof(Application.AppliedTime) + " desc"
                    : input.Sorting;

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy(sorting)
                        .PageBy(input)
                );

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<ApplicationDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<PagedResultDto<ApplicationDto>> GetListByCandidateIdAsync(
            Guid candidateId,
            PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                queryable = queryable.Where(x => x.CandidateId == candidateId);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var sorting = input.Sorting.IsNullOrWhiteSpace()
                    ? nameof(Application.AppliedTime) + " desc"
                    : input.Sorting;

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy(sorting)
                        .PageBy(input)
                );

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<ApplicationDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected override Application MapToEntity(CreateApplicationDto createInput)
        {
            var entity = base.MapToEntity(createInput);
            return entity;
        }

        protected override void MapToEntity(UpdateApplicationDto updateInput, Application entity)
        {
            base.MapToEntity(updateInput, entity);
        }

        protected override ApplicationDto MapToGetOutputDto(Application entity)
        {
            return base.MapToGetOutputDto(entity);
        }

        private async Task ValidateCreateAsync(CreateApplicationDto input)
        {
            var recruitmentRequestExists = await _recruitmentRequestRepository.AnyAsync(x => x.Id == input.RecruitmentRequestId);
            if (!recruitmentRequestExists)
            {
                throw new UserFriendlyException("Recruitment request does not exist.");
            }

            var candidateExists = await _candidateRepository.AnyAsync(x => x.Id == input.CandidateId);
            if (!candidateExists)
            {
                throw new UserFriendlyException("Candidate does not exist.");
            }

            var applicationCodeExists = await Repository.AnyAsync(x => x.ApplicationCode == input.ApplicationCode);
            if (applicationCodeExists)
            {
                throw new UserFriendlyException("Application code already exists.");
            }

            var duplicatedApplication = await Repository.AnyAsync(x =>
                x.RecruitmentRequestId == input.RecruitmentRequestId &&
                x.CandidateId == input.CandidateId);

            if (duplicatedApplication)
            {
                throw new UserFriendlyException("This candidate has already applied to this recruitment request.");
            }
        }

        private async Task ValidateUpdateAsync(Guid id, UpdateApplicationDto input)
        {
            var recruitmentRequestExists = await _recruitmentRequestRepository.AnyAsync(x => x.Id == input.RecruitmentRequestId);
            if (!recruitmentRequestExists)
            {
                throw new UserFriendlyException("Recruitment request does not exist.");
            }

            var candidateExists = await _candidateRepository.AnyAsync(x => x.Id == input.CandidateId);
            if (!candidateExists)
            {
                throw new UserFriendlyException("Candidate does not exist.");
            }

            if (!Enum.IsDefined(typeof(ApplicationStatus), input.Status))
            {
                throw new UserFriendlyException("Application status is invalid.");
            }

            var applicationCodeExists = await Repository.AnyAsync(x =>
                x.Id != id &&
                x.ApplicationCode == input.ApplicationCode);

            if (applicationCodeExists)
            {
                throw new UserFriendlyException("Application code already exists.");
            }

            var duplicatedApplication = await Repository.AnyAsync(x =>
                x.Id != id &&
                x.RecruitmentRequestId == input.RecruitmentRequestId &&
                x.CandidateId == input.CandidateId);

            if (duplicatedApplication)
            {
                throw new UserFriendlyException("This candidate has already applied to this recruitment request.");
            }
        }
    }
}