using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TweetCollector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{User.ScreenName}/{Text}")]
    [DataContract]
    public class BlarghTwitterStatus
    {
        [DataMember, JsonProperty(PropertyName = "id")]
        public decimal Id { get; set; }

        [DataMember]
        [JsonProperty(PropertyName = "created_at")]
        [JsonConverter(typeof(TwitterDateTimeConverter))]
        public DateTime CreatedDate { get; set; }

        [DataMember, JsonProperty(PropertyName = "source")]
        public string Source { get; set; }

        [DataMember, JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [DataMember, JsonProperty(PropertyName = "user")]
        public TwitterUser User { get; set; }
    }
}
