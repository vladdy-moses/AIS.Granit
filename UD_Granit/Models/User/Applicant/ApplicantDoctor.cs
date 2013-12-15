using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class ApplicantDoctor : Applicant
    {
        [Display(Name = "Серия и номер диплома кандидата наук")]
        public string CandidateDiplom { set; get; }
    }
}