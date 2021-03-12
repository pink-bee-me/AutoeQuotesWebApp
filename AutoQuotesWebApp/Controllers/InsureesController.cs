using AutoQuotesWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class InsureesController : Controller
    {
        private readonly InsuranceQuoteDBModelsContext db = new InsuranceQuoteDBModelsContext();

        // GET: Insurees
        public ActionResult Index()
        {

            return View(db.Insurees.ToList());
        }
        // GET: Insurees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insurees/Create
        public ActionResult Create()
        {
            Insuree insuree = new Insuree();
            return View(insuree);
        }

        // POST: Insurees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,EmailAddress," +
            "DateOfBirth,AutoYear,AutoMake,AutoModel,SpeedingTickets,DUI," +
            "CoverageType")] Insuree insuree)
        {

            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                List<Insuree> Insurees = new List<Insuree>();
                Insurees.Add(insuree);

            }


            AutoQuote autoQuote = new AutoQuote();

            //set QuoteGenerationDate
            autoQuote.QuoteGenerationDate = DateTime.Now;


            //set AutoQuote InsureeId to match Insuree InsureeId
            //database does that????

            //set BaseRate for AutoQuote
            int baseRate = 50;
            autoQuote.BaseRate = Convert.ToDecimal(baseRate);

            //set AutoQuote Rate values that are based on the DateOfBirth value gathered from the InsureeController "Create" ActionResult
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (insuree.DateOfBirth.Month > DateTime.Now.Month
            || insuree.DateOfBirth.Month == DateTime.Now.Month
            && insuree.DateOfBirth.Day > DateTime.Now.Day)
            {
                age--;
            }

            int under18 = (age < 18) ? 100 : 0;
            int btwn19and25 = ((age > 18) && (age < 26)) ? 50 : 0;
            int over25 = (age > 25) ? 25 : 0;

            autoQuote.AgeUnder18Rate = Convert.ToDecimal(under18);
            autoQuote.AgeBtwn19and25Rate = Convert.ToDecimal(btwn19and25);
            autoQuote.AgeOver25Rate = Convert.ToDecimal(over25);

            //set Insurance Rate Calc Values that are based on the year of the vehicle
            int autoYearBefore2000 = (insuree.AutoYear < 2000) ? 25 : 0;
            int autoYearAfter2015 = (insuree.AutoYear > 2015) ? 25 : 0;
            autoQuote.AutoYearBefore2000Rate = Convert.ToDecimal(autoYearBefore2000);
            autoQuote.AutoYearAfter2015Rate = Convert.ToDecimal(autoYearAfter2015);
            int autoYearBtwn2000and2015 = (insuree.AutoYear < 2000 && insuree.AutoYear > 2015) ? 0 : 0;
            autoQuote.AutoYearBtwn2000and2015Rate = Convert.ToDecimal(autoYearBtwn2000and2015);

            //set Insurance Rate Calc Values based on if the vehicle is a Porsche Carerra
            var yesIsPorsche = (insuree.AutoMake == "Porsche") ? 25 : 0;
            var yesIsCarrera = (insuree.AutoModel == "Carrera") ? 25 : 0;

            autoQuote.IsPorscheRate = Convert.ToDecimal(yesIsPorsche);
            autoQuote.IsCarreraRate = Convert.ToDecimal(yesIsCarrera);

            //set Insurance Rate Calc for the number of speeding tickets the Insuree has on record
            int tickets = insuree.SpeedingTickets;
            int ticketRate = tickets * 10;
            autoQuote.SpeedingTicketsRate = Convert.ToDecimal(ticketRate);

            //set value of subtotal of calculated items prior to dui calculation because to calculate dui rate you take a percentage of the subtotal
            var subtotalBeforeDUI = baseRate + under18 + btwn19and25 + over25 +
                                  +autoYearBefore2000 + autoYearAfter2015 + yesIsPorsche +
                                  yesIsCarrera + ticketRate;

            autoQuote.SubtotalBeforeDuiCalc = Convert.ToDecimal(subtotalBeforeDUI);

            //set dui rate based on if the insuree has had a dui 
            decimal isTrue = 1.00M;
            decimal isFalse = 0.00M;
            decimal yesDUI = (insuree.DUI == true) ? isTrue : isFalse;
            decimal twentyFivePercent = 0.25M;
            decimal duiRate = (yesDUI == isTrue) ? decimal.Multiply(autoQuote.SubtotalBeforeDuiCalc, twentyFivePercent) : isFalse;

            autoQuote.DuiRateUp25Percent = duiRate; // value that will be placed in Quote DuiRateUp25Percent

            //set subtotal after the dui rate is accessed
            autoQuote.SubtotalAfterDuiCalc = Decimal.Add(autoQuote.SubtotalBeforeDuiCalc, autoQuote.DuiRateUp25Percent);

            //figure Insurance Rate Calc based on whether the Insuree needs full coverage insurance or not
            decimal coverageType = (insuree.CoverageType == true) ? isTrue : isFalse;
            decimal fiftyPercent = 0.50M;
            autoQuote.CoverageTypeRateUp50Percent = (coverageType == isTrue) ? decimal.Multiply(autoQuote.SubtotalAfterDuiCalc, fiftyPercent) : isFalse;
            autoQuote.SubtotalAfterCoverageCalc = Decimal.Add(autoQuote.SubtotalAfterDuiCalc, autoQuote.CoverageTypeRateUp50Percent);

            //figure Insurance Rate per Month
            autoQuote.MonthlyQuoteRate = autoQuote.SubtotalAfterCoverageCalc;

            //figure Insurance Rate per Year
            decimal months = 12.00M;
            decimal yearlyQuoteRate = decimal.Multiply(autoQuote.MonthlyQuoteRate, months);
            decimal save20Percent = decimal.Multiply(yearlyQuoteRate, (decimal).20);

            autoQuote.YearlyQuoteRate = decimal.Subtract(autoQuote.YearlyQuoteRate, save20Percent);

            TempData["AutoQuoteData"] = autoQuote;

            return RedirectToAction("Create", "AutoQuotes");


        }

        //Get Insuree Data
        //Send new instance of Insuree's data to be used to figure the calculations to be placed in the AutoQuote to 
        // GET: Insurees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }


        // POST: Insurees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress,DateOfBirth,AutoYear,AutoMake,AutoModel,SpeedingTickets,DUI,CoverageType,MonthlyRate,YearlyRate")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insurees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
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
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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

