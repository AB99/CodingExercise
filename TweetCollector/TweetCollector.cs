using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetCollector.Model;

namespace TweetCollector
{
    public class TweetCollector
    {
        private readonly ITwitterFacade _twitterFacade;
        private readonly string[] _screenNamesOfInterest = { "@pay_by_phone", "@PayByPhone", " @PayByPhone_UK" };

        public TweetCollector(ITwitterFacade twitterFacade)
        {
            _twitterFacade = twitterFacade;
        }

        public TwitterAggregate CollectTweetsForLastTwoWeeks()
        {
            var tweets = new List<TwitterStatus>();

            foreach (string screenName in _screenNamesOfInterest)
            {
                var userTimelineTweets = _twitterFacade.UserTimeLine(screenName);
                tweets.AddRange(userTimelineTweets);
            }

            tweets = tweets
                .Where(t => t.CreatedDateGmt >= DateTimeOffset.UtcNow.AddDays(-14))
                .OrderByDescending(t => t.CreatedDateGmt).ToList();

            int tweetsMentioningUsersCount = tweets.Count(t => t.Text.Contains("@"));

            var aggregate = new TwitterAggregate
            {
                TweetCount =  tweets.Count,
                TweetsMentioningUsersCount = tweetsMentioningUsersCount,
                Tweets = tweets
            };

            return aggregate;
        }
    }
}
