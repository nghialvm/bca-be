using dvd.bca.Email;
using dvd.bca.Email.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Emailing;
using Microsoft.Extensions.Logging;

namespace dvd.bca.Service.Emails
{
    [Authorize]
    public class EmailAppService : ApplicationService, IEmailAppService
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<EmailAppService> _logger;

        public EmailAppService(IEmailSender emailSender,ILogger<EmailAppService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendAsync(SendEmailDto input)
        {
            try
            {
                _logger.LogWarning("Email sender type: {Type}", _emailSender.GetType().FullName);
                await _emailSender.SendAsync(
                    input.To,
                    input.Subject,
                    input.Body,
                    input.IsBodyHtml
                );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
    }
}