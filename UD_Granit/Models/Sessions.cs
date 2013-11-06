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
        public int Session_Id { set; get; }

        public DateTime Date { set; get; }
        public bool Was { set; get; }
        //идентификатор диссертации
    }

    public class SessionСonsideration : Session
    {
        public string Result { set; get; }
    }

    public class SessionDefence : Session
    {
        public bool Result { set; get; }
        public int Vote_Result { set; get; }
        public byte[] Recording { set; get; }
        public string Reliability { set; get; }
        public string Novelty { set; get; }
        public string Significance { set; get; }
    }
}