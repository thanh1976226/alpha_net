using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_location_tutor")]
    public class edu_location_tutor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int? parentid { get; set; }
        public string description { get; set; }
        public int? usercreate { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
    }
}
