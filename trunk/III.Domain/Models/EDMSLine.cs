using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("EDMS_LINE")]
    public class EDMSLine
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string LineCode { get; set; }

        [StringLength(255)]
        public string QR_Code { get; set; }

        [StringLength(255)]
        public string LPosition { get; set; }

        [StringLength(255)]
        public string LSize { get; set; }

        [StringLength(255)]
        public string LText { get; set; }

        [StringLength(255)]
        public string CNT_Rack { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(255)]
        public string FloorCode { get; set; }

        [StringLength(255)]
        public string LColor { get; set; }

        [StringLength(255)]
        public string LStatus { get; set; }
    }
}
