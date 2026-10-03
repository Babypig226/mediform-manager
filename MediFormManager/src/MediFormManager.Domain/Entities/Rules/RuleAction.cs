using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Components;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Domain.Entities.Rules
{
    public class RuleAction : BaseEntity
    {
        public Guid ComponentRuleId { get; set; }

        public RuleActionType ActionType { get; set; }

        public RuleTargetType TargetType { get; set; }

        public Guid? TargetComponentId { get; set; }

        public string? TargetGroupKey { get; set; }

        public ComponentRule? ComponentRule { get; set; }

        public FormComponent? TargetComponent { get; set; }
    }
}
