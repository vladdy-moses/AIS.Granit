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
    public class DissertationController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Dissertation/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Dissertation/Details/5

        public ActionResult Details(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (CanShow(dissertation))
            {
                UD_Granit.ViewModels.Dissertation.Details viewModel = new ViewModels.Dissertation.Details();
                viewModel.Dissertation = dissertation;

                User currentUser = Session.GetUser();
                viewModel.CanEdit = CanEdit(dissertation);
                viewModel.CanCreateSession = CanCreateSession();

                return View(viewModel);
            }
            return HttpNotFound();
        }

        //
        // GET: /Dissertation/Create

        public ActionResult Create()
        {
            if (Session.GetUser() != null)
            {
                User currentUser = Session.GetUser();
                if (currentUser is Applicant)
                {
                    var dissertations = from d in db.Dissertations where d.Applicant.Id == currentUser.Id select d;
                    if (dissertations.Count() == 0)
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Info, Message = "Заполните информацию о Вашей диссертации. Вы можете сделать это позже, также как и отредактировать информацию о ней." });
#warning Добавить проверку на то, что нельзя больше одной незащищённой диссертации

                    ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });
                    return View();
                }
            }
            return HttpNotFound();
        }

        //
        // POST: /Dissertation/Create

        [HttpPost]
        public ActionResult Create(UD_Granit.ViewModels.Dissertation.Create viewModel)
        {
            Applicant currentUser = Session.GetUser() as Applicant;

            if (currentUser == null)
                return HttpNotFound();

            //try
            //{
            Dissertation currentDissertation = viewModel.Dissertation;
            currentDissertation.Applicant = db.Applicants.Find(currentUser.Id);
            currentDissertation.File_Abstract = Path.GetExtension(viewModel.File_Abstract.FileName);
            currentDissertation.File_Text = Path.GetExtension(viewModel.File_Text.FileName);
            currentDissertation.File_Summary = Path.GetExtension(viewModel.File_Summary.FileName);
            currentDissertation.Defensed = false;
            currentDissertation.Speciality = db.Specialities.Find(viewModel.Speciality);

            db.Dissertations.Add(currentDissertation);
            db.SaveChanges();

#warning Проверять на существование файлов
            viewModel.File_Abstract.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Abstract" + currentDissertation.File_Abstract)));
            viewModel.File_Text.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Text" + currentDissertation.File_Text)));
            viewModel.File_Summary.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Id + "_Summary" + currentDissertation.File_Summary)));

            return RedirectToAction("Details", new { id = currentDissertation.Id });
            /*}
            catch (Exception ex)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Message = ex.Message, Type = NotificationManager.Notify.NotifyType.Error });
                ViewData["Speciality"] = db.Specialities.Select(s => new SelectListItem { Text = s.Number + " " + s.Name, Value = s.Number });
                return View();
            }*/
        }

        //
        // GET: /Dissertation/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Dissertation/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Dissertation/Delete/5

        public ActionResult Delete(int id)
        {
#warning При удалении удалять также заседания (каскадно), научного руководителя, если у него больше нет диссертаций и ФАЙЛЫ НА СЕРВЕРЕ
            return View();
        }

        //
        // POST: /Dissertation/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Dissertation/My

        public ActionResult My()
        {
            User currentUser = Session.GetUser();

            if ((currentUser is Applicant) == false)
                return HttpNotFound();

            var dissertations = db.Dissertations.Where(d => d.Applicant.Id == currentUser.Id);
            if (dissertations.Count() == 0)
                return RedirectToAction("Create");
            return RedirectToAction("Details", new { id = dissertations.Single().Id });
        }

        //
        // GET: /Dissertation/Download/5?type=Summary

        public ActionResult Download(int id, string type)
        {
            Dissertation currentDisserrtation = db.Dissertations.Find(id);
            if (!CanShow(currentDisserrtation))
                return HttpNotFound();

            string fileName = string.Empty;

            switch (type)
            {
                case "Abstract":
                    fileName = currentDisserrtation.Id + "_Abstract" + currentDisserrtation.File_Abstract;
                    break;
                case "Text":
                    fileName = currentDisserrtation.Id + "_Text" + currentDisserrtation.File_Abstract;
                    break;
                case "Summary":
                    fileName = currentDisserrtation.Id + "_Summary" + currentDisserrtation.File_Abstract;
                    break;
            }
            if (fileName.Length == 0)
                return HttpNotFound();

            return File("~/App_Data/" + fileName, "binary/octet-stream", fileName);
        }

        private bool CanShow(Dissertation dissertation)
        {
            if (dissertation != null)
            {
                User currentUser = Session.GetUser();
                if (currentUser == null)
                {
                    if (!dissertation.Defensed || dissertation.Administrative_Use)
                        return false;
                }
                else
                {
                    if ((currentUser is Applicant) && (currentUser.Id != dissertation.Applicant.Id))
                        if (!dissertation.Defensed || dissertation.Administrative_Use)
                            return false;
                }
                return true;
            }
            return false;
        }

        private bool CanEdit(Dissertation dissertation)
        {
            User currentUser = Session.GetUser();

            return !((currentUser is Applicant) && (currentUser.Id != dissertation.Applicant.Id));
        }

        private bool CanCreateSession()
        {
            return ((Session.GetUserPosition() == MemberPosition.Chairman) || (Session.GetUser() is Administrator));
        }
    }
}
