using TweetCollector.Model;

namespace TweetCollector
{
    public interface ITweetCollector
    {
        TwitterAggregate CollectTweetsForLastTwoWeeks();
    }
}