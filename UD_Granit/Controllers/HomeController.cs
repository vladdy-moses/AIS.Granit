using System;
using System.Collections.Generic;
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
            

            /*try
            {
                ApplicantDoctor ap = new ApplicantDoctor() { Address = "Lalka", City = "ulyanovsk", Birthday = DateTime.Now, Email = "v.mmm@asd.r", FirstName = "12313", Password = "ffsdf", LastName = "Vasya", University = "UlSTU", University_Departmant = "IVK", WasInGraduateSchool = true, Ph_D = false, Organization_Conclusion = "KG/AM", Organization_Depatment = "P51", SecondName = "YaLosharko", Organization = "УКБП", CandidateDiplom = "11 11 121121" };
                Dissertation ds = new Dissertation() { Administrative_Use = false, Date_Preliminary_Defense = DateTime.Now, Date_Sending = DateTime.Now, Publications = 5, Title = "тупая диссертация", Type = false };

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
            */
            /*ViewBag.asd = Request.GetUserIp();*/

            User currentUser = Session.GetUser();
            if (currentUser != null)
            {
                if (currentUser is Applicant)
                {
                    var dissertations = from d in db.Dissertations where d.Applicant.Id == currentUser.Id select d;
                    if(dissertations.Count() == 0)
                        ViewData.NotificationAdd(new NotificationManager.Notify() { Type = NotificationManager.Notify.NotifyType.Error, Message = "У Вас отсутствуют записи о Ваших диссертациях. Заведите запись о диссертации <a href=\"" + Url.Action("Create", "Dissertation") + "\">здесь</a>." });
                }
            }

            UD_Granit.ViewModels.Home.Index viewModel = new ViewModels.Home.Index();
            List<UD_Granit.ViewModels.Home.MemberView> memberList = new List<UD_Granit.ViewModels.Home.MemberView>();
            memberList.Add(new ViewModels.Home.MemberView() { FullName = "Василий", Position = "a" });
            memberList.Add(new ViewModels.Home.MemberView() { FullName = "Ваня", Position = "б" });
            memberList.Add(new ViewModels.Home.MemberView() { FullName = "Таня", Position = "d" });
#warning Добавить нормальный вывод в view
            viewModel.Members = memberList;

            return View(viewModel);
        }
    }
}
