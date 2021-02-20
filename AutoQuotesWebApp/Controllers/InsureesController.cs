using AutoQuotesWebApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class InsureesController : Controller
    {
        AutoQuotesDBEntities1 autoQuotesDB = new AutoQuotesDBEntities1();

        // GET: Insurees
        public ActionResult Index()
        {
            var insurees = autoQuotesDB.Insurees.ToList();
            return View(insurees);
        }

        // GET: Insurees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = autoQuotesDB.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insurees/Create
        public ActionResult Create()
        {
            var model = new Insuree();
            return View(model);
        }

        // POST: Insurees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InsureeId,AutoId,FirstName,LastName,EmailAddress,DateOfBirth,DUI,SpeedingTickets")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                autoQuotesDB.Insurees.Add(insuree);
                autoQuotesDB.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InsureeIdInfo = new SelectList(autoQuotesDB.Insurees, "InsureeId", "FirstName", insuree.InsureeId);

            return View(insuree);
        }

        // GET: Insurees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = autoQuotesDB.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            ViewBag.InsureeId = new SelectList(autoQuotesDB.Insurees, "InsureeId", "FirstName", insuree.InsureeId);
            ViewBag.InsureeId = new SelectList(autoQuotesDB.Insurees, "InsureeId", "FirstName", insuree.InsureeId);
            return View(insuree);
        }

        // POST: Insurees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InsureeId,AutoId,FirstName,LastName,EmailAddress,DateOfBirth,DUI,SpeedingTickets")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                autoQuotesDB.Entry(insuree).State = EntityState.Modified;
                autoQuotesDB.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InsureeId = new SelectList(autoQuotesDB.Insurees, "InsureeId", "FirstName", insuree.InsureeId);
            ViewBag.InsureeId = new SelectList(autoQuotesDB.Insurees, "InsureeId", "FirstName", insuree.InsureeId);
            return View(insuree);
        }

        // GET: Insurees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = autoQuotesDB.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insurees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = autoQuotesDB.Insurees.Find(id);
            autoQuotesDB.Insurees.Remove(insuree);
            autoQuotesDB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                autoQuotesDB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
