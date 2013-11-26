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
        [Display(Name="Электронный почтовый ящик")]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { set; get; }
    }
}