using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Account
{
    public class Register
    {
        public bool CanRegisterApplicant { set; get; }
        public bool CanRegisterMember { set; get; }
        public bool CanRegisterAdministrator { set; get; }
    }
}