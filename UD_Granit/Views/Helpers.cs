using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.Controllers//.Views
{
    public static class Helpers
    {
        public static string GetUserIp(this Controller controller)
        {
            string ipList = controller.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return controller.Request.ServerVariables["REMOTE_ADDR"];
            //return ;
        }
    }
}