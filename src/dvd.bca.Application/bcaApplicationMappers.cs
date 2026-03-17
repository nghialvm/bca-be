using AutoMapper;
using dvd.bca.Applications.Dtos;
using dvd.bca.ApplicationScreenings.Dtos;
using dvd.bca.CandidateDocuments.Dtos;
using dvd.bca.Candidates.Dtos;
using dvd.bca.Departments.Dtos;
using dvd.bca.Entity.ApplicationRoot;
using dvd.bca.Entity.CandidateRoot;
using dvd.bca.Entity.Recruitment;
using dvd.bca.InterviewEvaluations.Dtos;
using dvd.bca.InterviewSchedules.Dtos;
using dvd.bca.JobPositions.Dtos;
using dvd.bca.Offers.Dtos;
using dvd.bca.RecruitmentRequests.Dtos;

namespace dvd.bca;

/*
 * You can add your own mappings here.
 * [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
 * public partial class bcaApplicationMappers : MapperBase<BookDto, CreateUpdateBookDto>
 * {
 *    public override partial CreateUpdateBookDto Map(BookDto source);
 * 
 *    public override partial void Map(BookDto source, CreateUpdateBookDto destination);
 * }
 */

public class bcaApplicationAutoMapperProfile : Profile
{
    public bcaApplicationAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        CreateMap<JobPosition, JobPositionDto>();
        CreateMap<CreateJobPositionDto, JobPosition>();
        CreateMap<UpdateJobPositionDto, JobPosition>();

        CreateMap<RecruitmentRequest, RecruitmentRequestDto>();
        CreateMap<CreateRecruitmentRequestDto, RecruitmentRequest>();
        CreateMap<UpdateRecruitmentRequestDto, RecruitmentRequest>();

        CreateMap<Candidate, CandidateDto>();
        CreateMap<Candidate, CandidateDetailDto>();
        CreateMap<CreateCandidateDto, Candidate>();
        CreateMap<UpdateCandidateDto, Candidate>();

        CreateMap<CandidateDocument, CandidateDocumentDto>();
        CreateMap<CreateCandidateDocumentDto, CandidateDocument>();
        CreateMap<UpdateCandidateDocumentDto, CandidateDocument>();

        CreateMap<Application, ApplicationDto>();
        CreateMap<CreateApplicationDto, Application>();
        CreateMap<UpdateApplicationDto, Application>();

        CreateMap<ApplicationScreening, ApplicationScreeningDto>();
        CreateMap<ApplicationScreening, ApplicationScreeningDetailDto>();
        CreateMap<CreateApplicationScreeningDto, ApplicationScreening>();
        CreateMap<UpdateApplicationScreeningDto, ApplicationScreening>();

        CreateMap<InterviewSchedule, InterviewScheduleDto>();
        CreateMap<InterviewSchedule, InterviewScheduleDetailDto>();
        CreateMap<CreateInterviewScheduleDto, InterviewSchedule>();
        CreateMap<UpdateInterviewScheduleDto, InterviewSchedule>();

        CreateMap<InterviewEvaluation, InterviewEvaluationDto>();
        CreateMap<InterviewEvaluation, InterviewEvaluationDetailDto>();
        CreateMap<CreateInterviewEvaluationDto, InterviewEvaluation>();
        CreateMap<UpdateInterviewEvaluationDto, InterviewEvaluation>();

        CreateMap<Offer, OfferDto>();
        CreateMap<CreateOfferDto, Offer>();
        CreateMap<UpdateOfferDto, Offer>();
    }
}