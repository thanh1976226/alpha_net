using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_main_exam")]
    public  class edu_main_exam
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? subject_id { get; set; }
        public DateTime? test_date { get; set; }
        public int? test_number { get; set; }
        public string mark { get; set; }
        public string note { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public string language { get; set; }
        public bool? flag { get; set; }
        public int? student_id { get; set; }
    }
}
