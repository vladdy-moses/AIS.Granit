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
        //
        // GET: /Council/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Council/Edit

        public ActionResult Edit()
        {
            return View();
        }

        //
        // POST: /Council/Edit

        [HttpPost]
        public ActionResult Edit(Council council)
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
    }
}
