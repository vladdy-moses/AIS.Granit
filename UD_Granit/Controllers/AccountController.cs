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
        private DataContext db = new DataContext();

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
        public ActionResult Login(User user)
        {
            var q = from u in db.Users where ((u.Email == user.Email) && (u.Password == user.Password)) select u;
            if (q.Count() != 0)
            {
                this.SetUser(q.First());
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // GET: /Account/Logout/

        public ActionResult Logout()
        {
            this.SetUser(null);
            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        //
        // GET: /Account/Register/

        public ActionResult Register()
        {
            UD_Granit.Models.User currentUser = this.GetUser();
            if (currentUser != null)
            {
                if (currentUser is UD_Granit.Models.Administrator)
                    return View("RegisterAdministrator");
                if ((currentUser is Member) && ((currentUser as Member).Position == MemberPosition.Chairman))
                    return View("RegisterChairman");
            }
            else
            {
                return View("RegisterNewbie");
            }
            return HttpNotFound();
        }

        //
        // POST: /Account/Register/

        [HttpPost]
        public ActionResult RegisterNewbie(Applicant applicant)
        {
            //return applicant.FirstName + " " + applicant.SecondName;
            var q = from u in db.Users where u.Email == applicant.Email select u;//db.Users.Select(u => u.Email == applicant.Email);
            if (q.Count() > 0)
                return Redirect(q.Count().ToString()); //HttpNotFound();
            else
            {
                db.Applicants.Add(applicant);
                db.SaveChanges();
                this.SetUser(applicant);

                return RedirectToAction("Index", "Home");
            }
        }
    }
}
