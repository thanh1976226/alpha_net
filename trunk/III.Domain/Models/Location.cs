using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("Location")]
    public class Location
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(maximumLength: 50)]
        public string code { get; set; }
        [StringLength(maximumLength: 500)]
        public string name { get; set; }
        public int? parent_id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }

        [StringLength(maximumLength: 500)]
        public string description { get; set; }

        public int flag { get; set; }
    }
}