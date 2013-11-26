using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Dissertation
{
    public class Create
    {
        public UD_Granit.Models.Dissertation Dissertation { set; get; }

        [Required]
        [Display(Name = "Файл с авторефератом")]
        public HttpPostedFileBase File_Abstract { set; get; }

        [Required]
        [Display(Name = "Файл с текстом диссертации")]
        public HttpPostedFileBase File_Text { set; get; }
    }
}