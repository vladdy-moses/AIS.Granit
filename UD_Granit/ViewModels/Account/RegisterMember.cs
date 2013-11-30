using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.ViewModels.Account
{
    public enum MemberPositionWithoutChairman
    {
        [Description("Рядовой член совета")]
        Member = MemberPosition.Member,
        [Description("Учёный секретарь")]
        Secretary = MemberPosition.Secretary,
        [Description("Заместитель председателя")]
        ViceChairman = MemberPosition.ViceChairman
    }

    public class RegisterMember
    {
        public Member User { set; get; }

        [Required]
        [Display(Name = "Должность")]
        public MemberPositionWithoutChairman Position { set; get; }

        [Required]
        [Display(Name = "Специальность")]
        public string Speciality { set; get; }
    }
}