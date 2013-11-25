using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public class Details
    {
        public int User_Id { set; get; }
        public string FullName { set; get; }
        public string Role { set; get; }
        public bool CanControl { set; get; }
    }
}