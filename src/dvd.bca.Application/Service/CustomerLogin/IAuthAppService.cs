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
    }
}
