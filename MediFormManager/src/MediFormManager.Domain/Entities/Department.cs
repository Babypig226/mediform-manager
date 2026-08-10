using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string DepartmentCode { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
