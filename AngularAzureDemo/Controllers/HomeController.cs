using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AngularAzureDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subscriptions()
        {
            return View();
        }

        public ActionResult SketcherActions()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View();
        }


        public ActionResult ViewSingleImage(int id)
        {
            return View();
        }

    }
}
