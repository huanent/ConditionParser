using ConditionParser.Models;
using System.Linq;
using System.Text;

namespace ConditionParser.Analyzers
{
    public class ValueAnalyzer : IAnalyzer
    {
        static char[] _normalEndSymbols = new char[] { ' ', '>', '<', '=', '!', '&', '|', ')' };
        public object Extract(Iterator iterator)
        {
            char? symbol = null;

            var valueBuilder = new StringBuilder();

            if (iterator.Current == '\"')
            {
                symbol = '\"';
                iterator.Next();
            }

            if (iterator.Current == '\'')
            {
                symbol = '\'';
                iterator.Next();
            }

            while (true)
            {
                if (!symbol.HasValue && _normalEndSymbols.Any(a => iterator.Current == a)) break;

                if (symbol == iterator.Current)
                {
                    iterator.Next();
                    break;
                }

                valueBuilder.Append(iterator.Current);
                var hasNext = iterator.Next();
                if (!hasNext)
                {
                    if (symbol.HasValue) throw new ConditionParseException(iterator.Position);
                    break;
                }
            }

            iterator.TrimStart();
            return valueBuilder.ToString();
        }

        public bool IsMatched(Iterator iterator)
        {
            return true;
        }
    }
}
