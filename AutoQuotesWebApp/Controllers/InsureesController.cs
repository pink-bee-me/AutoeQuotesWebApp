using AutoQuotesWebApp.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace InsuranceQuoteGenerator.Controllers
{
    public class InsureesController : Controller

    {
        //gain access to the AutoInsuranceQuoteDbContext (Insurees Table and AutoQuotes Table in Database

        private readonly AutoInsuranceQuoteDbContext db = new AutoInsuranceQuoteDbContext();

        // ActionResult Index() returns the InsureeController Index View (/Views/Insuree/Index.cshtml)//
        // This Index View Displays a list of all the current Insurees stored in the Insurance Database//
        //(you can see this by looking at the parameters that are passed to the view)//

        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }


        //After Clicking Button for Free Quote on Landing Page (which points to NewInsureeDataForm Controller),
        //the user is sent to the NewInsureeDataForm View (NewInsureeDataForm.cshtml) which is the form that 
        //creates an instance of NewInsureeDataForm Model( ultimately, the Insuree Model)
        // URL:/Controllers/InsureeController/NewInsureeFormData

        //GET:Insuree/Create
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress," +
            "DateOfBirth,AutoYear,AutoMake,AutoModel,SpeedingTickets,DUI,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                if (insuree != null)
                {
                    insuree = new Insuree();
                    db.Insurees.Add(insuree);
                    db.SaveChanges();
                    ViewBag.Data = insuree;
                    return View("PostYesSuccess", "Insurees");
                }
                return View("PostNoSuccess", "Insurees");
            }
        }


        public ActionResult CalculateQuote()
        {
            using (AutoInsuranceQuoteDbContext db = new AutoInsuranceQuoteDbContext())



                //create a new autoQuote instance
                var autoQuote = new AutoQuote(insuree);


            // figure BaseRate for AutoQuote Insurance Rate Calculation
            double baseRate = 50.00;
            autoQuote.BaseRate = Convert.ToDecimal(baseRate);


            //figure Insurance Rate Calc values that are based on Age of Insuree
            var age = DateTime.Now.Year - insuree.DateOfBirth.Year;

            if (insuree.DateOfBirth.Month > DateTime.Now.Month
            || insuree.DateOfBirth.Month == DateTime.Now.Month
            && insuree.DateOfBirth.Day > DateTime.Now.Day)
                age--;

            var insureeAge = Convert.ToInt32(age);
            double under18 = (insureeAge < 18) ? 100.00 : 0.00;
            double btw19and25 = ((insureeAge > 18) && (age <= 25)) ? 50.00 : 0.00;
            double over25 = (insureeAge > 25) ? 25.00 : 0.00;

            autoQuote.AgeUnder18 = Convert.ToDecimal(under18);
            autoQuote.AgeBtw19and25 = Convert.ToDecimal(btw19and25);
            autoQuote.Age26andUp = Convert.ToDecimal(over25);


            //figure Insurance Rate Calc Values that are based on the year of the vehicle
            double autoYearPrior2000 = (insuree.CarYear < 2000) ? 25.00 : 0.00;
            double autoYearAfter2015 = (insuree.CarYear > 2015) ? 25.00 : 0.00;

            autoQuote.AutoYearBefore2000 = Convert.ToDecimal(autoYearPrior2000);
            autoQuote.AutoYearAfter2015 = Convert.ToDecimal(autoYearAfter2015);


            //figure Insurance Rate Calc Values based on if the vehicle is a Porsche Carerra
            double yesIsPorsche = (insuree.CarMake == "Porsche") ? 25.00 : 0.00;
            double yesIsCarrera = (insuree.CarModel == "Carrera") ? 25.00 : 0.00;

            autoQuote.IsPorsche = Convert.ToDecimal(yesIsPorsche);
            autoQuote.IsCarrera = Convert.ToDecimal(yesIsCarrera);


            //Calculate Subtotal to check for accuracy before DUI calculation
            double subtotalBeforeDUI = baseRate + under18 + btw19and25 + over25 +
                                       autoYearPrior2000 + autoYearAfter2015 + yesIsPorsche +
                                       yesIsCarrera;
            autoQuote.SubTotalBeforeDuiCalc = Convert.ToDecimal(subtotalBeforeDUI);


            //figure Insurance Rate Calc based on if the Insuree has a DUI on their record
            int yesDUI = (insuree.DUI == true) ? 1 : 0;
            double duiRate = (yesDUI == 1) ? (subtotalBeforeDUI * .025) : 0.00;

            var DuiRate = Convert.ToDecimal(duiRate);
            autoQuote.DuiRateUp25Percent = DuiRate; // value that will be placed in Quote DUIRateUP25Percent


            //figure Insurance Rate Calc after DUI Rate is computed and added to running sum of rates 

            autoQuote.SubTotalAfterDuiCalc = Convert.ToDecimal(subtotalBeforeDUI + duiRate);


            //figure Insurance Rate Calc for the number of speeding tickets the Insuree has on record
            int speedingTickets = insuree.SpeedingTickets;
            double speedingTicketsRate = speedingTickets * 10.00;

            autoQuote.SpeedingTicketsRate = Convert.ToDecimal(speedingTicketsRate);


            //figure subtotal of all figured rates after speeding tickets calculation(before Coverage Rate calc)
            autoQuote.SubTotalBeforeCoverageCalc = autoQuote.SubTotalAfterDuiCalc + autoQuote.SpeedingTicketsRate;


            //figure Insurance Rate Calc based on whether the Insuree needs full coverage insurance or not
            int coverageType = (insuree.CoverageType == true) ? 1 : 0;
            double coverageTypeRate = (coverageType == 1) ? (Convert.ToDouble(autoQuote.SubTotalBeforeCoverageCalc) * 0.50) : 0.00;// calculating the rate of increase if FullCoverage is true

            autoQuote.SubTotalAfterCoverageCalc = autoQuote.SubTotalBeforeCoverageCalc + (Convert.ToDecimal(coverageTypeRate));


            //figure Insurance Rate per Month
            autoQuote.MonthlyRate = autoQuote.SubTotalAfterCoverageCalc;

            //figure Insurance Rate per Year
            autoQuote.YearlyRate = autoQuote.SubTotalAfterCoverageCalc * 12;
            return RedirectToRoute("Index", "AutoQuotes");
        }
        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }



        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress,DateOfBirth,AutoYear,AutoMake,AutoModel,SpeedingTickets,DUI,CoverageType,AutoQuoteId")] Insuree model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
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

