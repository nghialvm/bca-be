using dvd.bca.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace dvd.bca.Offers.Dtos
{
    public class OfferDto : FullAuditedEntityDto<Guid>
    {
        public Guid ApplicationId { get; set; }

        public decimal Salary { get; set; }

        public DateTime? StartDate { get; set; }

        public int? ProbationMonths { get; set; }

        public string? WorkLocation { get; set; }

        public string? Benefit { get; set; }

        public string? Note { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime? SentTime { get; set; }

        public DateTime? ExpiredTime { get; set; }
    }
}