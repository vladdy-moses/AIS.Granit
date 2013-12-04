using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class ApplicantCandidate : Applicant
    {
        public string DocumentOfEducation { set; get; }
        public string CandidateExams { set; get; }
    }
}