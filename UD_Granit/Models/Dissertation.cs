using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public enum DissertationType : short
    {
        Candidate = 0,
        Doctor = 1
    }

    public class Dissertation
    {
        [Key]
        [ForeignKey("Applicant")]
        public int Dissertation_Id { private set; get; }
        public Applicant Applicant { set; get; }

        [Required]
        [Display(Name = "Тип диссертационной работы")]
        public DissertationType Type { set; get; }

        [Required]
        [Display(Name = "Заголовок")]
        public string Title { set; get; }

        [Required]
        [Display(Name = "Автореферат")]
        public byte[] File_Abstract { set; get; }

        [Required]
        [Display(Name = "Текст диссертации")]
        public byte[] File_Text { set; get; }

        [Display(Name = "Список литературы")]
        public string References { set; get; }

        [Required]
        [Display(Name = "Для внутреннего использования")]
        public bool Administrative_Use { set; get; }

        [Display(Name = "Заключение ведущей организации")]
        public byte[] Organization_Summary { set; get; }

        [Required]
        [Display(Name = "Число ВАКовских публикаций")]
        public int Publications { set; get; }

        [Display(Name = "Дата последней публикации")]
        public DateTime? Date_Sending { set; get; }

        [Display(Name = "Дата предварительной защиты")]
        public DateTime? Date_Preliminary_Defense { set; get; }

        public virtual ICollection<Reply> Replies { set; get; }


    }
}