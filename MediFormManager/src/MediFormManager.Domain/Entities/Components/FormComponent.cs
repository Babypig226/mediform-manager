using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Domain.Entities.Components
{
    public class FormComponent : BaseEntity
    {
        public Guid FormVersionId { get; set; }

      

        public ComponentType ComponentType { get; set; }

        public bool IsRequired { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsVisible { get; set; } = true;

        public string? Placeholder { get; set; }

        public string? DefaultValue { get; set; }
        public string? Label { get; set; }
        public string? Prompt { get; set; }
        public int Order { get; set; }
        public string? GroupKey { get; set; }

        public ICollection<ComponentOption> Options { get; set; } = new List<ComponentOption>();
        public FormVersion? FormVersion { get; set; }
    }
}
