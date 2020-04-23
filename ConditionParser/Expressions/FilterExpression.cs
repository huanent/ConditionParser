using ConditionParser.Modes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class FilterExpression : Expression
    {
        public string Property { get; set; }
        public Comparer Comparer { get; set; }

        public ValueExpression Value { get; set; }

        public override ExpressionType NodeType => ExpressionType.Filter;
    }
}
