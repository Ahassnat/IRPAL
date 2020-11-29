using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCIntro.ViewModles
{
    public class LoginViewModle
    {
        [Required(ErrorMessage ="Please Enter The User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}