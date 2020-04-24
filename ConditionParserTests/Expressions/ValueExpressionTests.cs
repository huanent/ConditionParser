using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConditionParser.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Expressions.Tests
{
    [TestClass()]
    public class ValueExpressionTests
    {
        [TestMethod()]
        public void ValueExpression_String_Test()
        {
            var value = new ValueExpression("23", true);
            Assert.AreEqual(value.Value, "23");
            Assert.AreEqual(value.Type, typeof(string));
        }

        [TestMethod()]
        public void ValueExpression_Bool_Test()
        {
            var value = new ValueExpression("false", false);
            Assert.AreEqual(value.Value, false);
            Assert.AreEqual(value.Type, typeof(bool));
        }

        [TestMethod()]
        public void ValueExpression_Number_Test()
        {
            var value = new ValueExpression("23.4", false);
            Assert.AreEqual(value.Value, 23.4m);
            Assert.AreEqual(value.Type, typeof(decimal));
        }
    }
}