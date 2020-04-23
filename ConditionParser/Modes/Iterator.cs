using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConditionParser.Models
{
    public class Iterator
    {
        static char[] _trimChars = new[] { '\t', '\r', '\n', ' ' };

        public string Raw { get; private set; }

        public int Position { get; private set; }

        public char Current => Raw[Position];

        public bool End => Position >= (Raw.Length - 1);

        public bool LeftHasGap
        {
            get
            {
                if (Position == 0) return false;
                return Raw[Position - 1] == ' ';
            }
        }

        public bool RightHasGap
        {
            get
            {
                if (Position == Raw.Length - 1) return false;
                return Raw[Position + 1] == ' ';
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
    }
}
