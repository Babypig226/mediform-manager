using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Entities.Common;
using MediFormManager.Domain.Entities.Forms;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Domain.Entities.Rules
{
    public class ComponentRule : BaseEntity
    {
        public Guid FormVersionId { get; set; }
        public string RuleName { get; set; } = string.Empty;
       
        public ConditionLogic Logic { get; set; }
        public FormVersion? FormVersion { get; set; }

        public ICollection<RuleCondition> Conditions { get; set; } = new List<RuleCondition>();

        public ICollection<RuleAction> Actions { get; set; } = new List<RuleAction>();
    }
}
