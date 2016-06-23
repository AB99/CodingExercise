namespace TweetCollector.Utility
{
    public interface ITweetCollectorConfig
    {
        void Init();
        string TwitterApiUrl { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string AccessToken { get; }
        string AccessTokenSecret { get; }
    }
}