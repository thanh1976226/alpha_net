using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_SCCF")]
    public  class edu_SCCF
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_SCCF { get; set; }
        public int? id_skillmaster { get; set; }
        public string id_filestore { get; set; }
        public int? id_course { get; set; }
        public int? id_catExam { get; set; }
        public string note { get; set; }
        public int? user_id { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updateTime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public DateTime? exam_start { get; set; }
        public DateTime? exam_end { get; set; }
        public int? exam_time { get; set; }
    }
}
