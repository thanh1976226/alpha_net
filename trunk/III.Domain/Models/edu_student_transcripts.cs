using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_student_transcripts")]
    public  class edu_student_transcripts
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string subject { get; set; }
        public string mark { get; set; }
        public string school { get; set; }
        public DateTime? starttime { get; set; }
        public DateTime? endtime { get; set; }
        public string note { get; set; }
        public string teacher { get; set; }
        public int? idstudent { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
        public string language { get; set; }
        public string classst { get; set; }
    }
}
