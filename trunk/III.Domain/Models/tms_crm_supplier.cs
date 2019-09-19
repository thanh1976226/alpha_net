using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ESEIM.Models
{
    [Table("tms_crm_supplier")]
    public class tms_crm_supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id    { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Tel { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        [StringLength(20)]
        public string Tax_no { get; set; }

        [StringLength(20)]
        public string Account_number { get; set; }

        [StringLength(100)]
        public string Bank { get; set; }

        [StringLength(50)]
        public string Supplier_type { get; set; }

        [StringLength(500)]
        public string Location_id { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        public int Userid { get; set; }

        public DateTime? Creattime { get; set; }

        public string Action { get; set; }

        public string Priority { get; set; }

        public bool Sponsor { get; set; }
    }
}
