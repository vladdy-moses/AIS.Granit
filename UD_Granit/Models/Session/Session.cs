using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Session
    {
        [Key]
        public int Id { set; get; }

        [Required]
        [Display(Name = "Дата проведения заседания")]
        public DateTime Date { set; get; }

        [Required]
        [Display(Name = "Заседание состоялось")]
        public bool Was { set; get; }

        public virtual Dissertation Dissertation { set; get; }
        public virtual ICollection<Member> Members { set; get; }
    }
}