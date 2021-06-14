using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AutoQuotesWebApp.ViewModels
{
    public class InsureeVM
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name ia a required field.")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name ia a required field.")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is a required field.")]
        public string EmailAddress { get; set; }
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Date of Birth is a required field.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Auto Year (i.e. 2014)")]
        [Required(ErrorMessage = "Auto Year is a required field.")]
        public int AutoYear { get; set; }
        [Display(Name = "Auto Make (i.e. Ford)")]
        [Required(ErrorMessage = "Auto Make is a required field.")]
        public string AutoMake { get; set; }
        [Display(Name = "Auto Model ( i.e. F-150)")]
        [Required(ErrorMessage = "Auto Model is a required field.")]
        public string AutoModel { get; set; }
        [Display(Name = "Speeding Tickets (if none enter 0)")]
        [Required(ErrorMessage = "A number entry is required.")]
        public int SpeedingTickets { get; set; }
        [Display(Name = "DUI (check if you have EVER had a DUI)")]
        public bool DUI { get; set; }
        [Display(Name = "Full Coverage (check to indicate full-coverage insurance is desired )")]
        public bool CoverageType { get; set; }

        public virtual IEnumerable InsureeVMs { get; set; }
    }
}