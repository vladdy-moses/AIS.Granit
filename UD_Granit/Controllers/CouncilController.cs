using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;
using System.Web.Mvc.Html;

namespace UD_Granit.Controllers
{
    public class CouncilController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Council/

        public ActionResult Index()
        {
            UD_Granit.ViewModels.Council.Index viewModel = new ViewModels.Council.Index();
            viewModel.HaveInformation = (db.Council.Count() > 0);
            if (viewModel.HaveInformation)
                viewModel.Council = db.Council.First();
            if (Session.GetUser() is Administrator)
                viewModel.CanControl = true;
            return View(viewModel);
        }

        //
        // GET: /Council/Edit

        public ActionResult Edit()
        {
            User currentUser = Session.GetUser();

            if (currentUser == null)
                return HttpNotFound();
            if ((currentUser is Administrator) ||
                (Session.GetUserPosition() == MemberPosition.Chairman))
            {
                UD_Granit.ViewModels.Council.Edit viewModel = new ViewModels.Council.Edit();
                if (db.Council.Count() > 0)
                    viewModel.Council = db.Council.First();
                return View(viewModel);
            }
            return HttpNotFound();
        }

        //
        // POST: /Council/Edit

        [HttpPost]
        public ActionResult Edit(UD_Granit.ViewModels.Council.Edit viewModel)
        {
            try
            {
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Council/Members

        public ActionResult Members()
        {
            UD_Granit.ViewModels.Council.Members viewModel = new ViewModels.Council.Members();

            var q = from m in db.Members orderby m.Position descending select m;
            List<UD_Granit.ViewModels.Council.MemberView> memberList = new List<ViewModels.Council.MemberView>();
            foreach (Member m in q)
            {
                memberList.Add(new UD_Granit.ViewModels.Council.MemberView() { Name = m.GetFullName(), Degree = m.Degree, Position = m.Position.ToDescription(), Speciality = m.Speciality.GetFullName(), Id = m.Id });
            }

            viewModel.CouncilMembers = memberList;

            viewModel.CanControl = false;
            if ((Session.GetUser() is Administrator) || (Session.GetUserPosition() == MemberPosition.Chairman))
                viewModel.CanControl = true;

            return View(viewModel);
        }
    }
}
