using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCIntro.Models;

namespace MVCIntro.Controllers
{
    public class AccountNewController : Controller
    {
        private IRPALG1Entities1 db = new IRPALG1Entities1();

        // GET: AccountNew
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Country);
            return View(accounts.ToList());
        }

        // GET: AccountNew/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: AccountNew/Create
        public ActionResult Create()
        {
            ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name");
            return View();
        }

        // POST: AccountNew/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FullName,Email,Gender,Active,CountryId,DOB")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                //هيعرض رسالة نجاح هنستدعيها في ملف اللياوت الرئيسي عشان ما تظهر في كل صفحىة 
                TempData["msg"] = "s:Data Entered Succsefully";
                //غيرنا توجيه الصفحة للكريت اللي هي الاساس ل اكشن الاندكس بترجع عشان في حال كان عنا ادخال بيانات فيرجع المستخدم لصفحة الكريت 
                return RedirectToAction("Create");
            }

            ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            return View(account);
        }

        // GET: AccountNew/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            return View(account);
        }

        // POST: AccountNew/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FullName,Email,Gender,Active,CountryId,DOB")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "e: Data EDIT Succsefully";
                return RedirectToAction("Index");
            }
            ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            return View(account);
        }

        // GET: AccountNew/Delete/5   => بتفتح صفحة وما بتحذف
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: AccountNew/Delete/5 => بتحذف لكن عليها قيود 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            TempData["msg"] = "d: Data Deleted Succsefully";
            return RedirectToAction("Index");
        }

        //للحذف مباشرة مش محطوط عليه 
        //Post
        public ActionResult DeleteLink(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //get Pagination
        public ActionResult Paging(int id = 1)
        {
            int TotalCount = db.Accounts.Count();//#Count of People of DB جبت اجمالي الصفوف
            int RowsPerPage = 5;//#number of rows per Page قديش بعرض بكل صفحة 
            int PageId = id;//Page ID Assume the Page Number 1
            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);  //يعني اجمالي الصفوف علي عدد الصفحات في كل صفحة وحولها لاكبر عدد صحيح

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;
            int SkipCount = RowsPerPage * (PageId - 1);//انتا اذا في صفحة 2 قديش هطنش 10  

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/AccountNew/Paging";
            #endregion

            var accounts = db.Accounts.Include(a => a.Country)
                .OrderBy(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            return View(accounts.ToList());
        }

        #region Search
        //get Search
        public ActionResult Search(string q="")
        {
            //if (q == null) q = "";//هاد السطر بيتاكد اذا ال q 
            var accounts = db.Accounts.Where(x => x.FullName.Contains(q) || x.Email.Contains(q)).Include(a => a.Country);
            ViewBag.q = q;//هاد بيرسل داتا للفيو عشان يخلي الكلمة ظاهرةاللي بتكون في صندوق البحث
            return View(accounts.ToList());
        }
        #endregion

        public ActionResult SearchAdvanced(bool? active,int? countryid, string gender = "", string q = "")
        {
            var accounts = db.Accounts.Include(a => a.Country)
                .Where(x => (x.FullName.Contains(q) || x.Email.Contains(q)) //ابحث في الايمل او الاسم 
                     && x.Gender.Contains(gender)                           //ابحث عن الجنس 
                     && (active == null || x.Active == active)              //اذا آكتف تساوي نلل او تساوي القيمة المدخلة من المستخدم
                     && (countryid == null || x.CountryId == countryid));  
            
            ViewBag.q = q;
            ViewBag.gender = gender;
            ViewBag.active = active + ""; // في السي شارب اي دالة + سترنق = سترنق
                                          //SelectList=>هي عبارة عن مصدر بيانات للقائمة المنسدلة( 1-بتوخذالجدول 2-وبنتوخذ العمود اللي بيعبر عن القيمة 3-وبتوخذ العمود اللي يعرض القيمة 4-وبتوخذ خذ شو يختار افتراضيا
            ViewBag.CountryList = new SelectList(db.Countries,"ID","Name", countryid); //هلا احنا ما بنقدر نستدعي الكنتري ك عنصر الا كمجموعة لانها مفتاح اجنبي وبتأشر ع جدول ثاني فلازم نستدعيها ب قائمة  ونحط فيها قيم الجدول باسماء الاعمدة 
           


            return View(accounts.ToList());
        }


        public ActionResult SearchPaging(string q = "",int PageId=1)
        {
            int TotalCount = db.Accounts.Count(x => x.FullName.Contains(q) || x.Email.Contains(q));
            int RowsPerPage = 5;
            
            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage); 

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/AccountNew/SearchPaging?q=" + q;
            #endregion

            int SkipCount = RowsPerPage * (PageId - 1);  
            var accounts = db.Accounts.Where(x => x.FullName.Contains(q) || x.Email.Contains(q))
                .Include(a => a.Country).OrderBy(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            return View(accounts.ToList());
        }

        public ActionResult SearchAdvancedPaging(bool? active, int? countryid, string gender = "", string q = "", int PageId = 1)
        {
            #region Paging
            int TotalCount = db.Accounts.Count(x => x.FullName.Contains(q) || x.Email.Contains(q) 
                     && x.Gender.Contains(gender)                           
                     && (active == null || x.Active == active)              
                     && (countryid == null || x.CountryId == countryid));
            int RowsPerPage = 5;

            int PageCount = (int)Math.Ceiling((float)TotalCount / RowsPerPage);

            if (PageId < 1 || PageId > PageCount)
                PageId = 1;

            #region  send data using viewBag to pagination Render
            ViewBag.PageId = PageId;
            ViewBag.PageCount = PageCount;
            ViewBag.BaseURL = "/AccountNew/SearchAdvancedPaging?q=" + q + "&gender=" + gender + "&countryid=" + countryid + "&active=" + active;
            #endregion
            #endregion Paging
            #region Advance Search
           
            int SkipCount = RowsPerPage * (PageId - 1);
            var accounts = db.Accounts.Where(x => (x.FullName.Contains(q) || x.Email.Contains(q))
                && x.Gender.Contains(gender)
                && (active == null || x.Active == active)
                && (countryid == null || x.CountryId == countryid))
                .Include(a => a.Country).OrderBy(x => x.Id).Skip(SkipCount).Take(RowsPerPage);
            ViewBag.q = q;
            ViewBag.gender = gender;
            ViewBag.active = active + "";
            ViewBag.CountryList = new SelectList(db.Countries, "ID", "Name", countryid);
            #endregion Advance Search
            return View(accounts.ToList());
        }

        
    }

}
