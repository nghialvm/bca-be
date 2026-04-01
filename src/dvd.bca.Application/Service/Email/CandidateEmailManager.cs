using dvd.bca.Enums;
using System;
using System.Net;
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
            int roundNumber,
            InterviewType interviewType,
            DateTime scheduledTime,
            int durationMinutes,
            string? location,
            string? meetingLink,
            string? contactPerson,
            string? note)
        {
            var subject = $"Interview Invitation - Round {roundNumber}";
            var interviewTypeLabel = interviewType switch
            {
                InterviewType.Offline => "Offline",
                InterviewType.Online => "Online",
                InterviewType.Phone => "Phone",
                _ => interviewType.ToString()
            };
            var venueLabel = string.IsNullOrWhiteSpace(meetingLink)
                ? "Location"
                : "Meeting link";
            var venueValue = string.IsNullOrWhiteSpace(meetingLink)
                ? location
                : meetingLink;
            var contactSection = string.IsNullOrWhiteSpace(contactPerson)
                ? string.Empty
                : $"<li><strong>Contact person:</strong> {WebUtility.HtmlEncode(contactPerson)}</li>";
            var venueSection = string.IsNullOrWhiteSpace(venueValue)
                ? string.Empty
                : $"<li><strong>{venueLabel}:</strong> {WebUtility.HtmlEncode(venueValue)}</li>";
            var noteSection = string.IsNullOrWhiteSpace(note)
                ? string.Empty
                : $"<li><strong>Note:</strong> {WebUtility.HtmlEncode(note)}</li>";
            var body = $@"
                <p>Dear {WebUtility.HtmlEncode(candidateName)},</p>
                <p>You are invited to interview with BCA. Please review the schedule details below:</p>
                <ul>
                    <li><strong>Round:</strong> {roundNumber}</li>
                    <li><strong>Interview type:</strong> {interviewTypeLabel}</li>
                    <li><strong>Time:</strong> {scheduledTime:dd/MM/yyyy HH:mm}</li>
                    <li><strong>Duration:</strong> {durationMinutes} minutes</li>
                    {contactSection}
                    {venueSection}
                    {noteSection}
                </ul>
                <p>Please be ready a few minutes before the scheduled time.</p>
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
                <p>Please log in to the recruitment system to review this offer and choose Accept or Decline on the application detail screen.</p>
                <p>This mailbox does not process offer acceptance or rejection replies.</p>
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
