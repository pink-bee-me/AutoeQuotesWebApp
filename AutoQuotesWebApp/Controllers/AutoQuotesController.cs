using AutoQuotesWebApp.Models;
using AutoQuotesWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AutoQuotesWebApp.Controllers
{
    public class AutoQuotesController : Controller
    {
        public InsuranceQuoteDBModelsContext db = new InsuranceQuoteDBModelsContext();

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

        //We are "Getting" the info necessary to compute the autoQuote from the infor that was gathered 
        //from the user in the InsureesController.
        //Technically this is not ypour standard get action, but while it is not a true HTTP Get Request
        //....it does "get The info to the conttroller -- so that we can create a quote for the user.
        public ActionResult Create()
        {
            var insuree = new Insuree();

            if (TempData.ContainsKey("InsureeId"))
            {
                insuree.InsureeId = Convert.ToInt32(TempData["InsureeId"]);

            }

            insuree = db.Insurees.Find(insuree.InsureeId);
            var autoQuote = new AutoQuote();

            autoQuote.QuoteGenerationDate = DateTime.Now;

            autoQuote.InsureeId = insuree.InsureeId;
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
            if (insuree.DUI == true)
            {
               autoQuote.DuiRateUp25Percent = decimal.Multiply(autoQuote.SubtotalBeforeDuiCalc, 0.25M);
            }
         
         else
            {
                autoQuote.DuiRateUp25Percent = decimal.Multiply(autoQuote.SubtotalBeforeDuiCalc, 1.00M);
            }


            //set subtotal after the dui rate is accessed
            autoQuote.SubtotalAfterDuiCalc = decimal.Add(autoQuote.SubtotalBeforeDuiCalc, autoQuote.DuiRateUp25Percent);

            //figure Insurance Rate Calc based on whether the Insuree needs full coverage insurance or not
           
           if (insuree.CoverageType == true)
            {
                autoQuote.CoverageTypeRateUp50Percent = decimal.Multiply(autoQuote.SubtotalAfterDuiCalc, 0.50M);
            }
            else
            {
                autoQuote.CoverageTypeRateUp50Percent = decimal.Multiply(autoQuote.SubtotalAfterDuiCalc, 1.00M);
            } 

            autoQuote.SubtotalAfterCoverageCalc = decimal.Add(autoQuote.SubtotalAfterDuiCalc, autoQuote.CoverageTypeRateUp50Percent);

            //figure Insurance Rate per Month
            autoQuote.MonthlyQuoteRate = autoQuote.SubtotalAfterCoverageCalc;

            //figure Insurance Rate per Year
            decimal months = 12.00M;
            autoQuote.YearlyQuoteRate = decimal.Multiply(autoQuote.MonthlyQuoteRate, months);
           
            List<QuoteItemizationVM> QuoteVMs = new List<QuoteItemizationVM>();
            var quoteVM = new QuoteItemizationVM(insuree, autoQuote);
            //quoteVM.InsureeId = insuree.InsureeId;
            //quoteVM.FirstName = insuree.FirstName;
            //quoteVM.LastName = insuree.LastName;
            //quoteVM.EmailAddress = insuree.EmailAddress;
            //quoteVM.DateOfBirth = insuree.DateOfBirth;
            //quoteVM.AutoYear = insuree.AutoYear;
            //quoteVM.AutoMake = insuree.AutoMake;
            //quoteVM.AutoModel = insuree.AutoModel;
            //quoteVM.SpeedingTickets = insuree.SpeedingTickets;
            //quoteVM.DUI = insuree.DUI;
            //quoteVM.CoverageType = insuree.CoverageType;

            //quoteVM.QuoteGenerationDate = autoQuote.QuoteGenerationDate;
            //quoteVM.BaseRate = autoQuote.BaseRate;
            //quoteVM.AgeUnder18Rate = autoQuote.AgeUnder18Rate;
            //quoteVM.AgeBtwn19and25Rate = autoQuote.AgeBtwn19and25Rate;
            //quoteVM.AgeOver25Rate = autoQuote.AgeOver25Rate;
            //quoteVM.AutoYearBefore2000Rate = autoQuote.AutoYearBefore2000Rate;
            //quoteVM.AutoYearBtwn2000and2015Rate = autoQuote.AutoYearBtwn2000and2015Rate;
            //quoteVM.AutoYearAfter2015Rate = autoQuote.AutoYearAfter2015Rate;
            //quoteVM.IsPorscheRate = autoQuote.IsPorscheRate;
            //quoteVM.IsCarreraRate = autoQuote.IsCarreraRate;
            //quoteVM.SpeedingTicketsRate = autoQuote.SpeedingTicketsRate;
            //quoteVM.SubtotalBeforeDuiCalc = autoQuote.SubtotalBeforeDuiCalc;
            //quoteVM.DuiRateUp25Percent = autoQuote.DuiRateUp25Percent;
            //quoteVM.SubtotalAfterDuiCalc = autoQuote.SubtotalAfterDuiCalc;
            //quoteVM.CoverageTypeRateUp50Percent = autoQuote.CoverageTypeRateUp50Percent;
            //quoteVM.MonthlyQuoteRate = autoQuote.MonthlyQuoteRate;
            //quoteVM.YearlyQuoteRate = autoQuote.YearlyQuoteRate;

            return View(quoteVM);


        }


        // POST: AutoQuotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteGenerationDate,InsureeId,BaseRate,AgeUnder18Rate," +
                                   "AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate," +
                                   "AutoYearBtwn2000and2015Rate,AutoYearAfter2015Rate,IsPorscheRate," +
                                   "IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent," +
                                   "SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageCalc," +
                                   "MonthlyQuoteRate,YearlyQuoteRate")] QuoteItemizationVM quoteVM)
        {


            if (ModelState.IsValid)
            {
                var autoQuote = new AutoQuote(quoteVM);
                db.AutoQuotes.Add(autoQuote);
                db.SaveChanges();

                var id = autoQuote.AutoQuoteId;
                List<AutoQuote> AutoQuotes = new List<AutoQuote>();
                autoQuote = db.AutoQuotes.Find(id);
                AutoQuotes.Add(autoQuote);
            }
            return View("FinalAutoQuoteCreation");
        }


        //public ActionResult QuoteItemization(InsureeVM id)
        //{
        //    List<InsureeVM> Insurees = new List<InsureeVM>();
        //    InsureeVM insuree = new InsureeVM();
        //    //var id = db.Insurees.Max(item => insuree.InsureeId);
        //    insuree = db.Insurees.Find(id);

        //    List<AutoQuote> AutoQuotes = new List<AutoQuote>();
        //    AutoQuote autoQuote = new AutoQuote();
        //    //var id = db.AutoQuotes.Max(item => autoQuote.AutoQuoteId);
        //    autoQuote = db.AutoQuotes.Find(id);

        //    List<QuoteItemizationVM> VmModels = new List<QuoteItemizationVM>();
        //    {
        //        foreach (InsureeVM insureeForQuote in Insurees)
        //        {
        //            var vmModel = new QuoteItemizationVM();

        //            vmModel.InsureeId = insureeForQuote.InsureeId;
        //            vmModel.FirstName = insureeForQuote.FirstName;
        //            vmModel.LastName = insureeForQuote.LastName;
        //            vmModel.EmailAddress = insureeForQuote.EmailAddress;
        //            vmModel.DateOfBirth = insureeForQuote.DateOfBirth;
        //            vmModel.AutoYear = insureeForQuote.AutoYear;
        //            vmModel.AutoMake = insureeForQuote.AutoMake;
        //            vmModel.AutoModel = insureeForQuote.AutoModel;
        //            vmModel.SpeedingTickets = insureeForQuote.SpeedingTickets;
        //            vmModel.DUI = insureeForQuote.DUI;
        //            vmModel.CoverageType = insureeForQuote.CoverageType;


        //            foreach (AutoQuote item in AutoQuotes)
        //            {

        //                vmModel.QuoteGenerationDate = autoQuote.QuoteGenerationDate;
        //                vmModel.BaseRate = autoQuote.BaseRate;
        //                vmModel.AgeUnder18Rate = autoQuote.AgeUnder18Rate;
        //                vmModel.AgeBtwn19and25Rate = autoQuote.AgeBtwn19and25Rate;
        //                vmModel.AgeOver25Rate = autoQuote.AgeOver25Rate;
        //                vmModel.AutoYearBefore2000Rate = autoQuote.AutoYearBefore2000Rate;
        //                vmModel.AutoYearBtwn2000and2015Rate = autoQuote.AutoYearBtwn2000and2015Rate;
        //                vmModel.AutoYearAfter2015Rate = autoQuote.AutoYearAfter2015Rate;
        //                vmModel.IsPorscheRate = autoQuote.IsPorscheRate;
        //                vmModel.IsCarreraRate = autoQuote.IsCarreraRate;
        //                vmModel.SpeedingTicketsRate = autoQuote.SpeedingTicketsRate;
        //                vmModel.SubtotalBeforeDuiCalc = autoQuote.SubtotalBeforeDuiCalc;
        //                vmModel.DuiRateUp25Percent = autoQuote.DuiRateUp25Percent;
        //                vmModel.SubtotalAfterDuiCalc = autoQuote.SubtotalAfterDuiCalc;
        //                vmModel.CoverageTypeRateUp50Percent = autoQuote.CoverageTypeRateUp50Percent;
        //                vmModel.SubtotalAfterCoverageCalc = autoQuote.SubtotalAfterCoverageCalc;
        //                vmModel.MonthlyQuoteRate = autoQuote.MonthlyQuoteRate;
        //                vmModel.YearlyQuoteRate = autoQuote.YearlyQuoteRate;



        //                VmModels.Add(vmModel);
        //            }
        //        }
        //    }
        //    return View(VmModels);
        //}

        //if (TempData.ContainsKey("InsureeId"))
        //{
        //    vmModel.InsureeId = Convert.ToInt32(TempData["InsureeId"]);
        //    if (TempData.ContainsKey("FirstName"))
        //    {
        //        vmModel.FirstName = TempData["FirstName"].ToString();
        //    }
        //    if (TempData.ContainsKey("LastName"))
        //    {
        //        vmModel.LastName = TempData["LastName"].ToString();
        //    }
        //    if (TempData.ContainsKey("EmailAddress"))
        //    {
        //        vmModel.EmailAddress = TempData["EmailAddress"].ToString();
        //    }
        //    if (TempData.ContainsKey("DateOfBirth"))
        //    {
        //        vmModel.DateOfBirth = Convert.ToDateTime(TempData["DateOfBirth"]);
        //    }
        //    if (TempData.ContainsKey("AutoYear"))
        //    {
        //        vmModel.AutoYear = Convert.ToInt32(TempData["AutoYear"]);
        //    }
        //    if (TempData.ContainsKey("AutoMake"))
        //    {
        //        vmModel.FirstName = TempData["AutoMake"].ToString();
        //    }
        //    if (TempData.ContainsKey("AutoModel"))
        //    {
        //        vmModel.AutoModel = TempData["AutoModel"].ToString();
        //    }
        //    if (TempData.ContainsKey("SpeedingTickets"))
        //    {
        //        vmModel.SpeedingTickets = Convert.ToInt32(TempData["SpeedingTickets"]);
        //    }
        //    if (TempData.ContainsKey("DUI"))
        //    {
        //        vmModel.DUI = Convert.ToBoolean(TempData["DUI"]);
        //    }
        //    if (TempData.ContainsKey("CoverageType"))
        //    {
        //        vmModel.CoverageType = Convert.ToBoolean(TempData["CoverageType"]);
        //    }
        //    if (TempData.ContainsKey("QuoteGenerationDate"))
        //    {
        //        vmModel.QuoteGenerationDate = Convert.ToDateTime(TempData["QuoteGenerationDate"]);
        //    }
        //    if (TempData.ContainsKey("AgeUnder18Rate"))
        //    {
        //        vmModel.AgeUnder18Rate = Convert.ToDecimal(TempData["AgeUnder18Rate"]);
        //    }
        //    if (TempData.ContainsKey("AgeBtwn19and25Rate"))
        //    {
        //        vmModel.AgeBtwn19and25Rate = Convert.ToDecimal(TempData["AgeBtwn19and25Rate"]);
        //    }
        //    if (TempData.ContainsKey("AgeOver25Rate"))
        //    {
        //        vmModel.AgeOver25Rate = Convert.ToDecimal(TempData["AgeOver25Rate"]);
        //    }
        //    if (TempData.ContainsKey("AutoYearBefore2000Rate"))
        //    {
        //        vmModel.AutoYearBefore2000Rate = Convert.ToDecimal(TempData["AutoYearBefore2000Rate"]);
        //    }
        //    if (TempData.ContainsKey("AutoYearBtwn2000and2015Rate"))
        //    {
        //        vmModel.AutoYearBtwn2000and2015Rate = Convert.ToDecimal(TempData["AutoYearBtwn2000and2015"]);
        //    }
        //    if (TempData.ContainsKey("AutoYearAfter2015Rate"))
        //    {
        //        vmModel.AutoYearAfter2015Rate = Convert.ToDecimal(TempData["AutoYearAfter2015Rate"]);
        //    }
        //    if (TempData.ContainsKey("IsPorscheRate"))
        //    {
        //        vmModel.IsPorscheRate = Convert.ToDecimal(TempData["IsPorscheRate"]);
        //    }
        //    if (TempData.ContainsKey("IsCarreraRate"))
        //    {
        //        vmModel.IsCarreraRate = Convert.ToDecimal(TempData["IsCarreraRate"]);
        //    }
        //    if (TempData.ContainsKey("SpeedingTicketsRate"))
        //    {
        //        vmModel.SpeedingTicketsRate = Convert.ToDecimal(TempData["SpeedingTicketsRate"]);
        //    }
        //    if (TempData.ContainsKey("SubtotalBeforeDuiCalc"))
        //    {
        //        vmModel.SubtotalBeforeDuiCalc = Convert.ToDecimal(TempData["SubtotalBeforeDuiCalc"]);
        //    }
        //    if (TempData.ContainsKey("DuiRateUp25Percent"))
        //    {
        //        vmModel.DuiRateUp25Percent = Convert.ToDecimal(TempData["DuiRateUp25Percent"]);
        //    }
        //    if (TempData.ContainsKey("SubtotalAfterDuiCalc"))
        //    {
        //        vmModel.SubtotalAfterDuiCalc = Convert.ToDecimal(TempData["SubtotalAfterDuiCalc"]);
        //    }
        //    if (TempData.ContainsKey("CoverageTypeRateUp50Percent"))
        //    {
        //        vmModel.CoverageTypeRateUp50Percent = Convert.ToDecimal(TempData["CoverageTypeRateUp50Percent"]);
        //    }
        //    if (TempData.ContainsKey("SubtotalAfterCoverageCalc"))
        //    {
        //        vmModel.SubtotalAfterCoverageCalc = Convert.ToDecimal(TempData["SubtotalAfterCoverageCalc"]);
        //    }
        //    if (TempData.ContainsKey("MonthlyQuoteRate"))
        //    {
        //        vmModel.MonthlyQuoteRate = Convert.ToDecimal(TempData["MonthlyQuoteRate"]);
        //    }
        //    if (TempData.ContainsKey("YearlyQuoteRate"))
        //    {
        //        vmModel.YearlyQuoteRate = Convert.ToDecimal(TempData["YearlyQuoteRate"]);
        //    }
        //}


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
        public ActionResult Edit([Bind(Include = "AutoQuoteId,AutoQuoteDateTime,InsureeId,BaseRate,AgeUnder18Rate,AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate,AutoYearAfter2015Rate,IsPorscheRate,IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent,SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageTypeCalc,MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
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

