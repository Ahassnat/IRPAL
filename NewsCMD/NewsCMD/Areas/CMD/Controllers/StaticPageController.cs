using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class StaticPageController : CMDBaseController
    {
        // GET: CMD/StaticPage//اصح شغل 
        public ActionResult Index( string q="",int PageId=1)
        {
            int TotalCount = db.StaticPages.Count(x => x.IsDelete == false && ( x.Title.Contains(q) || x.Slug.Contains(q)) );
            int RowsPerPage = 5;

            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/CMD/StaticPage/?q=" + q;
            #endregion

            int SkipCount = RowsPerPage * (PageId - 1);
            var items = db.StaticPages.Where(x => x.IsDelete == false && (x.Title.Contains(q) || x.Slug.Contains(q)))
                .OrderByDescending(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            return View(items.ToList());
            
        }

        public ActionResult Add()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(StaticPage item,HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.StaticPages.Count(x => x.IsDelete == false && (x.Title == item.Title || x.Slug == item.Slug));
                if(IsExist>0)
                {
                    TempData["msg"] = "e:الصقحة موجودة مسبقا ";
                    return Add();
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

                item.IsDelete = false;
                item.InsertedAt = DateTime.Now;
                item.UpdatedAdminId = AdminId;
                db.StaticPages.Add(item);
                db.SaveChanges();
                TempData["msg"] = "s:تمت عملية الاضافة بنجاح";
                return RedirectToAction("Add");
            }
            return View();
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                TempData["msg"] = "e:يرجي التاكد من صحة الرابط";
                return RedirectToAction("Index");
            }
            var item = db.StaticPages.Find(Id);
            if (item == null)
            {
                TempData["msg"] = "e: يرجي التاكد من الرابط";
                return RedirectToAction("Index");
            }

            return View(item);
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StaticPage item,HttpPostedFileBase  img)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.StaticPages.Count(x => x.IsDelete == false && 
                     (x.Title == item.Title || x.Slug == item.Slug) && x.Id != item.Id);
                if (IsExist > 0)
                {
                    TempData["msg"] = "e:عنوان الصفحة الثابتة موجود لدينا";
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
                var itemsFromDB = db.StaticPages.Find(item.Id);
                itemsFromDB.Title = item.Title;
                itemsFromDB.Slug = item.Slug;
                itemsFromDB.Details = item.Details;
                if(!string.IsNullOrEmpty(item.Image))
                    itemsFromDB.Image = item.Image;
                itemsFromDB.Published = item.Published;
                itemsFromDB.UpdatedAt = DateTime.Now;
                itemsFromDB.UpdatedAdminId = AdminId;
                db.Entry(itemsFromDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s:تمت عملية التعديل بنجاح ";
                return RedirectToAction("Index");
            }
           
            return View();
        }

        public ActionResult Delete(int? Id)
        {
            var item = db.StaticPages.Find(Id);
            item.IsDelete = true;
            item.UpdatedAt = DateTime.Now;
            item.UpdatedAdminId = AdminId;
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "s:تمت عملية الحذف بنجاح";
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());

            return RedirectToAction("Index");
        }
    }
}