using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student_absence")]
    public  class edu_student_absence
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_AppointmentTeach { get; set; }
        public int? id_Student { get; set; }
        public int? id_Teacher { get; set; }
        public int? id_Class { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public bool? flag { get; set; }
        public DateTime? start { get; set; }
        public DateTime? finish { get; set; }
        public int? lesson_of_class { get; set; }
        public int? reason_absence { get; set; }
        public string reason_absence_code { get; set; }
        public bool? report { get; set; }
        public string result_report { get; set; }
    }
}
