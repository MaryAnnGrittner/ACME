using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ACME.Models.ViewModels
{
    public class Loginvm
    {
        [Required, MaxLength(50)]
        [RegularExpression(@"^[\w-\.]+[@][\w-]+[.][\w]{2,4}$",
        ErrorMessage = "Email Address is not valid")]
        public string Username { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
    }
}