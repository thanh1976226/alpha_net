using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_room_purpose")]
    public class edu_room_purpose
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? id_edu_room { get; set; }
        public int? id_common { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public string code_common { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public bool? status { get; set; }
        public byte? flag { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        public int? parent_id { get; set; }
    }
}
