using ConditionParser.Analyzers;
using ConditionParser.Models;
using ConditionParser.Modes;
using System.Linq;

namespace ConditionParser
{
    public static class ConditionParser
    {
        static IAnalyzer[] _analyzers = new IAnalyzer[] {
            new ComparerAnalyzer(),
            new OperandAnalyzer(),
            new ValueAnalyzer()
        };

        public static Condition Parse(string condition)
        {
            var iterator = new Iterator(condition);
            iterator.TrimStart();
            return Analyze(iterator, null, null);
        }

        static Condition Analyze(Iterator iterator, Condition left, Operand? operand)
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
                var tree = new Condition();
                var analyzer = _analyzers.FirstOrDefault(f => f.IsMatched(iterator));
                tree.Left = analyzer.Extract(iterator);
                analyzer = _analyzers.FirstOrDefault(f => f.IsMatched(iterator));
                if (!(analyzer is ComparerAnalyzer)) throw new ConditionParseException(iterator.Position);
                tree.Connector = analyzer.Extract(iterator);
                analyzer = _analyzers.FirstOrDefault(f => f.IsMatched(iterator));
                if (!(analyzer is ValueAnalyzer)) throw new ConditionParseException(iterator.Position);
                tree.Right = analyzer.Extract(iterator);
                tree = Merge(left, operand, tree);
                if (iterator.End) return tree;
                analyzer = _analyzers.FirstOrDefault(f => f.IsMatched(iterator));

                if (analyzer is OperandAnalyzer operandAnalyzer)
                {
                    var mOperand = analyzer.Extract(iterator);
                    tree = Analyze(iterator, tree, (Operand)mOperand);
                }

                return tree;
            }
        }

        static Condition Merge(Condition left, Operand? operand, Condition right)
        {
            if (operand.HasValue)
            {
                return new Condition
                {
                    Left = left,
                    Connector = operand,
                    Right = right
                };
            }
            return left ?? right;
        }
    }
}
