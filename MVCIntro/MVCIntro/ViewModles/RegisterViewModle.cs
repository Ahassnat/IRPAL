using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;


namespace MVCIntro.ViewModles
{
    public class RegisterViewModle
    {
        
        [Required(ErrorMessageResourceType =typeof(App_GlobalResources.Labels),ErrorMessageResourceName = "Required")]
        [Display(Name ="FullName",ResourceType =(typeof(App_GlobalResources.Labels)))]
        public string   FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "EmailValidation")]
        [Display(Name ="Email",ResourceType =(typeof(App_GlobalResources.Labels)))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
        [Display(Name = "Mobile", ResourceType = (typeof(App_GlobalResources.Labels)))]
        public string Mobile { get; set; }

        [Display(Name = "Address", ResourceType = (typeof(App_GlobalResources.Labels)))]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Labels), ErrorMessageResourceName = "Required")]
        [Display(Name = "Gender", ResourceType = (typeof(App_GlobalResources.Labels)))]
        public string Gender { get; set; }
    }
}