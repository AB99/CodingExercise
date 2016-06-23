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
        ///// <summary>
        ///// Gets or sets the User ID.
        ///// </summary>
        ///// <value>The User ID.</value>
        //[DataMember, JsonProperty(PropertyName = "id")]
        //public decimal Id { get; set; }

        ///// <summary>
        ///// Gets or sets the name of the user.
        ///// </summary>
        ///// <value>The name of the user.</value>
        //[DataMember, JsonProperty(PropertyName = "name")]
        //public string Name { get; set; }

        /// <summary>
        /// Gets or sets the screenname.
        /// </summary>
        /// <value>The screenname.</value>
        [DataMember, JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }
    }
}
