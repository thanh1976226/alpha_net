using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_rollUp_student")]
    public class edu_rollUp_student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? student_id { get; set; }
        public int? reason_absence { get; set; }
        public string reason_absence_code { get; set; }
        public bool? report { get; set; }
        public string result_report { get; set; }
        public int? rollUp_id { get; set; }
        public bool? status { get; set; }
        public int? class_id { get; set; }
    }
}
