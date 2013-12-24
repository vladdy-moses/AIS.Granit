using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Helpers;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    // Управляет логикой по работе с отзывами
    public class ReplyController : Controller
    {
        private DataContext db = new DataContext();

        // Показывает форму добавления отзыва
        // GET: /Reply/Create/5

        public ActionResult Create(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (!RightsManager.Reply.Control(Session.GetUser(), dissertation))
                throw new HttpException(404, "Not found");

            UD_Granit.ViewModels.Reply.Create viewModel = new ViewModels.Reply.Create();
            viewModel.Dissertation_Id = dissertation.Id;

            return View(viewModel);
        }

        // Создаёт отзыв к диссертации
        // POST: /Reply/Create/5

        [HttpPost]
        public ActionResult Create(ViewModels.Reply.Create viewModel)
        {
            Dissertation dissertation = db.Dissertations.Find(viewModel.Dissertation_Id);
            if (!RightsManager.Reply.Control(Session.GetUser(), dissertation))
                throw new HttpException(404, "Not found");

            Reply currentReply = viewModel.Reply;
            currentReply.Dissertation = dissertation;
            db.Replies.Add(currentReply);
            db.SaveChanges();

            return RedirectToAction("Details", "Dissertation", new { id = dissertation.Id });
        }

        // Удаляет отзыв к диссертации
        // GET: /Reply/Delete/5

        public ActionResult Delete(int id)
        {
            Reply currentReply = db.Replies.Find(id);
            Dissertation currentDissertation = currentReply.Dissertation;

            if (!RightsManager.Reply.Control(Session.GetUser(), currentDissertation))
                throw new HttpException(404, "Not found");

            db.Replies.Remove(currentReply);
            db.SaveChanges();
            return RedirectToAction("Details", "Dissertation", new { id = currentDissertation.Id });
        }
    }
}
