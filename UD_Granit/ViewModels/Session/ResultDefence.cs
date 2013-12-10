using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class ResultDefence
    {
        public int Id { set; get; }

        [Required]
        [Display(Name = "Итог защиты")]
        public bool Result { set; get; }

        [Required]
        [Display(Name = "Результат голосования")]
        public int Vote_Result { set; get; }

        [Required]
        [Display(Name = "Аудиовидеозапись защиты")]
        public HttpPostedFileBase File_Recording { set; get; }

        [Required]
        [Display(Name="Степень достоверности результатов")]
        public string Reliability { set; get; }

        [Required]
        [Display(Name = "Научная новизна результатов")]
        public string Novelty { set; get; }

        [Required]
        [Display(Name = "Значимость работы")]
        public string Significance { set; get; }

        public string DissertationTitle { set; get; }
        public DateTime Date { set; get; }
    }
}