using System;
using System.Collections.Generic;
using System.Text;

namespace dvd.bca.Service.CustomerLogin.Dtos
{
    public class CurrentUserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Id { get; set; }
    }
}
