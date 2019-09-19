using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_file")]
    public class edu_file
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string format { get; set; }
        public byte[] content { get; set; }
        public int? userid { get; set; }
        public DateTime? creattime { get; set; }
        public string action { get; set; }
        public string priority { get; set; }
        public DateTime? updatetime { get; set; }
        public string language { get; set; }
        public byte? flag { get; set; }
        public bool? status { get; set; }
        public string url { get; set; }
        public int? idtest { get; set; }
    }
}
