using MVCIntro.Areas.Student.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.Student.Controllers
{
    public class FormsController : Controller
    {
        // GET: Student/Forms
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVModle login)
        {
            return View();
        }
    }
}