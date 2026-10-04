using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class ComponentRuleDto
    {
        public Guid Id { get; set; }

        public string RuleName { get; set; } = string.Empty;
        public ConditionLogic Logic { get; set; }
        public List<RuleConditionDto> Conditions { get; set; } = new List<RuleConditionDto>();
        public List<RuleActionDto> Actions { get; set; } = new List<RuleActionDto>();

    }
}
