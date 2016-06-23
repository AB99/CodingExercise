using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TweetCollector.Model;

namespace WebApplication1.Controllers
{
    public class TweetController : ApiController
    {
        //TODO: add OAuth verification

        public TweetController()
        {
            _tweets = new List<TwitterStatus>();

            var namesOfInterest = new TweetCollector.TweetCollector(null).ScreenNamesOfInterest;

            foreach (string name in namesOfInterest)
            {
                var tweetsForUser = new List<TwitterStatus>
                {
                    CreateTweet(2, Today.AddDays(-16), screenName: name, text: "I also mention @someUser"),
                    CreateTweet(1, Today.AddDays(-18), screenName: name, text: "I mention no one"),
                    CreateTweet(4, Today.AddDays(-2), screenName: name, text: "I mention @someUser"),
                    CreateTweet(3, Today.AddDays(-13), screenName: name, text: "I also mention no one"),
                };

                _tweets.AddRange(tweetsForUser);
            }
        }

        private List<TwitterStatus> _tweets;

        public IEnumerable<TwitterStatus> Get(ulong max_id, string screen_name)
        {
            return _tweets.Where(t => t.User.ScreenName == screen_name && t.Id <= max_id)
                .OrderByDescending(t => t.Id)
                .Take(2);
        }

        public IEnumerable<TwitterStatus> Get(string screen_name)
        {
            return _tweets.Where(t => t.User.ScreenName == screen_name)
                .OrderByDescending(t => t.Id)
                .Take(2);
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
                User = new TwitterUser { ScreenName = screenName }
            };


            return tweet;
        }
    }
}
