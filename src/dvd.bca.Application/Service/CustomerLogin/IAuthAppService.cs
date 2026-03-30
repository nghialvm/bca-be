using dvd.bca.Service.CustomerLogin.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace dvd.bca.Service.CustomerLogin
{
    public interface IAuthAppService
    {
        Task<CurrentUserDto> GetMeAsync();
        Task ChangePasswordAsync(ChangePasswordDto input);
        Task ForgotPasswordAsync(ForgotPasswordDto input);
        Task ResetPasswordAsync(ResetPasswordDto input);
    }
}
