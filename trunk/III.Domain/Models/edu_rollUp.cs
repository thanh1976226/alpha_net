using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_rollUp")]
    public  class edu_rollUp
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? date_rollUp { get; set; }
        public int? class_id { get; set; }
        public bool? isMockTest { get; set; }
        public bool? status { get; set; }
        public int? usercreate { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public bool? flag { get; set; }
        public string language { get; set; }
        public int? lesson_of_class { get; set; }
        public int? room_id { get; set; }
        public bool? makeUp_lesson { get; set; }
    }
}
