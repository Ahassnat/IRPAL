using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsCMD.Models
{
    [MetadataType(typeof (ArticleMetadata))]
    public partial class Article
    {
        class ArticleMetadata
        {
            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string Title { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string Slug { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string Summary { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public string Details { get; set; }

            [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
            public Nullable<int> CategoryId { get; set; }
            
            
        }
    }
}