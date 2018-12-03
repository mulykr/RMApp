using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVCApp.Models;

namespace MVCApp.Controllers
{
    public class MyTasksController : Controller
    {
        private readonly MyTaskDbContext _db = new MyTaskDbContext();

        // GET: MyTasks
        [Authorize]
        public ActionResult Index()
        {
            var uid = User.Identity.GetUserId();
            var tasks = (from item in _db.MyTasks where item.CreatorId == uid select item)
                .ToList();
            return View(tasks);
        }

        // GET: MyTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MyTask myTask = _db.MyTasks.Find(id);
            if (myTask == null)
            {
                return HttpNotFound();
            }

            return View(myTask);
        }

        // GET: MyTasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Level,IsDone")] MyTask myTask)
        {
            if (ModelState.IsValid)
            {
                myTask.CreatorId = User.Identity.GetUserId();
                _db.MyTasks.Add(myTask);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myTask);
        }

        // GET: MyTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MyTask myTask = _db.MyTasks.Find(id);
            if (myTask == null)
            {
                return HttpNotFound();
            }

            return View(myTask);
        }

        // POST: MyTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Level,IsDone")] MyTask myTask)
        {
            myTask.CreatorId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                _db.Entry(myTask).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myTask);
        }

        // GET: MyTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MyTask myTask = _db.MyTasks.Find(id);
            if (myTask == null)
            {
                return HttpNotFound();
            }

            return View(myTask);
        }

        // POST: MyTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyTask myTask = _db.MyTasks.Find(id);
            if (myTask != null)
            {
                _db.MyTasks.Remove(myTask);
                _db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
