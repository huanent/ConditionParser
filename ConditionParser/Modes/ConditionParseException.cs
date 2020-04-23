using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Models
{
    public class ConditionParseException : Exception
    {
        public ConditionParseException(int positon) : base($"Parse condition error at positon {positon}")
        {

        }
    }
}
