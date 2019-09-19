using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_test_beginner")]
    public  class edu_test_beginner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string tstcode { get; set; }
        public int? idstudent { get; set; }
        public string note { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public byte? flag { get; set; }
        public string tstname { get; set; }
        public DateTime? tststartime { get; set; }
        public DateTime? tstendtime { get; set; }
        public string published { get; set; }
        public int? idcoordinator { get; set; }
    }
}
