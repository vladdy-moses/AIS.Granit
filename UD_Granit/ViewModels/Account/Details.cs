using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public class ApplicantViewItem
    {
        [Display(Name = "Город")]
        public string City { set; get; }

        [Display(Name = "Организация")]
        public string Organization { set; get; }

        [Display(Name = "Подразделение")]
        public string Organization_Depatment { set; get; }
    }

    public class MemberViewItem
    {
        [Display(Name = "Должность")]
        public string Position { set; get; }

        [Display(Name = "Специальность")]
        public UD_Granit.Models.Speciality Speciality { set; get; }
    }

    public class Details
    {
        public int User_Id { set; get; }

        [Display(Name = "Полное имя")]
        public string FullName { set; get; }

        [Display(Name = "Роль")]
        public string Role { set; get; }

        public ApplicantViewItem ApplicantView { set; get; }
        public MemberViewItem MemberView { set; get; }

        public bool CanControl { set; get; }
    }
}