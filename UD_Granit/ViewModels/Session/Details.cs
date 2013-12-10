using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class Details
    {
        public UD_Granit.Models.Session Session { set; get; }
        public bool CanControl { set; get; }
    }
}