using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class FormComponentDto
    {
        public Guid Id { get; set; }
        public ComponentType ComponentType { get; set; }
        public string? Prompt { get; set; }
        public string? Label { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisible { get; set; }
        public string? DefaultValue { get; set; }
        public string? GroupKey { get; set; }
        public string? Placeholder { get; set; }
        public int Order { get; set; }      
        public List<ComponentOptionDto> Options { get; set; } = new List<ComponentOptionDto>();
    }
}
