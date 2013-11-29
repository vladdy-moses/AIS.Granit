using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class ScientificDirector
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Фамилия")]
        public string FirstName { set; get; }

        [Required]
        [Display(Name = "Имя")]
        public string SecondName { set; get; }

        [Display(Name = "Отчество")]
        public string LastName { set; get; }

        [Required]
        [Display(Name = "Учёная степень")]
        public string Degree { set; get; }

        [Required]
        [Display(Name = "Организация")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение")]
        public string Organization_Department { set; get; }

        [Required]
        [Display(Name = "Должность")]
        public string Organization_Post { set; get; }
    }
}