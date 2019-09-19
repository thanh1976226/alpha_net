using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESEIM.Models
{
    [Table("AspNetUsers")]
    public class AspNetUser : IdentityUser
    {
        public AspNetUser() : base()
        {
            //AspNetUserClaims = new HashSet<AspNetUserClaim>();
            //AspNetUserLogins = new HashSet<AspNetUserLogin>();
            //AspNetUserRoles = new HashSet<AspNetUserRole>();
            AdUserInGroups = new HashSet<AdUserInGroup>();
            AdPermissions = new HashSet<AdPermission>();
            //ESUserInGroups = new HashSet<ESUserInGroup>();
            //ESExtendAccounts = new HashSet<ESExtendAccount>();
            //ESUserApps = new HashSet<ESUserApp>();
            //ESUserPrivileges = new HashSet<ESUserPrivilege>();
        }

        public int? OfficeNumber { get; set; }
        [StringLength(256)]
        public string FamilyName { get; set; }
        [StringLength(256)]
        public string GivenName { get; set; }
        [StringLength(256)]
        public string MiddleName { get; set; }
        [StringLength(256)]
        public string Picture { get; set; }

        public DateTime? CreatedDate { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [StringLength(2000)]
        public string Note { get; set; }

        [StringLength(2000)]
        public string Reason { get; set; }

        public int? UserType { get; set; }
        public bool IsExceeded { get; set; }

        public bool? Gender { get; set; }

        public bool? IsCheckin { get; set; }

        //[StringLength(50)]
        //public string DepartmentId { set; get; }
        //[JsonIgnore]
        //[ForeignKey("DepartmentId")]
        //[InverseProperty("DepartmentUsers")]
        //public virtual VIBOrganization Department { get; set; }

        [StringLength(50)]
        public string BranchId { set; get; }
        [JsonIgnore]
        [ForeignKey("BranchId")]
        [InverseProperty("BranchUsers")]
        public virtual AdOrganization Branch { get; set; }

        //[StringLength(50)]
        //public string ProfitCenterId { set; get; }
        //[JsonIgnore]
        //[ForeignKey("ProfitCenterId")]
        //[InverseProperty("ProfitCenterUsers")]
        //public virtual VIBOrganization ProfitCenter { get; set; }

        [StringLength(5)]
        public string AccountExecutiveId { set; get; }
        //[JsonIgnore]
        //[ForeignKey("AccountExecutiveId")]
        //[InverseProperty("AccountExecutiveUsers")]
        //public virtual VIBOrganization AccountExecutive { get; set; }

        [StringLength(2000)]
        public string OrgReference { set; get; }

        [JsonIgnore]
        public virtual ICollection<AdUserInGroup> AdUserInGroups { get; set; }
        [JsonIgnore]
        public virtual ICollection<AdPermission> AdPermissions { get; set; }
        //public virtual ICollection<ESUserInGroup> ESUserInGroups { get; set; }
        //public virtual ICollection<ESExtendAccount> ESExtendAccounts { get; set; }
        //public virtual ICollection<ESUserApp> ESUserApps { get; set; }
        //public virtual ICollection<ESUserPrivilege> ESUserPrivileges { get; set; }
        [JsonIgnore]
        [NotMapped]
        public virtual ICollection<IdentityUserRole<string>> AspNetUserRoles { get; set; }
    }
    public class AspNetUserCustom
    {
        public string Id { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string Company_Code { set; get; }
        public int? OfficeNumber { set; get; }
        public string FamilyName { set; get; }
        public string MiddleName { set; get; }
        public string GivenName { set; get; }
        public string EmployeeCode { set; get; }
        public int? OrgId { set; get; }
        public string DepartmentId { set; get; }
        public string BranchId { set; get; }
        public string ProfitCenterId { set; get; }
        public string AccountExecutiveId { set; get; }

        public string ApplicationCode { set; get; }
        public bool Active { get; set; }
        public short UserType { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Reason { get; set; }
        public string Picture { get; set; }
        public string RoleId { set; get; }
        public ESEIM.Utils.TempSub TempSub { set; get; }
    }
    public class UserInfoAd
    {
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string Email { set; get; }
        public string EmployeeCode { set; get; }

        public bool Error { get; set; }
        public string Message { set; get; }
    }

    public class UserImportData
    {
        public int RowNum { set; get; }

        public string UserName { set; get; }
        public bool ErrorUserName { get; set; }
        public string MessageUserName { set; get; }

        public string FullName { set; get; }
        public string Email { set; get; }
        public string EmployeeCode { set; get; }
        public bool ErrorEmployeeCode { get; set; }
        public string MessageEmployeeCode { set; get; }

        public string DepartmentId { set; get; }
        public string Department { set; get; }
        public bool ErrorDepartment { get; set; }
        public string MessageDepartment { set; get; }

        public string BranchId { set; get; }
        public string Branch { set; get; }
        public bool ErrorBranch { get; set; }
        public string MessageBranch { set; get; }

        public string ProfitCenterId { set; get; }
        public string ProfitCenter { set; get; }
        public bool ErrorProfitCenter { get; set; }
        public string MessageProfitCenter { set; get; }

        public string AccountExecutiveId { set; get; }
        public string AccountExecutive { set; get; }
        public bool ErrorAccountExecutive { get; set; }
        public string MessageAccountExecutive { set; get; }

        public string GroupUserCode { set; get; }
        public string GroupUser { set; get; }
        public bool ErrorGroupUser { get; set; }
        public string MessageGroupUser { set; get; }

        public string RoleId { set; get; }
        public string Role { set; get; }
        public bool ErrorRole { get; set; }
        public string MessageRole { set; get; }

        public bool Status { get; set; }

        public bool Checked { get; set; }
        public bool Error { get; set; }
        public string Message { set; get; }
    }
    public class UserFailed
    {
        public string UserName { set; get; }
        public string Department { set; get; }
        public string Branch { set; get; }
        public string ProfitCenter { set; get; }
        public string AccountExecutive { set; get; }
        public string Role { set; get; }
        public bool Status { get; set; }
        public string validation { get; set; }
    }
}