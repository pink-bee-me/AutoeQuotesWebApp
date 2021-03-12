using AutoQuotesWebApp.Models;
using AutoQuotesWebApp.ViewModels;
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

        //Get Info for Creation of AutoQuote
        public ActionResult Create()
        {
            var autoQuote = new AutoQuote();

            ////Insuree insureeForQuote = new Insuree(id);
            //var insureeForQuote = new Insuree();
            //List<Insuree> Insurees = new List<Insuree>();
            //Insurees.Add(insureeForQuote);

            //foreach (Insuree item in Insurees)

            //{
            //    if (TempData.ContainsKey("InsureeId"))
            //    {
            //        insureeForQuote.InsureeId = Convert.ToInt32(TempData["InsureeId"]);
            //    }

            //    if (TempData.ContainsKey("FirstName"))
            //    {
            //        insureeForQuote.FirstName = TempData["FirstName"].ToString();
            //    }
            //    if (TempData.ContainsKey("LastName"))
            //    {
            //        insureeForQuote.LastName = TempData["LastName"].ToString();
            //    }
            //    if (TempData.ContainsKey("EmailAddress"))
            //    {
            //        insureeForQuote.EmailAddress = TempData["EmailAddress"].ToString();
            //    }
            //    if (TempData.ContainsKey("DateOfBirth"))
            //    {
            //        insureeForQuote.DateOfBirth = Convert.ToDateTime(TempData["DateOfBirth"]);
            //    }
            //    if (TempData.ContainsKey("AutoYear"))
            //    {
            //        insureeForQuote.AutoYear = Convert.ToInt32(TempData["AutoYear"]);
            //    }
            //    if (TempData.ContainsKey("AutoMake"))
            //    {
            //        insureeForQuote.AutoMake = TempData["AutoMake"].ToString();
            //    }
            //    if (TempData.ContainsKey("AutoModel"))
            //    {
            //        insureeForQuote.AutoModel = TempData["AutoModel"].ToString();
            //    }
            //    if (TempData.ContainsKey("SpeedingTickets"))
            //    {
            //        insureeForQuote.SpeedingTickets = Convert.ToInt32(TempData["SpeedingTickets"]);
            //    }
            //    if (TempData.ContainsKey("DUI"))
            //    {
            //        insureeForQuote.DUI = Convert.ToBoolean(TempData["DUI"]);
            //    }
            //    if (TempData.ContainsKey("CoverageType"))
            //    {
            //        insureeForQuote.CoverageType = Convert.ToBoolean(TempData["CoverageType"]);
            //    }
            //}
            //Now we will use the properties from the insuree model and build an autoQuote

            return View(autoQuote);
        }


        // POST: AutoQuotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuoteGenerationDate,InsureeId,BaseRate,AgeUnder18Rate," +
                                   "AgeBtwn19and25Rate,AgeOver25Rate,AutoYearBefore2000Rate," +
                                   "AutoYearBtwn2000and2015Rate,AutoYearAfter2015Rate,IsPorscheRate," +
                                   "IsCarreraRate,SpeedingTicketsRate,SubtotalBeforeDuiCalc,DuiRateUp25Percent," +
                                   "SubtotalAfterDuiCalc,CoverageTypeRateUp50Percent,SubtotalAfterCoverageCalc," +
                                   "MonthlyQuoteRate,YearlyQuoteRate")] AutoQuote autoQuote)
        {


            if (ModelState.IsValid)
            {
                db.AutoQuotes.Add(autoQuote);
                db.SaveChanges();
                List<AutoQuote> AutoQuotes = new List<AutoQuote>();
                AutoQuotes.Add(autoQuote);
            }

            return RedirectToAction("QuoteItemization");
        }


        public ActionResult QuoteItemization(Insuree id)
        {
            List<Insuree> Insurees = new List<Insuree>();
            Insuree insuree = new Insuree();
            //var id = db.Insurees.Max(item => insuree.InsureeId);
            insuree = db.Insurees.Find(id);

            List<AutoQuote> AutoQuotes = new List<AutoQuote>();
            AutoQuote autoQuote = new AutoQuote();
            //var id = db.AutoQuotes.Max(item => autoQuote.AutoQuoteId);
            autoQuote = db.AutoQuotes.Find(id);

            List<QuoteItemizationVM> VmModels = new List<QuoteItemizationVM>();
            {
                foreach (Insuree insureeForQuote in Insurees)
                {
                    var vmModel = new QuoteItemizationVM();

                    vmModel.InsureeId = insureeForQuote.InsureeId;
                    vmModel.FirstName = insureeForQuote.FirstName;
                    vmModel.LastName = insureeForQuote.LastName;
                    vmModel.EmailAddress = insureeForQuote.EmailAddress;
                    vmModel.DateOfBirth = insureeForQuote.DateOfBirth;
                    vmModel.AutoYear = insureeForQuote.AutoYear;
                    vmModel.AutoMake = insureeForQuote.AutoMake;
                    vmModel.AutoModel = insureeForQuote.AutoModel;
                    vmModel.SpeedingTickets = insureeForQuote.SpeedingTickets;
                    vmModel.DUI = insureeForQuote.DUI;
                    vmModel.CoverageType = insureeForQuote.CoverageType;


                    foreach (AutoQuote item in AutoQuotes)
                    {

                        vmModel.QuoteGenerationDate = autoQuote.QuoteGenerationDate;
                        vmModel.BaseRate = autoQuote.BaseRate;
                        vmModel.AgeUnder18Rate = autoQuote.AgeUnder18Rate;
                        vmModel.AgeBtwn19and25Rate = autoQuote.AgeBtwn19and25Rate;
                        vmModel.AgeOver25Rate = autoQuote.AgeOver25Rate;
                        vmModel.AutoYearBefore2000Rate = autoQuote.AutoYearBefore2000Rate;
                        vmModel.AutoYearBtwn2000and2015Rate = autoQuote.AutoYearBtwn2000and2015Rate;
                        vmModel.AutoYearAfter2015Rate = autoQuote.AutoYearAfter2015Rate;
                        vmModel.IsPorscheRate = autoQuote.IsPorscheRate;
                        vmModel.IsCarreraRate = autoQuote.IsCarreraRate;
                        vmModel.SpeedingTicketsRate = autoQuote.SpeedingTicketsRate;
                        vmModel.SubtotalBeforeDuiCalc = autoQuote.SubtotalBeforeDuiCalc;
                        vmModel.DuiRateUp25Percent = autoQuote.DuiRateUp25Percent;
                        vmModel.SubtotalAfterDuiCalc = autoQuote.SubtotalAfterDuiCalc;
                        vmModel.CoverageTypeRateUp50Percent = autoQuote.CoverageTypeRateUp50Percent;
                        vmModel.SubtotalAfterCoverageCalc = autoQuote.SubtotalAfterCoverageCalc;
                        vmModel.MonthlyQuoteRate = autoQuote.MonthlyQuoteRate;
                        vmModel.YearlyQuoteRate = autoQuote.YearlyQuoteRate;



                        VmModels.Add(vmModel);
                    }
                }
            }
            return View(VmModels);
        }

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

