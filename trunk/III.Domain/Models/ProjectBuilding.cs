using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ESEIM.Models
{
    [Table("PROJECT_BUILDING")]
    public class ProjectBuilding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
    
        [StringLength(maximumLength: 100)]
        public string PrjBuildingCode { get; set; }
        [StringLength(maximumLength: 255)]
        public string PrjBuildingName { get; set; }
        [StringLength(maximumLength: 255)]
        public string Adress { get; set; }
        [StringLength(maximumLength: 255)]
        public string Customer { get; set; }

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