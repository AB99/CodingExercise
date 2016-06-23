using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using TweetCollector.Model;
using TweetCollector.Utility;

namespace TweetCollector
{
    public class TwitterFacade : ITwitterFacade
    {
        private readonly ITweetCollectorConfig _config;

        public TwitterFacade(ITweetCollectorConfig config)
        {
            _config = config;
        }

        //TODO: make this async

        public List<TwitterStatus> GetUserTimeLine(string screenName)
        {
            return GetUserTimeLine(screenName, 0);
        }

        public List<TwitterStatus> GetUserTimeLine(string screenName, ulong maxId)
        {
            var uri = new Uri(_config.TwitterApiUrl);
            uri = AddQueryStringParametersToUri(uri, screenName, maxId);

            HttpWebRequest request = CreateRequest(uri, _config.ConsumerKey, _config.ConsumerSecret);

            var response = request.GetResponse();        
            var responseStream = response.GetResponseStream();

            if (responseStream == null)
                return new List<TwitterStatus>();

            var streamReader = new StreamReader(responseStream);
            var content = streamReader.ReadToEnd();

            var tweets = JsonConvert.DeserializeObject<List<TwitterStatus>>(content);
            return tweets;
        }

        private static HttpWebRequest CreateRequest(Uri uri, string consumerKey, string consumerSecret)
        {
            var oauth = new OAuth.Manager
            {
                ["consumer_key"] = consumerKey,
                ["consumer_secret"] = consumerSecret
            };

            string authenticationHeader = oauth.GenerateAuthzHeader(uri.AbsoluteUri, "GET");

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("Authorization", authenticationHeader);
            request.Method = "GET";

            return request;
        }


        private static Uri AddQueryStringParametersToUri(Uri uri, string screenName, ulong maxId)
        {
            var requestParametersBuilder = new StringBuilder(uri.AbsoluteUri);
            requestParametersBuilder.Append(uri.Query.Length == 0 ? "?" : "&");

            Dictionary<string, object> fieldsToInclude = new Dictionary<string, object>
            {
                {"screen_name", screenName},
                {"include_rts", "1"},
                {"exclude_replies", "false"}
            };

            if (maxId != 0)
            {
                fieldsToInclude.Add("max_id", maxId.ToString());
            }

            foreach (KeyValuePair<string, object> item in fieldsToInclude)
            {
                if (item.Value is string)
                    requestParametersBuilder.AppendFormat("{0}={1}&", item.Key, UrlEncode((string)item.Value));
            }

            requestParametersBuilder.Remove(requestParametersBuilder.Length - 1, 1);

            return new Uri(requestParametersBuilder.ToString());
        }

        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = Uri.EscapeDataString(value);

            return value;
        }
    }
}
