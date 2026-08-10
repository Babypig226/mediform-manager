using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Auth
{
    public class LoginRequest
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
