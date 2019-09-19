using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("EDMS_WAREHOUSE")]
    public class EDMSWareHouse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key, StringLength(255)]
        public string WHS_Code { get; set; }

        [StringLength(255)]
        public string QR_Code { get; set; }

        [StringLength(255)]
        public string WHS_Name { get; set; }

        public string WHS_Note { get; set; }

        [StringLength(255)]
        public string WHS_AreaSquare { get; set; }

        [StringLength(255)]
        public string WHS_CNT_Floor { get; set; }

        [StringLength(255)]
        public string WHS_ADDR_Text { get; set; }

        [StringLength(255)]
        public string WHS_ADDR_Gps { get; set; }

        [StringLength(255)]
        public string WHS_Avatar { get; set; }

        [StringLength(255)]
        public string IMG_WHS { get; set; }

        [StringLength(500)]
        public string WHS_Tags { get; set; }

        [StringLength(255)]
        public string WHS_DesignMap { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public bool WHS_Flag { get; set; }

        [StringLength(255)]
        public string WHS_Status { get; set; }
    }
}