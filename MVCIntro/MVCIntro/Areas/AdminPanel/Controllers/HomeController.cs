using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.AdminPanel.Controllers
{
    public class HomeController : Controller
    {
        // GET: AdminPanel/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Customers()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Integrations()
        {
            return View();
        }
    }
}