using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Dissertation
    {
        [Key]
        public int Dissertation_Id { private set; get; }

        /// <summary>
        /// Тип диисертации
        /// </summary>
        public bool Type { set; get; }

        public string Title { set; get; }

        public byte[] File_Abstract { set; get; }

        public byte[] File_Text { set; get; }

        /// <summary>
        /// Список литературы
        /// </summary>
        public string References { set; get; }

        public bool Administrative_Use { set; get; }

        public byte[] Organization_Summary { set; get; }

        public int Publications { set; get; }

        public DateTime Date_Sending { set; get; }

        public DateTime Date_Preliminary_Defense { set; get; }

        public virtual ICollection<Reply> Replies { set; get; }
    }
}