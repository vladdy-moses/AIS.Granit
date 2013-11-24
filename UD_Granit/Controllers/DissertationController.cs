using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;

namespace UD_Granit.Controllers
{
    public class DissertationController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Default1/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Default1/Details/5

        public ActionResult Details(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (dissertation != null)
            {
                #warning Разрешить просмотр для пользователей
                if(!dissertation.Administrative_Use)
                    return View(dissertation);
            }
            return HttpNotFound();
        }

        //
        // GET: /Default1/Create

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
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Default1/Edit/5

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
        // GET: /Default1/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Default1/Delete/5

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
