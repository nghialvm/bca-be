using System.ComponentModel.DataAnnotations;

namespace dvd.bca.Service.CustomerLogin.Dtos
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
