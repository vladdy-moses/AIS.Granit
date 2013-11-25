using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public class Login
    {
        [Required]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
    }
}