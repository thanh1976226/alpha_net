using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_location")]
    public class edu_location
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public int? parentid { get; set; }
        public string description { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        public int? flag { get; set; }
    }
}
