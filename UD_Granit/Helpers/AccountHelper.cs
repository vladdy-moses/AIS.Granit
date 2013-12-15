using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;

namespace UD_Granit.Helpers
{
    // Помогает при работе с учётными записями
    public static class AccountHelper
    {
        // Добавляет метод по извлечению информации об авторизованном пользователе
        public static User GetUser(this HttpSessionStateBase session)
        {
            return (session["User"] as User);
        }

        // Добавляет метод по заданию информации об авторизованном пользователе
        public static void SetUser(this HttpSessionStateBase session, User user)
        {
            session["User"] = user;
        }

        // Добавляет метод по определению IP-адреса пользователя
        public static string GetUserIp(this HttpRequestBase request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }

        // Добавляет метод по извлечению роли члена совета в системе
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