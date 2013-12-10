using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class Index
    {
        public ICollection<Models.Session> SessionsWas { set; get; }
        public ICollection<Models.Session> SessionsWill { set; get; }
    }
}