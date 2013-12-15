using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Council
{
    public class Edit
    {
        public UD_Granit.Models.Council Council { set; get; }

        public ICollection<System.Web.Mvc.SelectListItem> Members { set; get; }
        public bool CanDefineRoles { set; get; }

        [Display(Name="Председатель совета")]
        public int Chairman { set; get; }

        [Display(Name = "Заместитель председателя")]
        public int ViceChairman { set; get; }

        [Display(Name = "Учёный секретарь")]
        public int Secretary { set; get; }
    }
}