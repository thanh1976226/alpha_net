using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("WORK_OS_BOARD")]
    public class WORKOSBoard
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardID { get; set; }

        [StringLength(100)]
        public string BoardCode { get; set; }

        [StringLength(255)]
        public string BoardName { get; set; }

        [StringLength(255)]
        public string TeamCode { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        [StringLength(255)]
        public string Visibility { get; set; }

        [StringLength(255)]
        public string Background { get; set; }

        public bool IsDeleted { get; set; }
    }
}
