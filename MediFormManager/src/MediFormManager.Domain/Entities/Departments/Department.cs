using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities.Departments
{
    public class Department : BaseEntity
    {
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
