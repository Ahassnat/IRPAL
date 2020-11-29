using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Controllers
{
    public class SendDataToViewController : Controller
    {
        // GET: SendDataToView
        public ActionResult Index()
        {
            return View();
        }
        /***************** Usin ViewBag Function ***************************/
        public ActionResult UsingViewBagViewData()
        {
            ViewBag.Title = "Using View Bag and View Data";
            ViewBag.Name = "Abdallah ";
            ViewBag.Age = 30;

            Car bmw = new Car();
            bmw.Name = "BMW";  //set
            bmw.Model = "3 seies";
            bmw.Year = 1989;

            string name = bmw.Name;//get

            var mar = new Car()
            {
                Name = "Marsedies",
                Model = "5 Series",
                Year = 2015 //set
            
            };
            int year = mar.Year;//get

            ViewBag.BMW = bmw;
            ViewBag.MAR = mar;

            return View();
        }
        public class Car
        {
            public string Name { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }

                         /// <summary>
                         /// مفهوم 
                         /// Encapsolution
                         /// في هذه الحالة ما بيقدر يتعامل مع الاسم مباشرة الي من خلال دالة الست والقت لفلترة الاسم  
                         /// طبعا مايكروسفت تخلت عن الطريقة هاد واستخدمو مفهوم ال
                         /// proptities 
                         /// 
                         /// </summary>
            //private string name;
            //public void getName(string name)
            //{
            //    this.name= name;
            //}
            //public string setName()
            //{
            //    return this.name;
            //}
        }
        /************************END***********************************************/


        //*****************using TempData Function*****************************//
        public ActionResult UsingTempData()
        {
            //يستخدم لمرة واحدة فقط للانتقال بين الاكشن والكنترولر 
            // ال TempData
            // طالما لم يتم تحديثه يعرض مرة واحدة فقط
            // تسمح بنقل البيانات المؤقتة بين الاكشنز
            TempData["massge"] = "Welcome to tempData ";

            ViewBag.msg = TempData["msg"];

            return View();
        }

        public ActionResult SomeAction()
        {
            ViewBag.xyz = "XYZ";
            TempData["FromSomeAction"] = "MSG from Some Action To Temp Data Action ";
            TempData["msg"] = "I am in the tempdata action by viewBag function";
            //return Redirect("http://google.com");
             return RedirectToAction("UsingTempData");
            //return Redirect("/SendDataToView/UsingTempData");
        }

        //**************************END*******************************************//


        /*********************Using Modle **********************************/
        public ActionResult UsingModel()
        {
            var volvo = new Car
            {
                Name = "VOLVO",
                Model = "X-Series",
                Year=1999
            };
            return View(volvo);
        }

        //************************END***************************************//

        
           
    }
}