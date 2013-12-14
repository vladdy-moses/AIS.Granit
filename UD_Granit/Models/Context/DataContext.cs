using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
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
        {
            var dataBase = ConfigurationManager.AppSettings["UseLocalDatabase"];

            if ((dataBase != null) && (dataBase.ToLower() == "false"))
                this.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteConnection"].ConnectionString;
            else
                this.Database.Connection.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;
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
        }

        /*protected void InitDatabase()
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
        }*/
    }

    public class Initializer : IDatabaseInitializer<DataContext>
    {
        public void InitializeDatabase(DataContext context)
        {
            if (!context.Database.Exists() || !context.Database.CompatibleWithModel(false))
            {
                context.Database.Delete();
                context.Database.Create();

                context.Database.Connection.Open();
                DbCommand cmd = null;

                // Хранимая процедура, получающая все диссертации
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetDissertations]
AS
BEGIN
    SELECT D.* FROM [Dissertations] AS D ORDER BY D.Defensed ASC, D.[Type] DESC, D.Title ASC
END
";
                cmd.ExecuteNonQuery();

                // Хранимая процедура для получения списка членов совета по отраслям наук
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetMembersByScienceBranch]
	@scienceBranch nvarchar(128)
AS
	SELECT U.*, M.* FROM [Users] AS U INNER JOIN [guest].[Members] AS M ON U.Id = M.Id WHERE M.Speciality_Number IN (SELECT S.Number FROM Specialities AS S WHERE S.ScienceBranch = @scienceBranch)
RETURN 0
";
                cmd.ExecuteNonQuery();

                // Хранимая процедура для получения отраслей наук по зяявленным специальностям
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE PROCEDURE [dbo].[GetScienceBranches]
AS
	SELECT S.ScienceBranch FROM Specialities AS S GROUP BY S.ScienceBranch
RETURN 0
";
                cmd.ExecuteNonQuery();

                // Хранимая процедура для поиска диссертаций по названию
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE PROCEDURE [dbo].[FindDissertations]
	@phrase nvarchar(MAX)
AS
	SELECT D.* FROM Dissertations as D WHERE LOWER(D.Title) LIKE '%' + LOWER(@phrase) + '%'
RETURN 0
";
                cmd.ExecuteNonQuery();

                // Триггер для предотвращения дублирования научных руководителей 
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE TRIGGER ScientificDirectorSafeTrigger on ScientificDirectors
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @realyHave int;
	SELECT @realyHave = COUNT(*) FROM ScientificDirectors WHERE
		FirstName = (SELECT FirstName FROM inserted) AND
		SecondName = (SELECT SecondName FROM inserted) AND
		Organization = (SELECT Organization FROM inserted);

	IF @realyHave = 0
	BEGIN
		INSERT INTO ScientificDirectors SELECT FirstName, SecondName, LastName, Degree, Organization, Organization_Department, Organization_Post FROM inserted;
		SELECT TOP(1) * FROM ScientificDirectors WHERE Id = SCOPE_IDENTITY();
	END
	ELSE
		SELECT TOP(1) * FROM ScientificDirectors WHERE
		FirstName = (SELECT FirstName FROM inserted) AND
		SecondName = (SELECT SecondName FROM inserted) AND
		Organization = (SELECT Organization FROM inserted);
END;
";
                cmd.ExecuteNonQuery();

                // Транзакция для создания администратора
                cmd = context.Database.Connection.CreateCommand();
                cmd.CommandText = @"
BEGIN TRANSACTION;
	INSERT INTO [Users] (Email, Password, FirstName, SecondName, RegistrationDate) VALUES('admin@localhost', 'admin', 'Admin', 'Admin', GETDATE());
	INSERT INTO [Administrators] (Id, LastIP) VALUES(SCOPE_IDENTITY(), '-');
COMMIT TRANSACTION;
";
                cmd.ExecuteNonQuery();

                context.SaveChanges();

                var createData = ConfigurationManager.AppSettings["LoadExampleDataOnCreate"];
                if ((createData != null) && (createData.ToLower() == "true"))
                    FillExampleData(context);
            }
        }

        private void FillExampleData(DataContext context)
        {
            Speciality[] specialities = new Speciality[] {
                new Speciality() { Number = "05.13.11", Name = "Математическое и программное обеспечение вычислительных машин, комплексов и компьютерных сетей", ScienceBranch = "Технические науки" },
                new Speciality() { Number = "05.13.17", Name = "Теоретические основы информатики", ScienceBranch = "Технические науки" },
                new Speciality() { Number = "05.09.03", Name = "Электротехнические комплексы и системы", ScienceBranch = "Технические науки" },
                new Speciality() { Number = "01.01.07", Name = "Вычислительная математика", ScienceBranch = "Физико-математические науки" }
            };
            context.Specialities.AddRange(specialities);
            context.SaveChanges();

            Member[] members = new Member[] {
                new Member() { FirstName = "Иванов", SecondName = "Владимир", LastName = "Иванович", Email = "member1@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра математики", Organization_Position = "доцент", Degree = "Кандидат физико-математических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("01.01.07") },
                new Member() { FirstName = "Петров", SecondName = "Пётр", LastName = "Петрович", Email = "member2@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра математики", Organization_Position = "доцент", Degree = "Кандидат физико-математических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("01.01.07") },
                new Member() { FirstName = "Тхе", SecondName = "Чжонь", Email = "member3@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("05.09.03") },
                new Member() { FirstName = "Потапов", SecondName = "Михаил", LastName = "Потапович", Email = "member4@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "профессор", Degree = "Кандидат технических наук", Position = MemberPosition.Secretary, Speciality = context.Specialities.Find("05.09.03") },
                new Member() { FirstName = "Медовый", SecondName = "Николай", LastName = "Ильич", Email = "member5@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.ViceChairman, Speciality = context.Specialities.Find("05.13.11") },
                new Member() { FirstName = "Черпало", SecondName = "Алёна", LastName = "Валерьевна", Email = "member6@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("05.13.11") },
                new Member() { FirstName = "Ленон", SecondName = "Джон", Email = "member7@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра математики", Organization_Position = "доцент", Degree = "Кандидат физико-математических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("01.01.07") },
                new Member() { FirstName = "Вручтель", SecondName = "Серафима", LastName = "Вильямовна", Email = "member8@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("05.13.17") },
                new Member() { FirstName = "Нефёдов", SecondName = "Илья", LastName = "Евгеньевич", Email = "member9@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("05.13.17") },
                new Member() { FirstName = "Куцоконь", SecondName = "Ольга", LastName = "Вяльмисовна", Email = "member10@localhost", Password="password", RegistrationDate = DateTime.Now, Organization = "Ульяновский Государственный Технический Университет", Organization_Depatment = "Кафедра радиотехники", Organization_Position = "доцент", Degree = "Кандидат технических наук", Position = MemberPosition.Member, Speciality = context.Specialities.Find("05.13.17") }
            };
            context.Members.AddRange(members);
            context.SaveChanges();

            ScientificDirector director1 = new ScientificDirector() { FirstName = "Шишкин", SecondName = "Вадим", LastName = "Викторинович", Degree = "кандидат технических наук", Organization = "Ульяновский Государственный Технический Университет", Organization_Department = "Кафедра измерительно-вычислительных комплексов", Organization_Post = "профессор" };
            ScientificDirector director2 = new ScientificDirector() { FirstName = "Родионов", SecondName = "Виктор", LastName = "Викторович", Degree = "кандидат технических наук", Organization = "Ульяновский Государственный Технический Университет", Organization_Department = "Кафедра измерительно-вычислительных комплексов", Organization_Post = "доцент" };
            context.ScientificDirectors.Add(director1);
            context.ScientificDirectors.Add(director2);
            context.SaveChanges();

            Applicant[] applicants = new Applicant[] {
                new ApplicantCandidate() { FirstName = "Сорокин", SecondName = "Данила", LastName = "Алексеевич", Email = "applicant1@localhost", Password = "applicant", City = "Ульяносвк", Address = "ул. Серверный венец, д. 32", CandidateExams = "73 11 121454", DocumentOfEducation = "73 71 154525", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, Phone = "322-22-23", ScientificDirector = director1 },
                new ApplicantCandidate() { FirstName = "Загайчук", SecondName = "Иван", LastName = "Анатольевич", Email = "applicant2@localhost", Password = "applicant", City = "Самара", Address = "ул. Серверный венец, д. 32", CandidateExams = "73 31 121454", DocumentOfEducation = "73 01 156525", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, ScientificDirector = director2 },
                new ApplicantCandidate() { FirstName = "Загайчук", SecondName = "Алексей", LastName = "Анатольевич", Email = "applicant3@localhost", Password = "applicant", City = "Казань", Address = "ул. Серверный венец, д. 32", CandidateExams = "75 11 651454", DocumentOfEducation = "43 01 154325", Organization = "Казанский Федеральный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, Phone = "324-22-23", ScientificDirector = director1 },
                new ApplicantCandidate() { FirstName = "Желепов", SecondName = "Алексей", LastName = "Сергеевич", Email = "applicant4@localhost", Password = "applicant", City = "Ульяновск", Address = "ул. Серверный венец, д. 32", CandidateExams = "43 61 121484", DocumentOfEducation = "73 01 164525", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Birthday = DateTime.Now.AddYears(-28), RegistrationDate = DateTime.Now, WasInGraduateSchool = true, University = "Ульяновский Государственный Университет", University_Departmant = "Кафедра физики и философии", Phone = "542-82-25", ScientificDirector = director2 },
                new ApplicantCandidate() { FirstName = "Прохоров", SecondName = "Евгений", LastName = "Эдуардович", Email = "applicant5@localhost", Password = "applicant", City = "Ульяновск", Address = "ул. Серверный венец, д. 32", CandidateExams = "73 51 161444", DocumentOfEducation = "43 61 121484", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = true, University = "Ульяновский Государственный Университет", University_Departmant = "Кафедра физики и философии", ScientificDirector = director1 },
                new ApplicantDoctor() { FirstName = "Наумова", SecondName = "Екатерина", LastName = "Николаевна", Email = "applicant6@localhost", Password = "applicant", City = "Ульяносвк", Address = "ул. Серверный венец, д. 32", CandidateDiplom = "73 11 121454", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, Phone = "322-22-23", ScientificDirector = director1 },
                new ApplicantDoctor() { FirstName = "Синяков", SecondName = "Михаил", LastName = "Олегович", Email = "applicant7@localhost", Password = "applicant", City = "Самара", Address = "ул. Серверный венец, д. 32", CandidateDiplom = "73 31 121454", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, ScientificDirector = director2 },
                new ApplicantDoctor() { FirstName = "Авербух", SecondName = "Илья", LastName = "Моисеевич", Email = "applicant8@localhost", Password = "applicant", City = "Казань", Address = "ул. Серверный венец, д. 32", CandidateDiplom = "75 11 651454", Organization = "Казанский Федеральный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = false, Phone = "324-22-23", ScientificDirector = director1 },
                new ApplicantDoctor() { FirstName = "Подложнюк", SecondName = "Борис", LastName = "Константинович", Email = "applicant9@localhost", Password = "applicant", City = "Ульяновск", Address = "ул. Серверный венец, д. 32", CandidateDiplom = "43 61 121484", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Birthday = DateTime.Now.AddYears(-28), RegistrationDate = DateTime.Now, WasInGraduateSchool = true, University = "Ульяновский Государственный Университет", University_Departmant = "Кафедра физики и философии", Phone = "542-82-25", ScientificDirector = director2 },
                new ApplicantDoctor() { FirstName = "Шипатова", SecondName = "Мария", LastName = "Петровна", Email = "applicant10@localhost", Password = "applicant", City = "Ульяновск", Address = "ул. Серверный венец, д. 32", CandidateDiplom = "73 51 161444", Organization = "Ульяновский Государственный Университет", Organization_Depatment = "Кафедра информатики", Organization_Conclusion = "Соискатель обладает исключительными навыками в составлении алгоритмов.", RegistrationDate = DateTime.Now, WasInGraduateSchool = true, University = "Ульяновский Государственный Университет", University_Departmant = "Кафедра физики и философии", ScientificDirector = director1 }
            };
            context.Applicants.AddRange(applicants);
            context.SaveChanges();
        }
    }
}