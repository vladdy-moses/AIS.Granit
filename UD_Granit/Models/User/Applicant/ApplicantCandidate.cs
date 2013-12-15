using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class ApplicantCandidate : Applicant
    {
        [Display(Name="Серия и номер документа об образовании")]
        public string DocumentOfEducation { set; get; }

        [Display(Name = "Номер свидетельства о сдаче кандидатских экзамнов")]
        public string CandidateExams { set; get; }
    }
}