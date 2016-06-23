using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TweetCollector.Model;

namespace TweetCollector.UnitTests
{
    [TestFixture]
    public class TweetCollectorUnitTests
    {
        private TweetCollector _tweetCollector;
        private Mock<ITwitterFacade> _mockTwitterFacade;

        [SetUp]
        public void SetUp()
        {
            _mockTwitterFacade = new Mock<ITwitterFacade>();
            _tweetCollector = new TweetCollector(_mockTwitterFacade.Object);
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_NoTweets_ReturnsEmptyResult()
        {
            ITwitterFacade facade = CreateEmptyTwitterFacade();
            var tweetCollector = new TweetCollector(facade);

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();
            
            Assert.That(result.TweetCount, Is.EqualTo(0));
            Assert.That(result.TweetsMentioningUsersCount, Is.EqualTo(0));
            Assert.That(result.Tweets.Count, Is.EqualTo(0));
        }

        //[Test]
        //public void CollectTweetsForLastTwoWeeks_TweetsExist_TweetsAreAggregated()
        //{
        //    ITwitterFacade facade = CreateEmptyTwitterFacade();
        //    var tweetCollector = new TweetCollector(facade);

        //    TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

        //    Assert.That(result.TweetCount, Is.EqualTo(0));
        //    Assert.That(result.TweetsMentioningUsersCount, Is.EqualTo(0));
        //    Assert.That(result.Tweets.Count, Is.EqualTo(0));
        //}



        //private TwitterStatus CreateTwitterStatus()
        //{

        //}

        private ITwitterFacade CreateEmptyTwitterFacade()
        {
            List<TwitterStatus> tweets = new List<TwitterStatus>();
            var stubTwitterFacade = new Mock<ITwitterFacade>();
            stubTwitterFacade.Setup(stf => stf.GetUserTimeLine(It.IsAny<string>())).Returns(tweets);
            stubTwitterFacade.Setup(stf => stf.GetUserTimeLine(It.IsAny<string>(), It.IsAny<ulong>())).Returns(tweets);
            return stubTwitterFacade.Object;
        }
    }
}
