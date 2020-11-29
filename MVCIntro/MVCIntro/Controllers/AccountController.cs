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
    public class AccountController : Controller
    {
        private IRPALG1Entities1 db = new IRPALG1Entities1();

        // GET: Account
        public ActionResult Index()
        {
            int[] nums = new int[] { 20, 30, 40, 50, 60, 702, 90 };
            var numsTop50 = nums.Where(x => x > 50);


            //select * from account left join country on country.id=Account.CountryId
            var accounts = db.Accounts.Include(a => a.Country);
            //var accounts = db.Accounts;

            //var accounts = db.Accounts.Include(a => a.Country)
            //    .Where(x => x.Active == true);

            //var accounts = db.Accounts.Include(a => a.Country)
            //    .Where(x => x.Active == true && x.Gender=="M");

            /*var accounts = db.Accounts.Include(a => a.Country)
                //.OrderByDescending(x=>x.Gender);
                .OrderBy(x => x.Gender);*/


            //var accounts = db.Accounts.Include(a => a.Country)
            //.OrderBy(x => x.Gender).Skip(4).Take(4);
            //الريتيرن بيرجع مجموعة 
            return View(accounts.ToList());

        }

        // GET: Account/Details/5
        /*id 
         ممنوع تغيرها لانو هتغير ال 
         id
         اللي في الرواتنق */
        /*اشارة الاستفهام يعني 
         * id
         * يمكن تيجي ويمكن لا */
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //************//هنستخدم هان بس الدالة 1 //***************
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            // الريتيرن هان بترجع قيمةوحدة 
            return View(account);

        }
            /*****************1**************************/
            /*Find(id) => Only Work With Primary Key
            ....................1..................
            //Account account = db.Accounts.Find(id);
            /*if (account == null)
            {
                return HttpNotFound();
            }
            ...................1.......................
            /******************************************/

            /*****************2*****************************/
            /*FirstOrDefault 
             يعني هات اول واحد ان وجد وان لم يوجد رجع نلل 
             لكن لو حطينا في الرابط رقم مش موجود في الدتا بيز هيرجع صفحة انه هاذ الرقم اللي 
             في الرابط مش موجود مثلا 
             http://localhost:50196/Account/Account/Details/10000
             مش هيعطيك
             bad Rquest 
             هيعطيني 
             Not Found 
             ......................2...........
            Account account = db.Accounts.Where(x => x.Id == id).FirstOrDefault();
             if (account == null)
            {
                return HttpNotFound();
            }
            ......................2............
            /**************************************/

            /**********************3***********************************/
            /*Single() 
             *لازم يرجع  وان لم يوجد وقف لا تكمل بيعطيني خطأ 
             * هاد الدالة بتنفع للمبرمج ازا بدو يعرف وين الخط فمثلا اذا حطيت في الرابط
             * http://localhost:50196/Account/Account/Details/10000
             * هيطلع رسالة خطأ فيي المتصفح التالي 
             * خطأ في الخادم في التطبيق '/'.
             * 85السطر:              86:             Account account = db.Accounts.Where(x => x.Id == id).Single(); */
            /*
             .............3.............
             Account account = db.Accounts.Where(x => x.Id == id).Single();
              if (account == null)
           {
               return HttpNotFound();
           }
           ..............3..........
           */

            /**********************************************************************/


        

      

        // GET: Account/Create ==> هاد اللي عرضتلي صفحة ال
        public ActionResult Create()
        {                          /**  SelectList()
                                         *تمثل مصدرالبيانات  لل
                                           Drop Down List ==  (db.Countries, "Id", "Name")
                                          هاد الدالة اضافية في الكريت بس عشان
                                            الحقل تاع رقم الدولة اللي هوا اساسا مفتاح اجنبي *
                                             من جدول الدول  
                                               وهاد الدالة بس بتبين لما تبحث في مصدر الصفحة في المتصفح بتبين عنا قوتها */
             ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name");
            // ViewBag.CountryID = new SelectList(db.Countries.Where(x => x.Id < 4), "ID", "Name");
            
            return View();
        }

        // POST: Account/Create ==> هاد الصفحة اللي حملت اللصفحة تاعت الانشاء بعد تعبئتها بالبيانات  
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
         [ValidateAntiForgeryToken]
                                       /*[Bind(Include = "Id,FullName,Email,Gender,Active,CountryId,DOB")]
                                         هاد الدالة يعني انتا مجبر تدخل الحقول اللي جوا الانكلود لذلك ممكن نستغني عنها
                                           بيصير مجبر يدخل كل الحقول
                                         وبذلك بيصير مجبر يدخل كل الحقول اللي في الانكلود
                                         */
        public ActionResult Create(/*[Bind(Include = "Id,FullName,Email,Gender,Active,CountryId,DOB")]*/ Account account)
        {
            // sever side validtion 
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            /*
             *   هاد الكود بيرجعنا لصفحة للفيو اللي في صفحة الاكوانت اللي هو الاندكس وعشان نختصر ع حالنا بنرجعو لصحفة الكريت 
            ViewBag.CountryId = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            return View(account);
            هتلاقي نفس الكود اللي في فوقفي اكشن الكريت */
            /*******************/
            // هين بيرجعنا لصفحة الكريت 
            //والكودين صح لكن الثاني اختصار 
            
            return Create();
        }

        // GET: Account/Edit/5
        //نقس كود ال
        //Detalies
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
            /*غيرنا ال
           ViewBag.CountryId
           ل ViewBag.CountryList
           وهذا لانه هيتلخبط بينها وبين تاعت الانشاء فيفضي الداتا اللي في رقم الدولة 
           عشان هيك بنغيرها لل 
           CountryList
           لان المستخدم احيانا بكون بدو يغير معلومات ثانية غير رقم الدولة مثلا بدو يعدل ع الاسم وهيلاقي انو الدولة طارت
           فعشان يحتفظ باسم الدولة ويكون للمستخدم القدرة انو لو بدو يغيرها بيغيرها لو لا هوا حر*/

            ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            return View(account);

        }

        // POST: Account/Edit/5
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
                return RedirectToAction("Index");
            }
            
            //ViewBag.CountryList = new SelectList(db.Countries, "Id", "Name", account.CountryId);
            //return View(account);
            /************بس اختصار للكود اللي فوق**************/
            return Edit(account.Id);
        }

        // GET: Account/Delete/5
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

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {                               //Find(id)
                                        //بيجيبو من القاعدة بعدين بينفذ عليه امر الحذف
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //هاد بيطير الداتا بيز من المموري اذا خلص الكنترولر شغلو عليها 
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
