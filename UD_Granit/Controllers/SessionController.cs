using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Models;
using UD_Granit.Helpers;

namespace UD_Granit.Controllers
{
    public class SessionController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Session/

        public ActionResult Index()
        {
            return HttpNotFound();
        }

        //
        // GET: /Session/Details/5

        public ActionResult Details(int id)
        {
            return HttpNotFound();
        }

        //
        // GET: /Session/CreateConsideration

        public ActionResult CreateConsideration(int id)
        {
            UD_Granit.ViewModels.Session.CreateConsideration viewModel = new ViewModels.Session.CreateConsideration();

            Dissertation currentDissertation = db.Dissertations.Find(id);
            if (currentDissertation == null)
                return HttpNotFound();

            viewModel.Dissertation_Id = id;
            viewModel.Dissertation_Title = currentDissertation.Title;
            viewModel.MemberList = new List<SelectListItem>();

            foreach (var member in db.Members)
            {
                viewModel.MemberList.Add(new SelectListItem() { Text = member.GetFullName(), Value = member.Id.ToString() });
            }

            return View(viewModel);
        }

        //
        // POST: /Session/CreateConsideration

        [HttpPost]
        public ActionResult CreateConsideration(UD_Granit.ViewModels.Session.CreateConsideration viewModel)
        {
            try
            {
#warning TODO
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
