using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_catExam")]
    public class edu_catExam
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_catexam { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? user_id { get; set; }
        public int? flag { get; set; }
        public string language { get; set; }
    }
}
