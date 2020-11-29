using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Controllers
{
    public class AjaxExampleController : Controller
    {
        // GET: Ajax
        public ActionResult LoadExample()
        {
            return View();
        }
        public ActionResult PostExample()
        {
            return View();
        }
        public ActionResult GetExample()
        {
            return View();
        }
        public ActionResult AjaxExample()
        {
            return View();
        }
        public ActionResult Page1()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult PostEx(string age, string gender)
        {
            //ضيع وقت لمدة 2.5 ثانية
            //ضروري تشيلها لما ترفع الشغل
            System.Threading.Thread.Sleep(2500);
            return Content(DateTime.Now + "<br>" +
                "Gender: " + gender + ", Age: " + age);
        }

        [HttpGet]
        public ActionResult GetEx(string age, string gender)
        {
            System.Threading.Thread.Sleep(1500);
            return Content(DateTime.Now + "<br>" +
                "Gender: <b>" + gender + "</b>, Age: <b>" + age + "</b>");
        }

        public ActionResult GetXML()
        {
            Response.ContentType = "text/xml";
            return View();
        }
        public ActionResult GetJson()
        {
            return Json(new[] {
                new
                {
                    Name="Asser",
                    Age = 25,
                    Edu="Fininas"
                },
                new
                {
                    Name="Kh",
                    Age =28,
                    Edu ="IT"
                }
            },JsonRequestBehavior.AllowGet);
        }
    }
}