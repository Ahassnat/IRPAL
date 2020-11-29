
using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class AdminController : CMDBaseController
    {
        // GET: CMD/Admin
        public ActionResult Index(string q="",int PageId=1)
        {
            int TotalCount = db.Admins.Count(x => x.IsDelete == false && (x.Email.Contains(q) || x.FullName.Contains(q)));
            int RowsPerPage = 5;
            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);
            if (PageId < 1 || PageId > PageCount)
                PageId = 1;
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/CMD/Admin/?q=" + q;           
            int SkipCount = RowsPerPage * (PageId - 1);

            var items = db.Admins.Where(x => x.IsDelete == false &&( x.Email.Contains(q) || x.FullName.Contains(q)))
                .OrderByDescending(x=>x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            return View(items.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Admin item)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.Admins.Count(x => x.IsDelete == false && (x.Email == item.Email || x.FullName==item.FullName));
                if (IsExist > 0)
                {
                    TempData["msg"] = "e:البريد الالكتروني او اسم المستخدم  موجود مسبقا";
                    return Add();
                }
                item.InsertedAt = DateTime.Now;
                item.IsDelete = false;
                item.InsertedAdminId = AdminId;
                db.Admins.Add(item);
                db.SaveChanges();
                TempData["msg"] = "تمت الاضافة بنجاح";
                return RedirectToAction("Add");
            }

            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e: يرجي التأكد من الرابط";
                return RedirectToAction("Index");

            }
            var item = db.Admins.Find(id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من صحة المعرف في الرابط ";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin item)
        {
            if (ModelState.IsValid)
            {
                var IsExsit = db.Admins.Count(x => x.IsDelete == false && x.Email == item.Email && x.Id != item.Id);
                if (IsExsit > 0)
                {
                    TempData["msg"] = "e:  المستخدم موجود يرجي التاكد من  البريد الالكتروني";
                    return Edit(item.Id);
                }
                var itemFromDB = db.Admins.Find(item.Id);
                itemFromDB.FullName = item.FullName;
                itemFromDB.Email = item.Email;
                itemFromDB.Mobile = item.Mobile;
                itemFromDB.Active = item.Active;
                //itemFromDB.IsDelete = false;
                itemFromDB.UpdatedAt = DateTime.Now;
                itemFromDB.UpdatedAdminId = AdminId;
                db.Entry(itemFromDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s: تمت عملية التعديل بنجاح";
                return RedirectToAction("Index");
            }
            
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e:الرجاء التأكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.Admins.Find(id);
            if (item == null)
            {
                TempData["msg"] = "e:الرجاء التأكد من صحة الرابط";
                return RedirectToAction("Index");
            }

            item.IsDelete = true;
            item.UpdatedAt = DateTime.Now;
            item.UpdatedAdminId = AdminId;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "s:تمت عملية الحذف بنجاح";
            if (Request.UrlReferrer!=null)
                return Redirect(Request.UrlReferrer.ToString());
            return View();
        }

        public ActionResult Permission(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e: يرجي التأكد من الرابط";
                return RedirectToAction("Index");

            }
            var item = db.Admins.Find(id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من صحة المعرف في الرابط ";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Permission(int Id, int[] linkId)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admins.Find(Id);
                foreach (var link in admin.Links.ToList())
                {
                    admin.Links.Remove(link);
                }
                db.SaveChanges();
                foreach (var lid in linkId)
                {
                    var link = db.Links.Find(lid);
                    admin.Links.Add(link);
                }
                db.SaveChanges();
                TempData["msg"] = "s:تمت عملية الحفظ بنجاح";
                return Redirect("/CMD/Admin/Permission/" + Id);


            }
            return Permission(Id);
        }

        public ActionResult Active(int?id)
        {
           
                if (id == null)
                {
                    return Json(new { status = 0, msg = "Please Ensure From The Domain" }, JsonRequestBehavior.AllowGet);
                }
                var item = db.Admins.Find(id);
                if (item == null)
                {
                    return Json(new { status = 0, msg = "Please Ensure the Number Id " }, JsonRequestBehavior.AllowGet);
                }
                item.Active = !item.Active;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedAdminId = AdminId;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { status = 1, msg = "تمت بنجاح" }, JsonRequestBehavior.AllowGet);
        }

    }

}