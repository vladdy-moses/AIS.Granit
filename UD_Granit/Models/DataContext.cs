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
            var q = from u in this.Administrators select u;
            if (q.Count() == 0)
            {
                InitDatabase();
            }
        }

        public DbSet<User> Users { set; get; }
        public DbSet<Applicant> Applicants { set; get; }
        public DbSet<ApplicantCandidate> ApplicantCandidates { set; get; }
        public DbSet<ApplicantDoctor> ApplicantDoctors { set; get; }
        public DbSet<Administrator> Administrators { set; get; }
        public DbSet<Member> Members { set; get; }

        public DbSet<Dissertation> Dissertations { set; get; }
        public DbSet<Reply> Replies { set; get; }
        public DbSet<ScientificDirector> ScientificDirectors { set; get; }

        public DbSet<Session> Sessions { set; get; }
        public DbSet<SessionDefence> SessionsDefence { set; get; }
        public DbSet<SessionConsideration> SessionsСonsideration { set; get; }

        public DbSet<Council> Council { set; get; }

        public DbSet<Speciality> Specialities { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Applicant>().ToTable("Applicants");
            modelBuilder.Entity<ApplicantCandidate>().ToTable("ApplicantCandidates");
            modelBuilder.Entity<ApplicantDoctor>().ToTable("ApplicantDoctors");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");
            modelBuilder.Entity<Member>().ToTable("Members", "guest");

            modelBuilder.Entity<Session>().ToTable("Sessions");
            modelBuilder.Entity<SessionDefence>().ToTable("SessionsDefence");
            modelBuilder.Entity<SessionConsideration>().ToTable("SessionsСonsideration");

            modelBuilder.Entity<Council>().ToTable("Council", "guest");

            modelBuilder.Entity<Applicant>().HasOptional(t => t.Dissertation).WithRequired(t => t.Applicant);
            //modelBuilder.Entity<Member>().HasRequired(t => t.Speciality).WithOptional().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Speciality>().HasOptional(t => t.Members).WithOptionalDependent().WillCascadeOnDelete(false);
            /*modelBuilder.Entity<Session>().HasMany(s => s.Members).WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("SessionId");
                    m.MapRightKey("UserId");
                    m.ToTable("SessionMember");
                });*/
        }

        protected void InitDatabase()
        {
            Speciality s = new Speciality() { Number = "10.001.2001", Name = "Информатика и вычислительная техника", ScienceBranch = "Технические науки" };
            this.Specialities.Add(s);
            this.Specialities.Add(new Speciality() { Number = "15.001.2001", Name = "Конфликтология", ScienceBranch = "Философские науки" });
            this.SaveChanges();

            ScientificDirector sd = new ScientificDirector() { Degree = "Кандидат магических наук", FirstName = "Руководтелев", SecondName = "Руководитель", Organization = "СГУ", Organization_Department = "КНИиТ", Organization_Post = "Профессор" };
            this.ScientificDirectors.Add(sd);

            this.Users.Add(new Administrator() { Email = "admin", Password = "admin", FirstName = "Администраторов", SecondName = "Администратор", LastName = "Администраторович", LastIP = "", RegistrationDate = DateTime.Now });
            this.Users.Add(new ApplicantCandidate() { Email = "applicant", Password = "applicant", FirstName = "Соискателев", SecondName = "Соискатель", Organization = "Тестовая", Organization_Depatment = "Тестовый", City = "Тестовый", Address = "Тестовая улица", RegistrationDate = DateTime.Now, Phone = "32-32-23", ScientificDirector = sd });
            this.Users.Add(new Member() { Email = "member1", Password = "member", FirstName = "Родионов", SecondName = "Виктор", LastName = "Викторович", Position = MemberPosition.Member, Degree = "К.Маг.Н.", Speciality = s, Organization = "УлГТУ", Organization_Depatment = "ИВК", Organization_Position = "доцент", RegistrationDate = DateTime.Now });
            this.Users.Add(new Member() { Email = "member2", Password = "member", FirstName = "Шишкин", SecondName = "Вадим", LastName = "Викторинович", Position = MemberPosition.Chairman, Degree = "К.Т.Н.", Speciality = s, Organization = "УлГТУ", Organization_Depatment = "ИВК", Organization_Position = "профессор", RegistrationDate = DateTime.Now, Phone = "32-22-23" });
            this.SaveChanges();

            Database.Connection.Open();
            DbCommand cmd = null;
            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetDissertations]
AS
BEGIN
    SELECT D.* FROM [Dissertations] AS D ORDER BY D.Defensed ASC, D.[Type] DESC, D.Title ASC
END
";
            cmd.ExecuteNonQuery();


            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetMembersBySpeciality]
	@speciality nvarchar(128)
AS
	SELECT U.*, M.* FROM [Users] AS U INNER JOIN [guest].[Members] AS M ON U.Id = M.Id WHERE M.Speciality_Number = @speciality
RETURN 0
";
            cmd.ExecuteNonQuery();

            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetMembersByScienceBranch]
	@scienceBranch nvarchar(128)
AS
	SELECT U.*, M.* FROM [Users] AS U INNER JOIN [guest].[Members] AS M ON U.Id = M.Id WHERE M.Speciality_Number IN (SELECT S.Number FROM Specialities AS S WHERE S.ScienceBranch = @scienceBranch)
RETURN 0
";
            cmd.ExecuteNonQuery();

            cmd = Database.Connection.CreateCommand();
            cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetScienceBranches]
AS
	SELECT S.ScienceBranch FROM Specialities AS S GROUP BY S.ScienceBranch
RETURN 0
";
            cmd.ExecuteNonQuery();

            this.SaveChanges();
        }
    }
}