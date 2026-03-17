using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.ApplicationScreenings.Dtos
{
    public class ApplicationScreeningDetailDto : FullAuditedEntityDto<Guid>
    {
        public Guid ApplicationId { get; set; }

        public Guid ScreenedByUserId { get; set; }

        public DateTime ScreeningTime { get; set; }

        public ScreeningResult Result { get; set; }

        public string? Comment { get; set; }

        public decimal? Score { get; set; }

        public string? CriteriaSummary { get; set; }
    }
}
