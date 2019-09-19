using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("WORK_EXECUTIVE_OBJECTRELATIVE")]
    public class WorkExecutiveObjectrelative
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string ListCode { get; set; }

        [StringLength(255)]
        public string ObjectCode { get; set; }
        
        [StringLength(255)]
        public string ObjectModule { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(255)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        [StringLength(255)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public bool Flag { get; set; }
    }
}
