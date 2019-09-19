using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_entrance_test")]
    public class edu_entrance_test
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? student_id { get; set; }
        public int? course_id { get; set; }
        public string skills { get; set; }
        public string marks { get; set; }
        public string idfile { get; set; }
        public string note { get; set; }
        public bool? apply { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
    }
}
