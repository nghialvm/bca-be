using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;

namespace dvd.bca.Service.Emails
{
    public class CandidateEmailManager : ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public CandidateEmailManager(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task SendInterviewInvitationAsync(
            string toEmail,
            string candidateName,
            string interviewTime,
            string locationOrLink)
        {
            var subject = "Interview Invitation";
            var body = $@"
                <p>Dear {candidateName},</p>
                <p>You are invited to interview.</p>
                <p><strong>Time:</strong> {interviewTime}</p>
                <p><strong>Location / Link:</strong> {locationOrLink}</p>
                <p>Best regards,</p>
                <p>BCA Recruitment Team</p>";

            await _emailSender.SendAsync(toEmail, subject, body, true);
        }

        public async Task SendOfferAsync(
            string toEmail,
            string candidateName,
            string positionName,
            string salary,
            string startDate)
        {
            var subject = "Job Offer";
            var body = $@"
                <p>Dear {candidateName},</p>
                <p>We are pleased to offer you the position of <strong>{positionName}</strong>.</p>
                <p><strong>Salary:</strong> {salary}</p>
                <p><strong>Start date:</strong> {startDate}</p>
                <p>Please reply to this email for confirmation.</p>
                <p>Best regards,</p>
                <p>BCA Recruitment Team</p>";

            await _emailSender.SendAsync(toEmail, subject, body, true);
        }

        public async Task SendResultAsync(
            string toEmail,
            string candidateName,
            string resultMessage)
        {
            var subject = "Recruitment Result";
            var body = $@"
                <p>Dear {candidateName},</p>
                <p>{resultMessage}</p>
                <p>Best regards,</p>
                <p>BCA Recruitment Team</p>";

            await _emailSender.SendAsync(toEmail, subject, body, true);
        }
    }
}