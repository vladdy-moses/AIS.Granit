using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Administrator : User
    {
        public string LastIP { set; get; }
    }
}