//    if (insuree.DateOfBirth.Month > DateTime.Now.Month
//    || insuree.DateOfBirth.Month == DateTime.Now.Month
//    && insuree.DateOfBirth.Day > DateTime.Now.Day)
//    {
//        age--;
//    }

//    var insureeAge = Convert.ToInt32(age);
//    double under18 = (insureeAge < 18) ? 100.00 : 0.00;
//    double btwn19and25 = ((insureeAge > 18) && (age <= 25)) ? 50.00 : 0.00;
//    double over25 = (insureeAge > 25) ? 25.00 : 0.00;

//    autoQuote.AgeUnder18Rate = Convert.ToDecimal(under18);
//    autoQuote.AgeBtwn19and25Rate = Convert.ToDecimal(btwn19and25);
//    autoQuote.AgeOver25Rate = Convert.ToDecimal(over25);


//    //figure Insurance Rate Calc Values that are based on the year of the vehicle
//    double autoYearBefore2000 = (insuree.AutoYear < 2000) ? 25.00 : 0.00;
//    double autoYearAfter2015 = (insuree.AutoYear > 2015) ? 25.00 : 0.00;

//    autoQuote.AutoYearBefore2000Rate = Convert.ToDecimal(autoYearBefore2000);
//    autoQuote.AutoYearAfter2015Rate = Convert.ToDecimal(autoYearAfter2015);


//    //figure Insurance Rate Calc Values based on if the vehicle is a Porsche Carerra
//    double yesIsPorsche = (insuree.AutoMake == "Porsche") ? 25.00 : 0.00;
//    double yesIsCarrera = (insuree.AutoModel == "Carrera") ? 25.00 : 0.00;

//    autoQuote.IsPorscheRate = Convert.ToDecimal(yesIsPorsche);
//    autoQuote.IsCarreraRate = Convert.ToDecimal(yesIsCarrera);



//    //figure Insurance Rate Calc for the number of speeding tickets the Insuree has on record
//    int speedingTickets = insuree.SpeedingTickets;
//    double speedingTicketsRate = speedingTickets * 10.00;

//    autoQuote.SpeedingTicketsRate = Convert.ToDecimal(speedingTicketsRate);

//    //Calculate Subtotal to check for accuracy before DUI calculation
//    double subtotalBeforeDUI = baseRate + under18 + btwn19and25 + over25 +
//                               autoYearBefore2000 + autoYearAfter2015 + yesIsPorsche +
//                               yesIsCarrera + speedingTicketsRate;
//    autoQuote.SubtotalBeforeDuiCalc = Convert.ToDecimal(subtotalBeforeDUI);


//    //figure Insurance Rate Calc based on if the Insuree has a DUI on their record
//    int yesDUI = (insuree.DUI == true) ? 1 : 0;
//    double duiRate = (yesDUI == 1) ? (subtotalBeforeDUI * .025) : 0.00;

//    var DuiRate = Convert.ToDecimal(duiRate);
//    autoQuote.DuiRateUp25Percent = DuiRate; // value that will be placed in Quote DuiRateUp25Percent


//    //figure Insurance Rate Calc after DUI Rate is computed and added to running sum of rates 

//    autoQuote.SubtotalAfterDuiCalc = Convert.ToDecimal(subtotalBeforeDUI + duiRate);


//    //figure Insurance Rate Calc based on whether the Insuree needs full coverage insurance or not
//    int coverageType = (insuree.CoverageType == true) ? 1 : 0;
//    double coverageTypeRate = (coverageType == 1) ? (Convert.ToDouble(autoQuote.SubtotalAfterDuiCalc) * 0.50) : 0.00;// calculating the rate of increase if FullCoverage is true
//    autoQuote.CoverageTypeRateUp50Percent = Convert.ToDecimal(coverageTypeRate);
//    autoQuote.SubtotalAfterCoverageTypeCalc = autoQuote.SubtotalAfterDuiCalc + autoQuote.CoverageTypeRateUp50Percent;


//    //figure Insurance Rate per Month
//    autoQuote.MonthlyQuoteRate = autoQuote.SubtotalAfterCoverageTypeCalc;

//    //figure Insurance Rate per Year
//    autoQuote.YearlyQuoteRate = autoQuote.MonthlyQuoteRate * 12;
//    insuree.MonthlyRate = autoQuote.MonthlyQuoteRate;
//    insuree.YearlyRate = autoQuote.YearlyQuoteRate;
//    return View(autoQuote);







