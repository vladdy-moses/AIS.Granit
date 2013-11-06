using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public enum UserRole : int
    {
        Applicant = 1,
        Member = 2,
        Administrator = 3
    };

    public enum MemberPosition : int
    {
        Member = 1,
        Secretary = 2,
        ViceChairman = 3,
        Chairman = 4
    }

    public class User
    {
        [Key]
        public int User_Id { set; get; }

        [Required]
        public string Email { set; get; }
        [Required]
        public string Password { set; get; }

        [Required]
        public string FirstName { set; get; }
        [Required]
        public string SecondName { set; get; }
        public string LastName { set; get; }
    }

    public class Applicant : User
    {
        public string Organization { set; get; }
        public string Organization_Depatment { set; get; }
        public string Organization_Conclusion { set; get; }
        public DateTime Birthday { set; get; }
        public bool Ph_D { set; get; }
        //Идентификатор руководителя
        public string City { set; get; }
        public string Address { set; get; }
        public bool WasInGraduateSchool { set; get; }
        public string University { set; get; }
        public string University_Departmant { set; get; }
        //[Required]
        public Dissertation Dissertation { set; get; }
    }

    public class Administrator : User
    {
        public string LastIP { set; get; }
    }

    public class Member : User
    {
        public MemberPosition Position { set; get; }
        //public bool Ph_D { set; get; }
        //номер специальности
    }

    public class ApplicantCandidate : Applicant
    {
        public string DocumentOfEducation { set; get; }
        public string CandidateExams { set; get; }
    }

    public class ApplicantDoctor : Applicant
    {
        public string CandidateDiplom { set; get; }
    }
}