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

        //GET: Insuree/NewInsureeFormData
        [HttpGet]
        public ViewResult NewInsureeFormData()
        {
            var insuree = new Insuree();
            return View(insuree);
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress,DateOfBirth,VehicleYear,VehicleMake,VehicleModel,DUI,SpeedingTickets,CoverageType,MonthlyQuoteRate,YearlyQuoteRate")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                ViewBag.Message = insuree;
                return View("PostYesSuccess");
            }

            return View("PostNoSuccess");
        }

        public void CalculateQuote()
        {

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
        public ActionResult Edit([Bind(Include = "InsureeId,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,AutoQuoteId,MonthlyQuoteRate,YearlyQuoteRate")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
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