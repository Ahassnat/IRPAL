using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Menu = NewsCMD.Models.Menu;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class MenuController : CMDBaseController
    {
        // GET: CMD/Menus
        public ActionResult Index(int id=0)
        {
            var items = db.Menus.Where(x => x.IsDelete == false && x.ParentId == id);
            ViewBag.ParentId = id;
            if (id != 0)
            {
                var menu = db.Menus.Find(id);
                if (menu == null )
                {
                    TempData["msg"] = "e:الرجاء التاكد من الرابط";
                    return RedirectToAction("Index");
                }
                ViewBag.menuTitle = menu.Title;
            }
            return View(items.ToList());
        }

        public ActionResult Add(int id=0)
        {
            ViewBag.ParentId = id;
            if (id != 0)
            {
                var menu = db.Menus.Find(id);
                if (menu == null)
                {
                    TempData["msg"] = "e:الرجاء التاكد من الرابط";
                    return Add();
                }
                ViewBag.MneuTitel = menu.Title;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Menu item)
        {
            if (ModelState.IsValid)
            {
                var Isexist = db.Menus.Count(x => x.IsDelete == false && x.Title == item.Title && x.ParentId == item.ParentId);
                if (Isexist > 0)
                {
                    TempData["msg"] = "e:القائمة المدخلة موجودة مسبقا يرجي ادخال قام بعنوان اخر";
                    return Add();
                }
                item.IsDelete = false;
                item.InsertedAt = DateTime.Now;
                item.InsertedAdminId = AdminId;
                db.Menus.Add(item);
                db.SaveChanges();
                TempData["msg"] = "s:تم الحفظ بنجاح";
                return RedirectToAction("Add");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e:الرجاء التاكد من الرابط";
                return RedirectToAction("Index");
            }
            var items = db.Menus.Find(id);
            if (items == null)
            {
                TempData["msg"] = "e:الرجاء التاكد من الرابط";
                return RedirectToAction("Index");
            }
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu item)
        {
            if (ModelState.IsValid)
            {
                var IsExistInDB = db.Menus.Count(x => x.IsDelete == false && x.Title == item.Title && x.ParentId == item.ParentId && x.Id != item.Id);
                if (IsExistInDB > 0)
                {
                    TempData["msg"] = "عنوان القائمة موجود مسبقا";
                    return Edit(item.Id);
                }
                var FindItemsInDB = db.Menus.Find(item.Id);
                FindItemsInDB.Title = item.Title;
                FindItemsInDB.URL = item.URL;
                FindItemsInDB.Active = item.Active;
                FindItemsInDB.NewWindow = item.NewWindow;
                FindItemsInDB.UpdatedAt = DateTime.Now;
                FindItemsInDB.UpdatedAdminId = AdminId;
                db.Entry(FindItemsInDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s:تمت عملية التعديل بنجاح";
                return Redirect("/CMD/Menu/Index/" + item.ParentId);

            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var item = db.Menus.Find(id);
            item.IsDelete = true;
            item.UpdatedAt = DateTime.Now;
            item.UpdatedAdminId = AdminId;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "s:تمت عمليه الحذف بنجاح";
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            return View();
        }

        public ActionResult Active(int? id)
        {
            
            if (id == null)
            {
                return Json(new { status = 0, msg = "الرجاء التاكد من الرابط المطلوب" }, JsonRequestBehavior.AllowGet);
            }
            var item = db.Menus.Find(id);
            if (id == null)
            {
                return Json(new { status = 0, msg = "الرجاء التاكد من الرقم المطلوب" }, JsonRequestBehavior.AllowGet);
            }
            item.Active = !item.Active;//عكس الحالة 
            item.UpdatedAdminId = AdminId;
            item.UpdatedAt = DateTime.Now;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();


            return Json(new { status=1,msg="تمت بنجاح"}, JsonRequestBehavior.AllowGet);
        }
    }
}