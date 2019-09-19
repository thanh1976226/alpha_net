using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student_history")]
    public  class edu_student_history
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string namechange { get; set; }
        public string oldvalue { get; set; }
        public string newvalue { get; set; }
        public int? userchange { get; set; }
        public DateTime? datechange { get; set; }
        public int? student_id { get; set; }
    }
}
