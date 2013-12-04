using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Reply
{
    public class Create
    {
        [Required]
        public int Dissertation_Id { set; get; }

        [Required]
        public Models.Reply Reply { set; get; }
    }
}