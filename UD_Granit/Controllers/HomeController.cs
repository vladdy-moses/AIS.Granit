using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Helpers;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            User currentUser = Session.GetUser();
            if (currentUser != null)
            {
                if (currentUser is Applicant)
                {
                    var dissertations = from d in db.Dissertations where d.Applicant.Id == currentUser.Id select d;
                    if(dissertations.Count() == 0)
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "У Вас отсутствуют запись о Вашей диссертациях. Заведите запись о диссертации <a href=\"" + Url.Action("Create", "Dissertation") + "\">здесь</a>." });
                }
            }

            /*var members = db.Database.SqlQuery<Member>("GetMembersBySpeciality @speciality", new SqlParameter("speciality", "10.001.2001"));
            foreach (var member in members)
            {
                string nya = member.GetFullName();
            }*/

            UD_Granit.ViewModels.Home.Index viewModel = new ViewModels.Home.Index();
            return View(viewModel);
        }
    }
}
