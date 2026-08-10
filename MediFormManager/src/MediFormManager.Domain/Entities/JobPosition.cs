using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class JobPosition : BaseEntity
    {
        public string JobCode { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
