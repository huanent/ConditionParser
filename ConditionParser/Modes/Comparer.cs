using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Modes
{
    public enum Comparer
    {
        EqualTo = 0,
        GreaterThan = 1,
        GreaterThanOrEqual = 2,
        LessThan = 3,
        LessThanOrEqual = 4,
        NotEqualTo = 5,
        StartWith = 6,
        Contains = 7,
    }
}
