using System;
using System.Configuration;
using System.IO;

namespace TweetCollector.Utility
{
    public class TweetCollectorConfig : ITweetCollectorConfig
    {
        private Configuration _config;

        public void Init()
        {
            Init("TweetCollectorConfig.xml");
        }

        public void Init(string configFile)
        {
            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(Environment.CurrentDirectory, configFile)
            };

            _config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

        }

        private string GetConfigValue(string key)
        {
            var configElement = _config.AppSettings.Settings[key];
            return configElement?.Value;
        }

        public string TwitterApiUrl => GetConfigValue("TwitterApiUrl");

        //Tokens identifying the app
        public string ConsumerKey => GetConfigValue("ConsumerKey");
        public string ConsumerSecret => GetConfigValue("ConsumerSecret");

        //Tokens identifying the individual twitter account
        public string AccessToken => GetConfigValue("AccessToken");
        public string AccessTokenSecret => GetConfigValue("AccessTokenSecret");

    }
}
