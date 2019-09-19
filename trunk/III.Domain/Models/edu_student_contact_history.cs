using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student_contact_history")]
    public class edu_student_contact_history
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public DateTime? date_change { get; set; }
        public int? user_change { get; set; }
        public string column_change { get; set; }
        public string old_value { get; set; }
        public string new_value { get; set; }
        public int? edu_student_contact_id { get; set; }
    }
}
