using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_room")]
    public class edu_room
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string number_room { get; set; }
        public int? address { get; set; }
        public string note { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
        public string facilities { get; set; }
        public string image { get; set; }
        public int? seat { get; set; }
    }
}
