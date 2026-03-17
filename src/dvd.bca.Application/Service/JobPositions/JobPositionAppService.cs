using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.JobPositions;
using dvd.bca.JobPositions.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace dvd.bca.Service.JobPositions
{
    public class JobPositionAppService :
        CrudAppService<
            JobPosition,
            JobPositionDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateJobPositionDto,
            UpdateJobPositionDto>,
        IJobPositionAppService
    {
        public JobPositionAppService(IRepository<JobPosition, Guid> repository)
            : base(repository)
        {
        }
        // Create
        public override async Task<JobPositionDto> CreateAsync(CreateJobPositionDto input)
        {
            try
            {
                var entity = MapToEntity(input);

                await Repository.InsertAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        //Update
        public override async Task<JobPositionDto> UpdateAsync(Guid id, UpdateJobPositionDto input)
        {
            try
            {
                var entity = await Repository.GetAsync(id);

                MapToEntity(input, entity);

                await Repository.UpdateAsync(entity, autoSave: true);

                return MapToGetOutputDto(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        
        //Delete
        public override async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await Repository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new UserFriendlyException($"Không tìm thấy vị trí công việc với id: {id}");
                }

                await Repository.DeleteAsync(entity, autoSave: true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        //Get
        public override async Task<JobPositionDto> GetAsync(Guid id)
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

        public override async Task<PagedResultDto<JobPositionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var queryable = await Repository.GetQueryableAsync();

                var totalCount = await queryable.CountAsync();

                var entities = await queryable
                    .OrderBy(x => x.Name)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToListAsync();

                var dtos = entities
                    .Select(x => MapToGetOutputDto(x))
                    .ToList();

                return new PagedResultDto<JobPositionDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}