using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class RuleConditionDto
    {

        public Guid Id { get; set; }

        public Guid SourceComponentId { get; set; }

        public RuleOperator Operator { get; set; }

        public Guid? ExpectedOptionId { get; set; }

        public string? ExpectedValue { get; set; }
    }
}
