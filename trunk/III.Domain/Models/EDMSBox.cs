using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("EDMS_BOX")]
    public class EDMSBox
    { 
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string BoxCode { get; set; }

        [StringLength(255)]
        public string QR_Code { get; set; }

        [StringLength(255)]
        public string DepartCode { get; set; }

        [StringLength(255)]
        public string TypeProfile { get; set; }

        [StringLength(255)]
        public string BoxSize { get; set; }

        [StringLength(255)]
        public string M_CNT_Brief { get; set; }

        [StringLength(255)]
        public string CNT_Brief { get; set; }

        public DateTime? StartTime { get; set; }

        [StringLength(255)]
        public string NumBoxth { get; set; }

        public DateTime? TimeStorage { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        [StringLength(255)]
        public string LstMemberId { get; set; }

        [StringLength(255)]
        public string StatusBox { get; set; }

        [StringLength(255)]
        public string WHS_Code { get; set; }

        [StringLength(255)]
        public string FloorCode { get; set; }

        [StringLength(255)]
        public string LineCode { get; set; }

        [StringLength(255)]
        public string RackCode { get; set; }

        [StringLength(255)]
        public string CNT_Cell { get; set; }
    }
}
