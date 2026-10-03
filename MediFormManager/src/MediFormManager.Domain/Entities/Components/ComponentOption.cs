using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Domain.Entities.Components
{
    public class ComponentOption : BaseEntity
    {
        public Guid FormComponentId { get; set; }

        public string DisplayText { get; set; } = string.Empty;

        public int Order { get; set; }

        public FormComponent? FormComponent { get; set; }
    }
}
