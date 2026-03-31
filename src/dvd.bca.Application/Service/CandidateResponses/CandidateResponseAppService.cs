using dvd.bca.CandidateResponses;
using dvd.bca.CandidateResponses.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Enums;
using dvd.bca.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.CandidateResponses
{
    [Authorize(bcaPermissions.Recruitment.CandidateResponses.Default)]
    public class CandidateResponseAppService
        : CrudAppService<
            CandidateResponse,
            CandidateResponseDto,
            Guid,
            GetCandidateResponseListInput,
            CreateCandidateResponseDto,
            UpdateCandidateResponseDto>,
          ICandidateResponseAppService
    {
        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<Offer, Guid> _offerRepository;

        public CandidateResponseAppService(
            IRepository<CandidateResponse, Guid> repository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<Offer, Guid> offerRepository)
            : base(repository)
        {
            _applicationRepository = applicationRepository;
            _offerRepository = offerRepository;

            GetPolicyName = bcaPermissions.Recruitment.CandidateResponses.Default;
            GetListPolicyName = bcaPermissions.Recruitment.CandidateResponses.Default;
            CreatePolicyName = bcaPermissions.Recruitment.CandidateResponses.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.CandidateResponses.Update;
            DeletePolicyName = bcaPermissions.Recruitment.CandidateResponses.Delete;
        }

        public override async Task<CandidateResponseDto> CreateAsync(CreateCandidateResponseDto input)
        {
            try
            {
                await ValidateCreateOrUpdateAsync(input.ApplicationId, input.OfferId, input.ResponseType);

                var entity = MapToEntity(input);

                if (entity.ResponseTime == default)
                {
                    entity.ResponseTime = Clock.Now;
                }

                entity = await Repository.InsertAsync(entity, autoSave: true);
                await SyncOfferResponseAsync(entity);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        public override async Task<CandidateResponseDto> UpdateAsync(Guid id, UpdateCandidateResponseDto input)
        {
            try
            {
                await ValidateCreateOrUpdateAsync(input.ApplicationId, input.OfferId, input.ResponseType);

                var entity = await Repository.GetAsync(id);

                MapToEntity(input, entity);

                if (entity.ResponseTime == default)
                {
                    entity.ResponseTime = Clock.Now;
                }

                entity = await Repository.UpdateAsync(entity, autoSave: true);
                await SyncOfferResponseAsync(entity);

                return MapToGetOutputDto(entity);
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
                await Repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<CandidateResponseDto> GetAsync(Guid id)
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

        public override async Task<PagedResultDto<CandidateResponseDto>> GetListAsync(GetCandidateResponseListInput input)
        {
            var queryable = await Repository.GetQueryableAsync();

            queryable = queryable
                .WhereIf(input.ApplicationId.HasValue, x => x.ApplicationId == input.ApplicationId.Value)
                .WhereIf(input.OfferId.HasValue, x => x.OfferId == input.OfferId.Value)
                .WhereIf(input.ResponseType.HasValue, x => x.ResponseType == input.ResponseType.Value)
                .WhereIf(input.ResponseChannel.HasValue, x => x.ResponseChannel == input.ResponseChannel.Value)
                .WhereIf(input.ResponseTimeFrom.HasValue, x => x.ResponseTime >= input.ResponseTimeFrom.Value)
                .WhereIf(input.ResponseTimeTo.HasValue, x => x.ResponseTime <= input.ResponseTimeTo.Value);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var sorting = input.Sorting.IsNullOrWhiteSpace()
                ? "ResponseTime desc"
                : input.Sorting;

            var entities = await AsyncExecuter.ToListAsync(
                queryable
                    .OrderBy(sorting)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
            );

            var items = entities.Select(MapToGetOutputDto).ToList();

            return new PagedResultDto<CandidateResponseDto>(totalCount, items);
        }

        [Authorize(bcaPermissions.Recruitment.CandidateResponses.Default)]
        public async Task<List<CandidateResponseDto>> GetListByApplicationIdAsync(Guid applicationId)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Where(x => x.ApplicationId == applicationId)
                        .OrderByDescending(x => x.ResponseTime)
                );

                return entities.Select(MapToGetOutputDto).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        [Authorize(bcaPermissions.Recruitment.CandidateResponses.Default)]
        public async Task<List<CandidateResponseDto>> GetListByOfferIdAsync(Guid offerId)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Where(x => x.OfferId == offerId)
                        .OrderByDescending(x => x.ResponseTime)
                );

                return entities.Select(MapToGetOutputDto).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected override CandidateResponse MapToEntity(CreateCandidateResponseDto createInput)
        {
            return ObjectMapper.Map<CreateCandidateResponseDto, CandidateResponse>(createInput);
        }

        protected override void MapToEntity(UpdateCandidateResponseDto updateInput, CandidateResponse entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override CandidateResponseDto MapToGetOutputDto(CandidateResponse entity)
        {
            return ObjectMapper.Map<CandidateResponse, CandidateResponseDto>(entity);
        }

        private async Task ValidateCreateOrUpdateAsync(Guid applicationId, Guid? offerId, CandidateResponseType responseType)
        {
            var application = await _applicationRepository.FindAsync(applicationId);
            if (application == null)
            {
                throw new UserFriendlyException("Application does not exist.");
            }

            if (offerId.HasValue)
            {
                var offer = await _offerRepository.FindAsync(offerId.Value);
                if (offer == null)
                {
                    throw new UserFriendlyException("Offer does not exist.");
                }

                if (offer.ApplicationId != applicationId)
                {
                    throw new UserFriendlyException("The offer does not belong to the specified application.");
                }
            }

            var offerResponseTypes = new[]
            {
                CandidateResponseType.OfferAccepted,
                CandidateResponseType.OfferDeclined
            };

            if (offerResponseTypes.Contains(responseType) && !offerId.HasValue)
            {
                throw new UserFriendlyException("OfferId is required for offer response types.");
            }
        }

        private async Task SyncOfferResponseAsync(CandidateResponse response)
        {
            if (!response.OfferId.HasValue)
            {
                return;
            }

            var nextOfferStatus = response.ResponseType switch
            {
                CandidateResponseType.OfferAccepted => OfferStatus.Accepted,
                CandidateResponseType.OfferDeclined => OfferStatus.Declined,
                _ => (OfferStatus?)null
            };

            var nextApplicationStatus = response.ResponseType switch
            {
                CandidateResponseType.OfferAccepted => ApplicationStatus.Hired,
                CandidateResponseType.OfferDeclined => ApplicationStatus.OfferDeclined,
                _ => (ApplicationStatus?)null
            };

            if (!nextOfferStatus.HasValue || !nextApplicationStatus.HasValue)
            {
                return;
            }

            var offer = await _offerRepository.GetAsync(response.OfferId.Value);
            offer.Status = nextOfferStatus.Value;
            await _offerRepository.UpdateAsync(offer, autoSave: true);

            var application = await _applicationRepository.GetAsync(response.ApplicationId);
            application.Status = nextApplicationStatus.Value;
            await _applicationRepository.UpdateAsync(application, autoSave: true);
        }
    }
}
