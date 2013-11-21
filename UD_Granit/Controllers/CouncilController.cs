using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    public class CouncilController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Council/

        public ActionResult Index()
        {
            return View((db.Council.Count() > 0) ? db.Council.First() : null);
        }

        //
        // GET: /Council/Edit

        public ActionResult Edit()
        {
            User currentUser = this.GetUser();

            if (currentUser == null)
                return HttpNotFound();
            if ((currentUser is Administrator) ||
                ((currentUser is Member) && ((currentUser as Member).Position == MemberPosition.Chairman)))
                return View((db.Council.Count() > 0) ? db.Council.First() : null);
            return HttpNotFound();
        }

        //
        // POST: /Council/Edit

        [HttpPost]
        public ActionResult Edit(Council council)
        {
            try
            {
                if (db.Council.Count() == 0)
                {
                    db.Council.Add(council);
                }
                else
                {
                    council.Council_Id = 1;
                    db.Entry(council).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
