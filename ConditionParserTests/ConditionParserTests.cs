using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConditionParser.Tests
{
    [TestClass()]
    public class ConditionParserTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            var result = ConditionParser.Parse("age=23&&name=alex&&((address='xiameng' and job=\"IT\"))");
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new StringEnumConverter());
            var json = JsonConvert.SerializeObject(result, settings);
        }
    }
}