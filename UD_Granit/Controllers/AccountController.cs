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
            return RedirectToAction("All");
        }

        //
        // GET: /Account/Login/

        public ActionResult Login()
        {
            if (Session.GetUser() != null)
                return HttpNotFound();

            return View();
        }

        //
        // POST: /Account/Login/

        [HttpPost]
        public ActionResult Login(UD_Granit.ViewModels.Account.Login viewModel)
        {
            if (Session.GetUser() != null)
                return HttpNotFound();

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

            ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с такой комбинацией электронного почтового ящика и пароля не найден в системе. Пожалуйста, проверьте достоверность введённых данных." });
            return View();
        }

        //
        // GET: /Account/Logout/

        public ActionResult Logout()
        {
            if (Session.GetUser() == null)
                return HttpNotFound();

            Session.SetUser(null);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register/

        public ActionResult Register()
        {
            UD_Granit.Models.User currentUser = Session.GetUser();

            bool CanRegisterApplicant = RightsManager.Account.RegisterApplicant(currentUser);
            bool CanRegisterMember = RightsManager.Account.RegisterMember(currentUser);
            bool CanRegisterAdministrator = RightsManager.Account.RegisterAdministrator(currentUser);

            if (!CanRegisterAdministrator && !CanRegisterApplicant && !CanRegisterMember)
                return HttpNotFound();

            UD_Granit.ViewModels.Account.Register viewModel = new ViewModels.Account.Register();

            viewModel.CanRegisterAdministrator = CanRegisterAdministrator;
            viewModel.CanRegisterApplicant = CanRegisterApplicant;
            viewModel.CanRegisterMember = CanRegisterMember;

            return View(viewModel);
        }

        //
        // GET: /Account/RegisterApplicantCandidate/

        public ActionResult RegisterApplicantCandidate()
        {
            if (!RightsManager.Account.RegisterApplicant(Session.GetUser()))
                return HttpNotFound();
            return View();
        }

        //
        // GET: /Account/RegisterApplicantDoctor/

        public ActionResult RegisterApplicantDoctor()
        {
            if (!RightsManager.Account.RegisterApplicant(Session.GetUser()))
                return HttpNotFound();
            return View();
        }

        //
        // GET: /Account/RegisterMember/

        public ActionResult RegisterMember()
        {
            if (!RightsManager.Account.RegisterMember(Session.GetUser()))
                return HttpNotFound();

            ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });
            return View();
        }

        //
        // GET: /Account/RegisterAdministrator/

        public ActionResult RegisterAdministrator()
        {
            if (!RightsManager.Account.RegisterAdministrator(Session.GetUser()))
                return HttpNotFound();
            return View();
        }

        //
        // POST: /Account/RegisterApplicantCandidate/

        [HttpPost]
        public ActionResult RegisterApplicantCandidate(UD_Granit.ViewModels.Account.RegisterApplicantCandidate viewModel)
        {
            if (!RightsManager.Account.RegisterApplicant(Session.GetUser()))
                return HttpNotFound();

            var q = from u in db.Users where u.Email == viewModel.User.Email select u;
            if (q.Count() > 0)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с таким электронным почтовым ящиком уже зарегистрирован. Пожалуйста, выберите другой email." });
                return View();
            }
            else
            {
                ApplicantCandidate currentUser = viewModel.User;
                currentUser.RegistrationDate = DateTime.Now;
                currentUser.IsActive = false;

#warning Проверка на существование научного руководителя в БД
                db.ScientificDirectors.Add(viewModel.ScientificDirector);
                currentUser.ScientificDirector = viewModel.ScientificDirector;

                db.ApplicantCandidates.Add(currentUser);
                db.SaveChanges();
                Session.SetUser(currentUser);

                return RedirectToAction("Create", "Dissertation");
            }
        }

        //
        // POST: /Account/RegisterApplicantDoctor/

        [HttpPost]
        public ActionResult RegisterApplicantDoctor(UD_Granit.ViewModels.Account.RegisterApplicantDoctor viewModel)
        {
            if (!RightsManager.Account.RegisterApplicant(Session.GetUser()))
                return HttpNotFound();

            var q = from u in db.Users where u.Email == viewModel.User.Email select u;
            if (q.Count() > 0)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с таким электронным почтовым ящиком уже зарегистрирован. Пожалуйста, выберите другой email." });
                return View();
            }
            else
            {
                ApplicantDoctor currentUser = viewModel.User;
                currentUser.RegistrationDate = DateTime.Now;
                currentUser.IsActive = false;

#warning Проверка на существование научного руководителя в БД
                db.ScientificDirectors.Add(viewModel.ScientificDirector);
                currentUser.ScientificDirector = viewModel.ScientificDirector;

                db.ApplicantDoctors.Add(currentUser);
                db.SaveChanges();
                Session.SetUser(currentUser);

                return RedirectToAction("Create", "Dissertation");
            }
        }

        //
        // POST: /Account/RegisterMember/

        [HttpPost]
        public ActionResult RegisterMember(UD_Granit.ViewModels.Account.RegisterMember viewModel)
        {
            if (!RightsManager.Account.RegisterMember(Session.GetUser()))
                return HttpNotFound();

            var q = from u in db.Users where u.Email == viewModel.User.Email select u;
            if (q.Count() > 0)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с таким электронным почтовым ящиком уже зарегистрирован. Пожалуйста, выберите другой email." });
                return View();
            }
            else
            {
                Member currentUser = viewModel.User;
                currentUser.RegistrationDate = DateTime.Now;
                currentUser.Position = (MemberPosition)viewModel.Position;
                currentUser.Speciality = db.Specialities.Find(viewModel.Speciality);

                db.Members.Add(currentUser);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = currentUser.Id });
            }
        }

        //
        // POST: /Account/RegisterAdministrator/

        [HttpPost]
        public ActionResult RegisterAdministrator(UD_Granit.ViewModels.Account.RegisterAdministrator viewModel)
        {
            if (!RightsManager.Account.RegisterAdministrator(Session.GetUser()))
                return HttpNotFound();

            var q = from u in db.Users where u.Email == viewModel.User.Email select u;
            if (q.Count() > 0)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Пользователь с таким электронным почтовым ящиком уже зарегистрирован. Пожалуйста, выберите другой email." });
                return View();
            }
            else
            {
                Administrator currentUser = viewModel.User;
                currentUser.RegistrationDate = DateTime.Now;

                db.Administrators.Add(currentUser);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = currentUser.Id });
            }
        }

        //
        // GET: /Account/Details/[5]

        public ActionResult Details(int? id)
        {
            User currentUser = Session.GetUser();
            if (!id.HasValue)
            {
                if (currentUser == null)
                    return HttpNotFound();
                else
                    id = currentUser.Id;
            }

            var q = from u in db.Users where u.Id == id.Value select u;
            if (q.Count() == 0)
                return HttpNotFound();
            User showedUser = q.First();

            UD_Granit.ViewModels.Account.Details viewModel = new UD_Granit.ViewModels.Account.Details();

            viewModel.User = showedUser;
            viewModel.CanEdit = RightsManager.Account.Edit(currentUser, showedUser);
            viewModel.CanRemove = RightsManager.Account.Remove(currentUser, showedUser);
            viewModel.CanShowAdditionalInfo = RightsManager.Account.ShowAdditionalInfo(currentUser, showedUser);

            return View(viewModel);
        }

        //
        // GET: /Account/All

        public ActionResult All()
        {
            if (!RightsManager.Account.Edit(Session.GetUser()))
                return HttpNotFound();

            UD_Granit.ViewModels.Account.All viewModel = new ViewModels.Account.All();

            var q = from u in db.Users select u;
            List<UD_Granit.ViewModels.Account.AccountViev> accountList = new List<ViewModels.Account.AccountViev>();
            foreach (User u in q)
            {
                accountList.Add(new UD_Granit.ViewModels.Account.AccountViev() { Name = u.GetFullName(), Id = u.Id, CanEdit = RightsManager.Account.Edit(Session.GetUser(), u), Email = u.Email, Role = u.GetRole() });
            }

            viewModel.Accounts = accountList;

            return View(viewModel);
        }

        //
        // GET: /Account/Delete/5

        public ActionResult Delete(int id)
        {
            User currentUser = Session.GetUser();
            User deletedUser = db.Users.Find(id);

            if (!RightsManager.Account.Remove(currentUser, deletedUser))
                return HttpNotFound();

            ViewModels.Account.Delete viewModel = new ViewModels.Account.Delete();
            viewModel.CanDelete = true;

            if (deletedUser is Applicant)
            {
                if (db.Dissertations.Find(deletedUser.Id) != null)
                {
                    ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Невозможно удалить соискателя, так как он завёл запись о диссертации. Удалите диссертацию сначала." });
                    viewModel.CanDelete = false;
                }
            }

            viewModel.Id = id;
            viewModel.Name = deletedUser.GetFullName();
            viewModel.Referer = Request.UrlReferrer.AbsolutePath;
            return View(viewModel);
        }

        //
        // POST: /Account/Delete

        [HttpPost]
        public ActionResult Delete(ViewModels.Account.Delete viewModel)
        {
            User currentUser = Session.GetUser();
            User deletedUser = db.Users.Find(viewModel.Id);

            if (!RightsManager.Account.Remove(currentUser, deletedUser))
                return HttpNotFound();

#warning удаление пользователя

            if (viewModel.Referer == null)
                return RedirectToAction("Index", "Home");
            return Redirect(viewModel.Referer);
        }
    }
}
