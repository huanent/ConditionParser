using ConditionParser.Models;
using ConditionParser.Modes;
using System.Linq;

namespace ConditionParser.Analyzers
{
    public class OperandAnalyzer : IAnalyzer
    {
        static readonly string[] _or = new[] { "||", "or", "|", };
        static readonly string[] _and = new[] { "and", "&&", "&", };

        public object Extract(Iterator iterator)
        {
            var or = _or.FirstOrDefault(f => iterator.StartWith(f));
            var and = or == null ? _and.FirstOrDefault(f => iterator.StartWith(f)) : null;
            iterator.Next((or ?? and).Length);
            iterator.TrimStart();
            return or == null ? Operand.And : Operand.Or;
        }

        public bool IsMatched(Iterator iterator)
        {
            if (iterator.Current == '&' || iterator.Current == '|') return true;
            if (!iterator.LeftHasGap) return false;
            return iterator.StartWith("and ") || iterator.StartWith("or ");
        }
    }
}
