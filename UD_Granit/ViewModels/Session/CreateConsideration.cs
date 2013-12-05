using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class CreateConsideration
    {
        public int Dissertation_Id { set; get; }
        public string Dissertation_Title { set; get; }
        public UD_Granit.Models.SessionСonsideration Session { set; get; }
    }
}