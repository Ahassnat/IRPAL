using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class CMDBaseController : Controller
    {
        // GET: CMD/CMDBase
        protected int AdminId = 1;
        protected Models.FBaselNewsWebSiteEntities db = new Models.FBaselNewsWebSiteEntities();

        //دالة لتحجيم الصور برمجيا 
        protected void ResizeOrginalImage(String Image)
        {
            using (var img = System.Drawing.Image.FromFile(Server.MapPath("/Content/Images/Orginal/" + Image) ))
            {
                int w = 200;
                int h = img.Height * w / img.Width;

                using(var imgThump =new  System.Drawing.Bitmap(img, w, h))
                {
                    imgThump.Save(Server.MapPath("/Content/Images/Thumbe/" + Image),System.Drawing.Imaging.ImageFormat.Jpeg );
                }

                w = 500;
                if (img.Width < w) w = img.Width;//هان اذا عرض الصورة اقل من 500 هوخذ العرض للي انتا جاي مع االصورة
                h = img.Height * w / img.Width;
                using (var imgMedium = new System.Drawing.Bitmap(img, w, h))
                {
                    imgMedium.Save(Server.MapPath("/Content/Images/Medium/" + Image), System.Drawing.Imaging.ImageFormat.Jpeg);
                }


                w = 750;
                if (img.Width < w) w = img.Width;
                h = img.Height * w / img.Width;
                using (var imgLarg = new System.Drawing.Bitmap(img, w, h))
                {
                    imgLarg.Save(Server.MapPath("/Content/Images/Larg/" + Image), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
          
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ViewBag.db = db;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}