using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;
using System.Web.Mvc.Html;
using System.Data.SqlClient;

namespace UD_Granit.Controllers
{
    // Управляет логикой по работе с информацией о диссертационном совете
    public class CouncilController : Controller
    {
        private DataContext db = new DataContext();

        // Показывает информацию о диссертационном совете
        // GET: /Council/

        public ActionResult Index()
        {
            UD_Granit.ViewModels.Council.Index viewModel = new ViewModels.Council.Index();

            viewModel.HaveInformation = (db.Council.Count() > 0);
            if (viewModel.HaveInformation)
                viewModel.Council = db.Council.First();
            viewModel.CanControl = RightsManager.Council.Edit(Session.GetUser());

            return View(viewModel);
        }

        // Показывает форму редактирования информации о диссертационном совете
        // GET: /Council/Edit

        public ActionResult Edit()
        {
            User currentUser = Session.GetUser();

            if (currentUser == null)
                throw new HttpException(404, "Not found");

            if (!RightsManager.Council.Edit(currentUser))
                throw new HttpException(404, "Not found");

            var chairman = db.Members.Where(m => (m.Position == MemberPosition.Chairman)).FirstOrDefault();
            var viceCharman = db.Members.Where(m => (m.Position == MemberPosition.ViceChairman)).FirstOrDefault();
            var secretary = db.Members.Where(m => (m.Position == MemberPosition.Secretary)).FirstOrDefault();

            UD_Granit.ViewModels.Council.Edit viewModel = new ViewModels.Council.Edit();

            if (db.Council.Count() > 0)
                viewModel.Council = db.Council.First();
            viewModel.CanDefineChairman = RightsManager.Council.ChangeChairman(currentUser);
            viewModel.CanDefineRoles = (db.Members.Count() >= 3);
            viewModel.Chairman = (chairman != null) ? chairman.Id : 0;
            viewModel.ViceChairman = (viceCharman != null) ? viceCharman.Id : 0;
            viewModel.Secretary = (secretary != null) ? secretary.Id : 0;

            viewModel.Members = new List<SelectListItem>();
            viewModel.Members.Add(new SelectListItem() { Text = "== Выберите члена совета из списка ==", Value = "0" });
            foreach (var currentMember in db.Members)
                viewModel.Members.Add(new SelectListItem() { Text = currentMember.GetFullName(), Value = currentMember.Id.ToString() });

            
            return View(viewModel);
        }

        // Редактирует информацию о диссертационном совете
        // POST: /Council/Edit

        [HttpPost]
        public ActionResult Edit(UD_Granit.ViewModels.Council.Edit viewModel)
        {
            User currentUser = Session.GetUser();

            if (currentUser == null)
                throw new HttpException(404, "Not found");

            if (!RightsManager.Council.Edit(currentUser))
                throw new HttpException(404, "Not found");

            try
            {
                viewModel.CanDefineRoles = (db.Members.Count() >= 3);
                viewModel.CanDefineChairman = RightsManager.Council.ChangeChairman(currentUser);

                viewModel.Chairman = (viewModel.CanDefineChairman) ? viewModel.Chairman : db.Members.Where(m => m.Position == MemberPosition.Chairman).FirstOrDefault().Id;

                if(((viewModel.Chairman == viewModel.ViceChairman) && (viewModel.Chairman != 0)) ||
                    ((viewModel.Chairman == viewModel.Secretary) && (viewModel.Chairman != 0)) ||
                    ((viewModel.Secretary == viewModel.ViceChairman) && (viewModel.Secretary != 0)))
                {
                    throw new Exception("Член совета не может иметь две роли одновременно. Пожалуйста, измените свой выбор.");
                }

                // Удаление старых руководителей
                var chairman = (viewModel.CanDefineChairman) ? db.Members.Where(m => (m.Position == MemberPosition.Chairman)).FirstOrDefault() : null;
                var viceCharman = db.Members.Where(m => (m.Position == MemberPosition.ViceChairman)).FirstOrDefault();
                var secretary = db.Members.Where(m => (m.Position == MemberPosition.Secretary)).FirstOrDefault();

                if ((chairman != null) && (viewModel.CanDefineChairman))
                    chairman.Position = MemberPosition.Member;
                if (viceCharman != null)
                    viceCharman.Position = MemberPosition.Member;
                if (secretary != null)
                    secretary.Position = MemberPosition.Member;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;

                // Добавление новых руководителей
                chairman = (viewModel.CanDefineChairman) ? db.Members.Find(viewModel.Chairman) : null;
                viceCharman = db.Members.Find(viewModel.ViceChairman);
                secretary = db.Members.Find(viewModel.Secretary);

                if ((chairman != null) && (viewModel.CanDefineChairman))
                    chairman.Position = MemberPosition.Chairman;
                if (viceCharman != null)
                    viceCharman.Position = MemberPosition.ViceChairman;
                if (secretary != null)
                    secretary.Position = MemberPosition.Secretary;

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;


                if (currentUser is Member)
                    Session.SetUser(db.Members.Find(currentUser.Id));

                if (db.Council.Count() == 0)
                {
                    db.Council.Add(viewModel.Council);
                }
                else
                {
                    viewModel.Council.Id = 1;
                    db.Entry(viewModel.Council).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                HttpContext.Application["Name"] = viewModel.Council.Number;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                viewModel.Members = new List<SelectListItem>();
                viewModel.Members.Add(new SelectListItem() { Text = "== Выберите члена совета из списка ==", Value = "0" });
                foreach (var currentMember in db.Members)
                    viewModel.Members.Add(new SelectListItem() { Text = currentMember.GetFullName(), Value = currentMember.Id.ToString() });

                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = ex.Message });
                return View(viewModel);
            }
        }

