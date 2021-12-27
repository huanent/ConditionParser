using ConditionParser.Expressions;
using ConditionParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConditionParser.Tests
{
    [TestClass()]
    public class ConditionParserTests
    {
        [TestMethod()]
        public void Parse_One_Filter_Test()
        {
            var expression = ConditionParser.Parse("name=alex");
            Assert.AreEqual(expression.NodeType, ExpressionType.Filter);
            var filter = expression as FilterExpression;
            Assert.AreEqual(filter.Property, "name");
            Assert.AreEqual(filter.Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual(filter.Value.Value, "alex");
            Assert.AreEqual(filter.Value.Type, typeof(string));
        }

        [TestMethod()]
        public void Parse_One_Filter_With_Number_Test()
        {
            var expression = ConditionParser.Parse("age=23");
            Assert.AreEqual(expression.NodeType, ExpressionType.Filter);
            var filter = expression as FilterExpression;
            Assert.AreEqual(filter.Property, "age");
            Assert.AreEqual(filter.Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual(filter.Value.Value, 23M);
            Assert.AreEqual(filter.Value.Type, typeof(decimal));
        }

        [TestMethod()]
        public void Parse_One_Filter_With_Bool_Test()
        {
            var expression = ConditionParser.Parse("IsMan=true");
            Assert.AreEqual(expression.NodeType, ExpressionType.Filter);
            var filter = expression as FilterExpression;
            Assert.AreEqual(filter.Property, "IsMan");
            Assert.AreEqual(filter.Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual(filter.Value.Value, true);
            Assert.AreEqual(filter.Value.Type, typeof(bool));
        }

        [TestMethod()]
        public void Parse_One_Filter_With_String_Test()
        {
            var expression = ConditionParser.Parse("IsMan='true'");
            Assert.AreEqual(expression.NodeType, ExpressionType.Filter);
            var filter = expression as FilterExpression;
            Assert.AreEqual(filter.Property, "IsMan");
            Assert.AreEqual(filter.Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual(filter.Value.Value, "true");
            Assert.AreEqual(filter.Value.Type, typeof(string));
        }

        [TestMethod()]
        public void Parse_With_Operand_Test()
        {
            var expression = ConditionParser.Parse("IsMan='true'&&age=23");
            Assert.AreEqual(expression.NodeType, ExpressionType.Binary);
            var binary = expression as BinaryExpression;
            Assert.IsInstanceOfType(binary.Left, typeof(FilterExpression));
            Assert.IsInstanceOfType(binary.Right, typeof(FilterExpression));
            var left = binary.Left as FilterExpression;
            var right = binary.Right as FilterExpression;
            Assert.AreEqual(binary.Operand, Modes.Operand.And);

            Assert.AreEqual(left.Property, "IsMan");
            Assert.AreEqual(left.Value.Value, "true");
            Assert.AreEqual(left.Value.Type, typeof(string));

            Assert.AreEqual(right.Property, "age");
            Assert.AreEqual(right.Value.Value, 23M);
            Assert.AreEqual(right.Value.Type, typeof(decimal));
        }

        [TestMethod()]
        public void Parse_With_multi_Operand_Test()
        {
            var expression = ConditionParser.Parse(" age>23||name='alex' && IsMan='true'");
            Assert.AreEqual(expression.NodeType, ExpressionType.Binary);
            var binary = expression as BinaryExpression;
            Assert.IsInstanceOfType(binary.Left, typeof(FilterExpression));
            Assert.IsInstanceOfType(binary.Right, typeof(BinaryExpression));
            var left = binary.Left as FilterExpression;
            var right = binary.Right as BinaryExpression;
            
            Assert.AreEqual(left.Property, "age");
            Assert.AreEqual(left.Comparer, Modes.Comparer.GreaterThan);
            Assert.AreEqual(left.Value.Value, 23M);
            
            Assert.AreEqual(binary.Operand, Modes.Operand.Or);
            Assert.AreEqual(right.Operand, Modes.Operand.And);

            Assert.AreEqual((right.Left as FilterExpression).Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual((right.Left as FilterExpression).Property, "name");
            Assert.AreEqual((right.Left as FilterExpression).Value.Value, "alex");

            Assert.AreEqual((right.Right as FilterExpression).Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual((right.Right as FilterExpression).Property, "IsMan");
            Assert.AreEqual((right.Right as FilterExpression).Value.Value, "true");
        }

        [TestMethod()]
        public void Parse_With_multi_Operand_And_Group_Test()
        {
            var expression = ConditionParser.Parse("IsMan='true'&&(age>23||name='alex')");
            Assert.AreEqual(expression.NodeType, ExpressionType.Binary);
            var binary = expression as BinaryExpression;
            Assert.IsInstanceOfType(binary.Left, typeof(FilterExpression));
            Assert.IsInstanceOfType(binary.Right, typeof(BinaryExpression));
            var left = binary.Left as FilterExpression;
            var right = binary.Right as BinaryExpression;
            Assert.AreEqual(binary.Operand, Modes.Operand.And);
            Assert.AreEqual(right.Operand, Modes.Operand.Or);

            Assert.AreEqual(left.Property, "IsMan");
            Assert.AreEqual(left.Value.Value, "true");

            Assert.AreEqual((right.Left as FilterExpression).Comparer, Modes.Comparer.GreaterThan);
            Assert.AreEqual((right.Left as FilterExpression).Property, "age");
            Assert.AreEqual((right.Left as FilterExpression).Value.Value, 23M);

            Assert.AreEqual((right.Right as FilterExpression).Comparer, Modes.Comparer.EqualTo);
            Assert.AreEqual((right.Right as FilterExpression).Property, "name");
            Assert.AreEqual((right.Right as FilterExpression).Value.Value, "alex");
        }

        [TestMethod()]
        public void Parse_complex_Test()
        {
            var expression =
                ConditionParser.Parse(
                    "(name='alex' and age>=28) ||(address Contains \"厦门\" & (name startwith 'a' || school ='双十'))");
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            var json = JsonConvert.SerializeObject(expression, settings);
        }

        [TestMethod()]
        public void Parse_Fail_Test()
        {
            Assert.ThrowsException<ConditionParseException>(() => ConditionParser.Parse("("),
                "Parse condition error at positon 1");
            Assert.ThrowsException<ConditionParseException>(() => ConditionParser.Parse("(a=2"),
                "Parse condition error at positon 4");
            Assert.ThrowsException<ConditionParseException>(() => ConditionParser.Parse("a=2 ("),
                "Parse condition error at positon 4");
            Assert.ThrowsException<ConditionParseException>(() => ConditionParser.Parse("a=2 sd"),
                "Parse condition error at positon 4");
        }
    }
}