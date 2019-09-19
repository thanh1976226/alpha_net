using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_test_beginner_detail")]
    public  class edu_test_beginner_detail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string title { get; set; }
        public int? idfile { get; set; }
        public string note { get; set; }
        public int? id_test { get; set; }
        public double? mark { get; set; }
        public DateTime? creattime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public byte? flag { get; set; }
    }
}
