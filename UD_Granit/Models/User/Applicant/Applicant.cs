using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UD_Granit.Models
{
    public class Applicant : User
    {
        [Required]
        [Display(Name = "Учётная запись активна")]
        public bool IsActive { set; get; }

        [Required]
        [Display(Name = "Наименование организации")]
        public string Organization { set; get; }

        [Required]
        [Display(Name = "Подразделение")]
        public string Organization_Depatment { set; get; }

        [Display(Name = "Заключение организации")]
        public string Organization_Conclusion { set; get; }

        [Display(Name = "День рождения")]
        public DateTime? Birthday { set; get; }

        [Display(Name = "Учёная степень")]
        public bool Ph_D { set; get; }

        [Required]
        [Display(Name = "Город")]
        public string City { set; get; }

        [Required]
        [Display(Name = "Адрес")]
        public string Address { set; get; }

        [Required]
        [Display(Name = "Обучался в аспирантуре?")]
        public bool WasInGraduateSchool { set; get; }

        [Display(Name = "Высшее учебное заведение, где проходил аспирантуру")]
        public string University { set; get; }

        [Display(Name = "Подразделение, где проходил аспирантуру")]
        public string University_Departmant { set; get; }

        public virtual Dissertation Dissertation { set; get; }

        [Required]
        [Display(Name = "Научный руководитель")]
        public virtual ScientificDirector ScientificDirector { set; get; }
    }
}