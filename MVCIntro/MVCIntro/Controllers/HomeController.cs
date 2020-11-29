using MVCIntro.Models;
using MVCIntro.ViewModles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Controllers
{
    public class HomeController : Controller
    {
        private IRPALG1Entities1 db = new IRPALG1Entities1();

        public ActionResult Index()
        {
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

        public ActionResult Vision()
        {
            return View();
        }
        /* public ActionResult Abdallah()
         {

            return Content("Welcome Abduallah");
         }*/
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CSIntro ()
        {
            return View();
        }


        //من محاضرة ال 13/12/2016 فيديو ال Form
        #region Forms  with Authentication
        public ActionResult FormsLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormsLogin(LoginViewModle login)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Where(x => x.UserName == login.UserName && x.Password == login.Password).FirstOrDefault();
                if (user != null)
                {   //  عملنا لوقن باستخدام الفورم اوثنتكيشن باستخدام زر الرمبر التذكر اذا قلي اتذكر بتذكر اذا محطش صح عليه ما بيتذكر                         
                    System.Web.Security.FormsAuthentication.SetAuthCookie(user.UserName,login.Remember);
                    return Redirect("/WithForms");
                }
                else
                    TempData["msg"] = "e:Invaled User Name Or Password";
            }
           return RedirectToAction("FormsLogin");
        }
        #endregion

        public ActionResult Register()
        {
            #region Swip Language
            //لغة المتصفح الافتراضية 
            string lang = Request.UserLanguages[0];
            // متغير علي هوا لغة المتصفح بينشيء ثقافة 
            string cluture = lang.Contains("ar") ? "ar-eg" : "en-gb";

            //لوكان عند المتصفح كوكي بلغة جديدة
            if (Request.Cookies["lang"] != null)
            {
                lang = Request.Cookies["lang"].Value.ToLower();
                cluture= lang.Contains("ar") ? "ar-eg" : "en-gb";
            }
            //ممكن ينكتب بسطرين او بسطر 

            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cluture);//CurrentCultureهاد لل
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cluture);// CurrentUICultureهاد لل

            System.Threading.Thread.CurrentThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cluture);
            ViewBag.IsRTL = lang.Contains("ar");
            #endregion
            return View();
        }

        //هاد الاكشن شغلانتو يضيف كوكي عندك باللغة اللي انتا مختارها وبوجهك علي اكشن الرجستر  
        public ActionResult Lang(string id)
        {
            if(id=="en" || id == "ar")
            {
                // ضفت كوكي اسمها 
                //lang
                //قيمتها اللغة اللي هو بعتها 
                //وعمرها 15 يوم 
                Response.Cookies.Add(new HttpCookie ("lang", id));
                Response.Cookies["lang"].Expires = DateTime.Now.AddDays(15);
            }
            return RedirectToAction("Register");
        }
    }
}