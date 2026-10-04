using System;
using System.Collections.Generic;
using System.Text;
using MediFormManager.Domain.Enums;

namespace MediFormManager.Application.DTOs.FormSchemas
{
    public class RuleActionDto
    {
        public Guid Id { get; set; }
        public RuleActionType ActionType { get; set; } 
        public RuleTargetType TargetType { get; set; }
        public Guid? TargetComponentId { get; set; } 
        public string? TargetGroupKey { get; set; }
    }
}
