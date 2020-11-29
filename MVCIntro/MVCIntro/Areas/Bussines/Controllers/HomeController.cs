using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.Bussines.Controllers
{
    public class HomeController : Controller
    {
        // GET: Bussines/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsingViewBagViewData()
        {
            ViewBag.Star = "My name is Student";
            var Mohamed = new Student
            {
                StudentName = "Mohammad",
                StudentAge = 15,
                ClassNumber = 3,
            };

            ViewBag.Student = Mohamed; 

            return View();
        }

        public ActionResult UsingTempData()
        {
            TempData["massage"] = "Welcome To Temp Data Page ";
            ViewBag.x = 15;

            ViewBag.z = TempData["x"];
            
            return View();
        }

        public ActionResult XAction()
        {
            TempData["x"] = "Iam the xAction Go to TempData Page";
            ViewBag.y = 25;
            return RedirectToAction("UsingTempData");
        }

        public ActionResult usingModle()
        {
            var Sana = new Student
            {
                StudentName = "Sana Hamad",
                StudentAge = 30,
                ClassNumber = 3
            };
            return View(Sana);
        }

        public class Student
        {
            public string StudentName { get; set; }
            public int StudentAge { get; set; }
            public int ClassNumber { get; set; }

            private string name;
            public string Name
            {
                set
                {
                    this.name=value;
                }
                get
                {
                    return this.name;
                }
            }
           
        }


        public ActionResult CSIntro()
        {
            return View();
        }
    }
}