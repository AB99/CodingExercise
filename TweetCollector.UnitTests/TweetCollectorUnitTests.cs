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

        private ulong _tweetId;

        [SetUp]
        public void SetUp()
        {
            _mockTwitterFacade = new Mock<ITwitterFacade>();
            _tweetCollector = new TweetCollector(_mockTwitterFacade.Object);

            _tweetId = 1;
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

        [Test]
        public void CollectTweetsForLastTwoWeeks_TweetSingleTweet_PerformsCursoring()
        {
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(1)
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName)).Returns(tweets[0].AsList());
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, It.IsAny<ulong>()))
                .Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] {screenName}
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, tweets.Single().Id - 1));
            Assert.That(result.TweetCount, Is.EqualTo(1));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_TweetMultipleTweets_PerformsCursoring()
        {
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(2),
                CreateTweet(1),
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName)).Returns(tweets[0].AsList());
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, 1)).Returns(tweets[1].AsList());
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, 0)).Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, 1));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, 0));
            Assert.That(result.TweetCount, Is.EqualTo(2));
        }


        private static TwitterStatus CreateTweet(ulong id)
        {
            var tweet = new TwitterStatus
            {
                Id = id,
                CreatedDateGmt = DateTimeOffset.UtcNow,
            };

            tweet.Text = "This is tweet " + tweet.Id;

            return tweet;
        }

        private ITwitterFacade CreateEmptyTwitterFacade()
        {
            List<TwitterStatus> tweets = new List<TwitterStatus>();
            var stubTwitterFacade = new Mock<ITwitterFacade>();
            stubTwitterFacade.Setup(stf => stf.GetUserTimeLine(It.IsAny<string>())).Returns(tweets);
            stubTwitterFacade.Setup(stf => stf.GetUserTimeLine(It.IsAny<string>(), It.IsAny<ulong>())).Returns(tweets);
            return stubTwitterFacade.Object;
        }

    }

    public static class TestUtils
    {
        public static List<T> AsList<T>(this T obj)
        {
            return new List<T> {obj};
        }

        public static ITwitterFacade SetupMockToReturnTweets(
            this Mock<ITwitterFacade> mockFacade, string screenName, List<TwitterStatus> tweets)
        {
            if (tweets.Count < 1) throw new ArgumentException("should not be empty", nameof(tweets));

            var firstTweet = tweets[0];
            mockFacade.Setup(stf => stf.GetUserTimeLine(screenName)).Returns(firstTweet.AsList());

            mockFacade.Setup(stf => stf.GetUserTimeLine(screenName, It.IsAny<ulong>()))
                .Returns(
                    (string sn, ulong maxId) => tweets.OrderByDescending(t => t.Id).First(t => t.Id <= maxId).AsList());
        
            return mockFacade.Object;
        }
    }
}
