namespace TweetCollector.Utility
{
    public interface ITweetCollectorConfig
    {
        string TwitterApiUrl { get; }
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
    }
}