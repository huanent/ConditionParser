using ConditionParser.Models;
using ConditionParser.Modes;
using System.Collections.Generic;
using System.Linq;

namespace ConditionParser.Analyzers
{
    public class ComparerAnalyzer : IAnalyzer
    {
        readonly static Dictionary<string, Comparer> _dic = new Dictionary<string, Comparer>
        {
            { "startwith",Comparer.StartWith },
            { "contains",Comparer.Contains },
            { ">=",Comparer.GreaterThanOrEqual },
            { "<=",Comparer.LessThanOrEqual },
            { "==",Comparer.EqualTo },
            { "!=",Comparer.NotEqualTo },
            { "<>",Comparer.NotEqualTo },
            { "=",Comparer.EqualTo },
            { ">",Comparer.GreaterThan },
            { "<",Comparer.LessThan },
        };

        public object Extract(Iterator iterator)
        {
            var kv = _dic.First(w => iterator.StartWith(w.Key));
            iterator.Next(kv.Key.Length);
            iterator.TrimStart();
            return kv.Value;
        }

        public bool IsMatched(Iterator iterator)
        {
            if (iterator.Current == '>' || iterator.Current == '=' || iterator.Current == '<' || iterator.Current == '!') return true;
            if (!iterator.LeftHasGap) return false;
            return iterator.StartWith("startwith ") || iterator.StartWith("contains ");
        }
    }
}
