using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.Controllers
{
    // Управляет логикой по работе со стилями
    public class StyleController : Controller
    {
        // Задаёт обычный стиль отображения
        // GET: /Style/Basic

        public ActionResult Basic()
        {
            Session.Remove("Lite");
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            else
                return Redirect("/");
        }

        // Задаёт упрощённый стиль отображения
        // GET: /Style/Lite

        public ActionResult Lite()
        {
            Session["Lite"] = String.Empty;
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            else
                return Redirect("/");
        }
    }
}
