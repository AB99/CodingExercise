using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TweetCollector;
using TweetCollector.Model;

namespace PayByPhoneCodingExercise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITweetCollector _tweetCollector;

        public HomeController(ITweetCollector tweetCollector)
        {
            _tweetCollector = tweetCollector;
        }

        public ActionResult Index()
        {
            //TwitterAggregate results = _tweetCollector.CollectTweetsForLastTwoWeeks();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}