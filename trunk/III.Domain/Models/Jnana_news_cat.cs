using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("Jnana_news_cat")]
   public class Jnana_news_cat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(maximumLength: 250)]
        public string cat_code { get; set; }


        [StringLength(maximumLength: 255)]
        public string cat_title { get; set; }

        [StringLength(maximumLength: 255)]
        public string cat_description { get; set; }

        public int? cat_parent_code { get; set; }

        public int? created_by { get; set; }
        public int? update_by { get; set; }



        [StringLength(maximumLength: 255)]
        public string cat_avarta { get; set; }


        public DateTime? created_time { get; set; }
        public DateTime? update_time { get; set; }
        public int cat_status { get; set; }
    }
}