//using CarInsuranceMVC.Models;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web.Mvc;

//namespace CarInsuranceMVC.Controllers
//{
//    public class InsureeController : Controller
//    {
//        // GET: Insuree/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Insuree insuree = db.Insurees.Find(id);
//            if (insuree == null)
//            {
//                return HttpNotFound();
//            }
//            return View(insuree);
//        }
//        // Make the form To enter New Insuree 888User inputs data on this newly created form to submit***///
//        public ActionResult InsureeForm()
//        {
//            return View();
//        }

//        // POST: Insuree/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,QuoteMonthly,QuoteYearly")] Insuree insuree)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Insurees.Add(insuree);
//                db.SaveChanges();
//            }
//            return RedirectToAction("Index", "Insuree");
//        }

//        public ActionResult Edit(int id)
//        {
//            var insuree = _context.Insurees.SingleOrDefault(c => c.Id == id);

//        }

//         // Get The Id of the newly entered Insuree and return in the variable "newEntryId" //
//        public ActionResult GetNewInsureeId(int id)
//        {
//            var newEntryId = id;
//            return Content("id= " + newEntryId);
//        }

//        // GET: Insuree/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Insuree insuree = db.Insurees.Find(id);
//            if (insuree == null)
//            {
//                return HttpNotFound();
//            }
//            return View(insuree);
//        }

//        // POST: Insuree/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
//        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,QuoteMonthly,QuoteYearly")] Insuree insuree)
//        {
//            if (ModelState.IsValid)
//            {
//                db.Entry(insuree).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(insuree);
//        }

//        // GET: Insuree/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Insuree insuree = db.Insurees.Find(id);
//            if (insuree == null)
//            {
//                return HttpNotFound();
//            }
//            return View(insuree);
//        }

//        // POST: Insuree/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Insuree insuree = db.Insurees.Find(id);
//            db.Insurees.Remove(insuree);
//            db.SaveChanges();
//            return RedirectToAction("Index", "Insuree");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }



//    }