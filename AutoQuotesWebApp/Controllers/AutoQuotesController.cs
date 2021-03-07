using AutoQuotesWebApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class AutoQuotesController : Controller
    {
        public InsuranceQuoteDBModelsContext insuranceQuoteDB = new InsuranceQuoteDBModelsContext();

        // GET: AutoQuotes
        public ActionResult Index()
        {
            return View(insuranceQuoteDB.AutoQuotes.ToList());
        }

        // GET: AutoQuotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutoQuote autoQuote = insuranceQuoteDB.AutoQuotes.Find(id);
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
        public ActionResult Create([Bind(Include = "QuoteGenerationDate,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent,SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {
            if (ModelState.IsValid)
            {
                insuranceQuoteDB.AutoQuotes.Add(autoQuote);
                insuranceQuoteDB.SaveChanges();
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
            AutoQuote autoQuote = insuranceQuoteDB.AutoQuotes.Find(id);
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
        public ActionResult Edit([Bind(Include = "AutoQuoteId,AutoQuoteDateTime,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent,SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageTypeCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {
            if (ModelState.IsValid)
            {
                insuranceQuoteDB.Entry(autoQuote).State = EntityState.Modified;
                insuranceQuoteDB.SaveChanges();
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
            AutoQuote autoQuote = insuranceQuoteDB.AutoQuotes.Find(id);
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
            AutoQuote autoQuote = insuranceQuoteDB.AutoQuotes.Find(id);
            insuranceQuoteDB.AutoQuotes.Remove(autoQuote);
            insuranceQuoteDB.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                insuranceQuoteDB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

