using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class SpecialityHelper
    {
        public static string GetFullName(this Speciality speciality)
        {
            if (speciality != null)
            {
                return string.Format("{0} {1} ({2})", speciality.Number, speciality.Name, speciality.ScienceBranch);
            }
            return string.Empty;
        }
    }
}