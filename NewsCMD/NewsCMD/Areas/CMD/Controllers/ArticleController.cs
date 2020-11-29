using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class ArticleController : CMDBaseController
    {
        // GET: CMD/Article
        public ActionResult Index(string q = "", int PageId = 1)
        {
            int TotalCount = db.Articles.Count(x => x.Title.Contains(q) && x.IsDelete == false);
            int RowsPerPage = 5;

            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/CMD/Article/?q=" + q;
            #endregion

            int SkipCount = RowsPerPage * (PageId - 1);
            var items = db.Articles.Where(x => x.Title.Contains(q) && x.IsDelete == false)
                .OrderByDescending(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            return View(items.ToList());
        }


        public ActionResult Add()
        {
            //هين حنا ضفنا سلكتلست عشان نطلع قائمة بعنوان التصنيف للمقالات 
            ViewBag.CategoryList = new SelectList(db.ArticleCategories
                .Where(x => x.IsDelete==false ),"Id","Title");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Article item, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.Articles.Count(x => x.IsDelete == false && x.Slug == item.Slug) > 0;
                if (IsExist)
                {
                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { status = 0, msg = "عنوان رابط المقال  موجود" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["msg"] = "e: عنوان رابط المقال  موجود  ";
                        return Add();
                    }
                }

                if (img != null)
                {
                    if (img.ContentType.Contains("image"))
                    {
                        //اعطينا الصورة اسم + الامتداد 
                        item.Image = Guid.NewGuid() + System.IO.Path.GetExtension(img.FileName);
                        img.SaveAs(Server.MapPath("/Content/Images/Orginal/" + item.Image));
                        //للنسخ 3 الاخري للصورة 
                        ResizeOrginalImage(item.Image);
                    }
                    else
                    {
                        if (Request.IsAjaxRequest())
                        {
                            return Json(new { status = 0, msg = "الرجاء رفع صورة صحيحة" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            TempData["msg"] = "e:الرجاء رفع صورة صحيحة  ";
                            return Add();
                        }
                        
                    }
                }

                item.InsertedAt = DateTime.Now;
                item.IsDelete = false;
                item.InsertedAdminId = AdminId;
                
                db.Articles.Add(item);
                db.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { status = 1, msg = "تمت عملية الاضافة بنجاح" }, JsonRequestBehavior.AllowGet);
                }
                
                    TempData["msg"] = "s: تمت عملية الاضافة بنجاح";
                    return RedirectToAction("Add");
                
                
            }
            return View();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e: يرجي التاكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.Articles.Find(id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من الرابط";
                return RedirectToAction("Index");
            }
            //هين حنا ضفنا سلكتلست عشان نطلع قائمة بعنوان التصنيف للمقالات 
            ViewBag.CategoryList = new SelectList(db.ArticleCategories
                .Where(x => x.IsDelete == false), "Id", "Title",item.CategoryId);

            return View(item);
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Article item,HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.Articles
                    .Count(x => x.IsDelete == false && x.Slug == item.Slug && x.Id != item.Id) > 0;
                if (IsExist)
                {
                    TempData["msg"] = "e:  المقال موجود  ";
                    return Edit(item.Id);
                }
                if (img != null)
                {
                    if (img.ContentType.Contains("image"))
                    {
                        //اعطينا الصورة اسم + الامتداد 
                        item.Image = Guid.NewGuid() + System.IO.Path.GetExtension(img.FileName);
                        img.SaveAs(Server.MapPath("/Content/Images/Orginal/" + item.Image));
                        //للنسخ 3 الاخري للصورة 
                        ResizeOrginalImage(item.Image);
                    }
                    else
                    {
                        TempData["msg"] = "e:الرجاء رفع صورة صحيحة  ";
                        return Add();
                    }
                }

                var itemFromDB = db.Articles.Find(item.Id);//جبنا التصنيف من القاعدة كامل 

                #region الحقول التي سيتم التعديل عليها 
                itemFromDB.Title = item.Title;
                itemFromDB.Slug = item.Slug;
                itemFromDB.Summary = item.Summary;
                itemFromDB.Published = item.Published;
                if(!string.IsNullOrEmpty(item.Image))
                    itemFromDB.Image = item.Image;
                itemFromDB.CategoryId = item.CategoryId;
                itemFromDB.UpdatedAt = DateTime.Now;
                itemFromDB.UpdatedAdminId = AdminId;
                #endregion

                db.Entry(itemFromDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s:بنجاح تمت عملية الحفظ";
                return RedirectToAction("Index");
            }
            return View();
        }


        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                TempData["msg"] = "e: يرجي التاكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.Articles.Find(Id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من الرابط";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryList = new SelectList(db.ArticleCategories
                .Where(x => x.IsDelete == false), "Id", "Title", item.CategoryId);
            item.IsDelete = true;
            item.UpdatedAdminId = AdminId;
            item.UpdatedAt = DateTime.Now;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "e: تمت عملية الحذف بنجاح";
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            return RedirectToAction("Index");

        }


    }
}