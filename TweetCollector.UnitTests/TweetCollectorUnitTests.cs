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
        public void CollectTweetsForLastTwoWeeks_TweetMultipleTweets_PerformsCursoring()
        {
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(6),
                CreateTweet(5),

                CreateTweet(4),
                CreateTweet(3),

                CreateTweet(2),
                CreateTweet(1),
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName))
                .Returns(new List<TwitterStatus> {tweets[0], tweets[1] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[1].Id - 1))
                .Returns(new List<TwitterStatus> { tweets[2], tweets[3] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[3].Id - 1))
                .Returns(new List<TwitterStatus> { tweets[4], tweets[5] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[5].Id - 1))
                .Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, tweets[1].Id - 1));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, tweets[3].Id - 1));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, tweets[5].Id - 1));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName, 0));
            Assert.That(result.TweetCount, Is.EqualTo(6));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_TweetsInFirstPageOld_OnlyReternsMoreRecentTweets()
        {
            //NB: in this case the first page contains a value that is past the cutoff date, so it 
            //doesn't bother asking for further pages
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(4, Today.AddDays(-1)),
                CreateTweet(3, Today.AddDays(-15)),

                CreateTweet(2, Today.AddDays(-16)),
                CreateTweet(1, Today.AddDays(-17)),
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName))
                .Returns(new List<TwitterStatus> { tweets[0], tweets[1] });
           
            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName));
            Assert.That(result.TweetCount, Is.EqualTo(1));
            Assert.That(result.Tweets[0], Is.EqualTo(tweets[0]));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_TweetsInSecondPageOld_OnlyReternsMoreRecentTweets()
        {
            //NB: in this case the second page contains a value that is past the cutoff date, so it 
            //doesn't bother asking for further pages
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(4, Today.AddDays(-1)),
                CreateTweet(3, Today.AddDays(-2)),

                CreateTweet(2, Today.AddDays(-12)),
                CreateTweet(1, Today.AddDays(-17)),
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName))
                .Returns(new List<TwitterStatus> { tweets[0], tweets[1] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[1].Id - 1))
                .Returns(new List<TwitterStatus> { tweets[2], tweets[3] });

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName));
            Assert.That(result.TweetCount, Is.EqualTo(3));
            Assert.That(result.Tweets, Contains.Item(tweets[0]));
            Assert.That(result.Tweets, Contains.Item(tweets[1]));
            Assert.That(result.Tweets, Contains.Item(tweets[2]));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_MultipleScreenNamesOfInterset_AskForTweetsFromAllScreenNames()
        {
            var screenName1 = "someScreenName";
            var screenName2 = "someOtherScreenName";

            var tweets = new List<TwitterStatus>
            {
                CreateTweet(1, screenName: screenName1),
                CreateTweet(1, screenName: screenName2),
            };

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName1))
                .Returns(new List<TwitterStatus> { tweets[0] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName1, tweets[0].Id - 1))
                .Returns(new List<TwitterStatus>());

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName2))
                .Returns(new List<TwitterStatus> { tweets[1] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName2, tweets[1].Id - 1))
                .Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName1, screenName2 }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName1));
            _mockTwitterFacade.Verify(f => f.GetUserTimeLine(screenName2));
            Assert.That(result.TweetCount, Is.EqualTo(2));
            Assert.That(result.Tweets, Contains.Item(tweets[0]));
            Assert.That(result.Tweets, Contains.Item(tweets[1]));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_SomeTweetsMentionOtherusers_TweetsMentioningUsersCountSetCorrectly()
        {
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(4, text:"I don't mention a user"),
                CreateTweet(3, text: "I mention @SomeUser"),

                CreateTweet(2, text:"I don't mention a user either"),
                CreateTweet(1, text: "I mention @SomeOtherUser"),
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName))
                .Returns(new List<TwitterStatus> { tweets[0], tweets[1] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[1].Id - 1))
                .Returns(new List<TwitterStatus> { tweets[2], tweets[3] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[3].Id - 1))
                .Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();

            Assert.That(result.TweetsMentioningUsersCount, Is.EqualTo(2));
        }

        [Test]
        public void CollectTweetsForLastTwoWeeks_TweetsWithVariedDates_TweetsAreSortedInDescendingOrder()
        {
            var tweets = new List<TwitterStatus>
            {
                CreateTweet(4, Today.AddDays(-3)),
                CreateTweet(3, Today.AddDays(-1)),

                CreateTweet(2, Today.AddDays(-4)),
                CreateTweet(1, Today.AddDays(-2))
            };

            var screenName = "someScreenName";

            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName))
                .Returns(new List<TwitterStatus> { tweets[0], tweets[1] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[1].Id - 1))
                .Returns(new List<TwitterStatus> { tweets[2], tweets[3] });
            _mockTwitterFacade.Setup(stf => stf.GetUserTimeLine(screenName, tweets[3].Id - 1))
                .Returns(new List<TwitterStatus>());

            var tweetCollector = new TweetCollector(_mockTwitterFacade.Object)
            {
                ScreenNamesOfInterest = new[] { screenName }
            };

            TwitterAggregate result = tweetCollector.CollectTweetsForLastTwoWeeks();
            Assert.That(result.Tweets, Is.EqualTo(tweets.OrderByDescending(t => t.CreatedDateGmt)));
        }


        private static DateTimeOffset Today => DateTimeOffset.UtcNow;

        private static TwitterStatus CreateTweet(ulong id,             
            DateTimeOffset? createdDateGmt = null,
            string screenName = "someScreenName", string text = null)
        {
            createdDateGmt = createdDateGmt ?? DateTimeOffset.UtcNow;
            text = text ?? "This is tweet " + id;

            var tweet = new TwitterStatus
            {
                Id = id,
                CreatedDateGmt = createdDateGmt.Value,
                Text = text,
                User = new TwitterUser { ScreenName = screenName}
            };


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

    //public static class TestUtils
    //{
    //    public static List<T> AsList<T>(this T obj)
    //    {
    //        return new List<T> {obj};
    //    }

    //    public static ITwitterFacade SetupMockToReturnTweets(
    //        this Mock<ITwitterFacade> mockFacade, string screenName, List<TwitterStatus> tweets)
    //    {
    //        if (tweets.Count < 1) throw new ArgumentException("should not be empty", nameof(tweets));

    //        var firstTweet = tweets[0];
    //        mockFacade.Setup(stf => stf.GetUserTimeLine(screenName)).Returns(firstTweet.AsList());

    //        mockFacade.Setup(stf => stf.GetUserTimeLine(screenName, It.IsAny<ulong>()))
    //            .Returns(
    //                (string sn, ulong maxId) => tweets.OrderByDescending(t => t.Id).First(t => t.Id <= maxId).AsList());
        
    //        return mockFacade.Object;
    //    }
    //}
}
