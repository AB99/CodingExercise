using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using PayByPhoneCodingExercise.Models;
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
            TwitterAggregate results = _tweetCollector.CollectTweetsForLastTwoWeeks();

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(TwitterAggregate));
            ser.WriteObject(stream1, results);

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            var vm = new TweetsViewModel {TweetJson = sr.ReadToEnd()};

            return View(vm);
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