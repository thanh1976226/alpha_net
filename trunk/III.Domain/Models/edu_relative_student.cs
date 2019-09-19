using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_relative_student")]
    public  class edu_relative_student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? idstudenta { get; set; }//h/s đang xét
        public int? idstudentb { get; set; }//h/s có quan hệ với học sinh đang xét
        public int? idrelative { get; set; }
        public DateTime? creattime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
    }
}
