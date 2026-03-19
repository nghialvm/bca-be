using dvd.bca.Email.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace dvd.bca.Email
{
    public interface IEmailAppService : IApplicationService
    {
        Task SendAsync(SendEmailDto input);
    }
}
