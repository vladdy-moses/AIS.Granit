using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") {
            var q = from u in this.Administrators select u.User_Id;
            if (q.Count() == 0)
            {
                Administrator u = new Administrator() { Email = "v.moiseev94@gmail.com", FirstName = "Moiseev", SecondName = "Vladislav", Password = "123456", LastIP = "" };
                this.Users.Add(u);
                this.SaveChanges();
            }
        }

        public DbSet<User> Users { set; get; }
        public DbSet<Applicant> Applicants { set; get; }
        public DbSet<ApplicantCandidate> ApplicantsCandidates { set; get; }
        public DbSet<ApplicantDoctor> ApplicantsDoctors { set; get; }
        public DbSet<Administrator> Administrators { set; get; }

        public DbSet<Dissertation> Dissertations { set; get; }
        public DbSet<Reply> Replies { set; get; }

        public DbSet<Session> Sessions { set; get; }
        public DbSet<SessionDefence> SessionsDefence { set; get; }
        public DbSet<SessionСonsideration> SessionsСonsideration { set; get; }

        public DbSet<Council> Council { set; get; }
    }
}