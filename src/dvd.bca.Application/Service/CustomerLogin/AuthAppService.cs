using dvd.bca.Service.CustomerLogin.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace dvd.bca.Service.CustomerLogin
{
    [Authorize(AuthenticationSchemes = "Identity.Application,OpenIddict.Validation.AspNetCore")]
    public class AuthAppService : ApplicationService, IAuthAppService
    {
        private readonly IdentityUserManager _userManager;

        public AuthAppService(IdentityUserManager userManager)
        {
            _userManager = userManager;
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

            return new CurrentUserDto
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles[0]
            };
        }
    }
}
