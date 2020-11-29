using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class ArticleCategoryController : CMDBaseController
    {
        // GET: CMD/Article
        //public ActionResult Index()
        //{
        //    var item = db.ArticleCategories.Where(x => x.IsDelete == false)
        //        .OrderByDescending(x=>x.Id);
        //    return View(item.ToList());
        //}


        //start Coding 
        //اول عملية وهي عرض تصنيف المقالات وبيكون بالصفحة بحث وباجنيشن
        public ActionResult Index(string q = "", int PageId = 1)
        {
            int TotalCount = db.ArticleCategories.Count(x => x.Title.Contains(q) && x.IsDelete == false);
            int RowsPerPage = 5;

            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/CMD/ArticleCategory/?q=" + q;
            #endregion

            int SkipCount = RowsPerPage * (PageId - 1);
            var items = db.ArticleCategories.Where(x => x.Title.Contains(q) && x.IsDelete == false)
                .OrderByDescending(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            return View(items.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ArticleCategory item)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.ArticleCategories.Count(x => x.IsDelete == false && x.Title == item.Title) > 0;
                if (IsExist)
                {
                    TempData["msg"] = "e: تصنيف المقال موجود  ";
                    return Add();
                }
                item.InsertedAt = DateTime.Now;
                item.IsDelete = false;
                item.InsertedAdminId = AdminId;
                db.ArticleCategories.Add(item);
                db.SaveChanges();
                TempData["msg"] = "s: تمت عملية الاضافة بنجاح";
                return RedirectToAction("Add");
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //بنحط هاد الكود بدل منو 
                TempData["msg"] = "e: يرجي التاكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.ArticleCategories.Find(id);
            if (item == null)
            {
                //return HttpNotFound();
                TempData["msg"] = "e: يرجي التاكد من الرابط";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleCategory item)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.ArticleCategories
                    .Count(x => x.IsDelete == false && x.Title == item.Title && x.Id != item.Id) > 0;
                if (IsExist)
                {
                    TempData["msg"] = "e: تصنيف المقال موجود  ";
                    return Edit(item.Id);
                }

                var itemFromDB = db.ArticleCategories.Find(item.Id);//جبنا التصنيف من القاعدة كامل 
                itemFromDB.Title = item.Title;
                itemFromDB.UpdatedAt = DateTime.Now;
                itemFromDB.UpdatedAdminId = AdminId;
                db.Entry(itemFromDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s:بنجاح تمت عملية الحفظ";
                return RedirectToAction("Index");
            }
            return View();
        }

        #region delete
        //يتعامل مع القاعدة مباشرة حذف فعلي 
        //public ActionResult Delete(int id)
        //{
        //    var item = db.ArticleCategories.Find(id);
        //    db.ArticleCategories.Remove(item);
        //    db.SaveChanges();
        //    if (Request.UrlReferrer != null)
        //        return Redirect(Request.UrlReferrer.ToString());
        //    return RedirectToAction("Index");
        //}

        //يتعامل مع الموقع فقط حذف صوري ولا يحف من القاعدة انما يقوم بالتعديل
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e: يرجي التاكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.ArticleCategories.Find(id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من الرابط";
                return RedirectToAction("Index");
            }
            item.IsDelete = true;
            item.UpdatedAdminId = AdminId;
            item.UpdatedAt = DateTime.Now;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "e: تمت عملية الحذف بنجاح";
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            return RedirectToAction("Index");
            #endregion
        }

    }
}