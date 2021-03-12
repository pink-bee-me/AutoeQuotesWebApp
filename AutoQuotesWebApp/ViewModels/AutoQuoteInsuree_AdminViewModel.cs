using AutoQuotesWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoQuotesWebApp.ViewModels
{
    public class AutoQuoteInsuree_AdminViewModel
    {
        [Display(Name = "Insuree ID")]
        public Insuree InsureeId { get; set; }

        [Display(Name = "First Name")]
        public Insuree FirstName { get; set; }

        [Display(Name = "Last Name")]
        public Insuree LastName { get; set; }

        [Display(Name = "Email Address")]
        public Insuree EmailAddress { get; set; }

        [Display(Name = "AutoQuote Id")]
        public AutoQuote AutoquoteId { get; set; }

        [Display(Name = "Quote Generation Date")]
        public System.DateTime QuoteGenerationDate { get; set; }

        [Display(Name = "Monthly Rate")]
        public AutoQuote MonthlyQuoteRate { get; set; }

        [Display(Name = "Yearly Rate")]
        public AutoQuote YearlyQuoteRate { get; set; }

        [Display(Name = "Remove From Email List")]
        public Insuree DoNotEmail { get; set; }


    }
}

