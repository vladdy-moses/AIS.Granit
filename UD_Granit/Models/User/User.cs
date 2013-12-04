using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class User
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Электронный адрес")]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { set; get; }

        [Required]
        [Display(Name = "Фамилия")]
        public string FirstName { set; get; }

        [Required]
        [Display(Name = "Имя")]
        public string SecondName { set; get; }

        [Display(Name = "Отчество")]
        public string LastName { set; get; }

        [Required]
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { set; get; }

        [Display(Name = "Контактный телефон")]
        public string Phone { set; get; }
    }
}