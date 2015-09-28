using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Website.Domain.Shared.Controllers;
using Website.Domain.Social.Services;

namespace Website.Domain.Tasks
{
    public class TaskController : WebsiteController
    {
        private readonly TwitterService _twitter;

        public TaskController()
        {
            _twitter = new TwitterService();
        }

        public ActionResult Scheduler(int tweetCount = 10)
        {
            dynamic json = new ExpandoObject();

            json.result = true;

            _twitter.UpdateTweets(tweetCount);

            return Json(json);
        }
    }
}
