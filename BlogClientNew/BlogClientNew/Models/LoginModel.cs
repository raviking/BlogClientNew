using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogClientNew.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="UserName Required")]
        [Display(Name="UserName(*)")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage="Password Required")]
        [Display(Name="Password(*)")]
        public string Password { get; set; }
    }
}