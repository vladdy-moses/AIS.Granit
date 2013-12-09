using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.ViewModels.Session
{
    public class ResultDefence
    {
        public int Id { set; get; }

        [Required]
        public bool Result { set; get; }

        [Required]
        public int Vote_Result { set; get; }

#warning Файлы!
        /*public byte[] Recording { set; get; }*/

        [Required]
        public string Reliability { set; get; }

        [Required]
        public string Novelty { set; get; }

        [Required]
        public string Significance { set; get; }

        public string DissertationTitle { set; get; }
        public DateTime Date { set; get; }
    }
}