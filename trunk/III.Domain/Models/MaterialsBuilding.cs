using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("MATERIALS_BUILDING")]
    public class MaterialsBuilding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
    
        [StringLength(maximumLength: 100)]
        public string OptCode { get; set; }
        [StringLength(maximumLength: 100)]
        public string MaterCode { get; set; }
        public int Quantity { get; set; }
        [StringLength(maximumLength: 100)]
        public string Unit { get; set; }
        [StringLength(maximumLength: 100)]
        public string TrkCode { get; set; }


        public bool IsDeleted { get; set; }
        [StringLength(50)]
        public string DeletedBy { get; set; }
        public DateTime? DeletedTime { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}