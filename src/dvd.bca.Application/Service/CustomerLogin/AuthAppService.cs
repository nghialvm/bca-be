using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dvd.bca.Candidates;
using dvd.bca.Service.CustomerLogin.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;

namespace dvd.bca.Service.CustomerLogin
{
    [Authorize(AuthenticationSchemes = "Identity.Application,OpenIddict.Validation.AspNetCore")]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly ICurrentCandidateResolver _currentCandidateResolver;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly IdentityUserManager _userManager;

        public AuthAppService(
            IdentityUserManager userManager,
            ICurrentCandidateResolver currentCandidateResolver,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _currentCandidateResolver = currentCandidateResolver;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public async Task<CurrentUserDto> GetMeAsync()
        {
            var userId = CurrentUser.Id;
            if (userId == null)
            {
                throw new AbpAuthorizationException("User has not logged in.");
            }

            var user = await _userManager.GetByIdAsync(userId.Value);
            var roles = await _userManager.GetRolesAsync(user);
            await _currentCandidateResolver.FindCandidateIdForCurrentUserAsync();

            return new CurrentUserDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault()
            };
        }

        public async Task ChangePasswordAsync(ChangePasswordDto input)
        {
            if (CurrentUser.Id == null)
            {
                throw new AbpAuthorizationException("User has not logged in.");
            }

            if (string.IsNullOrWhiteSpace(input.CurrentPassword))
            {
                throw new UserFriendlyException("Vui lòng nhập mật khẩu hiện tại.");
            }

            if (string.IsNullOrWhiteSpace(input.NewPassword))
            {
                throw new UserFriendlyException("Vui lòng nhập mật khẩu mới.");
            }

            var user = await _userManager.GetByIdAsync(CurrentUser.Id.Value);
            var result = await _userManager.ChangePasswordAsync(
                user,
                input.CurrentPassword,
                input.NewPassword
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException(GetIdentityErrorMessage(result));
            }
        }

        [AllowAnonymous]
        public async Task ForgotPasswordAsync(ForgotPasswordDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Email))
            {
                throw new UserFriendlyException("Vui lòng nhập email.");
            }

            var user = await _userManager.FindByEmailAsync(input.Email.Trim());
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = BuildResetPasswordLink(user.Id, token);
            var displayName = user.Name ?? user.UserName ?? user.Email;
            var subject = "Đặt lại mật khẩu tài khoản";
            var body = $@"
                <p>Xin chào {displayName},</p>
                <p>Hệ thống đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
                <p>Để tiếp tục, vui lòng nhấn vào liên kết dưới đây:</p>
                <p><a href=""{resetLink}"">Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email.</p>
                <p>Trân trọng,</p>
                <p>BCA Recruitment</p>";

            await _emailSender.SendAsync(user.Email, subject, body, true);
        }

        [AllowAnonymous]
        public async Task ResetPasswordAsync(ResetPasswordDto input)
        {
            if (string.IsNullOrWhiteSpace(input.UserId))
            {
                throw new UserFriendlyException("Thiếu thông tin người dùng.");
            }

            if (string.IsNullOrWhiteSpace(input.Token))
            {
                throw new UserFriendlyException("Liên kết đặt lại mật khẩu không hợp lệ.");
            }

            if (!Guid.TryParse(input.UserId, out var userId))
            {
                throw new UserFriendlyException("Liên kết đặt lại mật khẩu không hợp lệ.");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UserFriendlyException("Tài khoản không tồn tại hoặc đã bị xóa.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                input.Token,
                input.NewPassword
            );

            if (!result.Succeeded)
            {
                throw new UserFriendlyException(GetIdentityErrorMessage(result));
            }
        }

        private string BuildResetPasswordLink(Guid userId, string token)
        {
            var clientRootUrl = GetClientRootUrl();
            var encodedToken = WebUtility.UrlEncode(token);

            return $"{clientRootUrl}/reset-password?userId={userId}&token={encodedToken}";
        }

        private string GetClientRootUrl()
        {
            var configuredClientUrl = _configuration["App:ClientUrl"];
            if (!string.IsNullOrWhiteSpace(configuredClientUrl))
            {
                return configuredClientUrl.Trim().TrimEnd('/');
            }

            var firstCorsOrigin = _configuration["App:CorsOrigins"]
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim().TrimEnd('/'))
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(firstCorsOrigin))
            {
                return firstCorsOrigin;
            }

            return "http://localhost:3000";
        }

        private static string GetIdentityErrorMessage(IdentityResult result)
        {
            var message = string.Join(
                " ",
                result.Errors
                    .Select(error => error.Description?.Trim())
                    .Where(description => !string.IsNullOrWhiteSpace(description))
            );

            return string.IsNullOrWhiteSpace(message)
                ? "Không thể xử lý yêu cầu mật khẩu."
                : message;
        }
    }
}
