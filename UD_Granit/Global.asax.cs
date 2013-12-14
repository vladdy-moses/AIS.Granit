using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace UD_Granit
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            System.Data.Entity.Database.SetInitializer<UD_Granit.Models.DataContext>(new UD_Granit.Models.Initializer());

            UD_Granit.Models.DataContext db = new UD_Granit.Models.DataContext();
            HttpContext.Current.Application["Name"] = (db.Council.Count() == 0) ? "Гранит" : db.Council.First().Number;
        }
    }
}