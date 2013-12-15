using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    // Помогает с выводом информации о специальности
    public static class SpecialityHelper
    {
        // Добавляет метод, выводящий полную информацию о специальности
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