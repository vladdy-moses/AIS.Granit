using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            return View();
        }

        //
        // GET: /Reply/Create/5

        public ActionResult Create(int id)
        {
            Dissertation dissertation = db.Dissertations.Find(id);
            if (dissertation == null)
                return HttpNotFound();
            return View();
        }

    }
}
