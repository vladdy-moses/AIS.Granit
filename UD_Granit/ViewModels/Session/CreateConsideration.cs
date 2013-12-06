using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UD_Granit.ViewModels.Session
{
    public class CreateConsideration
    {
        public int Dissertation_Id { set; get; }
        public string Dissertation_Title { set; get; }

        public UD_Granit.Models.SessionСonsideration Session { set; get; }

        public List<SelectListItem> MemberList { set; get; }
        public int MembersCount { set; get; }
    }
}