//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESEIM.Models
{
    [Table("cms_extra_fields_value")]
    public class cms_extra_fields_value
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string field_value { get; set; }
        public int? field_group { get; set; }
        public int? ordering { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_date { get; set; }
        public int?modified_by { get; set; }
        public DateTime? modified_date { get; set; }
        public bool? trash { get; set; }
        public bool? publish { get; set; }
        public DateTime? date_post { get; set; }
    }
}
//namespace III.WebApp.Domain.DbContext
//{
//    using System;
//    using System.Collections.Generic;
    
//    public partial class cms_extra_fields_value
//    {
//        public int id { get; set; }
//        public string field_value { get; set; }
//        public Nullable<int> field_group { get; set; }
//        public Nullable<int> ordering { get; set; }
//        public Nullable<int> created_by { get; set; }
//        public Nullable<System.DateTime> created_date { get; set; }
//        public Nullable<int> modified_by { get; set; }
//        public Nullable<System.DateTime> modified_date { get; set; }
//        public Nullable<bool> trash { get; set; }
//        public Nullable<bool> publish { get; set; }
//        public Nullable<System.DateTime> date_post { get; set; }
//    }
//}