        // Показывает членов диссертационного совета
        // GET: /Council/Members

        public ActionResult Members(string filter)
        {
            UD_Granit.ViewModels.Council.Members viewModel = new ViewModels.Council.Members();

            if (filter == null)
            {
                var q = from m in db.Members orderby m.Position descending select m;
                List<UD_Granit.ViewModels.Council.MemberView> memberList = new List<ViewModels.Council.MemberView>();
                foreach (Member m in q)
                {
                    memberList.Add(new UD_Granit.ViewModels.Council.MemberView() { Name = m.GetFullName(), Degree = m.Degree, Position = m.Position.ToDescription(), Speciality = m.Speciality.GetFullName(), Id = m.Id });
                }

                viewModel.CouncilMembers = memberList;
            }
            else
            {
                switch (filter)
                {
                    case "speciality":
                        var specialities = db.Specialities;
                        if (specialities != null)
                        {
                            viewModel.FilteredMembers = new Dictionary<string, IEnumerable<ViewModels.Council.MemberView>>();
                            foreach (var speciality in specialities)
                            {
                                var members = db.Members.Where(m => (m.Speciality.Number == speciality.Number));
                                if (members.Count() > 0)
                                {
                                    List<UD_Granit.ViewModels.Council.MemberView> memberList = new List<ViewModels.Council.MemberView>();
                                    foreach (Member m in members)
                                    {
                                        memberList.Add(new UD_Granit.ViewModels.Council.MemberView() { Name = m.GetFullName(), Degree = m.Degree, Position = m.Position.ToDescription(), Speciality = m.Speciality.GetFullName(), Id = m.Id });
                                    }
                                    viewModel.FilteredMembers.Add(speciality.GetFullName(), memberList);
                                }
                            }
                        }
                        break;
                    case "scienceBranch":
                        var branches = db.Database.SqlQuery<string>("GetScienceBranches");
                        if (branches != null)
                        {
                            viewModel.FilteredMembers = new Dictionary<string, IEnumerable<ViewModels.Council.MemberView>>();
                            foreach (var scienceBranch in branches)
                            {
                                var members = db.Database.SqlQuery<Member>("GetMembersByScienceBranch @Branch", new SqlParameter("Branch", scienceBranch));

                                List<UD_Granit.ViewModels.Council.MemberView> memberList = new List<ViewModels.Council.MemberView>();
                                foreach (Member m in members)
                                {
                                    m.Speciality = db.Members.Find(m.Id).Speciality;
                                    memberList.Add(new UD_Granit.ViewModels.Council.MemberView() { Name = m.GetFullName(), Degree = m.Degree, Position = m.Position.ToDescription(), Speciality = m.Speciality.GetFullName(), Id = m.Id });
                                }
                                viewModel.FilteredMembers.Add(scienceBranch, memberList);

                                if (memberList.Count == 0)
                                    viewModel.FilteredMembers.Remove(scienceBranch);
                            }
                        }
                        break;
                }
            }

            viewModel.CanControl = RightsManager.Account.Edit(Session.GetUser());

            return View(viewModel);
        }
    }
}
