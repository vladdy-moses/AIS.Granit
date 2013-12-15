using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UD_Granit.Helpers;
using UD_Granit.Models;

namespace UD_Granit.Controllers
{
    // Управляет логикой по отображению относительно статичных страниц
    public class HomeController : Controller
    {
        private DataContext db = new DataContext();

        // Показывает главную страницу
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
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "У Вас отсутствуют запись о Вашей диссертации. Заведите запись о диссертации <a href=\"" + Url.Action("Create", "Dissertation") + "\">здесь</a>." });
                }
            }

            var prevSessions = db.Sessions.Where(s => (!s.Dissertation.Administrative_Use && s.Was)).OrderByDescending(s => s.Date).Take(5);
            var nextSessions = db.Sessions.Where(s => (!s.Dissertation.Administrative_Use && !s.Was && (s.Date > DateTime.Now))).OrderBy(s => s.Date).Take(5);

            UD_Granit.ViewModels.Home.Index viewModel = new ViewModels.Home.Index();
            viewModel.SessionsWas = prevSessions;
            viewModel.SessionsWill = nextSessions;

            var createData = ConfigurationManager.AppSettings["LoadExampleDataOnCreate"];
            if ((createData != null) && (createData.ToLower() == "true"))
                viewModel.HaveExampleData = true;

            return View(viewModel);
        }

        // Показывает страницу "О системе"
        // GET: /Home/About

        public ActionResult About()
        {
            return View();
        }

        // Показывает страницу со статистикой
        // GET: /Home/Statistics

        public ActionResult Statistics()
        {
            ViewModels.Home.Statistics viewModel = new ViewModels.Home.Statistics();
            try
            {
                viewModel.Current = db.Database.SqlQuery<Statistics>("SELECT * FROM [dbo].[StatisticsFunction]()").FirstOrDefault();
            }
            catch
            {
                ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "Произошла ошибка при получении статистических данных. Обратитесь к администратору системы." });
                viewModel.Current = new Statistics();
            }
            return View(viewModel);
        }
    }
}
