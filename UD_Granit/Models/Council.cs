using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Council
    {
        [Key]
        public int Council_Id { set; get; }

        [Required]
        [Display(Name = "Название")]
        public string Name { set; get; }
    }
}