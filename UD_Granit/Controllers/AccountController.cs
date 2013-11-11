using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UD_Granit.Models;

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

        //
        // POST: /Account/Login/
        [HttpPost]
        public String Login(User user)
        {
            
            return user.ToString();
        }

        //
        // GET: /Account/Register/

        public ActionResult Register()
        {
            UD_Granit.Models.User currentUser = this.GetUser();
            if (currentUser != null)
            {
                if(currentUser is UD_Granit.Models.Administrator)
                    return View("RegisterAdministrator");
                if ((currentUser is Member) && ((currentUser as Member).Position == MemberPosition.Chairman))
                    return View("RegisterChairman");
            }
            return HttpNotFound();
        }
    }
}
