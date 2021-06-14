//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoQuotesWebApp.Models
{
    using AutoQuotesWebApp.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Insuree
    {
        public Insuree(int id)
        {
            InsureeId = id;
        }

        public Insuree() { }

        public Insuree(InsureeVM insureeVM)
        {
            FirstName = insureeVM.FirstName;
            LastName = insureeVM.LastName;
            EmailAddress = insureeVM.EmailAddress;
            DateOfBirth = insureeVM.DateOfBirth;
            AutoYear = insureeVM.AutoYear;
            AutoMake = insureeVM.AutoMake;
            AutoModel = insureeVM.AutoModel;
            SpeedingTickets = insureeVM.SpeedingTickets;
            DUI = insureeVM.DUI;
            CoverageType = insureeVM.CoverageType;
        }

        public int InsureeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int AutoYear { get; set; }
        public string AutoMake { get; set; }
        public string AutoModel { get; set; }
        public int SpeedingTickets { get; set; }
        public bool DUI { get; set; }
        public bool CoverageType { get; set; }
        public Nullable<System.DateTime> DoNotEmail { get; set; }

        public virtual ICollection<AutoQuote> AutoQuotes { get; set; }
        public virtual ICollection<Insuree> Insurees { get; set; }
    }
}
