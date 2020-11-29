using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCIntro.Areas.WithForms.Controllers
{
    //لما تكون هان ال Authorize
    //بيكون علي كل الاكشن اللي في هاد الكنترولر لو بدنا اياه علي اكشن معين بنحطو فوق الاكشن اللي بدنا اياه
    [Authorize]
    public class HomeController : Controller
    {
        // GET: WithForms/Home
        public ActionResult Index()
        {
            Response.Cache.SetNoStore();//منع تخزين الصفحة في الكاش .. وهذا امر للمتصفح
            return View();
        }

        public ActionResult SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return Redirect("/Home/WithForms");
        }


    }
}