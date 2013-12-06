using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class SessionСonsideration : Session
    {
        [Required]
        [Display(Name="Результат заседания")]
        public string Result { set; get; }
    }
}