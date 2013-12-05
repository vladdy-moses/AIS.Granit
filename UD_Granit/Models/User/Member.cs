using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual ICollection<Session> Sessions { set; get; }
    }
}