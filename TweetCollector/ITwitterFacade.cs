using System.Collections.Generic;
using TweetCollector.Model;

namespace TweetCollector
{
    public interface ITwitterFacade
    {
        List<TwitterStatus> GetUserTimeLine(string screenName);
        List<TwitterStatus> GetUserTimeLine(string screenName, ulong maxId);
    }
}