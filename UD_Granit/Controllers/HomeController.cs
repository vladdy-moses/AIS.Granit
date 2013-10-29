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
        //
        // GET: /Home/

        public ActionResult Index()
        {
            DataContext d = new DataContext();
            d.Users.Add(new User() { Email = "1@2.3", Password = "123", FirstName = "Ivanov", SecondName = "Vasya" });
            d.SaveChanges();
            d.Dispose();

            return View();
        }
    }
}
