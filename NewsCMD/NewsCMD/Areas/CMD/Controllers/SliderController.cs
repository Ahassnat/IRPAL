using NewsCMD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsCMD.Areas.CMD.Controllers
{
    public class SliderController : CMDBaseController
    {
        // GET: CMD/Slider
        public ActionResult Index(string q="", int PageId= 1)
        {
            int TotalCount = db.Sliders.Count(x => x.IsDelete == false && (x.Title.Contains(q) || x.URL.Contains(q)));
            int RowsPerPage = 5;
            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);
            if (PageId < 1 || PageId > PageCount)
                PageId = 1;
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/CMD/Slider/?q=" + q;
            int SkipCount = RowsPerPage * (PageId - 1);

            var items = db.Sliders.Where(x => x.IsDelete == false && (x.Title.Contains(q) || x.URL.Contains(q)))
                .OrderByDescending(x => x.Id).Skip(SkipCount).Take(RowsPerPage); ;
            ViewBag.q = q;
            
            return View(items.ToList());
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Slider item , HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img == null)
                {
                    TempData["msg"] = "e:الرجاء رفع صورة  ";
                    return Add();
                }
                var IsExist = db.Sliders.Count(x => x.IsDelete == false && (x.Title == item.Title || x.URL == item.URL))  ;
                if (IsExist > 0)
                {
                    TempData["msg"] = "e:عنوان الشريحة او الرابط موجود مسبقا ";
                    return Add();
                }
                //اذا نوع الملف اللي جاي صورة 
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
                

                item.IsDelete = false;
                item.InsertedAt = DateTime.Now;
                item.InsertedAdminId = AdminId;
                db.Sliders.Add(item);
                db.SaveChanges();
                return RedirectToAction("Add");
            }
           
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["msg"] = "e:الرجاء التاكد من الرابط";
                return RedirectToAction("Index");
            }
            var item = db.Sliders.Find(id);
            //if (item == null)
            //{
            //    TempData["msg"] = "e:الرجاء التاكد من صحة الرابط";
            //    return RedirectToAction("Index");
            //}
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider item, HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                var IsExist = db.Sliders.Count(x => x.IsDelete == false && x.Title == item.Title  && x.Id != item.Id);
                if (IsExist > 0)
                {
                    TempData["msg"] = "e:عنوان الشريحة  موجود مسبقا";
                    return Edit(item.Id);
                }
                // يعني اذا في  صورة
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
                        TempData["msg"] = "e:الرجاء ارجاع صورة صحيحة";
                        return Edit(item.Id);
                    }
                }
                var itemFromDB = db.Sliders.Find(item.Id);
                itemFromDB.Title = item.Title;
                //اذا فعلا تغيرت الصور خذ الاسم الجديد للصورة
                //واذا ما تغيرت سيبها مثل ما هو اسم الصورة
                if(!string.IsNullOrEmpty(item.Image))//في العاد مش هيوخذ الitem كبراميتر ومش هيخش علي الif تاعت الصورة
                    itemFromDB.Image = item.Image;
                itemFromDB.URL = item.URL;
                itemFromDB.UpdatedAt = DateTime.Now;
                itemFromDB.UpdatedAdminId = AdminId;
                db.Entry(itemFromDB).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "s:تمت عملية التعديل بنجاح";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    TempData["msg"] = "e:يرجي التحقق من  الرابط";
            //    return RedirectToAction("Index");
            //}
            var item = db.Sliders.Find(id);
            //if (item == null)
            //{
            //    TempData["msg"] = "e:يرجي التحقق من صحة الرابط";
            //    return RedirectToAction("Index");
            //}
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