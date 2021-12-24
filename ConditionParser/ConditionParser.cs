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
            if (iterator.Current == '(')
            {
                iterator.Next();
                iterator.TrimStart();
                var result = Analyze(iterator, null, null);
                result = Merge(left, operand, result);
                if (iterator.Current != ')') throw new ConditionParseException(iterator.Position);
                iterator.Next();
                iterator.TrimStart();
                if (iterator.End || iterator.Current == ')') return result;
                if (!iterator.IsOperand()) throw new ConditionParseException(iterator.Position);
                var mOperand = iterator.ExtractOperand();
                result = Analyze(iterator, result, mOperand);
                return result;
            }
            else
            {
                var filter = GetFilter(iterator);
                var result = Merge(left, operand, filter);
                if (iterator.End || iterator.Current == ')') return result;
                if (!iterator.IsOperand()) throw new ConditionParseException(iterator.Position);
                var mOperand = iterator.ExtractOperand();
                result = Analyze(iterator, result, mOperand);
                return result;
            }
        }

        private static FilterExpression GetFilter(Iterator iterator)
        {
            var filter = new FilterExpression();
            if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
            filter.Property = iterator.ExtractValue(true).Value.ToString();
            if (!iterator.IsComparer()) throw new ConditionParseException(iterator.Position);
            filter.Comparer = iterator.ExtractComparer();
            if (!iterator.IsValue()) throw new ConditionParseException(iterator.Position);
            filter.Value = iterator.ExtractValue();
            return filter;
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
