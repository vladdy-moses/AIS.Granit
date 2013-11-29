using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Speciality
{
    public class Create
    {
        [Required]
        public UD_Granit.Models.Speciality Speciality { set; get; }
    }
}