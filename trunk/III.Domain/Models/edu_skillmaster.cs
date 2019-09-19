using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_skillmaster")]
    public  class edu_skillmaster
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_skillmaster { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? user_id { get; set; }
        public int? flag { get; set; }
        public string language { get; set; }
    }
}
