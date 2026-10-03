using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Entities.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities.Forms
{
    public class FormVersion : BaseEntity
    {
        public Guid FormId { get; set; }

        public int Version { get; set; }

        public string Status { get; set; } = "Draft";

        public Form? Form { get; set; }

        public ICollection<FormComponent> Components { get; set; } = new List<FormComponent>();

        public ICollection<ComponentRule> Rules { get; set; } = new List<ComponentRule>();
    }
}
