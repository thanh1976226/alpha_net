using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_course")]
    public class edu_course
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string course_name { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public string note { get; set; }
        public int? coursetype { get; set; }
        public double? price { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public string course_code { get; set; }
        public int? teacher_id { get; set; }
        public int? min_member { get; set; }
        public int? totalday { get; set; }
        public int? place { get; set; }
        public int? level { get; set; }
        public int? number_lesson { get; set; }
    }
}
