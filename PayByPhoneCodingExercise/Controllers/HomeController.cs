using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
            //TODO: this would be better done as an ajax call or something so that it doesn't hold up the view
            TwitterAggregate results = _tweetCollector.CollectTweetsForLastTwoWeeks();

            string stuff = JsonConvert.SerializeObject(results);
            var vm = new TweetsViewModel { TweetJson = stuff };
    
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