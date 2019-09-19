using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("EX_SCHOOL")]
    public class ex_School
    {
        public ex_School()
        {
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        public string NameSchool { get; set; }

        [StringLength(10)]
        public string CodeSchool { get; set; }

        
        public DateTime Year { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(100)]
        public string Province { get; set; }
        [StringLength(100)]
        public string District { get; set; }
        [StringLength(100)]
        public string commune { get; set; }
        [StringLength(255)]
        public string Img { get; set; }


    }
}