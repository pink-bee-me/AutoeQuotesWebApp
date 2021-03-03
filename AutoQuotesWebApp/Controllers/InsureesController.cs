using AutoQuotesWebApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class InsureesController : Controller
    {
        private AutoQuotesDBEntities autoQuotesDB = new AutoQuotesDBEntities();

        // GET: Insurees
        public ActionResult Index()
        {
            return View(autoQuotesDB.Insurees.ToList());
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
            var insuree = new Insuree();

            return View("Create", insuree);
        }

        // POST: Insurees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress," +
            "DateOfBirth,AutoYear,AutoMake,AutoModel,SpeedingTickets,DUI," +
            "CoverageType")] Insuree insuree)
        {

            if (ModelState.IsValid)
            {

                autoQuotesDB.Insurees.Add(insuree);
                autoQuotesDB.SaveChanges();
                TempData["insuree"] = insuree;


            }
            return RedirectToAction("CalculateAutoQuote", "AutoQuotes");
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
                autoQuotesDB.Entry(insuree).State = EntityState.Modified;
                autoQuotesDB.SaveChanges();
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


        //public ActionResult CalculateAutoQuote(int? id)
        //{
        //    using (AutoQuotesDBEntities1 db = new AutoQuotesDBEntities1())
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //    Insuree insuree = db.Insurees.Find(id);
        //    if (insuree == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(insuree);

        //    insuree = new Insuree();
        //    //create a new autoQuote instance
        //    var autoQuote = new AutoQuote(insuree);


        //    // figure BaseRate for AutoQuote Insurance Rate Calculation
        //    double baseRate = 50.00;
        //    autoQuote.BaseRate = Convert.ToDecimal(baseRate);


        //    //figure Insurance Rate Calc values that are based on Age of Insuree
        //    int age = DateTime.Now.Year - insuree.DateOfBirth.Year;

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
    }
}




