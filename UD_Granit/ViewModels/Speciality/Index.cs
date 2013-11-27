using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Speciality
{
    public class Index
    {
        public IEnumerable<UD_Granit.Models.Speciality> Specialities;
        public bool CanControl { set; get; }
    }
}