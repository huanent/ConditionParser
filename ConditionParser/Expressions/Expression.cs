using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public abstract class Expression
    {
        public abstract ExpressionType NodeType { get; }
    }
}
