using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Domain.Entities.Rules
{
    public class RuleCondition : BaseEntity
    {
        public Guid ComponentRuleId { get; set; }

        public Guid SourceComponentId { get; set; }

        public RuleOperator Operator { get; set; }

        public Guid? ExpectedOptionId { get; set; }

        public string? ExpectedValue { get; set; }

        public ComponentRule? ComponentRule { get; set; }

        public FormComponent? SourceComponent { get; set; }

        public ComponentOption? ExpectedOption { get; set; }
    }
}
