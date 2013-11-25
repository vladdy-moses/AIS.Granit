using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    public static class SessionHelper
    {
        public static User GetUser(this HttpSessionStateBase session)
        {
            return (session["User"] as User);
        }

        public static void SetUser(this HttpSessionStateBase session, User user)
        {
            session["User"] = user;
        }

        public static string GetUserIp(this HttpRequestBase request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        public static MemberPosition? GetUserPosition(this HttpSessionStateBase session)
        {
            if (session.GetUser() == null)
                return null;
            if (!(session.GetUser() is Member))
                return null;
            return (session.GetUser() as Member).Position;
        }
    }
}