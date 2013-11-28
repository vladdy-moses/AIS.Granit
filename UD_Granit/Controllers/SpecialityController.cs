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
            if ((Session.GetUser() is Administrator) || (Session.GetUserPosition() == MemberPosition.Chairman))
                viewModel.CanControl = true;
            return View(viewModel);
        }

        //
        // GET: /Speciality/Details/5

        /*public ActionResult Details(int id)
        {
            return View();
        }*/

        //
        // GET: /Speciality/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Speciality/Create

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
        // GET: /Speciality/Edit/5

        public ActionResult Edit(string id)
        {
            return View();
        }

        //
        // POST: /Speciality/Edit/5

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
    }
}
