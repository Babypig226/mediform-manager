using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public   class Form : BaseEntity
    {
        public string FormCode { get; set; } = string.Empty;

        public string FormName { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public FormCategory? Category { get; set; }

        public ICollection<FormVersion> Versions { get; set; } = new List<FormVersion>();
        
    }
}
