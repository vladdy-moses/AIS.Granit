using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.ViewModels.Dissertation
{
    public class Edit
    {
        public UD_Granit.Models.Dissertation Dissertation { set; get; }

        [Display(Name = "Файл с авторефератом")]
        public HttpPostedFileBase File_Abstract { set; get; }

        [Display(Name = "Файл с текстом диссертации")]
        public HttpPostedFileBase File_Text { set; get; }

        [Display(Name = "Файл с заключением ведущей организации")]
        public HttpPostedFileBase File_Summary { set; get; }

        [Required]
        [Display(Name = "Специальность")]
        public string Speciality { set; get; }
    }
}