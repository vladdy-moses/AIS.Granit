using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class UserHelper
    {
        public static User GetUser(this Controller controller)
        {
            return (User)controller.Session["User"];
        }

        public static void SetUser(this Controller controller, User user)
        {
            controller.Session["User"] = user;
        }

        public static string GetUserIp(this Controller controller)
        {
            string ipList = controller.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return controller.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}