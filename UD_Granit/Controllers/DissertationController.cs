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
            if (dissertation != null)
            {
#warning Разрешить просмотр для пользователей
                // if (!dissertation.Administrative_Use)
                return View(dissertation);
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
                    var dissertations = from d in db.Dissertations where d.Applicant.User_Id == currentUser.User_Id select d;
                    if (dissertations.Count() == 0)
                    {
                        NotificationManager nManager = new NotificationManager();
                        nManager.Notifies.Add(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Info, Message = "Заполните информацию о Вашей диссертации. Вы можете сделать это позже, также как и отредактировать информацию о ней." });
                        ViewBag.UserNotification = nManager;
                    }
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
            if ((Session.GetUser() is Applicant) == false)
                return HttpNotFound();

            try
            {
                Dissertation currentDissertation = viewModel.Dissertation;
                currentDissertation.Applicant_Id = Session.GetUser().User_Id;
                currentDissertation.File_Abstract = Path.GetExtension(viewModel.File_Abstract.FileName);
                currentDissertation.File_Text = Path.GetExtension(viewModel.File_Text.FileName);
                db.Dissertations.Add(currentDissertation);
                db.SaveChanges();

                viewModel.File_Abstract.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Dissertation_Id + "_Abstract" + currentDissertation.File_Abstract)));
                viewModel.File_Text.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", currentDissertation.Dissertation_Id + "_Text" + currentDissertation.File_Text)));

                return RedirectToAction("Details", new { id = currentDissertation.Dissertation_Id });
            }
            catch (Exception ex)
            {
                NotificationManager nManager = new NotificationManager();
                nManager.Notifies.Add(new NotificationManager.Notify() { Message = ex.Message, Type = NotificationManager.Notify.NotifyType.Error });
                ViewBag.UserNotification = nManager;
                return View();
            }
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
    }
}
