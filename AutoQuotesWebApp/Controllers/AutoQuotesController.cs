using AutoQuotesWebApp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class AutoQuotesController : Controller
    {
        public AutoQuotesDBEntities autoQuotesDB = new AutoQuotesDBEntities();

        // GET: AutoQuotes
        public ActionResult Index()
        {
            return View(autoQuotesDB.AutoQuotes.ToList());
        }

        // GET: AutoQuotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AutoQuote autoQuote = autoQuotesDB.AutoQuotes.Find(id);
            if (autoQuote == null)
            {
                return HttpNotFound();
            }
            return View(autoQuote);
        }

        // GET: AutoQuotes/CalculateAutoQuote
        public ActionResult CalculateAutoQuote()
        {
            //create instance of insuree and then give it the values of the newly created insuree from the insurees
            //controller that were passed via TempData["InfoInsureeID"] (the newly created insuree id#) after converting that data to Int32 type
            // this is done so we can use the isurees info to calculate the autoQuote
            Insuree insuree = (Insuree)TempData["insuree"];

            //create a new autoQuote instance
            AutoQuote autoQuote = new AutoQuote();

            //set QuoteGenerationDate
            autoQuote.QuoteGenerationDate = DateTime.Now;

            // figure BaseRate for AutoQuote Insurance Rate Calculation
            double baseRate = 50.00;
            autoQuote.BaseRate = Convert.ToDecimal(baseRate);

            //figure AutoQuote Rate values that are based on the DateOfBirth value gathered from the InsureeController "Create" ActionResult
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (insuree.DateOfBirth.Month > DateTime.Now.Month
            || insuree.DateOfBirth.Month == DateTime.Now.Month
            && insuree.DateOfBirth.Day > DateTime.Now.Day)
            {
                age--;
            }
            var insureeAge = Convert.ToInt32(age);
            double under18 = (insureeAge < 18) ? 100.00 : 0.00;
            double btwn19and25 = ((insureeAge > 18) && (age <= 25)) ? 50.00 : 0.00;
            double over25 = (insureeAge > 25) ? 25.00 : 0.00;

            autoQuote.AgeUnder18Rate = Convert.ToDecimal(under18);
            autoQuote.AgeBtwn19and25Rate = Convert.ToDecimal(btwn19and25);
            autoQuote.AgeOver25Rate = Convert.ToDecimal(over25);


            //figure Insurance Rate Calc Values that are based on the year of the vehicle
            double autoYearBefore2000 = (insuree.AutoYear < 2000) ? 25.00 : 0.00;
            double autoYearAfter2015 = (insuree.AutoYear > 2015) ? 25.00 : 0.00;

            autoQuote.AutoYearBefore2000Rate = Convert.ToDecimal(autoYearBefore2000);
            autoQuote.AutoYearAfter2015Rate = Convert.ToDecimal(autoYearAfter2015);


            //figure Insurance Rate Calc Values based on if the vehicle is a Porsche Carerra
            double yesIsPorsche = (insuree.AutoMake == "Porsche") ? 25.00 : 0.00;
            double yesIsCarrera = (insuree.AutoModel == "Carrera") ? 25.00 : 0.00;

            autoQuote.IsPorscheRate = Convert.ToDecimal(yesIsPorsche);
            autoQuote.IsCarreraRate = Convert.ToDecimal(yesIsCarrera);



            //figure Insurance Rate Calc for the number of speeding tickets the Insuree has on record
            int speedingTickets = insuree.SpeedingTickets;
            double speedingTicketsRate = Convert.ToDouble(speedingTickets * 10);

            autoQuote.SpeedingTicketsRate = Convert.ToDecimal(speedingTicketsRate);

            //Calculate Subtotal to check for accuracy before DUI calculation
            double subtotalBeforeDUI = baseRate + under18 + btwn19and25 + over25 +
                                       autoYearBefore2000 + autoYearAfter2015 + yesIsPorsche +
                                       yesIsCarrera + speedingTicketsRate;
            autoQuote.SubtotalBeforeDuiCalc = Convert.ToDecimal(subtotalBeforeDUI);



            //figure Insurance Rate Calc based on if the Insuree has a DUI on their record
            int yesDUI = (insuree.DUI == true) ? 1 : 0;
            double twentyFivePercent = 0.25;
            decimal TwentyFivePercent = Convert.ToDecimal(twentyFivePercent);
            decimal duiRate = (yesDUI == 1) ? (decimal.Multiply(autoQuote.SubtotalBeforeDuiCalc, TwentyFivePercent)) : Convert.ToDecimal(0.00);


            autoQuote.DuiRateUp25Percent = duiRate; // value that will be placed in Quote DuiRateUp25Percent


            //figure Insurance Rate Calc after DUI Rate is computed and added to running sum of rates 

            autoQuote.SubtotalAfterDuiCalc = autoQuote.SubtotalBeforeDuiCalc + autoQuote.DuiRateUp25Percent;


            //figure Insurance Rate Calc based on whether the Insuree needs full coverage insurance or not
            int coverageType = (insuree.CoverageType == true) ? 1 : 0;
            double fiftyPercent = 0.50;
            decimal FiftyPercent = Convert.ToDecimal(fiftyPercent);
            autoQuote.CoverageTypeRateUp50Percent = (coverageType == 1) ? (decimal.Multiply(autoQuote.SubtotalAfterDuiCalc, FiftyPercent)) : Convert.ToDecimal(0.00);// calculating the rate of increase if FullCoverage is true
            autoQuote.SubtotalAfterCoverageCalc = autoQuote.SubtotalAfterDuiCalc + autoQuote.CoverageTypeRateUp50Percent;

            //figure Insurance Rate per Month
            autoQuote.MonthlyQuoteRate = autoQuote.SubtotalAfterCoverageCalc;

            //figure Insurance Rate per Year
            autoQuote.YearlyQuoteRate = decimal.Multiply(autoQuote.MonthlyQuoteRate, Convert.ToDecimal(12));

            // return autoQuote To View
            return View(autoQuote);
        }

        // POST: AutoQuotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutoQuoteId,AutoQuoteDateTime,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent,SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageTypeCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {
            if (ModelState.IsValid)
            {
                autoQuotesDB.AutoQuotes.Add(autoQuote);
                autoQuotesDB.SaveChanges();
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
            AutoQuote autoQuote = autoQuotesDB.AutoQuotes.Find(id);
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
                autoQuotesDB.Entry(autoQuote).State = EntityState.Modified;
                autoQuotesDB.SaveChanges();
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
            AutoQuote autoQuote = autoQuotesDB.AutoQuotes.Find(id);
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
            AutoQuote autoQuote = autoQuotesDB.AutoQuotes.Find(id);
            autoQuotesDB.AutoQuotes.Remove(autoQuote);
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

