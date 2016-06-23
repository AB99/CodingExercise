using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetCollector.Model;

namespace TweetCollector
{
    public class TweetCollector : ITweetCollector
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
            DateTimeOffset dateLimit = DateTimeOffset.UtcNow.AddDays(-14);

            //TODO: parallelize this

            foreach (string screenName in _screenNamesOfInterest)
            {
                var userTimelineTweets = GetUsersTimeLineForLastTwoWeeks(screenName, dateLimit);
                tweets.AddRange(userTimelineTweets);
            }

            tweets = tweets
                .Where(t => t.CreatedDateGmt >= dateLimit)
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

        private List<TwitterStatus> GetUsersTimeLineForLastTwoWeeks(string screenName, DateTimeOffset dateLimit)
        {
            var tweets = new List<TwitterStatus>();

            List<TwitterStatus> cursorTweets = _twitterFacade.GetUserTimeLine(screenName);
            tweets.AddRange(cursorTweets.Where(t => t.CreatedDateGmt >= dateLimit));
 
            while (!cursorTweets.Any(t => t.CreatedDateGmt < dateLimit) && cursorTweets.Any())
            {
                ulong maxId = cursorTweets.Min(t => t.Id);

                cursorTweets = _twitterFacade.GetUserTimeLine(screenName, maxId-1);
                tweets.AddRange(cursorTweets.Where(t => t.CreatedDateGmt >= dateLimit));
            }

            return tweets;
        }
    }
}
