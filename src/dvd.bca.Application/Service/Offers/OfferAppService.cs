using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Enums;
using dvd.bca.Offers;
using dvd.bca.Offers.Dtos;
using dvd.bca.Permissions;
using dvd.bca.Service.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.Offers
{
    [Authorize(bcaPermissions.Recruitment.Offers.Default)]
    public class OfferAppService
        : CrudAppService<
            Offer,
            OfferDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateOfferDto,
            UpdateOfferDto>,
          IOfferAppService
    {
        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;
        private readonly CandidateEmailManager _candidateEmailManager;

        public OfferAppService(
            IRepository<Offer, Guid> repository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<Candidate, Guid> candidateRepository,
            CandidateEmailManager candidateEmailManager)
     : base(repository)
        {
            _applicationRepository = applicationRepository;
            _candidateRepository = candidateRepository;
            _candidateEmailManager = candidateEmailManager;

            GetPolicyName = bcaPermissions.Recruitment.Offers.Default;
            GetListPolicyName = bcaPermissions.Recruitment.Offers.Default;
            CreatePolicyName = bcaPermissions.Recruitment.Offers.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.Offers.Update;
            DeletePolicyName = bcaPermissions.Recruitment.Offers.Delete;
        }

        public override async Task<OfferDto> CreateAsync(CreateOfferDto input)
        {
            try
            {
                await ValidateCreateAsync(input);

                var entity = MapToEntity(input);

                entity.Status = OfferStatus.Sent;
                entity.SentTime ??= Clock.Now;

                entity = await Repository.InsertAsync(entity, autoSave: true);

                var application = await _applicationRepository.GetAsync(entity.ApplicationId);
                application.Status = ApplicationStatus.Offered;
                await _applicationRepository.UpdateAsync(application, autoSave: true);

                var candidate = await _candidateRepository.GetAsync(application.CandidateId);
                if (candidate.Email != null)
                {
                    await _candidateEmailManager.SendOfferAsync(
                        candidate.Email,
                        candidate.FullName,
                        "Your Position",
                        entity.Salary.ToString("N0"),
                        entity.StartDate?.ToString("dd/MM/yyyy") ?? ""
                    );
                }
                else
                {
                    throw new UserFriendlyException("Email cannot be empty");
                }

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<OfferDto> UpdateAsync(Guid id, UpdateOfferDto input)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                await ValidateUpdateAsync(id, input, entity);

                MapToEntity(input, entity);

                entity = await Repository.UpdateAsync(entity, autoSave: true);
                await SyncApplicationStatusAsync(entity);

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
                var entity = await Repository.GetAsync(id);

                if (entity.Status == OfferStatus.Accepted)
                {
                    throw new UserFriendlyException("Accepted offer cannot be deleted.");
                }

                await Repository.DeleteAsync(entity, autoSave: true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<OfferDto> GetAsync(Guid id)
        {
            try
            {
                var queryable = await Repository.WithDetailsAsync(x => x.Application);

                var entity = await AsyncExecuter.FirstOrDefaultAsync(
                    queryable.Where(x => x.Id == id)
                );

                if (entity == null)
                {
                    throw new UserFriendlyException("Offer not found.");
                }

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<PagedResultDto<OfferDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.WithDetailsAsync(x => x.Application);

                queryable = queryable.OrderByDescending(x => x.CreationTime);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount)
                );

                var items = entities
                    .Select(x => MapToGetOutputDto(x))
                    .ToList();

                return new PagedResultDto<OfferDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected virtual async Task ValidateCreateAsync(CreateOfferDto input)
        {
            await ValidateApplicationExistsAsync(input.ApplicationId);

            var existingOffer = await Repository.FirstOrDefaultAsync(x => x.ApplicationId == input.ApplicationId);
            if (existingOffer != null)
            {
                throw new UserFriendlyException("This application already has an offer.");
            }

            ValidateOfferDate(input.StartDate, input.SentTime ?? Clock.Now, input.ExpiredTime);
        }

        protected virtual async Task ValidateUpdateAsync(Guid id, UpdateOfferDto input, Offer entity)
        {
            await ValidateApplicationExistsAsync(input.ApplicationId);

            ValidateStatus(input.Status);

            var existingOffer = await Repository.FirstOrDefaultAsync(x =>
                x.ApplicationId == input.ApplicationId && x.Id != id);

            if (existingOffer != null)
            {
                throw new UserFriendlyException("This application already has another offer.");
            }

            ValidateOfferDate(input.StartDate, input.SentTime, input.ExpiredTime);

            ValidateStatusTransition(entity.Status, input.Status);

            ValidateStatusDateConsistency(input.Status, input.SentTime, input.ExpiredTime);
        }

        protected virtual async Task ValidateApplicationExistsAsync(Guid applicationId)
        {
            var application = await _applicationRepository.FindAsync(applicationId);
            if (application == null)
            {
                throw new UserFriendlyException("Application does not exist.");
            }
        }

        protected virtual void ValidateStatus(OfferStatus status)
        {
            if (!Enum.IsDefined(typeof(OfferStatus), status))
            {
                throw new UserFriendlyException("Invalid Offer Status.");
            }
        }

        protected virtual void ValidateOfferDate(DateTime? startDate, DateTime? sentTime, DateTime? expiredTime)
        {
            if (sentTime.HasValue && expiredTime.HasValue && expiredTime.Value < sentTime.Value)
            {
                throw new UserFriendlyException("ExpiredTime cannot be earlier than SentTime.");
            }
        }

        protected virtual void ValidateStatusDateConsistency(OfferStatus status, DateTime? sentTime, DateTime? expiredTime)
        {
            if (status == OfferStatus.Draft)
            {
                if (sentTime.HasValue)
                {
                    throw new UserFriendlyException("Draft offer cannot have SentTime.");
                }
            }

            if (status == OfferStatus.Sent)
            {
                if (!sentTime.HasValue)
                {
                    throw new UserFriendlyException("Sent offer must have SentTime.");
                }
            }

            if (status == OfferStatus.Expired)
            {
                if (!expiredTime.HasValue)
                {
                    throw new UserFriendlyException("Expired offer must have ExpiredTime.");
                }
            }
        }

        protected virtual void ValidateStatusTransition(OfferStatus currentStatus, OfferStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(OfferStatus), currentStatus))
            {
                throw new UserFriendlyException("Current offer status is invalid.");
            }

            if (!Enum.IsDefined(typeof(OfferStatus), newStatus))
            {
                throw new UserFriendlyException("Invalid Offer Status.");
            }

            if (currentStatus == newStatus)
            {
                return;
            }

            switch (currentStatus)
            {
                case OfferStatus.Draft:
                    if (newStatus != OfferStatus.Sent &&
                        newStatus != OfferStatus.Draft)
                    {
                        throw new UserFriendlyException("Draft offer can only be updated to Draft or Sent.");
                    }
                    break;

                case OfferStatus.Sent:
                    if (newStatus != OfferStatus.Sent &&
                        newStatus != OfferStatus.Accepted &&
                        newStatus != OfferStatus.Declined &&
                        newStatus != OfferStatus.Expired)
                    {
                        throw new UserFriendlyException("Sent offer can only be updated to Sent, Accepted, Declined or Expired.");
                    }
                    break;

                case OfferStatus.Accepted:
                    throw new UserFriendlyException("Accepted offer cannot change status.");

                case OfferStatus.Declined:
                    throw new UserFriendlyException("Declined offer cannot change status.");

                case OfferStatus.Expired:
                    throw new UserFriendlyException("Expired offer cannot change status.");

                default:
                    throw new UserFriendlyException("Invalid Offer Status.");
            }
        }

        protected virtual async Task SyncApplicationStatusAsync(Offer offer)
        {
            var application = await _applicationRepository.GetAsync(offer.ApplicationId);

            switch (offer.Status)
            {
                case OfferStatus.Sent:
                    application.Status = ApplicationStatus.Offered;
                    break;
                case OfferStatus.Accepted:
                    application.Status = ApplicationStatus.OfferAccepted;
                    break;
                case OfferStatus.Declined:
                    application.Status = ApplicationStatus.OfferDeclined;
                    break;
                default:
                    return;
            }

            await _applicationRepository.UpdateAsync(application, autoSave: true);
        }
    }
}
