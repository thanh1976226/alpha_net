using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{ 
    [Table("WORK_OS_LIST")]
    public class WORKOSList
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListID { get; set; }

        [StringLength(100)]
        public string ListCode { get; set; }

        [StringLength(255)]
        public string ListName { get; set; }

        [StringLength(100)]
        public string BoardCode { get; set; }

        public int Order { get; set; }

        [StringLength(255)]
        public string Avatar { get; set; }

        public bool IsDeleted { get; set; }

        public int? Status { get; set; }

        [StringLength(100)]
        public string Background { get; set; }
    }
}
