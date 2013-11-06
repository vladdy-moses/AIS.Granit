using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            var q = from u in db.Users where u.Role == UserRole.Administrator select u.User_Id;
            if (q.Count() == 0)
            {
                User u = new User() { Email = "v.moiseev94@gmail.com", FirstName = "Moiseev", SecondName = "Vladislav", Password = "123456", Role = UserRole.Administrator };
                db.Users.Add(u);
                db.SaveChanges();
            }

            return View();
        }
    }
}
