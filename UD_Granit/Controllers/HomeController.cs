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
            var q = from u in db.Administrators select u.User_Id;
            if (q.Count() == 0)
            {
                Administrator u = new Administrator() { Email = "v.moiseev94@gmail.com", FirstName = "Moiseev", SecondName = "Vladislav", Password = "123456", LastIP = GetUserIP() };
                db.Users.Add(u);
                db.SaveChanges();
            }

            try
            {
                ApplicantDoctor ap = new ApplicantDoctor() { Address = "Lalka", City = "ulyanovsk", Birthday = DateTime.Now, Email = "v.mmm@asd.r", FirstName = "12313", Password = "ffsdf", LastName = "Vasya", University = "UlSTU", University_Departmant = "IVK", WasInGraduateSchool = true, Ph_D = false, Organization_Conclusion = "KG/AM", Organization_Depatment = "P51", SecondName = "YaLosharko", Organization = "УКБП", CandidateDiplom = "11 11 121121" };
                Dissertation ds = new Dissertation() { Administrative_Use = false, Date_Preliminary_Defense = DateTime.Now, Date_Sending = DateTime.Now, Publications = 5, Title = "тупая диссертация", Type = false };

                //ap.Dissertation = ds;
                ds.Applicant = ap;

                db.ApplicantsDoctors.Add(ap);
                db.Dissertations.Add(ds);
                
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (System.Data.Entity.Validation.DbEntityValidationResult r in ex.EntityValidationErrors)
                {
                    if (r.IsValid == false)
                    {
                        foreach (System.Data.Entity.Validation.DbValidationError err in r.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine(err.ErrorMessage + "! ! " + err.PropertyName);
                        }
                    }
                }
            }

            return View();
        }


        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}
