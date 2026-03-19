using dvd.bca.Reports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace dvd.bca.Reports
{
    public interface IRecruitmentReportAppService : IApplicationService
    {
        Task<RecruitmentDashboardDto> GetDashboardSummaryAsync(RecruitmentDashboardFilterDto input);

        Task<RecruitmentFunnelDto> GetRecruitmentFunnelAsync(RecruitmentDashboardFilterDto input);

        Task<List<ApplicationStatusCountDto>> GetApplicationStatusStatisticsAsync(RecruitmentDashboardFilterDto input);

        Task<OfferStatisticsDto> GetOfferStatisticsAsync(RecruitmentDashboardFilterDto input);

        Task<HiringStatisticsDto> GetHiringStatisticsAsync(RecruitmentDashboardFilterDto input);

        Task<List<RecruitmentTrendItemDto>> GetRecruitmentTrendAsync(RecruitmentDashboardFilterDto input);

        Task<List<DepartmentRecruitmentStatisticsDto>> GetDepartmentStatisticsAsync(RecruitmentDashboardFilterDto input);
    }
}
