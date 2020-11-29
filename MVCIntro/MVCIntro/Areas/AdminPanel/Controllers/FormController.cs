using MVCIntro.Areas.AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.AdminPanel.Controllers
{
    public class FormController : Controller
    {
        // GET: AdminPanel/Form
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAdminPanel()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdminPanel(LoginVMAdminPanel loginAdminPanel)
        {
            if (ModelState.IsValid)
            {
                TempData["msg"] = "Login to Admin Panel Sucessfully";
                return RedirectToAction("LoginAdminPanel");
            }
            return View();
        }
    }
}