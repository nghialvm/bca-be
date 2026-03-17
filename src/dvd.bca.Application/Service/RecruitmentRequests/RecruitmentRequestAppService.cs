using AutoMapper.Internal.Mappers;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Enums;
using dvd.bca.RecruitmentRequests;
using dvd.bca.RecruitmentRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.RecruitmentRequests
{
    [Authorize]
    public class RecruitmentRequestAppService
        : CrudAppService<
            RecruitmentRequest,
            RecruitmentRequestDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateRecruitmentRequestDto,
            UpdateRecruitmentRequestDto>,
          IRecruitmentRequestAppService
    {
        private readonly IRepository<Department, Guid> _departmentRepository;
        private readonly IRepository<JobPosition, Guid> _jobPositionRepository;

        public RecruitmentRequestAppService(
            IRepository<RecruitmentRequest, Guid> repository,
            IRepository<Department, Guid> departmentRepository,
            IRepository<JobPosition, Guid> jobPositionRepository)
            : base(repository)
        {
            _departmentRepository = departmentRepository;
            _jobPositionRepository = jobPositionRepository;
        }

        protected override async Task<IQueryable<RecruitmentRequest>> CreateFilteredQueryAsync(PagedAndSortedResultRequestDto input)
        {
            return await Repository.GetQueryableAsync();
        }

        protected override async Task<RecruitmentRequest> MapToEntityAsync(CreateRecruitmentRequestDto input)
        {
            try
            {
                await ValidateForeignKeysAsync(input.DepartmentId, input.PositionId);
                ValidateBusinessData(input.Headcount, input.SalaryMin, input.SalaryMax, input.ApplicationDeadline);

                var entity = ObjectMapper.Map<CreateRecruitmentRequestDto, RecruitmentRequest>(input);

                entity.Status = RecruitmentRequestStatus.Draft;
                entity.CreatedByUserId = CurrentUser.Id;
                entity.ApprovedByUserId = null;
                entity.ApprovedTime = null;
                entity.RejectReason = null;
                entity.PublishedTime = null;
                entity.ClosedTime = null;

                return entity;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        protected override async Task MapToEntityAsync(UpdateRecruitmentRequestDto input, RecruitmentRequest entity)
        {
            try
            {
                await ValidateForeignKeysAsync(input.DepartmentId, input.PositionId);
                ValidateBusinessData(input.Headcount, input.SalaryMin, input.SalaryMax, input.ApplicationDeadline);

                if (entity.Status == RecruitmentRequestStatus.Approved ||
                    entity.Status == RecruitmentRequestStatus.Published ||
                    entity.Status == RecruitmentRequestStatus.Closed)
                {
                    throw new UserFriendlyException("Approved, published or closed recruitment requests cannot be edited.");
                }

                ObjectMapper.Map(input, entity);

                if (entity.Status == RecruitmentRequestStatus.Rejected)
                {
                    entity.Status = RecruitmentRequestStatus.Draft;
                    entity.RejectReason = null;
                    entity.ApprovedByUserId = null;
                    entity.ApprovedTime = null;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        protected override RecruitmentRequestDto MapToGetOutputDto(RecruitmentRequest entity)
        {
            return ObjectMapper.Map<RecruitmentRequest, RecruitmentRequestDto>(entity);
        }

        public override async Task<RecruitmentRequestDto> CreateAsync(CreateRecruitmentRequestDto input)
        {
            try
            {
                return await base.CreateAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public override async Task<RecruitmentRequestDto> UpdateAsync(Guid id, UpdateRecruitmentRequestDto input)
        {
            try
            {
                return await base.UpdateAsync(id, input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status == RecruitmentRequestStatus.Published ||
                    entity.Status == RecruitmentRequestStatus.Closed)
                {
                    throw new UserFriendlyException("Published or closed recruitment requests cannot be deleted.");
                }

                await base.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<RecruitmentRequestDto> SubmitForApprovalAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status != RecruitmentRequestStatus.Draft &&
                    entity.Status != RecruitmentRequestStatus.Rejected)
                {
                    throw new UserFriendlyException("Only draft or rejected recruitment requests can be submitted for approval.");
                }

                entity.Status = RecruitmentRequestStatus.PendingApproval;
                entity.RejectReason = null;

                await Repository.UpdateAsync(entity, true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<RecruitmentRequestDto> ApproveAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status != RecruitmentRequestStatus.PendingApproval)
                {
                    throw new UserFriendlyException("Only pending approval recruitment requests can be approved.");
                }

                entity.Status = RecruitmentRequestStatus.Approved;
                entity.ApprovedByUserId = CurrentUser.Id;
                entity.ApprovedTime = Clock.Now;
                entity.RejectReason = null;

                await Repository.UpdateAsync(entity, true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<RecruitmentRequestDto> RejectAsync(Guid id, RejectRecruitmentRequestDto input)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status != RecruitmentRequestStatus.PendingApproval)
                {
                    throw new UserFriendlyException("Only pending approval recruitment requests can be rejected.");
                }

                if (string.IsNullOrWhiteSpace(input.Reason))
                {
                    throw new UserFriendlyException("Reject reason is required.");
                }

                entity.Status = RecruitmentRequestStatus.Rejected;
                entity.RejectReason = input.Reason;
                entity.ApprovedByUserId = CurrentUser.Id;
                entity.ApprovedTime = Clock.Now;

                await Repository.UpdateAsync(entity, true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<RecruitmentRequestDto> PublishAsync(Guid id)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status != RecruitmentRequestStatus.Approved)
                {
                    throw new UserFriendlyException("Only approved recruitment requests can be published.");
                }

                if (entity.ApplicationDeadline.HasValue && entity.ApplicationDeadline.Value < Clock.Now)
                {
                    throw new UserFriendlyException("Cannot publish because application deadline is in the past.");
                }

                entity.Status = RecruitmentRequestStatus.Published;
                entity.PublishedTime = Clock.Now;

                await Repository.UpdateAsync(entity, true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<RecruitmentRequestDto> CloseAsync(Guid id, CloseRecruitmentRequestDto input)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                if (entity.Status != RecruitmentRequestStatus.Published)
                {
                    throw new UserFriendlyException("Only published recruitment requests can be closed.");
                }

                entity.Status = RecruitmentRequestStatus.Closed;
                entity.ClosedTime = Clock.Now;

                await Repository.UpdateAsync(entity, true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.InnerException?.Message ?? ex.Message);
            }
        }

        private async Task ValidateForeignKeysAsync(Guid departmentId, Guid PositionId)
        {
            var department = await _departmentRepository.FirstOrDefaultAsync(x => x.Id == departmentId);
            if (department == null)
            {
                throw new UserFriendlyException($"Department does not exist: {departmentId}");
            }

            var jobPosition = await _jobPositionRepository.FirstOrDefaultAsync(x => x.Id == PositionId);
            if (jobPosition == null)
            {
                throw new UserFriendlyException($"Job position does not exist: {PositionId}");
            }
        }

        private void ValidateBusinessData(
            int headcount,
            decimal? salaryMin,
            decimal? salaryMax,
            DateTime? applicationDeadline)
        {
            if (headcount <= 0)
            {
                throw new UserFriendlyException("Headcount must be greater than 0.");
            }

            if (salaryMin.HasValue && salaryMax.HasValue && salaryMin > salaryMax)
            {
                throw new UserFriendlyException("SalaryMin cannot be greater than SalaryMax.");
            }

            if (applicationDeadline.HasValue && applicationDeadline.Value.Date < Clock.Now.Date)
            {
                throw new UserFriendlyException("Application deadline cannot be in the past.");
            }
        }
    }
}
