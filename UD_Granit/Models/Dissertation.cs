using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public enum DissertationType : short
    {
        [Description("Кандидатская")]
        Candidate = 0,
        [Description("Докторская")]
        Doctor = 1
    }

    public class Dissertation
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Тип диссертационной работы")]
        public DissertationType Type { set; get; }

        [Display(Name = "Диссертация защищена")]
        public bool Defensed { set; get; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { set; get; }

        [Required]
        [Display(Name = "Автореферат")]
        public string File_Abstract { set; get; }

        [Required]
        [Display(Name = "Текст диссертации")]
        public string File_Text { set; get; }

        [Required]
        [Display(Name = "Заключение ведущей организации")]
        public string File_Summary { set; get; }

        [Display(Name = "Список литературы")]
        public string References { set; get; }

        [Required]
        [Display(Name = "Для внутреннего использования")]
        public bool Administrative_Use { set; get; }

        [Required]
        [Display(Name = "Число ВАКовских публикаций")]
        public int Publications { set; get; }

        [Display(Name = "Дата последней публикации")]
        public DateTime? Date_Sending { set; get; }

        [Display(Name = "Дата предварительной защиты")]
        public DateTime? Date_Preliminary_Defense { set; get; }

        public virtual ICollection<Reply> Replies { set; get; }

        [Required]
        [Display(Name = "Специальность")]
        public virtual Speciality Speciality { set; get; }

        /*[Required]
        [ForeignKey("Applicant")]
        public int Applicant_Id { set; get; }*/

        [Required]
        public virtual Applicant Applicant { set; get; }
    }
}