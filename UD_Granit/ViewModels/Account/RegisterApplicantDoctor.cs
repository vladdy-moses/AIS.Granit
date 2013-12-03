using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.ViewModels.Account
{
    public class RegisterApplicantDoctor
    {
        public ApplicantDoctor User { set; get; }

        [Display(Name = "Научный руководитель")]
        public UD_Granit.Models.ScientificDirector ScientificDirector { set; get; }
    }
}