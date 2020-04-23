using ConditionParser.Expressions;
using ConditionParser.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConditionParser.Models
{
    public class Iterator
    {
        static char[] _trimChars = new[] { '\t', '\r', '\n', ' ' };
        static readonly string[] _or = new[] { "||", "or", "|", };
        static readonly string[] _and = new[] { "and", "&&", "&", };

        readonly static Dictionary<string, Comparer> _comparerMapping = new Dictionary<string, Comparer>
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

        readonly static char[] _comparerStartSymbols = new char[] { '>', '=', '<', '!' };
        static char[] _normalEndSymbols = new char[] { ' ', '>', '<', '=', '!', '&', '|', ')' };

        public string Raw { get; private set; }

        public int Position { get; private set; }

        public char Current => Raw[Position];

        public bool End => Position >= (Raw.Length - 1);

        public bool HasLeftGap
        {
            get
            {
                if (Position == 0) return false;
                return Raw[Position - 1] == ' ';
            }
        }

        public Iterator(string raw)
        {
            Raw = raw;
        }

        public bool Next(int charNumber = 1)
        {
            Position += charNumber;
            return Position <= Raw.Length - 1;
        }

        public void TrimStart()
        {
            while (!End)
            {
                if (_trimChars.Any(a => a == Current))
                {
                    if (!Next()) break;
                }
                else break;
            }
        }

        public bool StartWith(string s)
        {
            if (s.Length + Position > Raw.Length) return false;

            for (int i = 0; i < s.Length; i++)
            {
                if (char.ToLower(Raw[Position + i]) != char.ToLower(s[i])) return false;
            }

            return true;
        }

        public bool IsOperand()
        {
            if (Current == '&' || Current == '|') return true;
            if (!HasLeftGap) return false;
            return StartWith("and ") || StartWith("or ");
        }

        public Operand ExtractOperand()
        {
            var or = _or.FirstOrDefault(f => StartWith(f));
            var and = or == null ? _and.FirstOrDefault(f => StartWith(f)) : null;
            Next((or ?? and).Length);
            TrimStart();
            return or == null ? Operand.And : Operand.Or;
        }

        public bool IsComparer()
        {
            if (_comparerStartSymbols.Any(a => Current == a)) return true;
            if (!HasLeftGap) return false;
            return StartWith("startwith ") || StartWith("contains ");
        }

        public Comparer ExtractComparer()
        {
            var kv = _comparerMapping.First(w => StartWith(w.Key));
            Next(kv.Key.Length);
            TrimStart();
            return kv.Value;
        }

        public bool IsValue() => !IsComparer() && !IsOperand();

        public ValueExpression ExtractValue()
        {
            char? symbol = null;

            var valueBuilder = new StringBuilder();

            if (Current == '\"')
            {
                symbol = '\"';
                Next();
            }

            if (Current == '\'')
            {
                symbol = '\'';
                Next();
            }

            while (true)
            {
                if (!symbol.HasValue && _normalEndSymbols.Any(a => Current == a)) break;

                if (symbol == Current)
                {
                    Next();
                    break;
                }

                valueBuilder.Append(Current);
                var hasNext = Next();
                if (!hasNext)
                {
                    if (symbol.HasValue) throw new ConditionParseException(Position);
                    break;
                }
            }

            TrimStart();
            return new ValueExpression(valueBuilder.ToString(), symbol.HasValue);
        }
    }
}
