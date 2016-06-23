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

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int max_id, string screen_name)
        {
            return "value";
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
