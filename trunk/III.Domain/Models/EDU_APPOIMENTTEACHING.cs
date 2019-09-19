using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("EDU_APPOIMENTTEACHING")]
    public class EDU_APPOIMENTTEACHING
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SUBJECT { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime? START { get; set; }
        public DateTime? FINISH { get; set; }
        public int? STATUS { get; set; }
        public int? CLASS_ID { get; set; }
        public int? TEACHER_ID { get; set; }
    }
}
