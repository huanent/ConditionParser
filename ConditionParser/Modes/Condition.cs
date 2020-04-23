using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ConditionParser.Modes
{
    public class Condition
    {
        public enum Type
        {
            Filter,
            Group
        }

        public object Left { get; set; }
        public object Connector { get; set; }
        public object Right { get; set; }

        public Comparer ConnectorAsComparer()
        {
            return (Comparer)Connector;
        }

        public Operand ConnectorAsOperand()
        {
            return (Operand)Connector;
        }
    }
}
