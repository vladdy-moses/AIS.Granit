using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;
using System.IO;

namespace UD_Granit.Controllers
{
    // Управляет логикой по работе с сессиями
    public class SessionController : Controller
    {
        private DataContext db = new DataContext();

        // Показывает список доступных сессий
        // GET: /Session/

        public ActionResult Index()
        {
            User currentUser = Session.GetUser();

            var prevSessions = db.Sessions.Where(s => (s.Was)).OrderByDescending(s => s.Date);
            var nextSessions = db.Sessions.Where(s => (!s.Was)).OrderBy(s => s.Date);

            List<Session> SessionsWas = new List<Session>();
            List<Session> SessionsWill = new List<Session>();

            foreach (Session currentSession in prevSessions)
                if (RightsManager.Dissertation.Show(currentUser, currentSession.Dissertation))
                    SessionsWas.Add(currentSession);

            foreach (Session currentSession in nextSessions)
                if (RightsManager.Dissertation.Show(currentUser, currentSession.Dissertation))
                    SessionsWill.Add(currentSession);

            ViewModels.Session.Index viewModel = new ViewModels.Session.Index();
            viewModel.SessionsWas = SessionsWas;
            viewModel.SessionsWill = SessionsWill;
            return View(viewModel);
        }

        // Показывает форму создания заседания по рассмотрению диссертации
        // GET: /Session/CreateConsideration

        public ActionResult CreateConsideration(int id)
        {
            UD_Granit.ViewModels.Session.Create viewModel = new ViewModels.Session.Create();

            Dissertation currentDissertation = db.Dissertations.Find(id);
            if (currentDissertation == null)
                return HttpNotFound();

            viewModel.SessionType = "Consideration";
            viewModel.Dissertation_Id = id;
            viewModel.Dissertation_Title = currentDissertation.Title;
            viewModel.MemberList = new List<SelectListItem>();

            viewModel.MemberList.Add(new SelectListItem() { Text = "== Выберите члена совета из списка ==", Value = "-1", Selected = true });
            foreach (var member in db.Members)
            {
                viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
            }

            return View("Create", viewModel);
        }

        // Показывает форму создания заседания по защите
        // GET: /Session/CreateDefence

        public ActionResult CreateDefence(int id)
        {
            if (!RightsManager.Session.Create(Session.GetUser()))
                return HttpNotFound();

            UD_Granit.ViewModels.Session.Create viewModel = new ViewModels.Session.Create();

            Dissertation currentDissertation = db.Dissertations.Find(id);
            if (currentDissertation == null)
                return HttpNotFound();

            viewModel.SessionType = "Defence";
            viewModel.Dissertation_Id = id;
            viewModel.Dissertation_Title = currentDissertation.Title;
            viewModel.MemberList = new List<SelectListItem>();

            viewModel.MemberList.Add(new SelectListItem() { Text = "== Выберите члена совета из списка ==", Value = "-1", Selected = true });
            foreach (var member in db.Members)
            {
                viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
            }

            return View("Create", viewModel);
        }

        // Создаёт заседание
        // POST: /Session/Create

