using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace ESEIM.Models
{
    [Table("edu_student_contact")]
    public class edu_student_contact
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string company { get; set; }
        public string telephone { get; set; }
        public string mobilephone { get; set; }
        public string email { get; set; }
        public int? relationship { get; set; }
        public string address { get; set; }
        public string note { get; set; }
        public int? studentID { get; set; }
        public int? position { get; set; }
        public int? career { get; set; }
        public int? usercreate { get; set; }
        public int? flag { get; set; }
    }
}
