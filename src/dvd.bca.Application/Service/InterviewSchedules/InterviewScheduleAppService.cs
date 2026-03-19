using AutoMapper.Internal.Mappers;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Enums;
using dvd.bca.InterviewSchedules;
using dvd.bca.InterviewSchedules.Dtos;
using dvd.bca.Permissions;
using dvd.bca.Service.Emails;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.InterviewSchedules
{
    [Authorize(bcaPermissions.Recruitment.InterviewSchedules.Default)]
    public class InterviewScheduleAppService
        : CrudAppService<
            InterviewSchedule,
            InterviewScheduleDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateInterviewScheduleDto,
            UpdateInterviewScheduleDto>,
          IInterviewScheduleAppService
    {
        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;
        private readonly CandidateEmailManager _candidateEmailManager;


        public InterviewScheduleAppService(
            IRepository<InterviewSchedule, Guid> repository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<Candidate, Guid> candidateRepository,
            CandidateEmailManager candidateEmailManager)
            : base(repository)
        {
            _applicationRepository = applicationRepository;
            _candidateRepository = candidateRepository;
            _candidateEmailManager = candidateEmailManager;

            GetPolicyName = bcaPermissions.Recruitment.InterviewSchedules.Default;
            GetListPolicyName = bcaPermissions.Recruitment.InterviewSchedules.Default;
            CreatePolicyName = bcaPermissions.Recruitment.InterviewSchedules.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.InterviewSchedules.Update;
            DeletePolicyName = bcaPermissions.Recruitment.InterviewSchedules.Delete;
        }

        public override async Task<InterviewScheduleDto> CreateAsync(CreateInterviewScheduleDto input)
        {
            try
            {
                await ValidateCreateAsync(input);

                var entity = MapToEntity(input);

                entity.Status = InterviewStatus.Pending;

                entity = await Repository.InsertAsync(entity, autoSave: true);
                var application = await _applicationRepository.GetAsync(entity.ApplicationId);
                var candidate = await _candidateRepository.GetAsync(application.CandidateId);
                if (candidate.Email != null)
                {
                    await _candidateEmailManager.SendInterviewInvitationAsync(
                        candidate.Email,
                        candidate.FullName,
                        entity.ScheduledTime.ToString("dd/MM/yyyy HH:mm"),
                        (string.IsNullOrWhiteSpace(entity.MeetingLink) ? entity.Location : entity.MeetingLink) ?? ""
                    );
                }
                else {
                    throw new UserFriendlyException("Email cannot be empty");
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

        public override async Task<InterviewScheduleDto> UpdateAsync(Guid id, UpdateInterviewScheduleDto input)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

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
                var entity = await Repository.GetAsync(id);
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

        public override async Task<InterviewScheduleDto> GetAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);
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

        public override async Task<PagedResultDto<InterviewScheduleDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                queryable = queryable.OrderByDescending(x => x.CreationTime);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount)
                );

                var items = entities.Select(MapToGetOutputDto).ToList();

                return new PagedResultDto<InterviewScheduleDto>(totalCount, items);
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

        [Authorize(bcaPermissions.Recruitment.InterviewSchedules.Default)]
        public async Task<PagedResultDto<InterviewScheduleDto>> GetListByApplicationIdAsync(Guid applicationId)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                queryable = queryable
                    .Where(x => x.ApplicationId == applicationId)
                    .OrderBy(x => x.RoundNumber)
                    .ThenBy(x => x.ScheduledTime);

                var totalCount = await AsyncExecuter.CountAsync(queryable);
                var entities = await AsyncExecuter.ToListAsync(queryable);

                var items = entities.Select(MapToGetOutputDto).ToList();

                return new PagedResultDto<InterviewScheduleDto>(totalCount, items);
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

        protected override InterviewSchedule MapToEntity(CreateInterviewScheduleDto createInput)
        {
            var entity = ObjectMapper.Map<CreateInterviewScheduleDto, InterviewSchedule>(createInput);
            return entity;
        }

        protected override void MapToEntity(UpdateInterviewScheduleDto updateInput, InterviewSchedule entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override InterviewScheduleDto MapToGetOutputDto(InterviewSchedule entity)
        {
            return ObjectMapper.Map<InterviewSchedule, InterviewScheduleDto>(entity);
        }

        private async Task ValidateCreateAsync(CreateInterviewScheduleDto input)
        {
            var applicationExists = await _applicationRepository.AnyAsync(x => x.Id == input.ApplicationId);
            if (!applicationExists)
            {
                throw new UserFriendlyException("Application does not exist.");
            }

            if (!Enum.IsDefined(typeof(InterviewType), input.InterviewType))
            {
                throw new UserFriendlyException("InterviewType is invalid.");
            }

            var duplicatedRound = await Repository.AnyAsync(x =>
                x.ApplicationId == input.ApplicationId &&
                x.RoundNumber == input.RoundNumber);

            if (duplicatedRound)
            {
                throw new UserFriendlyException("RoundNumber already exists in this application.");
            }
        }

        private async Task ValidateUpdateAsync(Guid id, UpdateInterviewScheduleDto input)
        {
            var applicationExists = await _applicationRepository.AnyAsync(x => x.Id == input.ApplicationId);
            if (!applicationExists)
            {
                throw new UserFriendlyException("Application does not exist.");
            }

            if (!Enum.IsDefined(typeof(InterviewType), input.InterviewType))
            {
                throw new UserFriendlyException("InterviewType is invalid.");
            }

            if (!Enum.IsDefined(typeof(InterviewStatus), input.Status))
            {
                throw new UserFriendlyException("Status is invalid.");
            }

            var duplicatedRound = await Repository.AnyAsync(x =>
                x.Id != id &&
                x.ApplicationId == input.ApplicationId &&
                x.RoundNumber == input.RoundNumber);

            if (duplicatedRound)
            {
                throw new UserFriendlyException("RoundNumber already exists in this application.");
            }
        }
    }
}