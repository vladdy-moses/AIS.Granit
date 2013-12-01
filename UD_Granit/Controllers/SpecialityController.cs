using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Helpers;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    public class SpecialityController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Speciality/

        public ActionResult Index()
        {
            UD_Granit.ViewModels.Speciality.Index viewModel = new ViewModels.Speciality.Index();
            viewModel.Specialities = db.Specialities;
            viewModel.CanControl = CanControl();

            return View(viewModel);
        }

        //
        // GET: /Speciality/Create

        public ActionResult Create()
        {
            if (!CanControl())
                return HttpNotFound();

            return View();
        }

        //
        // POST: /Speciality/Create

        [HttpPost]
        public ActionResult Create(UD_Granit.ViewModels.Speciality.Create viewModel)
        {
            if (!CanControl())
                return HttpNotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    if (db.Specialities.Find(viewModel.Speciality.Number) == null)
                    {
                        db.Specialities.Add(viewModel.Speciality);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Message = "Специальность с таким номером уже существует.", Type = NotificationManager.Notify.NotifyType.Error });
                        return View(viewModel);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Message = ex.Message, Type = NotificationManager.Notify.NotifyType.Error });
                return View();
            }
        }

        //
        // GET: /Speciality/Edit/5

        public ActionResult Edit(string id)
        {
            if (CanControl())
            {
                UD_Granit.ViewModels.Speciality.Edit viewModel = new ViewModels.Speciality.Edit();
                viewModel.Speciality = db.Specialities.Find(id);
                if (viewModel.Speciality != null)
                    return View(viewModel);
            }
            return HttpNotFound();
        }

        //
        // POST: /Speciality/Edit/5

        [HttpPost]
        public ActionResult Edit(UD_Granit.ViewModels.Speciality.Edit viewModel)
        {
            if (!CanControl())
                return HttpNotFound();

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry<Speciality>(viewModel.Speciality).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Message = ex.Message, Type = NotificationManager.Notify.NotifyType.Error });
                return View();
            }
        }

        //
        // GET: /Speciality/Delete/5

        public ActionResult Delete(string id)
        {
#warning Удалять только те, к которым не прикреплено что-либо (процедура)
            return HttpNotFound();
        }

        //
        // POST: /Speciality/Delete/5

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

        private bool CanControl()
        {
            return ((Session.GetUser() is Administrator) || (Session.GetUserPosition() == MemberPosition.Chairman)) ? true : false;
        }
    }
}
