using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Reply
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Автор отзыва")]
        public string Author { set; get; }

        [Required]
        [Display(Name = "Организация (университет)")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение (кафедра)")]
        public string Departmant { set; get; }

        [Display(Name = "Учёная степень")]
        public string Degree { set; get; }

        [Required]
        [Display(Name = "Текст отзыва")]
        public string Text { set; get; }

        [Required]
        [ForeignKey("Dissertation")]
        public int Dissertation_Id { get; set; }
        public virtual Dissertation Dissertation { set; get; }
    }
}