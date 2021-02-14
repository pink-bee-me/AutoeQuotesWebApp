using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoQuotesWebApp.Models;

namespace AutoQuotesWebApp.Controllers
{
    public class AutoQuotesController : Controller
    {
        private AutoInsuranceQuoteDbContext db = new AutoInsuranceQuoteDbContext();

        // GET: AutoQuotes
        public ActionResult Index()
        {
            return View(db.AutoQuotes.ToList());
        }

        // GET: AutoQuotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutoQuote autoQuote = db.AutoQuotes.Find(id);
            if (autoQuote == null)
            {
                return HttpNotFound();
            }
            return View(autoQuote);
        }

        // GET: AutoQuotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AutoQuotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoQuoteId,AutoQuoteDateTime,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRate,SubtotalAfterDuiCalc,CoverageTypeRate,SubtotalAfterCoverageTypeCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {
            if (ModelState.IsValid)
            {
                db.AutoQuotes.Add(autoQuote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(autoQuote);
        }

        // GET: AutoQuotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutoQuote autoQuote = db.AutoQuotes.Find(id);
            if (autoQuote == null)
            {
                return HttpNotFound();
            }
            return View(autoQuote);
        }

        // POST: AutoQuotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutoQuoteId,AutoQuoteDateTime,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRate,SubtotalAfterDuiCalc,CoverageTypeRate,SubtotalAfterCoverageTypeCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autoQuote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(autoQuote);
        }

        // GET: AutoQuotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutoQuote autoQuote = db.AutoQuotes.Find(id);
            if (autoQuote == null)
            {
                return HttpNotFound();
            }
            return View(autoQuote);
        }

        // POST: AutoQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AutoQuote autoQuote = db.AutoQuotes.Find(id);
            db.AutoQuotes.Remove(autoQuote);
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
    }
}
