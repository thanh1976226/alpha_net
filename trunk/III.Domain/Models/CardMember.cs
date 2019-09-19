﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("CARD_MEMBER")]
    public class CardMember
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string MemberId { get; set; }

        [StringLength(255)]
        public string CardCode { get; set; }

        [StringLength(255)]
        public string TeamCode { get; set; }

        public bool Flag { get; set; }
    }
}
