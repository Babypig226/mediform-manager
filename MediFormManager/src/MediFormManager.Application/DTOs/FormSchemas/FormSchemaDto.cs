using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class FormSchemaDto
    {
        public Guid FormVersionId { get; set; }
        public int Version { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<FormComponentDto> Components { get; set; } = new List<FormComponentDto>();
        public List<ComponentRuleDto> Rules { get; set; } = new List<ComponentRuleDto>();

        public DateTime? UpdatedAt { get; set; }
    }
}
