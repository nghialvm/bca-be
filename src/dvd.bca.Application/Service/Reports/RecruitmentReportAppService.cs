using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.Entity.Results;
using dvd.bca.Enums;
using dvd.bca.Permissions;
using dvd.bca.Reports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace dvd.bca.Reports
{
    [Authorize(bcaPermissions.Recruitment.Reports.Default)]
    public class RecruitmentReportAppService : ApplicationService, IRecruitmentReportAppService
    {
        private readonly IRepository<RecruitmentRequest, Guid> _recruitmentRequestRepository;
        private readonly IRepository<Application, Guid> _applicationRepository;
        private readonly IRepository<ApplicationScreening, Guid> _applicationScreeningRepository;
        private readonly IRepository<InterviewSchedule, Guid> _interviewScheduleRepository;
        private readonly IRepository<InterviewEvaluation, Guid> _interviewEvaluationRepository;
        private readonly IRepository<Offer, Guid> _offerRepository;
        private readonly IRepository<Employee, Guid> _employeeRepository;
        private readonly IRepository<Candidate, Guid> _candidateRepository;
        private readonly IRepository<Department, Guid> _departmentRepository;

        public RecruitmentReportAppService(
            IRepository<RecruitmentRequest, Guid> recruitmentRequestRepository,
            IRepository<Application, Guid> applicationRepository,
            IRepository<ApplicationScreening, Guid> applicationScreeningRepository,
            IRepository<InterviewSchedule, Guid> interviewScheduleRepository,
            IRepository<InterviewEvaluation, Guid> interviewEvaluationRepository,
            IRepository<Offer, Guid> offerRepository,
            IRepository<Employee, Guid> employeeRepository,
            IRepository<Candidate, Guid> candidateRepository,
            IRepository<Department, Guid> departmentRepository)
        {
            _recruitmentRequestRepository = recruitmentRequestRepository;
            _applicationRepository = applicationRepository;
            _applicationScreeningRepository = applicationScreeningRepository;
            _interviewScheduleRepository = interviewScheduleRepository;
            _interviewEvaluationRepository = interviewEvaluationRepository;
            _offerRepository = offerRepository;
            _employeeRepository = employeeRepository;
            _candidateRepository = candidateRepository;
            _departmentRepository = departmentRepository;
        }
        /// Lấy dữ liệu tổng quan dashboard tuyển dụng (KPI tổng)
        /// Dùng để hiển thị các số liệu chính như: số đợt tuyển, số hồ sơ, số offer, số nhân sự được tuyển
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<RecruitmentDashboardDto> GetDashboardSummaryAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var candidatesQuery = await _candidateRepository.GetQueryableAsync();
                var screeningsQuery = await _applicationScreeningRepository.GetQueryableAsync();
                var interviewSchedulesQuery = await _interviewScheduleRepository.GetQueryableAsync();
                var interviewEvaluationsQuery = await _interviewEvaluationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();
                var employeesQuery = await _employeeRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                candidatesQuery = ApplyCandidateFilter(candidatesQuery, input);
                screeningsQuery = ApplyScreeningFilter(screeningsQuery, applicationsQuery);
                interviewSchedulesQuery = ApplyInterviewScheduleFilter(interviewSchedulesQuery, applicationsQuery);
                interviewEvaluationsQuery = ApplyInterviewEvaluationFilter(interviewEvaluationsQuery, applicationsQuery);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);
                employeesQuery = ApplyEmployeeFilter(employeesQuery, input);

                var totalRecruitmentRequests = await recruitmentRequestsQuery.CountAsync();
                var totalPublishedRecruitmentRequests = await recruitmentRequestsQuery
                    .CountAsync(x => x.Status == RecruitmentRequestStatus.Published);

                var totalApplications = await applicationsQuery.CountAsync();
                var totalCandidates = await candidatesQuery.CountAsync();

                var totalScreenedApplications = await screeningsQuery
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var totalInterviewScheduled = await interviewSchedulesQuery
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var totalInterviewPassed = await interviewEvaluationsQuery
                    .CountAsync(x => x.Result == InterviewResult.Pass);

                var totalOffers = await offersQuery.CountAsync();

                var totalOfferAccepted = await offersQuery
                    .CountAsync(x => x.Status == OfferStatus.Accepted);

                var totalHiredEmployees = await employeesQuery.CountAsync();

                decimal hiringRate = 0;
                decimal offerAcceptanceRate = 0;

                if (totalApplications > 0)
                {
                    hiringRate = Math.Round((decimal)totalHiredEmployees / totalApplications * 100, 2);
                }

                if (totalOffers > 0)
                {
                    offerAcceptanceRate = Math.Round((decimal)totalOfferAccepted / totalOffers * 100, 2);
                }

                return new RecruitmentDashboardDto
                {
                    TotalRecruitmentRequests = totalRecruitmentRequests,
                    TotalPublishedRecruitmentRequests = totalPublishedRecruitmentRequests,
                    TotalApplications = totalApplications,
                    TotalCandidates = totalCandidates,
                    TotalScreenedApplications = totalScreenedApplications,
                    TotalInterviewScheduled = totalInterviewScheduled,
                    TotalInterviewPassed = totalInterviewPassed,
                    TotalOffers = totalOffers,
                    TotalOfferAccepted = totalOfferAccepted,
                    TotalHiredEmployees = totalHiredEmployees,
                    HiringRate = hiringRate,
                    OfferAcceptanceRate = offerAcceptanceRate
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// Lấy dữ liệu funnel tuyển dụng
        /// Dùng để hiển thị luồng chuyển đổi từ ứng viên nộp hồ sơ đến khi được tuyển (apply → screening → interview → offer → hired)
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<RecruitmentFunnelDto> GetRecruitmentFunnelAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var screeningsQuery = await _applicationScreeningRepository.GetQueryableAsync();
                var interviewSchedulesQuery = await _interviewScheduleRepository.GetQueryableAsync();
                var interviewEvaluationsQuery = await _interviewEvaluationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();
                var employeesQuery = await _employeeRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                screeningsQuery = ApplyScreeningFilter(screeningsQuery, applicationsQuery);
                interviewSchedulesQuery = ApplyInterviewScheduleFilter(interviewSchedulesQuery, applicationsQuery);
                interviewEvaluationsQuery = ApplyInterviewEvaluationFilter(interviewEvaluationsQuery, applicationsQuery);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);
                employeesQuery = ApplyEmployeeFilter(employeesQuery, input);

                var totalApplications = await applicationsQuery.CountAsync();

                var screeningPassed = await screeningsQuery
                    .Where(x => x.Result == ScreeningResult.Pass)
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var interviewScheduled = await interviewSchedulesQuery
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var interviewPassed = await interviewEvaluationsQuery
                    .Where(x => x.Result == InterviewResult.Pass)
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var offered = await offersQuery
                    .Select(x => x.ApplicationId)
                    .Distinct()
                    .CountAsync();

                var hired = await employeesQuery.CountAsync();

                return new RecruitmentFunnelDto
                {
                    TotalApplications = totalApplications,
                    ScreeningPassed = screeningPassed,
                    InterviewScheduled = interviewScheduled,
                    InterviewPassed = interviewPassed,
                    Offered = offered,
                    Hired = hired
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        /// Thống kê số lượng hồ sơ theo trạng thái
        /// Dùng để vẽ biểu đồ trạng thái hồ sơ (Submitted, Screening, Interview, Rejected, ...)
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<List<ApplicationStatusCountDto>> GetApplicationStatusStatisticsAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);

                var result = await applicationsQuery
                    .GroupBy(x => x.Status)
                    .Select(g => new ApplicationStatusCountDto
                    {
                        Status = g.Key.ToString(),
                        Count = g.Count()
                    })
                    .OrderBy(x => x.Status)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// Thống kê tình trạng offer
        /// Dùng để theo dõi số lượng offer theo từng trạng thái (Draft, Sent, Accepted, Declined, ...)
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<OfferStatisticsDto> GetOfferStatisticsAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);

                var totalOffers = await offersQuery.CountAsync();
                var draftOffers = await offersQuery.CountAsync(x => x.Status == OfferStatus.Draft);
                var sentOffers = await offersQuery.CountAsync(x => x.Status == OfferStatus.Sent);
                var acceptedOffers = await offersQuery.CountAsync(x => x.Status == OfferStatus.Accepted);
                var declinedOffers = await offersQuery.CountAsync(x => x.Status == OfferStatus.Declined);
                var expiredOffers = await offersQuery.CountAsync(x => x.Status == OfferStatus.Expired);

                decimal acceptanceRate = 0;
                if (totalOffers > 0)
                {
                    acceptanceRate = Math.Round((decimal)acceptedOffers / totalOffers * 100, 2);
                }

                return new OfferStatisticsDto
                {
                    TotalOffers = totalOffers,
                    DraftOffers = draftOffers,
                    SentOffers = sentOffers,
                    AcceptedOffers = acceptedOffers,
                    DeclinedOffers = declinedOffers,
                    ExpiredOffers = expiredOffers,
                    AcceptanceRate = acceptanceRate
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// Thống kê hiệu quả tuyển dụng
        /// Dùng để tính tỷ lệ chuyển đổi từ hồ sơ → tuyển thành công và từ offer → tuyển thành công
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<HiringStatisticsDto> GetHiringStatisticsAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();
                var employeesQuery = await _employeeRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);
                employeesQuery = ApplyEmployeeFilter(employeesQuery, input);

                var totalApplications = await applicationsQuery.CountAsync();
                var totalOffersAccepted = await offersQuery.CountAsync(x => x.Status == OfferStatus.Accepted);
                var totalHiredEmployees = await employeesQuery.CountAsync();

                decimal applicationToHireRate = 0;
                decimal offerAcceptedToHireRate = 0;

                if (totalApplications > 0)
                {
                    applicationToHireRate = Math.Round((decimal)totalHiredEmployees / totalApplications * 100, 2);
                }

                if (totalOffersAccepted > 0)
                {
                    offerAcceptedToHireRate = Math.Round((decimal)totalHiredEmployees / totalOffersAccepted * 100, 2);
                }

                return new HiringStatisticsDto
                {
                    TotalApplications = totalApplications,
                    TotalOffersAccepted = totalOffersAccepted,
                    TotalHiredEmployees = totalHiredEmployees,
                    ApplicationToHireRate = applicationToHireRate,
                    OfferAcceptedToHireRate = offerAcceptedToHireRate
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// Thống kê xu hướng tuyển dụng theo thời gian
        /// Dùng để hiển thị biểu đồ theo tháng/ngày (số hồ sơ, offer, nhân sự được tuyển)
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<List<RecruitmentTrendItemDto>> GetRecruitmentTrendAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();
                var employeesQuery = await _employeeRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);
                employeesQuery = ApplyEmployeeFilter(employeesQuery, input);

                var applicationTrend = await applicationsQuery
                    .GroupBy(x => new { x.AppliedTime.Year, x.AppliedTime.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        TotalApplications = g.Count()
                    })
                    .ToListAsync();

                var offerTrend = await offersQuery
                    .GroupBy(x => new { x.CreationTime.Year, x.CreationTime.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        TotalOffers = g.Count()
                    })
                    .ToListAsync();

                var hiredTrend = await employeesQuery
                    .GroupBy(x => new { x.CreationTime.Year, x.CreationTime.Month })
                    .Select(g => new
                    {
                        g.Key.Year,
                        g.Key.Month,
                        TotalHired = g.Count()
                    })
                    .ToListAsync();

                var periods = applicationTrend
                    .Select(x => new { x.Year, x.Month })
                    .Union(offerTrend.Select(x => new { x.Year, x.Month }))
                    .Union(hiredTrend.Select(x => new { x.Year, x.Month }))
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToList();

                var result = periods
                    .Select(p => new RecruitmentTrendItemDto
                    {
                        Period = $"{p.Year}-{p.Month:D2}",
                        TotalApplications = applicationTrend
                            .Where(x => x.Year == p.Year && x.Month == p.Month)
                            .Select(x => x.TotalApplications)
                            .FirstOrDefault(),
                        TotalOffers = offerTrend
                            .Where(x => x.Year == p.Year && x.Month == p.Month)
                            .Select(x => x.TotalOffers)
                            .FirstOrDefault(),
                        TotalHired = hiredTrend
                            .Where(x => x.Year == p.Year && x.Month == p.Month)
                            .Select(x => x.TotalHired)
                            .FirstOrDefault()
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        /// Thống kê tuyển dụng theo phòng ban
        /// Dùng để so sánh hiệu quả tuyển dụng giữa các phòng ban (số request, hồ sơ, offer, tuyển thành công)
        [Authorize(bcaPermissions.Recruitment.Reports.ViewReport)]
        public async Task<List<DepartmentRecruitmentStatisticsDto>> GetDepartmentStatisticsAsync(RecruitmentDashboardFilterDto input)
        {
            try
            {
                var departmentsQuery = await _departmentRepository.GetQueryableAsync();
                var recruitmentRequestsQuery = await _recruitmentRequestRepository.GetQueryableAsync();
                var applicationsQuery = await _applicationRepository.GetQueryableAsync();
                var offersQuery = await _offerRepository.GetQueryableAsync();
                var employeesQuery = await _employeeRepository.GetQueryableAsync();

                recruitmentRequestsQuery = ApplyRecruitmentRequestFilter(recruitmentRequestsQuery, input);
                applicationsQuery = ApplyApplicationFilter(applicationsQuery, recruitmentRequestsQuery, input);
                offersQuery = ApplyOfferFilter(offersQuery, applicationsQuery);
                employeesQuery = ApplyEmployeeFilter(employeesQuery, input);

                var requestStats = await recruitmentRequestsQuery
                    .GroupBy(x => x.DepartmentId)
                    .Select(g => new
                    {
                        DepartmentId = g.Key,
                        TotalRecruitmentRequests = g.Count()
                    })
                    .ToListAsync();

                var applicationStats = await (
                    from app in applicationsQuery
                    join rr in recruitmentRequestsQuery on app.RecruitmentRequestId equals rr.Id
                    group app by rr.DepartmentId into g
                    select new
                    {
                        DepartmentId = g.Key,
                        TotalApplications = g.Count()
                    })
                    .ToListAsync();

                var offerStats = await (
                    from offer in offersQuery
                    join app in applicationsQuery on offer.ApplicationId equals app.Id
                    join rr in recruitmentRequestsQuery on app.RecruitmentRequestId equals rr.Id
                    group offer by rr.DepartmentId into g
                    select new
                    {
                        DepartmentId = g.Key,
                        TotalOffers = g.Count()
                    })
                    .ToListAsync();

                var departmentList = await departmentsQuery.ToListAsync();

                var result = departmentList
                    .Select(d => new DepartmentRecruitmentStatisticsDto
                    {
                        DepartmentId = d.Id,
                        DepartmentName = d.Name,
                        TotalRecruitmentRequests = requestStats
                            .Where(x => x.DepartmentId == d.Id)
                            .Select(x => x.TotalRecruitmentRequests)
                            .FirstOrDefault(),
                        TotalApplications = applicationStats
                            .Where(x => x.DepartmentId == d.Id)
                            .Select(x => x.TotalApplications)
                            .FirstOrDefault(),
                        TotalOffers = offerStats
                            .Where(x => x.DepartmentId == d.Id)
                            .Select(x => x.TotalOffers)
                            .FirstOrDefault(),
                        TotalHired = 0
                    })
                    .OrderByDescending(x => x.TotalApplications)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        private IQueryable<RecruitmentRequest> ApplyRecruitmentRequestFilter(
            IQueryable<RecruitmentRequest> query,
            RecruitmentDashboardFilterDto input)
        {
            if (input.FromDate.HasValue)
            {
                query = query.Where(x => x.CreationTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                var toDate = input.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreationTime <= toDate);
            }

            if (input.DepartmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == input.DepartmentId.Value);
            }

            if (input.JobPositionId.HasValue)
            {
                query = query.Where(x => x.PositionId == input.JobPositionId.Value);
            }

            return query;
        }

        private IQueryable<Application> ApplyApplicationFilter(
            IQueryable<Application> query,
            IQueryable<RecruitmentRequest> recruitmentRequestsQuery,
            RecruitmentDashboardFilterDto input)
        {
            if (input.FromDate.HasValue)
            {
                query = query.Where(x => x.AppliedTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                var toDate = input.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.AppliedTime <= toDate);
            }

            if (input.DepartmentId.HasValue || input.JobPositionId.HasValue)
            {
                query =
                    from app in query
                    join rr in recruitmentRequestsQuery on app.RecruitmentRequestId equals rr.Id
                    select app;
            }

            if (input.DepartmentId.HasValue)
            {
                query =
                    from app in query
                    join rr in recruitmentRequestsQuery on app.RecruitmentRequestId equals rr.Id
                    where rr.DepartmentId == input.DepartmentId.Value
                    select app;
            }

            if (input.JobPositionId.HasValue)
            {
                query =
                    from app in query
                    join rr in recruitmentRequestsQuery on app.RecruitmentRequestId equals rr.Id
                    where rr.PositionId == input.JobPositionId.Value
                    select app;
            }

            return query;
        }

        private IQueryable<Candidate> ApplyCandidateFilter(
            IQueryable<Candidate> query,
            RecruitmentDashboardFilterDto input)
        {
            if (input.FromDate.HasValue)
            {
                query = query.Where(x => x.CreationTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                var toDate = input.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreationTime <= toDate);
            }

            return query;
        }

        private IQueryable<ApplicationScreening> ApplyScreeningFilter(
            IQueryable<ApplicationScreening> query,
            IQueryable<Application> applicationsQuery)
        {
            query =
                from screening in query
                join app in applicationsQuery on screening.ApplicationId equals app.Id
                select screening;

            return query;
        }

        private IQueryable<InterviewSchedule> ApplyInterviewScheduleFilter(
            IQueryable<InterviewSchedule> query,
            IQueryable<Application> applicationsQuery)
        {
            query =
                from schedule in query
                join app in applicationsQuery on schedule.ApplicationId equals app.Id
                select schedule;

            return query;
        }

        private IQueryable<InterviewEvaluation> ApplyInterviewEvaluationFilter(
            IQueryable<InterviewEvaluation> query,
            IQueryable<Application> applicationsQuery)
        {
            query =
                from evaluation in query
                join app in applicationsQuery on evaluation.ApplicationId equals app.Id
                select evaluation;

            return query;
        }

        private IQueryable<Offer> ApplyOfferFilter(
            IQueryable<Offer> query,
            IQueryable<Application> applicationsQuery)
        {
            query =
                from offer in query
                join app in applicationsQuery on offer.ApplicationId equals app.Id
                select offer;

            return query;
        }

        private IQueryable<Employee> ApplyEmployeeFilter(
            IQueryable<Employee> query,
            RecruitmentDashboardFilterDto input)
        {
            if (input.FromDate.HasValue)
            {
                query = query.Where(x => x.CreationTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                var toDate = input.ToDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(x => x.CreationTime <= toDate);
            }

            if (input.DepartmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == input.DepartmentId.Value);
            }

            if (input.JobPositionId.HasValue)
            {
                query = query.Where(x => x.JobPositionId == input.JobPositionId.Value);
            }

            return query;
        }
    }
}