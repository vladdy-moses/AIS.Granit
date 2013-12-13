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
        // GET: /Speciality/[1]

        public ActionResult Index(int? err)
        {
            UD_Granit.ViewModels.Speciality.Index viewModel = new ViewModels.Speciality.Index();
            viewModel.Specialities = db.Specialities;
            viewModel.CanControl = CanControl();

            if (err.HasValue && (err.Value == 1))
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Специальность связана с диссертациями и (или) членами совета. Удаление невозможно." });

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
            if(!CanControl())
                return HttpNotFound();

            Speciality currentSpeciality = db.Specialities.Find(id);
            int err;

            if(currentSpeciality == null)
                return HttpNotFound();

            if ((db.Members.Where(m => m.Speciality.Number == id).Count() == 0) && (db.Dissertations.Where(d => d.Speciality.Number == id).Count() == 0))
            {
                err = 0;
                db.Specialities.Remove(currentSpeciality);
                db.SaveChanges();
            }
            else
            {
                err = 1;
            }

            return RedirectToAction("Index", new { err = err });
        }

        private bool CanControl()
        {
            return ((Session.GetUser() is Administrator) || (Session.GetUserPosition() == MemberPosition.Chairman)) ? true : false;
        }
    }
}
