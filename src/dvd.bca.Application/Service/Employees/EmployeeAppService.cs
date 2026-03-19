using dvd.bca.Employees;
using dvd.bca.Employees.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Entity.Results;
using dvd.bca.Enums;
using dvd.bca.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Service.Employees
{
    [Authorize(bcaPermissions.Recruitment.Employees.Default)]
    public class EmployeeAppService
        : CrudAppService<
            Employee,
            EmployeeDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateEmployeeDto,
            UpdateEmployeeDto>,
          IEmployeeAppService
    {
        private readonly IRepository<Candidate, Guid> _candidateRepository;
        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<Offer, Guid> _offerRepository;
        private readonly IRepository<Department, Guid> _departmentRepository;
        private readonly IRepository<JobPosition, Guid> _jobPositionRepository;

        public EmployeeAppService(
            IRepository<Employee, Guid> repository,
            IRepository<Candidate, Guid> candidateRepository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<Offer, Guid> offerRepository,
            IRepository<Department, Guid> departmentRepository,
            IRepository<JobPosition, Guid> jobPositionRepository)
            : base(repository)
        {
            _candidateRepository = candidateRepository;
            _applicationRepository = applicationRepository;
            _offerRepository = offerRepository;
            _departmentRepository = departmentRepository;
            _jobPositionRepository = jobPositionRepository;

            GetPolicyName = bcaPermissions.Recruitment.Employees.Default;
            GetListPolicyName = bcaPermissions.Recruitment.Employees.Default;
            CreatePolicyName = bcaPermissions.Recruitment.Employees.Create;
            UpdatePolicyName = bcaPermissions.Recruitment.Employees.Update;
            DeletePolicyName = bcaPermissions.Recruitment.Employees.Delete;
        }

        public override async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            try
            {
                await CheckCreatePolicyAsync();

                await ValidateCreateAsync(input);

                var entity = MapToEntity(input);
                entity.Status = EmployeeStatus.PendingActivation; // Mới tạo, chưa chính thức active
                entity.SourceType = GetSourceType(input.ApplicationId, input.OfferId); // Xác định nguồn tạo

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

        public override async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            try
            {
                await CheckUpdatePolicyAsync();

                var entity = await GetEntityByIdAsync(id);

                await ValidateUpdateAsync(id, input);

                MapToEntity(input, entity);
                entity.SourceType = GetSourceType(input.ApplicationId, input.OfferId); // Cập nhật lại nguồn tạo

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

        public override async Task<EmployeeDto> GetAsync(Guid id)
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

        public override async Task<PagedResultDto<EmployeeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                await CheckGetListPolicyAsync();

                var queryable = await Repository.GetQueryableAsync();

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var sorting = input.Sorting.IsNullOrWhiteSpace()
                    ? nameof(Employee.JoinDate) + " desc"
                    : input.Sorting;

                var entities = await AsyncExecuter.ToListAsync(
                    queryable
                        .OrderBy(sorting)
                        .PageBy(input)
                );

                var items = entities
                    .Select(MapToGetOutputDto)
                    .ToList();

                return new PagedResultDto<EmployeeDto>(totalCount, items);
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

        [Authorize(bcaPermissions.Recruitment.Employees.Default)]
        public async Task<EmployeeDto> GetByCandidateIdAsync(Guid candidateId)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                var entity = queryable.FirstOrDefault(x => x.CandidateId == candidateId);

                if (entity == null)
                {
                    throw new UserFriendlyException("Employee not found.");
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

        protected override Employee MapToEntity(CreateEmployeeDto createInput)
        {
            var entity = base.MapToEntity(createInput);
            return entity;
        }

        protected override void MapToEntity(UpdateEmployeeDto updateInput, Employee entity)
        {
            base.MapToEntity(updateInput, entity);
        }

        protected override EmployeeDto MapToGetOutputDto(Employee entity)
        {
            return base.MapToGetOutputDto(entity);
        }

        private async Task ValidateCreateAsync(CreateEmployeeDto input)
        {
            var candidateExists = await _candidateRepository.AnyAsync(x => x.Id == input.CandidateId);
            if (!candidateExists)
            {
                throw new UserFriendlyException("Candidate does not exist.");
            }

            var departmentExists = await _departmentRepository.AnyAsync(x => x.Id == input.DepartmentId);
            if (!departmentExists)
            {
                throw new UserFriendlyException("Department does not exist.");
            }

            var jobPositionExists = await _jobPositionRepository.AnyAsync(x => x.Id == input.JobPositionId);
            if (!jobPositionExists)
            {
                throw new UserFriendlyException("Job position does not exist.");
            }

            if (input.ApplicationId.HasValue)
            {
                var applicationExists = await _applicationRepository.AnyAsync(x => x.Id == input.ApplicationId.Value);
                if (!applicationExists)
                {
                    throw new UserFriendlyException("Application does not exist.");
                }
            }

            if (input.OfferId.HasValue)
            {
                var offerExists = await _offerRepository.AnyAsync(x => x.Id == input.OfferId.Value);
                if (!offerExists)
                {
                    throw new UserFriendlyException("Offer does not exist.");
                }

                var acceptedOfferExists = await _offerRepository.AnyAsync(x =>
                    x.Id == input.OfferId.Value &&
                    x.Status == OfferStatus.Accepted);

                if (!acceptedOfferExists)
                {
                    throw new UserFriendlyException("Only accepted offers can create employee.");
                }
            }

            var employeeCodeExists = await Repository.AnyAsync(x => x.EmployeeCode == input.EmployeeCode);
            if (employeeCodeExists)
            {
                throw new UserFriendlyException("Employee code already exists.");
            }

            var candidateHasEmployee = await Repository.AnyAsync(x => x.CandidateId == input.CandidateId);
            if (candidateHasEmployee)
            {
                throw new UserFriendlyException("This candidate already has an employee record.");
            }

            if (input.ApplicationId.HasValue)
            {
                var applicationHasEmployee = await Repository.AnyAsync(x => x.ApplicationId == input.ApplicationId.Value);
                if (applicationHasEmployee)
                {
                    throw new UserFriendlyException("This application already has an employee record.");
                }
            }

            if (input.OfferId.HasValue)
            {
                var offerHasEmployee = await Repository.AnyAsync(x => x.OfferId == input.OfferId.Value);
                if (offerHasEmployee)
                {
                    throw new UserFriendlyException("This offer already has an employee record.");
                }
            }
        }

        private async Task ValidateUpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            var candidateExists = await _candidateRepository.AnyAsync(x => x.Id == input.CandidateId);
            if (!candidateExists)
            {
                throw new UserFriendlyException("Candidate does not exist.");
            }

            var departmentExists = await _departmentRepository.AnyAsync(x => x.Id == input.DepartmentId);
            if (!departmentExists)
            {
                throw new UserFriendlyException("Department does not exist.");
            }

            var jobPositionExists = await _jobPositionRepository.AnyAsync(x => x.Id == input.JobPositionId);
            if (!jobPositionExists)
            {
                throw new UserFriendlyException("Job position does not exist.");
            }

            if (input.ApplicationId.HasValue)
            {
                var applicationExists = await _applicationRepository.AnyAsync(x => x.Id == input.ApplicationId.Value);
                if (!applicationExists)
                {
                    throw new UserFriendlyException("Application does not exist.");
                }
            }

            if (input.OfferId.HasValue)
            {
                var offerExists = await _offerRepository.AnyAsync(x => x.Id == input.OfferId.Value);
                if (!offerExists)
                {
                    throw new UserFriendlyException("Offer does not exist.");
                }

                var acceptedOfferExists = await _offerRepository.AnyAsync(x =>
                    x.Id == input.OfferId.Value &&
                    x.Status == OfferStatus.Accepted);

                if (!acceptedOfferExists)
                {
                    throw new UserFriendlyException("Only accepted offers can create employee.");
                }
            }

            if (!Enum.IsDefined(typeof(EmployeeStatus), input.Status))
            {
                throw new UserFriendlyException("Employee status is invalid.");
            }

            if (!Enum.IsDefined(typeof(EmployeeSourceType), input.SourceType))
            {
                throw new UserFriendlyException("Employee source type is invalid.");
            }

            var employeeCodeExists = await Repository.AnyAsync(x =>
                x.Id != id &&
                x.EmployeeCode == input.EmployeeCode);

            if (employeeCodeExists)
            {
                throw new UserFriendlyException("Employee code already exists.");
            }

            var candidateHasEmployee = await Repository.AnyAsync(x =>
                x.Id != id &&
                x.CandidateId == input.CandidateId);

            if (candidateHasEmployee)
            {
                throw new UserFriendlyException("This candidate already has an employee record.");
            }

            if (input.ApplicationId.HasValue)
            {
                var applicationHasEmployee = await Repository.AnyAsync(x =>
                    x.Id != id &&
                    x.ApplicationId == input.ApplicationId.Value);

                if (applicationHasEmployee)
                {
                    throw new UserFriendlyException("This application already has an employee record.");
                }
            }

            if (input.OfferId.HasValue)
            {
                var offerHasEmployee = await Repository.AnyAsync(x =>
                    x.Id != id &&
                    x.OfferId == input.OfferId.Value);

                if (offerHasEmployee)
                {
                    throw new UserFriendlyException("This offer already has an employee record.");
                }
            }
        }

        private EmployeeSourceType GetSourceType(Guid? applicationId, Guid? offerId)
        {
            if (offerId.HasValue)
            {
                return EmployeeSourceType.FromOffer; // Tạo từ Offer
            }

            if (applicationId.HasValue)
            {
                return EmployeeSourceType.FromApplication; // Tạo từ Application
            }

            return EmployeeSourceType.FromCandidate; // Còn lại xem như từ Candidate
        }
    }
}