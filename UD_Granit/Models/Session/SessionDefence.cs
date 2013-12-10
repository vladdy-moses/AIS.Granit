using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class SessionDefence : Session
    {
        [Display(Name = "Итог защиты")]
        public bool Result { set; get; }

        [Display(Name = "Результат голосования")]
        public int Vote_Result { set; get; }

        [Display(Name = "Запись защиты")]
        public string File_Recording { set; get; }

        [Display(Name = "Степень достоверности результатов")]
        public string Reliability { set; get; }

        [Display(Name = "Научная новизна результатов")]
        public string Novelty { set; get; }

        [Display(Name = "Значимость работы")]
        public string Significance { set; get; }
    }
}