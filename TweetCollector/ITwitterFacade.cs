using System.Collections.Generic;
using TweetCollector.Model;

namespace TweetCollector
{
    public interface ITwitterFacade
    {
        List<TwitterStatus> UserTimeLine(string screenName);
    }
}