        [HttpPost]
        public ActionResult Create(UD_Granit.ViewModels.Session.Create viewModel)
        {
            if (!RightsManager.Session.Create(Session.GetUser()))
                return HttpNotFound();

            Dissertation currentDissertation = db.Dissertations.Find(viewModel.Dissertation_Id);
            if (currentDissertation == null)
                return HttpNotFound();

            try
            {
                Session currentSession = viewModel.Session;
                currentSession.Dissertation = db.Dissertations.Find(viewModel.Dissertation_Id);
                currentSession.Was = false;

                if (viewModel.MembersCount > 0)
                {
                    currentSession.Members = new List<Member>();
                    for (int i = 0; i < viewModel.MembersCount; i++)
                    {
                        try
                        {
                            Member currentMember = db.Members.Find(Convert.ToInt32(Request.Form[string.Format("Member{0}", i)]));
                            if (currentMember != null)
                                currentSession.Members.Add(currentMember);
                        }
                        catch { }
                    }
                }

                switch (viewModel.SessionType)
                {
                    case "Consideration":
                        currentSession = new SessionConsideration() { Dissertation = currentSession.Dissertation, Date = currentSession.Date, Was = currentSession.Was, Members = currentSession.Members };
                        db.SessionsСonsideration.Add(currentSession as SessionConsideration);
                        break;
                    case "Defence":
                        currentSession = new SessionDefence() { Dissertation = currentSession.Dissertation, Date = currentSession.Date, Was = currentSession.Was, Members = currentSession.Members };
                        db.SessionsDefence.Add(currentSession as SessionDefence);
                        break;
                }
                db.SaveChanges();

                return RedirectToAction("Details", new { id = currentSession.Id });
            }
            catch (Exception ex)
            {
                viewModel.Dissertation_Title = currentDissertation.Title;
                viewModel.MemberList = new List<SelectListItem>();

                viewModel.MemberList.Add(new SelectListItem() { Text = "== Выберите члена совета из списка ==", Value = "-1", Selected = true });
                foreach (var member in db.Members)
                {
                    viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
                }

                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = ex.Message });
                return View(viewModel);
            }
        }

        // Показывает заседания, к которым прикреплён пользователь
        // GET: /Session/My

        public ActionResult My()
        {
            User currentUser = Session.GetUser() as User;
            if (currentUser == null)
                return HttpNotFound();

            if (!(currentUser is Member) && !(currentUser is Applicant))
                return HttpNotFound();

            UD_Granit.ViewModels.Session.My viewModel = new ViewModels.Session.My();
            if (currentUser is Member)
            {
                viewModel.ViewType = ViewModels.Session.SessionViewType.MemberView;
                viewModel.Sessions = db.Members.Find(currentUser.Id).Sessions.OrderBy(s => s.Was).ThenByDescending(s => s.Date);
            }
            if (currentUser is Applicant)
            {
                viewModel.ViewType = ViewModels.Session.SessionViewType.ApplicantView;
                Dissertation currentDissertation = db.Dissertations.Find(currentUser.Id);
                viewModel.Sessions = (currentDissertation != null) ? db.Dissertations.Find(currentUser.Id).Sessions.OrderBy(s => s.Was).ThenByDescending(s => s.Date) : null;
            }
            return View(viewModel);
        }

        // Показыает подробности заседания
        // GET: /Session/Details/5

        public ActionResult Details(int id)
        {
            User currentUser = Session.GetUser();
            Session currentSession = db.Sessions.Find(id);

            if (currentSession == null)
                return HttpNotFound();

            if (!RightsManager.Dissertation.Show(currentUser, currentSession.Dissertation))
                return HttpNotFound();

            UD_Granit.ViewModels.Session.Details viewModel = new ViewModels.Session.Details();
            viewModel.Session = currentSession;
            viewModel.CanControl = RightsManager.Session.Edit(currentUser);
            return View(viewModel);
        }

        // Показывает форму заполнения результата заседания
        // GET: /Session/Result/5

        public ActionResult Result(int id)
        {
            User currentUser = Session.GetUser() as User;
            Session currentSession = db.Sessions.Find(id);

            if (!RightsManager.Session.Edit(currentUser))
                return HttpNotFound();

            if (currentSession is SessionConsideration)
            {
                ViewModels.Session.ResultConsideration viewModel = new ViewModels.Session.ResultConsideration();

                viewModel.DissertationTitle = currentSession.Dissertation.Title;
                viewModel.Id = currentSession.Id;
                viewModel.Date = currentSession.Date;

                return View("ResultConsideration", viewModel);
            }
            if (currentSession is SessionDefence)
            {
                ViewModels.Session.ResultDefence viewModel = new ViewModels.Session.ResultDefence();

                viewModel.DissertationTitle = currentSession.Dissertation.Title;
                viewModel.Id = currentSession.Id;
                viewModel.Date = currentSession.Date;

                return View("ResultDefence", viewModel);
            }
            return HttpNotFound();
        }

        // Заполняет результат заседания по рассмотрению
        // POST: /Session/ResultConsideration

        [HttpPost]
        public ActionResult ResultConsideration(ViewModels.Session.ResultConsideration viewModel)
        {
            User currentUser = Session.GetUser() as User;
            SessionConsideration currentSession = db.SessionsСonsideration.Find(viewModel.Id);

            if (!RightsManager.Session.Edit(currentUser))
                return HttpNotFound();

            if (currentSession.Was)
                return HttpNotFound();

            currentSession.Result = viewModel.Result;
            currentSession.Was = true;
            db.Entry<SessionConsideration>(currentSession).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Details", new { id = currentSession.Id });
        }

        // Заполняет результат заседания по защите
        // POST: /Session/ResultDefence

        [HttpPost]
        public ActionResult ResultDefence(ViewModels.Session.ResultDefence viewModel)
        {
            User currentUser = Session.GetUser() as User;
            SessionDefence currentSession = db.SessionsDefence.Find(viewModel.Id);

            if (!RightsManager.Session.Edit(currentUser))
                return HttpNotFound();

            if (currentSession.Was)
                return HttpNotFound();

            if (viewModel.Vote_Result < 0)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Число голосов не может быть отрицательным. Пожалуйста, измените значение поля." });
                return View(viewModel);
            }

            currentSession.Result = viewModel.Result;
            currentSession.Novelty = viewModel.Novelty;
            currentSession.Reliability = viewModel.Reliability;
            currentSession.Significance = viewModel.Significance;
            currentSession.Vote_Result = viewModel.Vote_Result;
            currentSession.File_Recording = Path.GetExtension(viewModel.File_Recording.FileName);

            currentSession.Was = true;
            db.Entry<SessionDefence>(currentSession).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            viewModel.File_Recording.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentSession.Id + "_Recording" + currentSession.File_Recording)));
            
            Dissertation currentDissertation = db.Dissertations.Find(currentSession.Dissertation.Id);
            currentDissertation.Defensed = viewModel.Result;
            db.Entry<Dissertation>(currentDissertation).State = System.Data.Entity.EntityState.Modified;
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

            return RedirectToAction("Details", new { id = currentSession.Id });
        }

        // Загружает файл с аудиовидеозаписью заседания
        // GET: /Session/Download/5

        public ActionResult Download(int id)
        {
            User currentUser = Session.GetUser();
            SessionDefence currentSession = db.SessionsDefence.Find(id);

            if (!RightsManager.Dissertation.Show(currentUser, currentSession.Dissertation))
                return HttpNotFound();

            string fileName = currentSession.Id + "_Recording" + currentSession.File_Recording;

            var result = File("~/App_Data/" + fileName, "binary/octet-stream", fileName);
            if (System.IO.File.Exists(Server.MapPath(result.FileName)))
                return result;

            return Redirect(Request.UrlReferrer.AbsolutePath);
        }

        // Показывает форму удаления заседания
        // GET: /Dissertation/Delete/5

        public ActionResult Delete(int id)
        {
            User currentUser = Session.GetUser() as User;
            Session currentSession = db.Sessions.Find(id);

            if (!RightsManager.Session.Edit(currentUser))
                return HttpNotFound();

            ViewModels.Session.Delete viewModel = new ViewModels.Session.Delete();

            viewModel.Id = id;
            viewModel.Title = currentSession.Dissertation.Title;
            viewModel.Date = currentSession.Date;

            return View(viewModel);
        }

        // Удаляет заседание
        // POST: /Dissertation/Delete

        [HttpPost]
        public ActionResult Delete(ViewModels.Session.Delete viewModel)
        {
            User currentUser = Session.GetUser() as User;
            Session currentSession = db.Sessions.Find(viewModel.Id);

            if (!RightsManager.Session.Edit(currentUser))
                return HttpNotFound();

            if (currentSession is SessionDefence)
            {
                System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentSession.Id + "_Recording" + (currentSession as SessionDefence).File_Recording)));
            }
            db.Sessions.Remove(currentSession);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
