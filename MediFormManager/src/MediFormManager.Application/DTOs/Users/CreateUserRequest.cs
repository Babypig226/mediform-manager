using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediFormManager.Application.DTOs.Users
{
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(50)]
        public string LoginId { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public Guid RoleId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobPositionId { get; set; }
    }
}
