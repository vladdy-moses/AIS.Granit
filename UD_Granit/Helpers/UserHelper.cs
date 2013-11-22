using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class UserHelper
    {
        public static string GetFullName(User user)
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

        public static string GetFullNameWithInitials(User user)
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

        public static string GetRole(User user)
        {
            if (user != null)
            {
                if (user is Administrator)
                    return "администратор";
                if (user is Member)
                    return "член совета";
                if (user is Applicant)
                    return "соискатель";
            }
            return string.Empty;
        }
    }
}