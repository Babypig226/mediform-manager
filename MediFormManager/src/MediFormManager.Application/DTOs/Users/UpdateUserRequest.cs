using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.Users
{
    public class UpdateUserRequest
    {
        public string UserName { get; set; } = string.Empty;

        public string? Password { get; set; }

        public Guid RoleId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobPositionId { get; set; }

        public bool IsActive { get; set; }
    }
}
