using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsCMD.Models
{
    [MetadataType(typeof (SliderMetadata))]
    public partial class Slider
    {
        class SliderMetadata
        {
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
           
            public string Title { get; set; }
        }
    }
}