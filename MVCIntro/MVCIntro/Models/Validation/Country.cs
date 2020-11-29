using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntro.Models
{
    [MetadataType(typeof(CountryMetaData))]
    public partial class Country
    {
        public class CountryMetaData
        {
            [Required]
            public string Name { get; set; }
            [Required]
            public string Iso2 { get; set; }
            [Required]
            public string Code { get; set; }


        }
    }
}