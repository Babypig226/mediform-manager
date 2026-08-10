using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class FormComponent : BaseEntity
    {
        public Guid FormVersionId { get; set; }

        public string ComponentKey { get; set; } = string.Empty;

        public string ComponentType { get; set; } = string.Empty;

        public bool Required { get; set; }

        public FormVersion? FormVersion { get; set; }
    }
}
