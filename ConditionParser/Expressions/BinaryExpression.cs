using ConditionParser.Modes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; set; }
        public Operand Operand { get; set; }
        public Expression Right { get; set; }

        public override ExpressionType NodeType => ExpressionType.Binary;
    }
}
