using AutoQuotesWebApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AutoQuotesWebApp
{
    public class InsureeVM : Insuree
    {
        [Display(Name = "Insuree ID")]
        public int VmInsureeId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Required Field. Enter First Name.")]
        public string VmFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Required Field. Enter Last Name.")]
        public string VmLastName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Required Field. Enter Email Address.")]
        public string VmEmailAddress { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Required Field. Enter Date Of Birth.")]
        [DataType(DataType.Date)]
        public System.DateTime VmDateOfBirth { get; set; }

        [Display(Name = "Auto Year (2014)")]
        [Required(ErrorMessage = "Required Field. Enter Year Of Auto.")]
        public int VmAutoYear { get; set; }

        [Display(Name = "Auto Make (Ford)")]
        [Required(ErrorMessage = "Required Field. Enter Make Of Auto.")]
        public string VmAutoMake { get; set; }

        [Display(Name = "Auto Model (F-150)")]
        [Required(ErrorMessage = "Required Field. Enter Model Of Auto.")]
        public string VmAutoModel { get; set; }

        [Display(Name = "Enter the Number of Speeding Tickets")]
        [Required(ErrorMessage = "Required Field. Enter Number Of Speeding Tickets (if None, Enter 0).")]
        public int VmSpeedingTickets { get; set; }

        [Display(Name = "Check Here if DUI on Driving Record")]
        public bool VmDUI { get; set; }

        [Display(Name = "Check Here if Full Coverage Is Needed")]
        public bool VmCoverageType { get; set; }

        [Display(Name = "Monthly Rate")]
        public Nullable<decimal> VmMonthlyRate { get; set; }

        [Display(Name = "Yearly Rate")]
        public Nullable<decimal> VmYearlyRate { get; set; }



        public InsureeVM()
        {

        }

        public InsureeVM(Insuree insuree)
        {
            VmInsureeId = insuree.InsureeId;
            VmFirstName = insuree.FirstName;
            VmLastName = insuree.LastName;
            VmEmailAddress = insuree.EmailAddress;
            VmDateOfBirth = insuree.DateOfBirth;
            VmAutoYear = insuree.AutoYear;
            VmAutoMake = insuree.AutoMake;
            VmAutoModel = insuree.AutoModel;
            VmSpeedingTickets = insuree.SpeedingTickets;
            VmDUI = insuree.DUI;
            VmCoverageType = insuree.CoverageType;
            VmMonthlyRate = insuree.MonthlyRate;
            VmYearlyRate = insuree.YearlyRate;

        }
    }
}
