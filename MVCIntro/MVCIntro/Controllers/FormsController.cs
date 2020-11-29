using MVCIntro.ViewModles;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        //هاد الدالة بنتيها  بس اذا بدنا نحط عليها بوست
        //هاد الدالة يجب ينبعث عليها بوست 
        [HttpPost]
        [ValidateAntiForgeryToken]
                               //هاد بتعمل استدعاء تحقق من الحقول الموجودة في الكلاس الاسم والباسوورد    
        public ActionResult Login(LoginViewModle login )
        {
            //المودل شيك ع المطلوب ولقاه مزبوط
            //حسب الركويرد اللي محطوط علي الحقول اللي في الكلاس 
            // هل الداتا اللي وصلت السيرفر بيطابق جقول الكلاس الموديل ولا لا
            if(ModelState.IsValid)
            {
                /*وهاد الشرط بيتنفذ في صحفة ال
                HTML*/
                TempData["msg"] = "Login Successfully";
                //عشان يرجعنا للاكشن
                //Login
                //ويفضيه
                return RedirectToAction("Login");
            }
            return View();
        }

        /*من محاضرة تاريخ 10-12-2016 بنحكي عن رفع الملفات */
        public ActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase fle)
        {
            if (fle != null)
            {               //Server.MapPath => Convert Virtual Pathes To Phiscal Phathes
                fle.SaveAs(Server.MapPath("~/Content/Uploads/")+fle.FileName);//fle.FileName => احفظلي الملف بنفس اسمو 
                TempData["msg"] = "s:Successfuly Upload Your File";

            }
            else
            {
                TempData["msg"] = "e:Plese Select file to Upload ";
            }
            return View();
        }
    }
}