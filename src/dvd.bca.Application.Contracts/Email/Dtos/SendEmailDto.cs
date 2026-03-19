using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dvd.bca.Email.Dtos
{
    public class SendEmailDto
    {
        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string To { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public bool IsBodyHtml { get; set; } = true;
    }
}