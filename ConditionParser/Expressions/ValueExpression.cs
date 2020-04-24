using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class ValueExpression : Expression
    {

        public ValueExpression(string value, bool sureToBeString)
        {
            if (!sureToBeString)
            {
                var mValue = value.ToLower();

                if (mValue == "true" || mValue == "false")
                {
                    Type = typeof(bool);
                    Value = bool.Parse(mValue);
                    return;
                }

                if (decimal.TryParse(value, out var number))
                {
                    Type = typeof(decimal);
                    Value = number;
                    return;
                }

                if (DateTime.TryParse(value, out var datetime))
                {
                    Type = typeof(DateTime);
                    Value = datetime;
                    return;
                }
            }

            Value = value;
            Type = typeof(string);
        }

        public override ExpressionType NodeType => ExpressionType.Value;

        public object Value { get; set; }
        public Type Type { get; set; }
    }
}
