using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public enum MemberPosition : int
    {
        [Description("Рядовой член")]
        Member = 1,
        [Description("Учёный секретарь")]
        Secretary = 2,
        [Description("Заместитель председателя")]
        ViceChairman = 3,
        [Description("Председатель")]
        Chairman = 4
    }

    public class User
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Электронный адрес")]
        public string Email { set; get; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { set; get; }

        [Required]
        [Display(Name = "Фамилия")]
        public string FirstName { set; get; }

        [Required]
        [Display(Name = "Имя")]
        public string SecondName { set; get; }

        [Display(Name = "Отчество")]
        public string LastName { set; get; }

        [Required]
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { set; get; }

        [Display(Name = "Контактный телефон")]
        public string Phone { set; get; }
    }

    public class Applicant : User
    {
        [Required]
        [Display(Name = "Учётная запись активна")]
        public bool IsActive { set; get; }

        [Required]
        [Display(Name = "Наименование организации")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение")]
        public string Organization_Depatment { set; get; }
        
        [Display(Name = "Заключение организации")]
        public string Organization_Conclusion { set; get; }
        
        [Display(Name = "День рождения")]
        public DateTime? Birthday { set; get; }
        
        [Display(Name = "Учёная степень")]
        public bool Ph_D { set; get; }

        [Required]
        [Display(Name = "Город")]
        public string City { set; get; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { set; get; }

        [Required]
        [Display(Name = "Обучался в аспирантуре?")]
        public bool WasInGraduateSchool { set; get; }

        [Display(Name = "Высшее учебное заведение, где проходил аспирантуру")]
        public string University { set; get; }

        [Display(Name = "Подразделение, где проходил аспирантуру")]
        public string University_Departmant { set; get; }

        public virtual ICollection<Dissertation> Dissertations { set; get; }
    }

    public class Administrator : User
    {
        public string LastIP { set; get; }
    }

    public class Member : User
    {
        [Required]
        [Display(Name = "Должность в совете")]
        public MemberPosition Position { set; get; }

        [Display(Name = "Учёная степень")]
        public string Degree { set; get; }

        [Required]
        [Display(Name = "Наименование организации")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение")]
        public string Organization_Depatment { set; get; }

        [Display(Name = "Должность в организации")]
        public string Organization_Position { set; get; }

        [Required]
        [Display(Name = "Специальность")]
        public virtual Speciality Speciality { set; get; }
    }
}