using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Helpers;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    public class ReplyController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Reply/

        public ActionResult Index()
        {
            return HttpNotFound();
        }

        //
        // GET: /Reply/Create/5

        public ActionResult Create(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (!RightsManager.Reply.AddReply(Session.GetUser(), dissertation))
                return HttpNotFound();

            UD_Granit.ViewModels.Reply.Create viewModel = new ViewModels.Reply.Create();
            viewModel.Dissertation_Id = dissertation.Id;

            return View(viewModel);
        }

        //
        // POST: /Reply/Create/5

        [HttpPost]
        public ActionResult Create(ViewModels.Reply.Create viewModel)
        {
            Dissertation dissertation = db.Dissertations.Find(viewModel.Dissertation_Id);
            if (!RightsManager.Reply.AddReply(Session.GetUser(), dissertation))
                return HttpNotFound();

            Reply currentReply = viewModel.Reply;
            currentReply.Dissertation = dissertation;
            db.Replies.Add(currentReply);
            db.SaveChanges();

            return RedirectToAction("Details", "Dissertation", new { id = dissertation.Id });
        }

        //
        // GET: /Reply/Delete/5

        public ActionResult Delete(int id)
        {
            Reply currentReply = db.Replies.Find(id);
            Dissertation currentDissertation = currentReply.Dissertation;

            if (!RightsManager.Reply.AddReply(Session.GetUser(), currentDissertation))
                return HttpNotFound();

            db.Replies.Remove(currentReply);
            db.SaveChanges();
            return RedirectToAction("Details", "Dissertation", new { id = currentDissertation.Id });
        }
    }
}
