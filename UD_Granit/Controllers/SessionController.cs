using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;

namespace UD_Granit.Controllers
{
    public class SessionController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Session/

        public ActionResult Index()
        {
            return HttpNotFound();
        }

        //
        // GET: /Session/Details/5

        public ActionResult Details(int id)
        {
            return HttpNotFound();
        }

        //
        // GET: /Session/CreateConsideration

        public ActionResult CreateConsideration(int id)
        {
            UD_Granit.ViewModels.Session.CreateConsideration viewModel = new ViewModels.Session.CreateConsideration();

            Dissertation currentDissertation = db.Dissertations.Find(id);
            if (currentDissertation == null)
                return HttpNotFound();

            viewModel.Dissertation_Id = id;
            viewModel.Dissertation_Title = currentDissertation.Title;
            viewModel.MemberList = new List<SelectListItem>();

            viewModel.MemberList.Add(new SelectListItem() { Text = "==Выберите члена совета из списка==", Value = "-1", Selected = true });
            foreach (var member in db.Members)
            {
                viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
            }

            return View(viewModel);
        }

        //
        // POST: /Session/CreateConsideration

        [HttpPost]
        public ActionResult CreateConsideration(UD_Granit.ViewModels.Session.CreateConsideration viewModel)
        {
            Dissertation currentDissertation = db.Dissertations.Find(viewModel.Dissertation_Id);
            if (currentDissertation == null)
                return HttpNotFound();

            try
            {
                SessionСonsideration currentSession = viewModel.Session;
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

                db.SessionsСonsideration.Add(currentSession);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = currentSession.Id });
            }
            catch (Exception ex)
            {
                viewModel.Dissertation_Title = currentDissertation.Title;
                viewModel.MemberList = new List<SelectListItem>();

                viewModel.MemberList.Add(new SelectListItem() { Text = "==Выберите члена совета из списка==", Value = "-1", Selected = true });
                foreach (var member in db.Members)
                {
                    viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
                }

                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = ex.Message });
                return View();
            }
        }

        //
        // GET: /Session/My

        public ActionResult My()
        {
            Member currentUser = Session.GetUser() as Member;
            if (currentUser == null)
                return HttpNotFound();

            UD_Granit.ViewModels.Session.My viewModel = new ViewModels.Session.My();
            viewModel.Sessions = db.Members.Find(currentUser.Id).Sessions;
            return View(viewModel);
        }
    }
}
