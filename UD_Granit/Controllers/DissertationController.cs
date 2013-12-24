using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;
using System.IO;
using System.Data.SqlClient;

namespace UD_Granit.Controllers
{
    // Управляет логикой по работе с диссертациями
    public class DissertationController : Controller
    {
        private DataContext db = new DataContext();

        // Отображает список диссертаций
        // GET: /Dissertation/

        public ActionResult Index(string search)
        {
            UD_Granit.ViewModels.Dissertation.Index viewModel = new ViewModels.Dissertation.Index();
            viewModel.Dissertations = new List<Dissertation>();

            IEnumerable<Dissertation> dissertations = null;

            if ((search != null) && (search.Length > 0))
            {
                viewModel.SearchHaveResults = true;
                viewModel.SearchString = search;
                dissertations = db.Database.SqlQuery<Dissertation>("FindDissertations @phrase", new SqlParameter("phrase", search));
            }
            else
            {
                dissertations = db.Database.SqlQuery<Dissertation>("GetDissertations");
            }

            foreach (var currentDissertation in dissertations)
            {
                if (RightsManager.Dissertation.Show(Session.GetUser(), currentDissertation))
                {
                    viewModel.Dissertations.Add(db.Dissertations.Find(currentDissertation.Id));
                }
            }
            return View(viewModel);
        }

        // Показывает подробности о диссертации
        // GET: /Dissertation/Details/5

        public ActionResult Details(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (RightsManager.Dissertation.Show(Session.GetUser(), dissertation))
            {
                UD_Granit.ViewModels.Dissertation.Details viewModel = new ViewModels.Dissertation.Details();
                viewModel.Dissertation = dissertation;

                User currentUser = Session.GetUser();
                viewModel.CanEdit = RightsManager.Dissertation.Edit(currentUser, dissertation) && (dissertation.Sessions.Count() == 0);
                viewModel.CanCreateSession = RightsManager.Session.Create(currentUser);
                viewModel.CanAddReplies = RightsManager.Reply.Control(currentUser, dissertation);
                viewModel.CanEditReplies = RightsManager.Reply.Control(currentUser, dissertation);

                return View(viewModel);
            }
            throw new HttpException(404, "Not found");
        }

        // Показывает форму создания диссертации
        // GET: /Dissertation/Create

        public ActionResult Create()
        {
            if (Session.GetUser() != null)
            {
                User currentUser = Session.GetUser();
                if (currentUser is Applicant)
                {
                    var dissertations = db.Dissertations.Where(d => d.Applicant.Id == currentUser.Id);
                    if (dissertations.Count() == 0)
                    {
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Info, Message = "Заполните информацию о Вашей диссертации. Вы можете сделать это позже, также как и отредактировать информацию о ней." });
                        ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });

