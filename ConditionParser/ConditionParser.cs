using ConditionParser.Expressions;
using ConditionParser.Models;
using ConditionParser.Modes;
using System.Linq;

namespace ConditionParser
{
    public static class ConditionParser
    {

        public static Expression Parse(string condition)
        {
            var iterator = new Iterator(condition);
            iterator.TrimStart();
            return Analyze(iterator, null, null);
        }

        static Expression Analyze(Iterator iterator, Expression left, Operand? operand)
        {
            if (iterator.End) return left;

            if (iterator.Current == '(')
            {
                iterator.Next();
                var tree = Analyze(iterator, null, null);
                tree = Merge(left, operand, tree);
                if (iterator.Current != ')') throw new ConditionParseException(iterator.Position);
                iterator.Next();
                return tree;
            }
            else
            {
                var filter = new FilterExpression();
                if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
                filter.Property = iterator.ExtractValue().Value.ToString();
                if (!iterator.IsComparer()) throw new ConditionParseException(iterator.Position);
                filter.Comparer = iterator.ExtractComparer();
                if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
                filter.Value = iterator.ExtractValue();
                var result = Merge(left, operand, filter);
                if (iterator.End) return filter;

                if (iterator.IsOperand())
                {
                    var mOperand = iterator.ExtractOperand();
                    result = Analyze(iterator, filter, mOperand);
                }

                return result;
            }
        }

        static Expression Merge(Expression left, Operand? operand, Expression right)
        {
            if (operand.HasValue)
            {
                return new BinaryExpression
                {
                    Left = left,
                    Operand = operand.Value,
                    Right = right
                };
            }
            return left ?? right;
        }

    }
}
