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
        public DateTime Date { set; get; }

        [Required]
        public bool Was { set; get; }

        [Required]
        [ForeignKey("Dissertation")]
        public int Dissertation_Id { get; set; }
        public virtual Dissertation Dissertation { set; get; }

        public virtual ICollection<Member> Members { set; get; }
    }
}