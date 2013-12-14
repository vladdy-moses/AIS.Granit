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

        public ActionResult All(string filter, string subfilter)
        {
            User currentUser = Session.GetUser();

            if (!RightsManager.Account.Edit(currentUser))
                return HttpNotFound();

            UD_Granit.ViewModels.Account.All viewModel = new ViewModels.Account.All();
            List<UD_Granit.ViewModels.Account.AccountViev> accountList = null;
            IEnumerable<User> q = null;

            if ((filter == null) || (filter == "administrators"))
            {
                q = from u in db.Administrators select u;
                accountList = new List<ViewModels.Account.AccountViev>();
                foreach (User u in q)
                    accountList.Add(new UD_Granit.ViewModels.Account.AccountViev() { Name = u.GetFullName(), Id = u.Id, CanEdit = RightsManager.Account.Edit(currentUser, u), CanRemove = RightsManager.Account.Remove(currentUser, u), Email = u.Email });
                viewModel.Administrators = accountList;
            }

            if ((filter == null) || (filter == "members"))
            {
                if ((subfilter == null) || (subfilter != "only"))
                    q = from u in db.Members orderby u.Position descending select u;
                else
                    q = from u in db.Members where u.Position == MemberPosition.Member select u;

                accountList = new List<ViewModels.Account.AccountViev>();
                foreach (User u in q)
                    accountList.Add(new UD_Granit.ViewModels.Account.AccountViev() { Name = u.GetFullName(), Id = u.Id, CanEdit = RightsManager.Account.Edit(currentUser, u), CanRemove = RightsManager.Account.Remove(currentUser, u), Email = u.Email, Note = u.GetPosition() });
                viewModel.Members = accountList;
            }

            if ((filter == null) || (filter == "applicants"))
            {
                if (subfilter == null)
                    q = from u in db.Applicants select u;
                else
                switch(subfilter) {
                    case "candidates":
                        q = from u in db.ApplicantCandidates select u;
                        break;
                    case "doctors":
                        q = from u in db.ApplicantDoctors select u;
                        break;
                    default:
                        q = from u in db.Applicants select u;
                        break;
                }
                
                accountList = new List<ViewModels.Account.AccountViev>();
                foreach (User u in q)
                    accountList.Add(new UD_Granit.ViewModels.Account.AccountViev() { Name = u.GetFullName(), Id = u.Id, CanEdit = RightsManager.Account.Edit(currentUser, u), CanRemove = RightsManager.Account.Remove(currentUser, u), Email = u.Email, Note = u.GetPosition() });
                viewModel.Applicants = accountList;
            }

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

            if ((deletedUser is Applicant) && (db.Dissertations.Find(deletedUser.Id) != null))
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Невозможно удалить соискателя, так как он завёл запись о диссертации. Удалите диссертацию сначала." });
                viewModel.CanDelete = false;
            }

            if ((deletedUser is Member) && ((deletedUser as Member).Sessions.Count > 0))
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Невозможно удалить члена диссертационного совета, так как он участвует в заседаниях. Удалите заседания сначала." });
                viewModel.CanDelete = false;
            }

            if ((deletedUser is Administrator) && (db.Administrators.Count() == 1))
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Невозможно удалить последнего администратора в системе." });
                viewModel.CanDelete = false;
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

            db.Users.Remove(deletedUser);
            db.SaveChanges();

            if (currentUser.Id == deletedUser.Id)
                return RedirectToAction("Logout");

            if (viewModel.Referer == null)
                return RedirectToAction("Index", "Home");
            return Redirect(viewModel.Referer);
        }

        //
        // GET: /Account/Edit

        public ActionResult Edit(int id)
        {
            User currentUser = Session.GetUser();
            User editedUser = db.Users.Find(id);

            if (!RightsManager.Account.Edit(currentUser, editedUser))
                return HttpNotFound();

            ViewModels.Account.Edit viewModel = new ViewModels.Account.Edit()
            {
                Id = editedUser.Id,
                FirstName = editedUser.FirstName,
                SecondName = editedUser.SecondName,
                LastName = editedUser.LastName
            };
            return View(viewModel);
        }

        //
        // POST: /Account/Edit

        [HttpPost]
        public ActionResult Edit(ViewModels.Account.Edit viewModel)
        {
            User currentUser = Session.GetUser();
            User editedUser = db.Users.Find(viewModel.Id);

            if (!RightsManager.Account.Edit(currentUser, editedUser))
                return HttpNotFound();

            if (viewModel.OldPassword != currentUser.Password)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Неверный текущий пароль. Пожалуйста, попытайтесь снова." });
                return View(viewModel);
            }

            if (viewModel.NewPassword != null)
                editedUser.Password = viewModel.NewPassword;

            if (viewModel.FirstName != editedUser.FirstName)
                editedUser.FirstName = viewModel.FirstName;

            if (viewModel.SecondName != editedUser.SecondName)
                editedUser.SecondName = viewModel.SecondName;

            if (viewModel.LastName != null)
                editedUser.LastName = viewModel.LastName;
            else
                editedUser.LastName = null;

            db.Entry<User>(editedUser).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
            }
            catch (Exception ex)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = ex.Message });
                return View(viewModel);
            }

            if (currentUser.Id == editedUser.Id)
            {
                Session.SetUser(editedUser);
                return RedirectToAction("Details");
            }
            return RedirectToAction("Details", new { id = editedUser.Id });
        }
    }
}
