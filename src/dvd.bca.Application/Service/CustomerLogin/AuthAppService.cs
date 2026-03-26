using dvd.bca.Candidates;
using dvd.bca.Service.CustomerLogin.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;

namespace dvd.bca.Service.CustomerLogin
{
    [Authorize(AuthenticationSchemes = "Identity.Application,OpenIddict.Validation.AspNetCore")]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly ICurrentCandidateResolver _currentCandidateResolver;
        private readonly IdentityUserManager _userManager;

        public AuthAppService(
            IdentityUserManager userManager,
            ICurrentCandidateResolver currentCandidateResolver)
        {
            _userManager = userManager;
            _currentCandidateResolver = currentCandidateResolver;
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
    }
}
