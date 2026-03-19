namespace dvd.bca.Permissions
{
    public static class bcaPermissions
    {
        public const string GroupName = "bca";

        public static class Recruitment
        {
            public const string Default = GroupName + ".Recruitment";

            public static class Departments
            {
                public const string Default = Recruitment.Default + ".Departments";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class JobPositions
            {
                public const string Default = Recruitment.Default + ".JobPositions";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class RecruitmentRequests
            {
                public const string Default = Recruitment.Default + ".RecruitmentRequests";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string SubmitForApproval = Default + ".SubmitForApproval";
                public const string Approve = Default + ".Approve";
                public const string Reject = Default + ".Reject";
                public const string Publish = Default + ".Publish";
                public const string Close = Default + ".Close";
            }

            public static class Candidates
            {
                public const string Default = Recruitment.Default + ".Candidates";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string ChangeStatus = Default + ".ChangeStatus";
            }

            public static class CandidateDocuments
            {
                public const string Default = Recruitment.Default + ".CandidateDocuments";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string Download = Default + ".Download";
            }

            public static class Applications
            {
                public const string Default = Recruitment.Default + ".Applications";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string ChangeStatus = Default + ".ChangeStatus";
                public const string Reject = Default + ".Reject";
                public const string Cancel = Default + ".Cancel";
            }

            public static class ApplicationScreenings
            {
                public const string Default = Recruitment.Default + ".ApplicationScreenings";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string Approve = Default + ".Approve";
                public const string Reject = Default + ".Reject";
            }

            public static class InterviewSchedules
            {
                public const string Default = Recruitment.Default + ".InterviewSchedules";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string Confirm = Default + ".Confirm";
                public const string Cancel = Default + ".Cancel";
                public const string Reschedule = Default + ".Reschedule";
                public const string Complete = Default + ".Complete";
            }

            public static class InterviewEvaluations
            {
                public const string Default = Recruitment.Default + ".InterviewEvaluations";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string Submit = Default + ".Submit";
                public const string Approve = Default + ".Approve";
                public const string Reject = Default + ".Reject";
            }

            public static class Offers
            {
                public const string Default = Recruitment.Default + ".Offers";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string SendOffer = Default + ".SendOffer";
                public const string AcceptOffer = Default + ".AcceptOffer";
                public const string RejectOffer = Default + ".RejectOffer";
                public const string Expire = Default + ".Expire";
            }

            public static class Employees
            {
                public const string Default = Recruitment.Default + ".Employees";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";
            }

            public static class CandidateResponses
            {
                public const string Default = Recruitment.Default + ".CandidateResponses";
                public const string Create = Default + ".Create";
                public const string Update = Default + ".Update";
                public const string Delete = Default + ".Delete";

                public const string AcceptOffer = Default + ".AcceptOffer";
                public const string RejectOffer = Default + ".RejectOffer";
                public const string ConfirmInterview = Default + ".ConfirmInterview";
                public const string RejectInterview = Default + ".RejectInterview";
            }

            public static class Reports
            {
                public const string Default = Recruitment.Default + ".Reports";
                public const string ViewReport = Default + ".ViewReport";
                public const string Export = Default + ".Export";
            }
        }
    }
}