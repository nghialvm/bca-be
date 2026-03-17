using dvd.bca.Entity.ApplicationRoot;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;

namespace dvd.bca.Departments.Dtos
{
    public class DepartmentDto : EntityDto<Guid>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid? ManagerUserId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
