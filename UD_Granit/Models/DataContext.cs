using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("DefaultConnection")
        {
            var q = from u in this.Administrators select u.Id;
            if (q.Count() == 0)
            {
                InitDatabase();
            }
        }

        public DbSet<User> Users { set; get; }
        public DbSet<Applicant> Applicants { set; get; }
        //public DbSet<ApplicantCandidate> ApplicantsCandidates { set; get; }
        //public DbSet<ApplicantDoctor> ApplicantsDoctors { set; get; }
        public DbSet<Administrator> Administrators { set; get; }
        public DbSet<Member> Members { set; get; }

        public DbSet<Dissertation> Dissertations { set; get; }
        public DbSet<Reply> Replies { set; get; }

        public DbSet<Session> Sessions { set; get; }
        public DbSet<SessionDefence> SessionsDefence { set; get; }
        public DbSet<SessionСonsideration> SessionsСonsideration { set; get; }

        public DbSet<Council> Council { set; get; }

        public DbSet<Speciality> Specialities { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Applicant>().ToTable("Applicants");
            //modelBuilder.Entity<ApplicantCandidate>().ToTable("ApplicantCandidates");
            //modelBuilder.Entity<ApplicantDoctor>().ToTable("ApplicantDoctors");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");
            modelBuilder.Entity<Member>().ToTable("Members");
        }

        protected void InitDatabase()
        {
            this.Users.Add(new Administrator() { Email = "admin", Password = "admin", FirstName = "Администраторов", SecondName = "Администратор", LastName = "Администраторович", LastIP = "" });
            this.Users.Add(new Applicant() { Email = "applicant", Password = "applicant", FirstName = "Соискателев", SecondName = "Соискатель", Organization = "Тестовая", Organization_Depatment = "Тестовый", City = "Тестовый", Address = "Тестовая улица" });
            this.Users.Add(new Member() { Email = "member", Password = "member", FirstName = "Членов", SecondName = "Член", LastName = "Членович", Position = MemberPosition.Member, Degree = "К.Маг.Н." });
            this.SaveChanges();

            Database.Connection.Open();
            DbCommand cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"
CREATE PROCEDURE [dbo].[Procedure]
	@param1 int = 0,
	@param2 int
AS
	SELECT @param1, @param2
RETURN 0
";
            cmd.ExecuteNonQuery();
            this.SaveChanges();
        }
    }
}