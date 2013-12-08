using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class ResultConsideration
    {
        public int Id { set; get; }

        [Required]
        [Display(Name="Результат рассмотрения")]
        public string Result { set; get; }

        public string DissertationTitle { set; get; }
        public DateTime Date { set; get; }
    }
}