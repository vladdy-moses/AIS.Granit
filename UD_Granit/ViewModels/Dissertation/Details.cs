using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Dissertation
{
    public class Details
    {
        public UD_Granit.Models.Dissertation Dissertation { set; get; }

        public bool CanEdit { set; get; }
        public bool CanCreateSession { set; get; }
    }
}