using ConditionParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Analyzers
{
    public interface IAnalyzer
    {
        bool IsMatched(Iterator iterator);

        object Extract(Iterator iterator);
    }
}
