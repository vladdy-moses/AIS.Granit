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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { set; get; }

        [Required]
        [Display(Name = "Заседание состоялось")]
        public bool Was { set; get; }

        public virtual Dissertation Dissertation { set; get; }
        public virtual ICollection<Member> Members { set; get; }
    }
}