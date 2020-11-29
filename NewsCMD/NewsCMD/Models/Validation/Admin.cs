using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsCMD.Models
{
    [MetadataType(typeof (AdminMetadata))]
    public partial class Admin
    {
        class AdminMetadata
        {
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string FullName { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            [EmailAddress(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "EmailAddress")]
            public string Email { get; set; }


        }
    }
}