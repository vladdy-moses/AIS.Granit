using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class UserHelper
    {
        public static string GetFullName(this User user)
        {
            if (user != null)
            {
                string result = string.Format("{0} {1}", user.FirstName, user.SecondName);

                if ((user.LastName != null) && (user.LastName.Length > 0))
                    result += string.Format(" {0}", user.LastName);

                return result;
            }
            return string.Empty;
        }

        public static string GetFullNameWithInitials(this User user)
        {
            if (user != null)
            {
                string result = string.Format("{0} {1}.", user.FirstName, user.SecondName.Substring(0, 1));

                if ((user.LastName != null) && (user.LastName.Length > 0))
                    result += string.Format(" {0}.", user.LastName.Substring(0, 1));

                return result;
            }
            return string.Empty;
        }

        public static string GetRole(this User user)
        {
            if (user != null)
            {
                if (user is Administrator)
                    return "Администратор";
                if (user is Member)
                    return "Член совета";
                if (user is Applicant)
                    return "Соискатель";
            }
            return string.Empty;
        }

        public static string GetPosition(this User user)
        {
            if (user is Member)
                return (user as Member).Position.ToDescription();
            return String.Empty;
        }
    }
}