using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_school_message")]
    public  class edu_school_message
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int? company_id { get; set; }
        public string company_title { get; set; }
        public string branch_title { get; set; }
        public string cound_id { get; set; }
        public string cound_title { get; set; }
        public string class_id { get; set; }
        public string class_title { get; set; }
        public string student_id { get; set; }
        public int? sender_id { get; set; }
        public string recepierent_id { get; set; }
        public string message_content { get; set; }
        public int? message_type { get; set; }
        public DateTime? datesend { get; set; }
        public int? branch_id { get; set; }
    }
}
