using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Entities
{
    public class FormVersion : BaseEntity
    {
        public Guid FormId { get; set; }

        public int Version { get; set; }

        public string Status { get; set; } = "Draft";

        public Form? Form { get; set; }

        public ICollection<FormComponent> Components { get; set; } = new List<FormComponent>();
    }
}
