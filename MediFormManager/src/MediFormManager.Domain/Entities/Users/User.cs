using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Departments;
using System;
using System.Collections.Generic;
using System.Text;


namespace MediFormManager.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public string LoginId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public Boolean IsActive { get; set; } = true;

        public Guid RoleId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobPositionId { get; set; }

        public Role? Role { get; set; }

        public Department? Department { get; set; }

        public JobPosition? JobPosition { get; set; }
    }
}
