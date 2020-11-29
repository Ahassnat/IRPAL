using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCMultiLanguage.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var lang = ControllerContext.RouteData.Values["lang"].ToString().ToLower();
            var url = Request.Url.ToString().ToLower();
            if (!url.Contains("/" + lang )) 
             return Redirect("/" + lang);
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