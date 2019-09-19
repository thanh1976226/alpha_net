using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_class_student")]
    public  class edu_class_student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? class_id { get; set; }
        public int? student_id { get; set; }
        public int? school_id { get; set; }
        public int? level_id { get; set; }
        public string Class { get; set; }
        public int? classSpecial { get; set; }
        public int? course_id { get; set; }
        public int? status { get; set; }
        public DateTime? create_time { get; set; }
        public string note { get; set; }
        public string file_id { get; set; }
        public int? status_fee { get; set; }
        public int? usercreate { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public DateTime? updatetime { get; set; }
    }
}
