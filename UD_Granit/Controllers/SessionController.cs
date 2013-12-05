using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.Controllers
{
    public class SessionController : Controller
    {
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

#warning TODO
            viewModel.Dissertation_Id = 0;
            viewModel.Dissertation_Title = "";

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
