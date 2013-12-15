using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Council
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Номер диссертационного совета")]
        public string Number { set; get; }
        
        [Required]
        [Display(Name = "Организация")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение")]
        public string Department { set; get; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { set; get; }

        [Display(Name = "Дата основания")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FoundationDate { set; get; }
    }
}