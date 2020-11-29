using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsCMD.Models
{
    [MetadataType(typeof (MenuMetadata))]
    public partial class Menu
    {
        class MenuMetadata
        {
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string Title { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string URL { get; set; }
            
        }
    }
}