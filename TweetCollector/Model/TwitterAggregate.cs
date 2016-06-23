using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TweetCollector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    [DataContract]
    public class TwitterAggregate
    {
        [DataMember, JsonProperty(PropertyName = "tweet_count")]
        public int TweetCount { get; set; }

        [DataMember, JsonProperty(PropertyName = "tweets_mentioning_users_count")]
        public int TweetsMentioningUsersCount { get; set; }

        [DataMember, JsonProperty(PropertyName = "tweets")]
        public IList<TwitterStatus> Tweets { get; set; }
    }
}
