using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Session
    {
        [Key]
        public int Id { set; get; }

        public DateTime Date { set; get; }
        public bool Was { set; get; }
        //идентификатор диссертации
    }
}