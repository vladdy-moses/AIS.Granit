using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Reply
    {
        [Key]
        public int Reply_Id { set; get; }

        public string Author { set; get; }
        
        public string Organization { set; get; }
        public string Departmant { set; get; }

        public bool Ph_D { set; get; }

        public string Text { set; get; }

        [ForeignKey("Dissertation")]
        public int Dissertation_Id { get; set; }
        public virtual Dissertation Dissertation { set; get; }
    }
}