using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntro.Models
{
    [MetadataType(typeof(AccountMetaData))]
    public partial class Account
    {
        public class AccountMetaData
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Gender { get; set; }
            public Nullable<bool> Active { get; set; }
            [Required]
            public Nullable<int> CountryId { get; set; }
            [Required]
            [Display(Name = "Birth Of Date")]
            [DisplayFormat(DataFormatString = "{0:dd:MM:yyyy}")]
            public Nullable<System.DateTime> DOB { get; set; }


        }
    }
}