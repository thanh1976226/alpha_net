using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_class_teacher")]
    public  class edu_class_teacher
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? class_id { get; set; }
        public int? teacher_id { get; set; }
        public string skills { get; set; }
        public string schedule { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public string hour_start { get; set; }
        public string hour_end { get; set; }
    }
}
