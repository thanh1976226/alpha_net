using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student_exam")]
    public class edu_student_exam
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? class_id { get; set; }
        public int exam_id { get; set; }
        public int? student_id { get; set; }
        public bool? status { get; set; }
        public string note { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public string note2 { get; set; }
        public DateTime test_date { get; set; }
        public string recommendations1 { get; set; }
        public string tecommendations2 { get; set; }
        public string lessons_attende { get; set; }
        public string hw_complete { get; set; }
        public bool? reported_result { get; set; }
        public int? course_eligible { get; set; }
        public int? exam_time { get; set; }
    }
}
