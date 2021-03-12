using AutoQuotesWebApp.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoQuotesWebApp.ViewModels
{
    public class QuoteItemizationVM
    {
        // Insuree Model Data for The AutoQuoteController ActionResult QuoteItemization View
        [Display(Name = "Insuree ID")]
        public int InsureeId { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfBirth { get; set; }
        [Display(Name = "Auto Year")]
        public int AutoYear { get; set; }
        [Display(Name = "Auto Make")]
        public string AutoMake { get; set; }
        [Display(Name = "Auto Model")]
        public string AutoModel { get; set; }
        [Display(Name = "Speeding Tickets")]
        public int SpeedingTickets { get; set; }
        [Display(Name = "DUI On Driving Record")]
        public bool DUI { get; set; }
        [Display(Name = "Full Coverage Insurance")]
        public bool CoverageType { get; set; }



        // AutoQuote Model Data for The AutoQuoteController ActionResult QuoteItemization View
        [Display(Name = "AutoQuote ID")]
        public int AutoQuoteId { get; set; }
        [Display(Name = "Quote Date")]
        public System.DateTime QuoteGenerationDate { get; set; }
        [Display(Name = "Base Rate")]
        public decimal BaseRate { get; set; }
        [Display(Name = "Age Under 18 Rate")]
        public decimal AgeUnder18Rate { get; set; }
        [Display(Name = "Age Between 19 and 25 Rate")]
        public decimal AgeBtwn19and25Rate { get; set; }
        [Display(Name = "Age Over 25 Rate")]
        public decimal AgeOver25Rate { get; set; }
        [Display(Name = "Auto Year Before 2000 Rate")]
        public decimal AutoYearBefore2000Rate { get; set; }
        [Display(Name = "AutoYear Between 2000 and 2015 Rate")]
        public decimal AutoYearBtwn2000and2015Rate { get; set; }
        [Display(Name = "Auto Year After 2015 Rate")]
        public decimal AutoYearAfter2015Rate { get; set; }
        [Display(Name = "Is Porsche Rate")]
        public decimal IsPorscheRate { get; set; }
        [Display(Name = "Is Carrera Rate")]
        public decimal IsCarreraRate { get; set; }
        [Display(Name = "Speeding Tickets Rate")]
        public decimal SpeedingTicketsRate { get; set; }
        [Display(Name = "Subtotal:")]
        public decimal SubtotalBeforeDuiCalc { get; set; }
        [Display(Name = "DUI Rate (Increase of 25%)")]
        public decimal DuiRateUp25Percent { get; set; }
        [Display(Name = "Subtotal:")]
        public decimal SubtotalAfterDuiCalc { get; set; }
        [Display(Name = "Full Coverage Rate (Increase of 50%)")]
        public decimal CoverageTypeRateUp50Percent { get; set; }
        [Display(Name = "Total:")]
        public decimal SubtotalAfterCoverageCalc { get; set; }
        [Display(Name = "Monthly Payment Option (1st Installment Due Today...The following 11 installmants due on this day each month):")]
        public decimal MonthlyQuoteRate { get; set; }
        [Display(Name = "Yearly Payment Option (Pay Today and Save 20% !!!) :")]
        public decimal YearlyQuoteRate { get; set; }

        public List<Insuree> Insurees { get; set; }
        public List<AutoQuote> AutoQuotes { get; set; }
        public IEnumerable<QuoteItemizationVM> VmModels { get; set; }
        
    }
}

