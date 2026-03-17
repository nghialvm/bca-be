using dvd.bca.Entity.ApplicationRoot;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.JobPositions.Dtos
{
    public class JobPositionDto : EntityDto<Guid>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid DepartmentId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
