using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public class Edit
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Ваш текущий пароль")]
        public string OldPassword { set; get; }

        [Display(Name = "Новый пароль")]
        public string NewPassword { set; get; }

        [Required]
        [Display(Name = "Фамилия")]
        public string FirstName { set; get; }

        [Required]
        [Display(Name = "Имя")]
        public string SecondName { set; get; }

        [Display(Name = "Отчество")]
        public string LastName { set; get; }
    }
}