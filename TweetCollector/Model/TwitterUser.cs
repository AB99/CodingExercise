using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TweetCollector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("@{ScreenName}")]
    [DataContract]
    public class TwitterUser
    {
        [DataMember, JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
    }
}
