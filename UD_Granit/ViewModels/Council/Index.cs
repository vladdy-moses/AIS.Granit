using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Council
{
    public class Index
    {
        public bool HaveInformation { set; get; }
        public UD_Granit.Models.Council Council { set; get; }
        public bool CanControl { set; get; }
    }
}