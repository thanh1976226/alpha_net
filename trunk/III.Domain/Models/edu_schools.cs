using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_schools")]
    public  class edu_schools
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public int? level { get; set; }
        public string address { get; set; }
        public int? country { get; set; }
        public int? district { get; set; }
        public int? flag{ get; set; }
        public DateTime? creattime { get; set; }
        public DateTime? updatetime { get; set; }
    }
}
