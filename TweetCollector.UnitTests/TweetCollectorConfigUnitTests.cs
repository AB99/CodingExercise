using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TweetCollector.Utility;

namespace TweetCollector.UnitTests
{
    [TestFixture]
    public class TweetCollectorConfigUnitTests
    {
        private TweetCollectorConfig _config;

        [SetUp]
        public void Setup()
        {
            _config = new TweetCollectorConfig("TweetCollectorTestConfig.xml");
        }

        [Test]
        public void TestTwitterApiUrl()
        {
            Assert.That(_config.TwitterApiUrl, Is.EqualTo("TestTwitterApiUrl"));
        }

        [Test]
        public void TestConsumerKey()
        {
            Assert.That(_config.ConsumerKey, Is.EqualTo("TestConsumerKey"));
        }

        [Test]
        public void TestConsumerSecret()
        {
            Assert.That(_config.ConsumerSecret, Is.EqualTo("TestConsumerSecret"));
        }

        [Test]
        public void TestAccessToken()
        {
            Assert.That(_config.AccessToken, Is.EqualTo("TestAccessToken"));
        }

        [Test]
        public void TestAccessTokenSecret()
        {
            Assert.That(_config.AccessTokenSecret, Is.EqualTo("TestAccessTokenSecret"));
        }
    }
}
