using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_hocbu")]
    public class edu_hocbu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int class_idold { get; set; }
        public int? student_id { get; set; }
        public string dayold { get; set; }
        public int? class_idnew { get; set; }
        public string daynew { get; set; }
        public string timestart { get; set; }
        public string timerend { get; set; }
        public int? usercreate { get; set; }
        public string note { get; set; }
        public bool? flag { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public string language { get; set; }
        public bool? status { get; set; }
        public string skill_name { get; set; }
    }
}
