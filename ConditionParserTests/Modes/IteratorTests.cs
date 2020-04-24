using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConditionParser.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConditionParser.Models.Tests
{
    [TestClass()]
    public class IteratorTests
    {
        [TestMethod()]
        public void ExtractValueTest()
        {
            var iterator = new Iterator("'ab\\'c'");
            var valueExpression = iterator.ExtractValue();
            Assert.AreEqual(valueExpression.Value, "ab\\'c");
        }

        [TestMethod()]
        public void IsValue_True_Test()
        {
            var iterator = new Iterator("'ab\\'c'");
            var isValue = iterator.IsValue();
            Assert.IsTrue(isValue);
        }

        [TestMethod()]
        public void IsValue_False_Test()
        {
            var iterator = new Iterator(">=");
            var isValue = iterator.IsValue();
            Assert.IsFalse(isValue);
        }

        [TestMethod()]
        public void IsComparer_True_Test()
        {
            var iterator = new Iterator(" startwith ");
            iterator.TrimStart();
            Assert.IsTrue(iterator.IsComparer());
        }

        [TestMethod()]
        public void IsComparer_False_Test()
        {
            var iterator = new Iterator("startwith");
            iterator.TrimStart();
            Assert.IsFalse(iterator.IsComparer());
        }

        [TestMethod()]
        public void ExtractComparerTest()
        {
            var iterator = new Iterator(">=");
            iterator.TrimStart();
            Assert.AreEqual(iterator.ExtractComparer(), Modes.Comparer.GreaterThanOrEqual);
        }

        [TestMethod()]
        public void NextTest()
        {
            var iterator = new Iterator(">=");
            Assert.AreEqual(iterator.Current, '>');
            Assert.AreEqual(iterator.Position, 0);
            Assert.IsTrue(iterator.Next());
            Assert.AreEqual(iterator.Current, '=');
            Assert.AreEqual(iterator.Position, 1);
            Assert.IsFalse(iterator.Next());

        }

        [TestMethod()]
        public void StartWith_True_Test()
        {
            var iterator = new Iterator("Name");
            Assert.IsTrue(iterator.StartWith("name"));
        }

        [TestMethod()]
        public void StartWith_False_Test()
        {
            var iterator = new Iterator("Name");
            Assert.IsFalse(iterator.StartWith("a"));
        }
    }
}