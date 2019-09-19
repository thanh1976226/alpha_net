using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("EDMS_RACK")]
    public class EDMSRack
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string RackCode { get; set; }

        [StringLength(255)]
        public string QR_Code { get; set; }

        [StringLength(255)]
        public string RPosition { get; set; }

        [StringLength(255)]
        public string RSize { get; set; }

        [StringLength(255)]
        public string RStatus { get; set; }

        [StringLength(255)]
        public string CNT_Box { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(255)]
        public string LineCode { get; set; }
    }
}
