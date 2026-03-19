using dvd.bca.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace dvd.bca.Permissions
{
    public class bcaPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(bcaPermissions.GroupName, L("Permission:bca"));

            var recruitment = group.AddPermission(
                bcaPermissions.Recruitment.Default,
                L("Permission:Recruitment")
            );

            var departments = recruitment.AddChild(
                bcaPermissions.Recruitment.Departments.Default,
                L("Permission:Departments")
            );
            departments.AddChild(bcaPermissions.Recruitment.Departments.Create, L("Permission:Create"));
            departments.AddChild(bcaPermissions.Recruitment.Departments.Update, L("Permission:Update"));
            departments.AddChild(bcaPermissions.Recruitment.Departments.Delete, L("Permission:Delete"));

            var jobPositions = recruitment.AddChild(
                bcaPermissions.Recruitment.JobPositions.Default,
                L("Permission:JobPositions")
            );
            jobPositions.AddChild(bcaPermissions.Recruitment.JobPositions.Create, L("Permission:Create"));
            jobPositions.AddChild(bcaPermissions.Recruitment.JobPositions.Update, L("Permission:Update"));
            jobPositions.AddChild(bcaPermissions.Recruitment.JobPositions.Delete, L("Permission:Delete"));

            var recruitmentRequests = recruitment.AddChild(
                bcaPermissions.Recruitment.RecruitmentRequests.Default,
                L("Permission:RecruitmentRequests")
            );
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Create, L("Permission:Create"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Update, L("Permission:Update"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Delete, L("Permission:Delete"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.SubmitForApproval, L("Permission:SubmitForApproval"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Approve, L("Permission:Approve"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Reject, L("Permission:Reject"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Publish, L("Permission:Publish"));
            recruitmentRequests.AddChild(bcaPermissions.Recruitment.RecruitmentRequests.Close, L("Permission:Close"));

            var candidates = recruitment.AddChild(
                bcaPermissions.Recruitment.Candidates.Default,
                L("Permission:Candidates")
            );
            candidates.AddChild(bcaPermissions.Recruitment.Candidates.Create, L("Permission:Create"));
            candidates.AddChild(bcaPermissions.Recruitment.Candidates.Update, L("Permission:Update"));
            candidates.AddChild(bcaPermissions.Recruitment.Candidates.Delete, L("Permission:Delete"));
            candidates.AddChild(bcaPermissions.Recruitment.Candidates.ChangeStatus, L("Permission:ChangeStatus"));

            var candidateDocuments = recruitment.AddChild(
                bcaPermissions.Recruitment.CandidateDocuments.Default,
                L("Permission:CandidateDocuments")
            );
            candidateDocuments.AddChild(bcaPermissions.Recruitment.CandidateDocuments.Create, L("Permission:Create"));
            candidateDocuments.AddChild(bcaPermissions.Recruitment.CandidateDocuments.Update, L("Permission:Update"));
            candidateDocuments.AddChild(bcaPermissions.Recruitment.CandidateDocuments.Delete, L("Permission:Delete"));
            candidateDocuments.AddChild(bcaPermissions.Recruitment.CandidateDocuments.Download, L("Permission:Download"));

            var applications = recruitment.AddChild(
                bcaPermissions.Recruitment.Applications.Default,
                L("Permission:Applications")
            );
            applications.AddChild(bcaPermissions.Recruitment.Applications.Create, L("Permission:Create"));
            applications.AddChild(bcaPermissions.Recruitment.Applications.Update, L("Permission:Update"));
            applications.AddChild(bcaPermissions.Recruitment.Applications.Delete, L("Permission:Delete"));
            applications.AddChild(bcaPermissions.Recruitment.Applications.ChangeStatus, L("Permission:ChangeStatus"));
            applications.AddChild(bcaPermissions.Recruitment.Applications.Reject, L("Permission:Reject"));
            applications.AddChild(bcaPermissions.Recruitment.Applications.Cancel, L("Permission:Cancel"));

            var applicationScreenings = recruitment.AddChild(
                bcaPermissions.Recruitment.ApplicationScreenings.Default,
                L("Permission:ApplicationScreenings")
            );
            applicationScreenings.AddChild(bcaPermissions.Recruitment.ApplicationScreenings.Create, L("Permission:Create"));
            applicationScreenings.AddChild(bcaPermissions.Recruitment.ApplicationScreenings.Update, L("Permission:Update"));
            applicationScreenings.AddChild(bcaPermissions.Recruitment.ApplicationScreenings.Delete, L("Permission:Delete"));
            applicationScreenings.AddChild(bcaPermissions.Recruitment.ApplicationScreenings.Approve, L("Permission:Approve"));
            applicationScreenings.AddChild(bcaPermissions.Recruitment.ApplicationScreenings.Reject, L("Permission:Reject"));

            var interviewSchedules = recruitment.AddChild(
                bcaPermissions.Recruitment.InterviewSchedules.Default,
                L("Permission:InterviewSchedules")
            );
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Create, L("Permission:Create"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Update, L("Permission:Update"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Delete, L("Permission:Delete"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Confirm, L("Permission:Confirm"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Cancel, L("Permission:Cancel"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Reschedule, L("Permission:Reschedule"));
            interviewSchedules.AddChild(bcaPermissions.Recruitment.InterviewSchedules.Complete, L("Permission:Complete"));

            var interviewEvaluations = recruitment.AddChild(
                bcaPermissions.Recruitment.InterviewEvaluations.Default,
                L("Permission:InterviewEvaluations")
            );
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Create, L("Permission:Create"));
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Update, L("Permission:Update"));
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Delete, L("Permission:Delete"));
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Submit, L("Permission:Submit"));
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Approve, L("Permission:Approve"));
            interviewEvaluations.AddChild(bcaPermissions.Recruitment.InterviewEvaluations.Reject, L("Permission:Reject"));

            var offers = recruitment.AddChild(
                bcaPermissions.Recruitment.Offers.Default,
                L("Permission:Offers")
            );
            offers.AddChild(bcaPermissions.Recruitment.Offers.Create, L("Permission:Create"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.Update, L("Permission:Update"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.Delete, L("Permission:Delete"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.SendOffer, L("Permission:SendOffer"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.AcceptOffer, L("Permission:AcceptOffer"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.RejectOffer, L("Permission:RejectOffer"));
            offers.AddChild(bcaPermissions.Recruitment.Offers.Expire, L("Permission:Expire"));

            var employees = recruitment.AddChild(
                bcaPermissions.Recruitment.Employees.Default,
                L("Permission:Employees")
            );
            employees.AddChild(bcaPermissions.Recruitment.Employees.Create, L("Permission:Create"));
            employees.AddChild(bcaPermissions.Recruitment.Employees.Update, L("Permission:Update"));
            employees.AddChild(bcaPermissions.Recruitment.Employees.Delete, L("Permission:Delete"));

            var candidateResponses = recruitment.AddChild(
                bcaPermissions.Recruitment.CandidateResponses.Default,
                L("Permission:CandidateResponses")
            );
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.Create, L("Permission:Create"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.Update, L("Permission:Update"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.Delete, L("Permission:Delete"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.AcceptOffer, L("Permission:AcceptOffer"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.RejectOffer, L("Permission:RejectOffer"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.ConfirmInterview, L("Permission:ConfirmInterview"));
            candidateResponses.AddChild(bcaPermissions.Recruitment.CandidateResponses.RejectInterview, L("Permission:RejectInterview"));

            var reports = recruitment.AddChild(
                bcaPermissions.Recruitment.Reports.Default,
                L("Permission:Reports")
            );
            reports.AddChild(bcaPermissions.Recruitment.Reports.ViewReport, L("Permission:ViewReport"));
            reports.AddChild(bcaPermissions.Recruitment.Reports.Export, L("Permission:Export"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<bcaResource>(name);
        }
    }
}