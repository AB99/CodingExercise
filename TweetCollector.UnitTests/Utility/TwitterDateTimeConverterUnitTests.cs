using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using TweetCollector.Utility;

namespace TweetCollector.UnitTests.Utility
{
    [TestFixture]
    public class TwitterDateTimeConverterUnitTests
    {
        private TwitterDateTimeConverter _converter;
        

        [SetUp]
        public void Setup()
        {
            _converter = new TwitterDateTimeConverter();
        }

        [Test]
        public void TestReadJson()
        {
            var twitterDate = "\"Wed Jun 22 01:56:34 +0000 2016\"";

            var jsonTextReader = new JsonTextReader(new StringReader(twitterDate));
            jsonTextReader.Read();

            var result = _converter.ReadJson(jsonTextReader, null, null, null);

            var expectedDateTimeOffset = new DateTimeOffset(2016, 06, 22, 01, 56, 34, TimeSpan.Zero);
            Assert.That(result, Is.EqualTo(expectedDateTimeOffset));
        }

        [Test]
        public void WriteJson()
        {
            var dateTimeOffset = new DateTimeOffset(2016, 06, 22, 01, 56, 34, TimeSpan.Zero);

            var stringBuilder = new StringBuilder();
            var jsonTextWriter = new JsonTextWriter(new StringWriter(stringBuilder));

            _converter.WriteJson(jsonTextWriter, dateTimeOffset, null);

            var expectedTwitterDate = "\"Wed Jun 22 01:56:34 +0000 2016\"";    
            Assert.That(stringBuilder.ToString(), Is.EqualTo(expectedTwitterDate));
        }
    }
}
