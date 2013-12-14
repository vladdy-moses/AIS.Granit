using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Statistics
    {
        public int Dissertations { set; get; }
        public int DissertationsAdministrative { set; get; }
        public int Users { set; get; }
        public int Members { set; get; }
        public int Applicants { set; get; }
        public int Replies { set; get; }
        public int Sessions { set; get; }
        public int SessionsDefenced { set; get; }
        public int ScientificDirectors { set; get; }
    }
}