                        return View();
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = dissertations.Single().Id });
                    }
                }
            }
            throw new HttpException(404, "Not found");
        }

        // Создаёт запись о диссертации
        // POST: /Dissertation/Create

        [HttpPost]
        public ActionResult Create(UD_Granit.ViewModels.Dissertation.Create viewModel)
        {
            Applicant currentUser = Session.GetUser() as Applicant;

            if (currentUser == null)
                throw new HttpException(404, "Not found");

            if (db.Dissertations.Where(d => d.Applicant.Id == currentUser.Id).Count() != 0)
                throw new HttpException(404, "Not found");

            Dissertation currentDissertation = viewModel.Dissertation;
            currentDissertation.Type = (currentUser is ApplicantCandidate) ? DissertationType.Candidate : DissertationType.Doctor;
            currentDissertation.Applicant = db.Applicants.Find(currentUser.Id);
            currentDissertation.File_Abstract = Path.GetExtension(viewModel.File_Abstract.FileName);
            currentDissertation.File_Text = Path.GetExtension(viewModel.File_Text.FileName);
            currentDissertation.File_Summary = Path.GetExtension(viewModel.File_Summary.FileName);
            currentDissertation.Defensed = false;
            currentDissertation.Speciality = db.Specialities.Find(viewModel.Speciality);

            db.Dissertations.Add(currentDissertation);
            db.SaveChanges();

            viewModel.File_Abstract.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Abstract" + currentDissertation.File_Abstract)));
            viewModel.File_Text.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Text" + currentDissertation.File_Text)));
            viewModel.File_Summary.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Summary" + currentDissertation.File_Summary)));

            return RedirectToAction("Details", new { id = currentDissertation.Id });
        }

        // Показывает форму редактирования диссертации
        // GET: /Dissertation/Edit/5

        public ActionResult Edit(int id)
        {
            User currentUser = Session.GetUser();
            Dissertation currentDissertation = db.Dissertations.Find(id);

            if (currentDissertation == null)
                throw new HttpException(404, "Not found");

            if (!RightsManager.Dissertation.Edit(currentUser, currentDissertation))
                throw new HttpException(404, "Not found");

            if (currentDissertation.Sessions.Count() > 0)
                throw new HttpException(404, "Not found");

            UD_Granit.ViewModels.Dissertation.Edit viewModel = new ViewModels.Dissertation.Edit();
            viewModel.Dissertation = currentDissertation;

            ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });
            return View(viewModel);
        }

        // Редактирует запись о диссертации
        // POST: /Dissertation/Edit/5

        [HttpPost]
        public ActionResult Edit(UD_Granit.ViewModels.Dissertation.Edit viewModel)
        {
            try
            {
                Dissertation currentDissertation = viewModel.Dissertation;
                currentDissertation.Speciality = db.Specialities.Find(viewModel.Speciality);

                Dissertation baseDissertation = db.Dissertations.Find(currentDissertation.Id);

                baseDissertation.Title = currentDissertation.Title;
                baseDissertation.Publications = currentDissertation.Publications;
                baseDissertation.Administrative_Use = currentDissertation.Administrative_Use;
                baseDissertation.Date_Preliminary_Defense = currentDissertation.Date_Preliminary_Defense;
                baseDissertation.Date_Sending = currentDissertation.Date_Sending;
                baseDissertation.Defensed = currentDissertation.Defensed;
                baseDissertation.References = currentDissertation.References;

                baseDissertation.Speciality = currentDissertation.Speciality;
                baseDissertation.Applicant = db.Applicants.Find(Session.GetUser().Id);

                if (viewModel.File_Abstract != null)
                {
                    System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Abstract" + baseDissertation.File_Abstract)));
                    baseDissertation.File_Abstract = Path.GetExtension(viewModel.File_Abstract.FileName);
                    viewModel.File_Abstract.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Abstract" + baseDissertation.File_Abstract)));
                }
                if (viewModel.File_Text != null)
                {
                    System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Text" + baseDissertation.File_Text)));
                    baseDissertation.File_Text = Path.GetExtension(viewModel.File_Text.FileName);
                    viewModel.File_Text.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Text" + baseDissertation.File_Text)));
                }
                if (viewModel.File_Summary != null)
                {
                    System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Summary" + baseDissertation.File_Summary)));
                    baseDissertation.File_Summary = Path.GetExtension(viewModel.File_Summary.FileName);
                    viewModel.File_Summary.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Summary" + baseDissertation.File_Summary)));
                }

                db.Entry<Dissertation>(baseDissertation).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", new { id = currentDissertation.Id });
            }
            catch (Exception ex)
            {
                ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = ex.Message });
                return View(viewModel);
            }
        }

        // Показывает форму удаления диссертации
        // GET: /Dissertation/Delete/5

        public ActionResult Delete(int id)
        {
            Dissertation currentDissertation = db.Dissertations.Find(id);

            if (!RightsManager.Reply.Control(Session.GetUser(), currentDissertation))
                throw new HttpException(404, "Not found");

            UD_Granit.ViewModels.Dissertation.Delete viewModel = new ViewModels.Dissertation.Delete();
            viewModel.Id = currentDissertation.Id;
            viewModel.Title = currentDissertation.Title;
            viewModel.CanDelete = (currentDissertation.Sessions.Count() == 0);
            return View(viewModel);
        }

        // Удаляет диссертацию
        // POST: /Dissertation/Delete/5

        [HttpPost]
        public ActionResult Delete(ViewModels.Dissertation.Delete viewModel)
        {
            try
            {
                Dissertation currentDissertation = db.Dissertations.Find(viewModel.Id);
                if (currentDissertation == null)
                    throw new HttpException(404, "Not found");

                if (!RightsManager.Dissertation.Edit(Session.GetUser(), currentDissertation))
                    throw new HttpException(404, "Not found");

                if (currentDissertation.Sessions.Count() > 0)
                    throw new HttpException(404, "Not found");

                System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Abstract" + currentDissertation.File_Abstract)));
                System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Text" + currentDissertation.File_Text)));
                System.IO.File.Delete(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Summary" + currentDissertation.File_Summary)));
                db.Dissertations.Remove(currentDissertation);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Details", new { id = viewModel.Id });
            }
        }

        // Показывает диссертацию соискателя
        // GET: /Dissertation/My

        public ActionResult My()
        {
            User currentUser = Session.GetUser();

            if ((currentUser is Applicant) == false)
                throw new HttpException(404, "Not found");

            var dissertations = db.Dissertations.Where(d => d.Applicant.Id == currentUser.Id);
            if (dissertations.Count() == 0)
                return RedirectToAction("Create");
            return RedirectToAction("Details", new { id = dissertations.Single().Id });
        }

        // Загружает файл, относящийся к диссертации
        // GET: /Dissertation/Download/5?type=Summary

        public ActionResult Download(int id, string type)
        {
            Dissertation currentDisserrtation = db.Dissertations.Find(id);
            if (!RightsManager.Dissertation.Show(Session.GetUser(), currentDisserrtation))
                throw new HttpException(404, "Not found");

            try
            {
                string fileName = string.Empty;

                switch (type)
                {
                    case "Abstract":
                        fileName = currentDisserrtation.Id + "_Abstract" + currentDisserrtation.File_Abstract;
                        break;
                    case "Text":
                        fileName = currentDisserrtation.Id + "_Text" + currentDisserrtation.File_Text;
                        break;
                    case "Summary":
                        fileName = currentDisserrtation.Id + "_Summary" + currentDisserrtation.File_Summary;
                        break;
                }
                if (fileName.Length == 0)
                    throw new HttpException(404, "Not found");

                var result = File("~/App_Data/" + fileName, "binary/octet-stream", fileName);
                if (System.IO.File.Exists(Server.MapPath(result.FileName)))
                    return result;

                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
            catch
            {
                return Redirect(Request.UrlReferrer.AbsolutePath);
            }
        }
    }
}
