using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UD_Granit.Models;

namespace UD_Granit.ViewModels.Home
{
    public struct MemberView
    {
        public string FullName;
        public string Position;
    }

    public class Index
    {
        public IEnumerable<Models.Session> SessionsWill { set; get; }
        public IEnumerable<Models.Session> SessionsWas { set; get; }
        public IEnumerable<MemberView> Members { set; get; }
        public bool HaveExampleData { set; get; }
    }
}