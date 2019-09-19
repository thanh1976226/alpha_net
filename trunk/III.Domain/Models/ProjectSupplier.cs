﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("PROJECT_SUPPLIER")]
    public class ProjectSupplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [StringLength(100)]
        public string ProjectCode { get; set; }

        [StringLength(255)]
        public string SupplierCode { get; set; }

        public int? Level { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public bool FlagDeleted { get; set; }
    }
}
