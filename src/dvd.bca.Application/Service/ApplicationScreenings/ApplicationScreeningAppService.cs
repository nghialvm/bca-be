using dvd.bca.ApplicationScreenings;
using dvd.bca.ApplicationScreenings.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.ApplicationScreenings
{
    [Authorize]
    public class ApplicationScreeningAppService
        : CrudAppService<
            ApplicationScreening,
            ApplicationScreeningDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateApplicationScreeningDto,
            UpdateApplicationScreeningDto>,
          IApplicationScreeningAppService
    {
        private readonly IRepository<ApplicationScreening, Guid> _applicationScreeningRepository;
        private readonly IRepository<Application, Guid> _applicationRepository;

        public ApplicationScreeningAppService(
            IRepository<ApplicationScreening, Guid> applicationScreeningRepository,
            IRepository<Application, Guid> applicationRepository)
            : base(applicationScreeningRepository)
        {
            _applicationScreeningRepository = applicationScreeningRepository;
            _applicationRepository = applicationRepository;
        }

        public override async Task<ApplicationScreeningDto> CreateAsync(CreateApplicationScreeningDto input)
        {
            try
            {
                await ValidateCreateAsync(input);

                var entity = MapToEntity(input);

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

        public override async Task<ApplicationScreeningDto> UpdateAsync(Guid id, UpdateApplicationScreeningDto input)
        {
            try
            {
                await ValidateUpdateAsync(id, input);

                var entity = await Repository.GetAsync(id);

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
                var entity = await Repository.FindAsync(id);
                if (entity == null)
                {
                    throw new UserFriendlyException("Application screening does not exist.");
                }

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

        public override async Task<ApplicationScreeningDto> GetAsync(Guid id)
        {
            try
            {
                var entity = await Repository.FindAsync(id);
                if (entity == null)
                {
                    throw new UserFriendlyException("Application screening does not exist.");
                }

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

        public override async Task<PagedResultDto<ApplicationScreeningDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                queryable = queryable.OrderBy(
                    input.Sorting.IsNullOrWhiteSpace()
                        ? $"{nameof(ApplicationScreening.ScreeningTime)} desc"
                        : input.Sorting
                );

                var totalCount = queryable.Count();

                var entities = queryable
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<ApplicationScreeningDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<ApplicationScreeningDto>> GetListByApplicationIdAsync(Guid applicationId)
        {
            try
            {
                await CheckApplicationExistsAsync(applicationId);

                var queryable = await Repository.GetQueryableAsync();

                var entities = queryable
                    .Where(x => x.ApplicationId == applicationId)
                    .OrderByDescending(x => x.ScreeningTime)
                    .ToList();

                return entities
                    .Select(MapToGetOutputDto)
                    .ToList();
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

        public async Task<ApplicationScreeningDto> RecordScreeningResultAsync(CreateApplicationScreeningDto input)
        {
            try
            {
                return await CreateAsync(input);
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

        public async Task<ApplicationScreeningDto> ChangeScreeningResultAsync(Guid id, UpdateApplicationScreeningDto input)
        {
            try
            {
                return await UpdateAsync(id, input);
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

        protected override ApplicationScreening MapToEntity(CreateApplicationScreeningDto createInput)
        {
            var entity = ObjectMapper.Map<CreateApplicationScreeningDto, ApplicationScreening>(createInput);
            return entity;
        }

        protected override void MapToEntity(UpdateApplicationScreeningDto updateInput, ApplicationScreening entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override ApplicationScreeningDto MapToGetOutputDto(ApplicationScreening entity)
        {
            return ObjectMapper.Map<ApplicationScreening, ApplicationScreeningDto>(entity);
        }

        private async Task ValidateCreateAsync(CreateApplicationScreeningDto input)
        {
            await CheckApplicationExistsAsync(input.ApplicationId);

            if (input.ScreenedByUserId == Guid.Empty)
            {
                throw new UserFriendlyException("ScreenedByUserId is required.");
            }

            if (input.ApplicationId == Guid.Empty)
            {
                throw new UserFriendlyException("ApplicationId is required.");
            }

            if (input.ScreeningTime == default)
            {
                throw new UserFriendlyException("ScreeningTime is required.");
            }
            if (!Enum.IsDefined(typeof(ScreeningResult), input.Result))
            {
                throw new UserFriendlyException("Invalid screening result.");
            }

            if (input.Score.HasValue && input.Score < 0)
            {
                throw new UserFriendlyException("Score must be greater than or equal to 0.");
            }
        }

        private async Task ValidateUpdateAsync(Guid id, UpdateApplicationScreeningDto input)
        {
            var existingEntity = await Repository.FindAsync(id);
            if (existingEntity == null)
            {
                throw new UserFriendlyException("Application screening does not exist.");
            }

            await CheckApplicationExistsAsync(input.ApplicationId);

            if (input.ScreenedByUserId == Guid.Empty)
            {
                throw new UserFriendlyException("ScreenedByUserId is required.");
            }

            if (input.ApplicationId == Guid.Empty)
            {
                throw new UserFriendlyException("ApplicationId is required.");
            }
            if (!Enum.IsDefined(typeof(ScreeningResult), input.Result))
            {
                throw new UserFriendlyException("Invalid screening result.");
            }
            if (input.ScreeningTime == default)
            {
                throw new UserFriendlyException("ScreeningTime is required.");
            }

            if (input.Score.HasValue && input.Score < 0)
            {
                throw new UserFriendlyException("Score must be greater than or equal to 0.");
            }
        }

        private async Task CheckApplicationExistsAsync(Guid applicationId)
        {
            var application = await _applicationRepository.FindAsync(applicationId);

            if (application == null)
            {
                throw new UserFriendlyException("Application does not exist.");
            }
        }
    }
}