using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_rank_condition")]
    public class edu_rank_condition
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string statement { get; set; }
        public string sql_condition { get; set; }
        public string expression { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? user_id { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
    }
}
