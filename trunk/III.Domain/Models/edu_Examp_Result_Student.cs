using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_Examp_Result_Student")]
    public class edu_Examp_Result_Student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_SCCF { get; set; }
        public int? id_student { get; set; }
        public string mark { get; set; }
        public string raw { get; set; }
        public string note { get; set; }
        public int? user_id { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public string id_filestore { get; set; }
        public int? exam_time { get; set; }
    }
}
