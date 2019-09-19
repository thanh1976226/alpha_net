using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_classChange")]
    public  class edu_classChange
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public int? student_id { get; set; }
        public int? class_old { get; set; }
        public int? class_new { get; set; }
        public int? course_id { get; set; }
        public double? fee_class_old { get; set; }
        public double? fee_class_new { get; set; }
        public double? pay { get; set; }
        public double? promotion { get; set; }
        public string note { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? usercreate { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
    }
}
