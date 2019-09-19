using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("edu_student")]
    public class edu_student
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string code { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int? sex { get; set; }
        public DateTime? birthday { get; set; }
        public string address { get; set; }
        public int? city { get; set; }
        public int? districts { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? updatetime { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
        public bool? Public { get; set; }
        public string facebook { get; set; }
        public string sky { get; set; }
        public string google { get; set; }
        public string twitter { get; set; }
        public string wordpress { get; set; }
        public string other { get; set; }
        public string mobilephone { get; set; }
        public int? school { get; set; }
        public int? Class { get; set; }
        //public int? classSpecial { get; set; }
        public int? course { get; set; }
        public int? type { get; set; }
        public int? status { get; set; }
        public string note { get; set; }
        public string image { get; set; }
        public int? idrank { get; set; }
        public string objective { get; set; }
        //public string inforSM { get; set; }
        public string noteinfor { get; set; }
        public string studying_oversea { get; set; }
        public string desired_score { get; set; }
        public string chuyennganh { get; set; }
        public string file_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
