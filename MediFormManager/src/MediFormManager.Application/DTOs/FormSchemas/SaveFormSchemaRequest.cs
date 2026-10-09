using MediFormManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class SaveFormSchemaRequest
    {
        public DateTime? ExpectedUpdatedAt { get; set; }
        public List<SaveFormComponentDto> Components { get; set; } = new();
    }

    public class SaveFormComponentDto { 
        public Guid Id { get; set; }
        public ComponentType ComponentType { get; set; }
        public string? Label { get; set; }
        public string? Prompt { get; set; }
        public string? Placeholder { get; set; }
        public string? DefaultValue { get; set; }

        public bool IsRequired { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsVisible { get; set; }

        public string? GroupKey { get; set; }
        public int Order { get; set; }
        public List<SaveComponentOptionDto> Options { get; set; } = new();
    
    }

    public class SaveComponentOptionDto {
        public Guid Id { get; set; }
        public string DisplayText { get; set; } = string.Empty;

        public int Order { get; set; }

    }

  
}
