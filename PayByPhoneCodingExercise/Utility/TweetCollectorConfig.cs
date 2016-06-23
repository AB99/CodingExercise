using System;
using System.Configuration;
using System.IO;
using TweetCollector.Utility;

namespace PayByPhoneCodingExercise.Utility
{
    public class TweetCollectorConfig : ITweetCollectorConfig
    {
        private string GetConfigValue(string key)
        {
            var configElement = ConfigurationManager.AppSettings.Get(key);
            return configElement;
        }

        public string TwitterApiUrl => GetConfigValue("TwitterApiUrl");

        //Tokens identifying the app
        public string ConsumerKey => GetConfigValue("ConsumerKey");
        public string ConsumerSecret => GetConfigValue("ConsumerSecret");
    }
}
