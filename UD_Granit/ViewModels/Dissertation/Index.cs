using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Dissertation
{
    public class Index
    {
        public ICollection<Models.Dissertation> Dissertations { set; get; }
        public string SearchString { set; get; }
        public bool SearchHaveResults { set; get; }
    }
}