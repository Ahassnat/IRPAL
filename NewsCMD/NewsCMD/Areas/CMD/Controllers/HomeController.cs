using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class HomeController : CMDBaseController
    {
        // GET: CMD/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChancePassword()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            return Redirect("/Account/Login");
        }
    }
}