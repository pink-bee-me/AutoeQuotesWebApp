using AutoQuotesWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace AutoQuotesWebApp.ViewModels
{
    public class AdminVM
    {
        [Display(Name = "Insuree ID")]
        public InsureeVM InsureeId { get; set; }

        [Display(Name = "First Name")]
        public InsureeVM FirstName { get; set; }

        [Display(Name = "Last Name")]
        public InsureeVM LastName { get; set; }

        [Display(Name = "Email Address")]
        public InsureeVM EmailAddress { get; set; }

        [Display(Name = "AutoQuote Id")]
        public AutoQuote AutoquoteId { get; set; }

        [Display(Name = "Quote Generation Date")]
        public System.DateTime QuoteGenerationDate { get; set; }

        [Display(Name = "Monthly Rate")]
        public AutoQuote MonthlyQuoteRate { get; set; }

        [Display(Name = "Yearly Rate")]
        public AutoQuote YearlyQuoteRate { get; set; }

        [Display(Name = "Remove From Email List")]
        public InsureeVM DoNotEmail { get; set; }


    }
}

