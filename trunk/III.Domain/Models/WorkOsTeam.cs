using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("WORK_OS_TEAM")]
    public class WORKOSTeam
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string TeamCode { get; set; }

        [StringLength(255)]
        public string TeamName { get; set; }

        public bool Flag { get; set; }

        [StringLength(255)]
        public string Member { get; set; }
        
        [StringLength(255)]
        public string Leader { get; set; }
    }
}
