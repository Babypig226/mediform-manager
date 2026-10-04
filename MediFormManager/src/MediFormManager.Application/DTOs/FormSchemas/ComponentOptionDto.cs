using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class ComponentOptionDto
    {
        public Guid Id { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
