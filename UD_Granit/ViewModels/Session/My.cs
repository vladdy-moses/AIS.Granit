using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public enum SessionViewType
    {
        ApplicantView,
        MemberView
    }

    public class My
    {
        public IEnumerable<Models.Session> Sessions { set; get; }
        public SessionViewType ViewType { set; get; }
    }
}