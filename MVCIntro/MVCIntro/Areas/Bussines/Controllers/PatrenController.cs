using MVCIntro.Areas.Bussines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.Bussines.Controllers
{
    public class PatrenController : Controller
    {
        // GET: Bussines/Patren
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
        public ActionResult Login(LoginVM login)
        {
            if (ModelState.IsValid)
            {
                TempData["msg"] = "Login Sucessful";
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}