using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UD_Granit.Models;
using UD_Granit.Helpers;

namespace UD_Granit.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Account/

        public ActionResult Index()
        {
            return HttpNotFound();
        }

        //
        // GET: /Account/Login/

        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login/

        [HttpPost]
        public ActionResult Login(UD_Granit.ViewModels.Account.Login viewModel)
        {
            var q = from u in db.Users where ((u.Email == viewModel.Email) && (u.Password == viewModel.Password)) select u;
            if (q.Count() != 0)
            {
                User currentUser = q.First();
                if (currentUser is Administrator)
                {
                    (currentUser as Administrator).LastIP = Request.GetUserIp();
                    db.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                Session.SetUser(currentUser);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //
        // GET: /Account/Logout/

        public ActionResult Logout()
        {
            if (Session.GetUser() != null)
            {
                Session.SetUser(null);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // GET: /Account/Register/

        public ActionResult Register()
        {
            UD_Granit.Models.User currentUser = Session.GetUser();
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
        // POST: /Account/RegisterNewbie/

        [HttpPost]
        public ActionResult RegisterNewbie(UD_Granit.ViewModels.Account.RegisterNewbie viewModel)
        {
            var q = from u in db.Users where u.Email == viewModel.User.Email select u;
            if (q.Count() > 0)
            {
                NotificationManager nManager = new NotificationManager();
                nManager.Notifies.Add(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с таким электронным почтовым ящиком уже зарегистрирован. Пожалуйста, выберите другой email." });
                ViewBag.UserNotification = nManager;

                return View();
            }
            else
            {
                db.Applicants.Add(viewModel.User);
                db.SaveChanges();
                Session.SetUser(viewModel.User);

                return RedirectToAction("Create", "Dissertation");
            }
        }

        //
        // GET: /Account/Details/5

        public ActionResult Details(int? id)
        {
            User currentUser = Session.GetUser();
            if (!id.HasValue)
            {
                if (currentUser == null)
                    return HttpNotFound();
                else
                    id = currentUser.User_Id;
            }

            var q = from u in db.Users where u.User_Id == id.Value select u;
            if(q.Count() == 0)
                return HttpNotFound();
            User showedUser = q.First();

            UD_Granit.ViewModels.Account.Details viewModel = new UD_Granit.ViewModels.Account.Details();
            viewModel.FullName = UserHelper.GetFullName(showedUser);
            viewModel.Role = UserHelper.GetRole(showedUser);
            viewModel.User_Id = showedUser.User_Id;
            viewModel.CanControl = ((currentUser is Administrator) || (Session.GetUserPosition() == MemberPosition.Chairman));
            return View(viewModel);
        }
    }
}
