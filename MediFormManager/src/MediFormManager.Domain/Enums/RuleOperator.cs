using System;
using System.Collections.Generic;
using System.Text;

namespace MediFormManager.Domain.Enums
{
    public enum RuleOperator
    {
        Equals,
        NotEquals,

        IsEmpty,
        IsNotEmpty,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual
    }
}
