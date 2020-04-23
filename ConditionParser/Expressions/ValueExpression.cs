using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class ValueExpression : Expression
    {
        string _value;
        bool _sureToBeString;

        public ValueExpression(string value, bool sureToBeString)
        {
            _value = value;
            _sureToBeString = sureToBeString;
        }

        public override ExpressionType NodeType => ExpressionType.Value;

        public object Value { get; set; }
        public Type Type { get; set; }
    }
}
