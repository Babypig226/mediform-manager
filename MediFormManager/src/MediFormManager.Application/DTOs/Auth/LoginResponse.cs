using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Auth
{
    public class LoginResponse
    {
        public string AccessTocken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }

        public Guid UserId { get; set; }
        public string LoginId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
    
}
