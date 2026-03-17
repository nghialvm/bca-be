using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.InterviewEvaluations;
using dvd.bca.InterviewEvaluations.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.InterviewEvaluations
{
    public class InterviewEvaluationAppService
        : CrudAppService<
            InterviewEvaluation,
            InterviewEvaluationDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateInterviewEvaluationDto,
            UpdateInterviewEvaluationDto>,
          IInterviewEvaluationAppService
    {
        private readonly IRepository<InterviewEvaluation, Guid> _interviewEvaluationRepository;
        private readonly IRepository<InterviewSchedule, Guid> _interviewScheduleRepository;
        private readonly IRepository<Application, Guid> _applicationRepository;

        public InterviewEvaluationAppService(
            IRepository<InterviewEvaluation, Guid> repository,
            IRepository<InterviewSchedule, Guid> interviewScheduleRepository,
            IRepository<Application, Guid> applicationRepository)
            : base(repository)
        {
            _interviewEvaluationRepository = repository;
            _interviewScheduleRepository = interviewScheduleRepository;
            _applicationRepository = applicationRepository;
        }

        public override async Task<InterviewEvaluationDto> CreateAsync(CreateInterviewEvaluationDto input)
        {
            try
            {
                await ValidateCreateOrUpdateAsync(input.InterviewScheduleId, input.ApplicationId, input.EvaluatorUserId, null);

                var entity = MapToEntity(input);

                entity = await _interviewEvaluationRepository.InsertAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<InterviewEvaluationDto> UpdateAsync(Guid id, UpdateInterviewEvaluationDto input)
        {
            try
            {
                var entity = await _interviewEvaluationRepository.GetAsync(id);

                await ValidateCreateOrUpdateAsync(input.InterviewScheduleId, input.ApplicationId, input.EvaluatorUserId, id);

                MapToEntity(input, entity);

                entity = await _interviewEvaluationRepository.UpdateAsync(entity, autoSave: true);

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
                await _interviewEvaluationRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<InterviewEvaluationDto> GetAsync(Guid id)
        {
            try
            {
                var entity = await _interviewEvaluationRepository.GetAsync(id);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public override async Task<PagedResultDto<InterviewEvaluationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await _interviewEvaluationRepository.GetQueryableAsync();

                queryable = queryable.OrderByDescending(x => x.CreationTime);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount)
                );

                var dtos = entities.Select(MapToGetOutputDto).ToList();

                return new PagedResultDto<InterviewEvaluationDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<InterviewEvaluationDto>> GetListByInterviewScheduleIdAsync(Guid interviewScheduleId)
        {
            try
            {
                var queryable = await _interviewEvaluationRepository.GetQueryableAsync();

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Where(x => x.InterviewScheduleId == interviewScheduleId)
                        .OrderByDescending(x => x.CreationTime)
                );

                return entities.Select(MapToGetOutputDto).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<InterviewEvaluationDto>> GetListByApplicationIdAsync(Guid applicationId)
        {
            try
            {
                var queryable = await _interviewEvaluationRepository.GetQueryableAsync();

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .Where(x => x.ApplicationId == applicationId)
                        .OrderByDescending(x => x.CreationTime)
                );

                return entities.Select(MapToGetOutputDto).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<InterviewEvaluationDto> RecordEvaluationResultAsync(CreateInterviewEvaluationDto input)
        {
            return await CreateAsync(input);
        }

        public async Task<InterviewEvaluationDto> ChangeEvaluationResultAsync(Guid id, UpdateInterviewEvaluationDto input)
        {
            return await UpdateAsync(id, input);
        }

        protected override InterviewEvaluation MapToEntity(CreateInterviewEvaluationDto createInput)
        {
            return ObjectMapper.Map<CreateInterviewEvaluationDto, InterviewEvaluation>(createInput);
        }

        protected override void MapToEntity(UpdateInterviewEvaluationDto updateInput, InterviewEvaluation entity)
        {
            ObjectMapper.Map(updateInput, entity);
        }

        protected override InterviewEvaluationDto MapToGetOutputDto(InterviewEvaluation entity)
        {
            return ObjectMapper.Map<InterviewEvaluation, InterviewEvaluationDto>(entity);
        }

        private async Task ValidateCreateOrUpdateAsync(
            Guid interviewScheduleId,
            Guid applicationId,
            Guid evaluatorUserId,
            Guid? currentInterviewEvaluationId)
        {
            var interviewSchedule = await _interviewScheduleRepository.FindAsync(interviewScheduleId);
            if (interviewSchedule == null)
            {
                throw new UserFriendlyException("InterviewScheduleId is not valid.");
            }

            var application = await _applicationRepository.FindAsync(applicationId);
            if (application == null)
            {
                throw new UserFriendlyException("ApplicationId is not valid.");
            }

            if (interviewSchedule.ApplicationId != applicationId)
            {
                throw new UserFriendlyException("InterviewSchedule does not belong to the specified Application.");
            }

            if (evaluatorUserId == Guid.Empty)
            {
                throw new UserFriendlyException("EvaluatorUserId is required.");
            }

            // Rule tùy chọn:
            // Nếu 1 evaluator chỉ được đánh giá 1 lần trên 1 schedule thì bật đoạn check dưới đây
            /*
            var queryable = await _interviewEvaluationRepository.GetQueryableAsync();

            var existed = await AsyncExecuter.AnyAsync(
                queryable.Where(x =>
                    x.InterviewScheduleId == interviewScheduleId &&
                    x.EvaluatorUserId == evaluatorUserId &&
                    (!currentInterviewEvaluationId.HasValue || x.Id != currentInterviewEvaluationId.Value))
            );

            if (existed)
            {
                throw new UserFriendlyException("This evaluator has already evaluated this interview schedule.");
            }
            */
        }
    }
}