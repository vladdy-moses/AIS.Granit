using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UD_Granit.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Login/

        public ActionResult Login()
        {
            //FormsAuthentication.
            return View();
        }
    }
}
