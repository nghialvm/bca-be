using dvd.bca.Departments;
using dvd.bca.Departments.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace dvd.bca.Service.Departments
{
    public class DepartmentAppService :
        CrudAppService<
            Department,
            DepartmentDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateDepartmentDto,
            UpdateDepartmentDto>,
        IDepartmentAppService
    {

        public DepartmentAppService(IRepository<Department, Guid> repository)
            : base(repository)
        {
        }

        // CREATE
        public override async Task<DepartmentDto> CreateAsync(CreateDepartmentDto input)
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

        // UPDATE
        public override async Task<DepartmentDto> UpdateAsync(Guid id, UpdateDepartmentDto input)
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

        // DELETE
        public override async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await Repository.FirstOrDefaultAsync(x => x.Id == id);
                if (entity == null)
                {
                    throw new UserFriendlyException($"Department with id '{id}' was not found.");
                }

                await Repository.DeleteAsync(entity, autoSave: true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        // GET BY ID
        public override async Task<DepartmentDto> GetAsync(Guid id)
        {
            var entity = await Repository.FindAsync(id);

            if (entity == null)
            {
                throw new UserFriendlyException($"Không tìm thấy phòng ban với id: {id}");
            }

            return MapToGetOutputDto(entity);
        }

        // GET LIST
        public override async Task<PagedResultDto<DepartmentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
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

            return new PagedResultDto<DepartmentDto>(totalCount, dtos);
        }
    }
}
