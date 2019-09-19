﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("CONTRACT_NOTE")]
    public class ContactNote
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractNoteId { get; set; }

        public string ContractCode { get; set; }

        [StringLength(100)]
        public int? ContractVersion { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Tags { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        [StringLength(50)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
