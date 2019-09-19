using System;
using Microsoft.EntityFrameworkCore;
using III.Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ESEIM.Models
{
    public partial class EIMDBContext : IdentityDbContext<AspNetUser, AspNetRole, string>
    {
        public EIMDBContext(DbContextOptions<EIMDBContext> options) : base(options)
        {
        }
        /// <summary>
        /// Amin System management
        /// </summary>
        public virtual DbSet<AdDivision> AdDivisions { get; set; }
        public virtual DbSet<AdAccessLog> AdAccessLogs { get; set; }
        public virtual DbSet<AdActionLog> AdActionLogs { get; set; }
        public virtual DbSet<AdAppFunction> AdAppFunctions { get; set; }
        public virtual DbSet<AdApplication> AdApplications { get; set; }
        public virtual DbSet<AdFunction> AdFunctions { get; set; }
        public virtual DbSet<AdGroupUser> AdGroupUsers { get; set; }
        public virtual DbSet<AdLanguage> AdLanguages { get; set; }
        public virtual DbSet<AdLanguageText> AdLanguageTexts { get; set; }
        public virtual DbSet<AdOrganization> AdOrganizations { get; set; }
        public virtual DbSet<AdParameter> AdParameters { get; set; }
        public virtual DbSet<AdPermission> AdPermissions { get; set; }
        public virtual DbSet<AdPrivilege> AdPrivileges { get; set; }
        public virtual DbSet<AdResource> AdResources { get; set; }
        public virtual DbSet<AdUserInGroup> AdUserInGroups { get; set; }
        public virtual DbSet<AdAuthoring> AdAuthorings { get; set; }

        /// <summary>
        /// Customer
        /// </summary>
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerFile> CustomerFiles { get; set; }
        public virtual DbSet<CustomerExtend> CustomerExtends { get; set; }
        public virtual DbSet<OrderRequestRaw> OrderRequestRaws { get; set; }

        /// <summary>
        /// Supplier
        /// </summary>
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierExtend> SupplierExtends { get; set; }
        public virtual DbSet<SupplierFile> SupplierFiles { get; set; }

        /// <summary>
        /// Service
        /// </summary>
        public virtual DbSet<ServiceCategory> ServiceCategorys { get; set; }
        public virtual DbSet<ProductCat> ProductCats { get; set; }

        /// <summary>
        /// Notification
        /// </summary>
        public virtual DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Map
        /// </summary>
        public virtual DbSet<MapDataGps> MapDataGpss { get; set; }

        /// <summary>
        /// Contract
        /// </summary>
        public virtual DbSet<ContractHeader> ContractHeaders { get; set; }
        public virtual DbSet<ContractDetail> ContractDetails { get; set; }
        public virtual DbSet<ContractFile> ContractFiles { get; set; }
        public virtual DbSet<ContractPeopleTag> ContractPeopleTags { get; set; }
        public virtual DbSet<ContractAttribute> ContractAttributes { get; set; }
        public virtual DbSet<ContractActivity> ContractActivitys { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<ContactNote> ContactNotes { get; set; }
        public virtual DbSet<ContractMemberTag> ContractMemberTags { get; set; }

        ///<summary>
        ///Warehouse
        ///</summary>  
        public virtual DbSet<MaterialProduct> MaterialProducts { get; set; }
        public virtual DbSet<MaterialProductGroup> MaterialProductGroups { get; set; }
        public virtual DbSet<MaterialType> MaterialTypes { get; set; }
        public virtual DbSet<MaterialAttribute> MaterialAttributes { get; set; }
        public virtual DbSet<MaterialFile> MaterialFiles { get; set; }
        public virtual DbSet<MaterialStoreExpGoodsHeader> MaterialStoreExpGoodsHeaders { get; set; }
        public virtual DbSet<MaterialStoreBatchGoods> MaterialStoreBatchGoodss { get; set; }
        public virtual DbSet<MaterialStoreImpGoodsHeader> MaterialStoreImpGoodsHeaders { get; set; }
        public virtual DbSet<MaterialStoreImpGoodsDetail> MaterialStoreImpGoodsDetails { get; set; }
        public virtual DbSet<MaterialInvoice> MaterialInvoices { get; set; }
        public virtual DbSet<CommonSetting> CommonSettings { get; set; }
        public virtual DbSet<MaterialStore> MaterialStores { get; set; }
        public virtual DbSet<MaterialPaymentTicket> MaterialPaymentTickets { get; set; }

        /// <summary>
        /// Asset
        /// </summary>
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<AssetAttribute> AssetAttributes { get; set; }
        public virtual DbSet<AssetActivity> AssetAtivitys { get; set; }
        public virtual DbSet<AssetTransferHeader> AssetTransferHeaders { get; set; }
        public virtual DbSet<AssetTransferDetail> AssetTransferDetails { get; set; }
        public virtual DbSet<AssetCancelHeader> AssetCancelHeaders { get; set; }
        public virtual DbSet<AssetCancelDetail> AssetCancelDetails { get; set; }

        /// <summary>
        /// HR
        /// </summary>
        public virtual DbSet<HRAddress> HRAddress { get; set; }
        public virtual DbSet<HRContact> HRContacts { get; set; }
        public virtual DbSet<HRContract> HRContracts { get; set; }
        public virtual DbSet<HREmployee> HREmployees { get; set; }
        public virtual DbSet<HRTrainingCourse> HRTrainingCourses { get; set; }
        public virtual DbSet<HRWorkFlows> HRWorkFlows { get; set; }
        public virtual DbSet<HRWorkingProcess> HRWorkingProcesss { get; set; }

        /// <summary>
        /// Project
        /// </summary>
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectCustomer> ProjectCustomers { get; set; }
        public virtual DbSet<ProjectGantt> ProjectGantts { get; set; }
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; }
        public virtual DbSet<ProjectFile> ProjectFiles { get; set; }
        public virtual DbSet<ProjectSupplier> ProjectSuppliers { get; set; }
        public virtual DbSet<ProjectNote> ProjectNotes { get; set; }

        /// <summary>
        /// Card
        /// </summary>
        public virtual DbSet<WORKOSBoard> WORKOSBoards { get; set; }
        public virtual DbSet<WORKOSList> WORKOSLists { get; set; }
        public virtual DbSet<WORKOSCard> WORKOSCards { get; set; }
        public virtual DbSet<WORKOSTeam> WORKOSTeams { get; set; }
        public virtual DbSet<CardCheckList> CardCheckList { get; set; }
        public virtual DbSet<CardChkItem> CardChkItem { get; set; }
        public virtual DbSet<CardAttachment> CardAttachment { get; set; }
        public virtual DbSet<CardCommentList> CardCommentList { get; set; }
        public virtual DbSet<WorkExecutiveObjectrelative> WorkExecutiveObjectrelative { get; set; }
        public virtual DbSet<CardForWObj> CardForWObj { get; set; }
        public virtual DbSet<CardMember> CardMember { get; set; }
       
        /// <summary>
        /// Candidate
        /// </summary>
        public virtual DbSet<CandidateBasic> CandiateBasic { get; set; }
        public virtual DbSet<CandidateWorkEvent> CandidateWorkEvents { get; set; }
        public virtual DbSet<CandidateInterview> CandidateInterviews { get; set; }

        /// <summary>
        /// Staff
        /// </summary>
        public virtual DbSet<StaffScheduleWork> StaffScheduleWorks { get; set; }
        public virtual DbSet<StaffTimetableWorking> StaffTimetableWorkings { get; set; }

        /// <summary>
        /// Keyword
        /// </summary>
        public virtual DbSet<GalaxyKeyword> GalaxyKeywords { get; set; }

        /// <summary>
        /// Google API
        /// </summary>
        public virtual DbSet<ApiGoogleServices> ApiGoogleServices { get; set; }
        public virtual DbSet<CountRequestGoogle> CountRequestGoogle { get; set; }

        /// <summary>
        /// Addon app
        /// </summary>
        public virtual DbSet<AddonApp> AddonApps { get; set; }
        public virtual DbSet<AddonAppServer> AddonAppServers { get; set; }
        public virtual DbSet<AppVendor> AppVendors { get; set; }
        public virtual DbSet<HolidayDate> HolidayDates { get; set; }

        /// <summary>
        /// Dispatches
        /// </summary>
        public virtual DbSet<DispatchesCategory> DispatchesCategorys { get; set; }
        public virtual DbSet<DispatchesHeader> DispatchesHeaders { get; set; }
        public virtual DbSet<DispatchTrackingProcess> DispatchTrackingProcesss { get; set; }
        public virtual DbSet<DispatchesMemberActivity> DispatchesMemberActivitys { get; set; }
        public virtual DbSet<DispatchesFileACT> DispatchesFileACTs { get; set; }
        public virtual DbSet<DispatchesCommentACT> DispatchesCommentACTs { get; set; }
        public virtual DbSet<DispatchesUser> DispatchesUsers { get; set; }
        public virtual DbSet<DispatchesWeekWorkingScheduler> DispatchesWeekWorkingSchedulerss { get; set; }


        /// <summary>
        /// Contruction
        /// </summary>
        public virtual DbSet<MaterialsBuilding> MaterialsBuildings { get; set; }
        public virtual DbSet<ProjectBuilding> ProjectBuildings { get; set; }
        public virtual DbSet<TrackingBuilding> TrackingBuildings { get; set; }
        public virtual DbSet<TrackingMedia> TrackingMedias { get; set; }


        //Face Id
        public virtual DbSet<FaceFaceId> FaceFaceIds { get; set; }

        public virtual DbSet<ObeListDevice> ObelistDevices { get; set; }
        public virtual DbSet<ObeAiRecognitionTracking> ObeAiRecognitionTrackings { get; set; }
        public virtual DbSet<ObeAccount> ObeAccounts { get; set; }

        /// <summary>
        /// Faco
        /// </summary>
        public virtual DbSet<FacoProductCat> FacoProductCats { get; set; }
        public virtual DbSet<OperationOnlineSupport> OperationOnlineSupports { get; set; }
        public virtual DbSet<OperationOnlineSupportTracking> OperationOnlineSupportTrackings { get; set; }
        /// <summary>
        ///EDMS
        /// </summary>
        public virtual DbSet<EDMSRepositoryFile> EDMSRepositoryFiles { get; set; }
        public virtual DbSet<EDMSRepository> EDMSRepositorys { get; set; }
        public virtual DbSet<EDMSFile> EDMSFiles { get; set; }
        public virtual DbSet<EDMSFilePermission> EDMSFilePermissions { get; set; }
        public virtual DbSet<EDMSWareHouse> EDMSWareHouses { get; set; }
        public virtual DbSet<EDMSWareHouseUsers> EDMSWareHouseUsers { get; set; }
        /// <summary>

        /// <summary>
        /// SmartEdu
        /// </summary>
        public virtual DbSet<EDU_APPOIMENTTEACHING> EDU_APPOIMENTTEACHING { get; set; }
        public virtual DbSet<edu_AppointmentModify> edu_AppointmentModify { get; set; }
        public virtual DbSet<edu_AppointmentTeaching> edu_AppointmentTeaching { get; set; }
        public virtual DbSet<edu_catExam> edu_catExam { get; set; }
        public virtual DbSet<edu_cat_invoice> edu_cat_invoice { get; set; }
        public virtual DbSet<edu_cat_ranking> edu_cat_ranking { get; set; }
        public virtual DbSet<edu_catrank_rankcondition> edu_catrank_rankcondition { get; set; }
        public virtual DbSet<edu_class> edu_class { get; set; }
        public virtual DbSet<edu_classChange> edu_classChange { get; set; }
        public virtual DbSet<edu_class_history> edu_class_history { get; set; }
        public virtual DbSet<edu_class_student> edu_class_student { get; set; }
        public virtual DbSet<edu_class_student_history> edu_class_student_history { get; set; }
        public virtual DbSet<edu_class_teacher> edu_class_teacher { get; set; }
        public virtual DbSet<edu_class_tutor> edu_class_tutor { get; set; }
        public virtual DbSet<edu_config_score_student> edu_config_score_student { get; set; }
        public virtual DbSet<edu_contract_file> edu_contract_file { get; set; }
        public virtual DbSet<edu_contract_student_app> edu_contract_student_app { get; set; }
        public virtual DbSet<edu_course> edu_course { get; set; }
        public virtual DbSet<edu_currency> edu_currency { get; set; }
        public virtual DbSet<edu_debt> edu_debt { get; set; }
        public virtual DbSet<edu_Document> edu_Document { get; set; }
        public virtual DbSet<edu_entrance_test> edu_entrance_test { get; set; }
        public virtual DbSet<edu_Examp> edu_Examp { get; set; }
        public virtual DbSet<edu_Examp_Result_Student> edu_Examp_Result_Student { get; set; }
        public virtual DbSet<edu_feedback_mobile> edu_feedback_mobile { get; set; }
        public virtual DbSet<edu_file> edu_file { get; set; }
        public virtual DbSet<edu_history_price> edu_history_price { get; set; }
        public virtual DbSet<edu_hocbu> edu_hocbu { get; set; }
        public virtual DbSet<edu_invoice> edu_invoice { get; set; }
        public virtual DbSet<edu_invoice1> edu_invoice1 { get; set; }
        public virtual DbSet<edu_invoice11> edu_invoice11 { get; set; }
        public virtual DbSet<edu_invoice_advice> edu_invoice_advice { get; set; }
        public virtual DbSet<edu_invoice_detailExpenses> edu_invoice_detailExpenses { get; set; }
        public virtual DbSet<edu_invoice_detailReceipt> edu_invoice_detailReceipt { get; set; }
        public virtual DbSet<edu_invoice_document> edu_invoice_document { get; set; }
        public virtual DbSet<edu_invoice_document_history> edu_invoice_document_history { get; set; }
        public virtual DbSet<edu_location> edu_location { get; set; }
        public virtual DbSet<edu_location_tutor> edu_location_tutor { get; set; }
        public virtual DbSet<edu_main_exam> edu_main_exam { get; set; }
        public virtual DbSet<edu_notification_admin_mobile> edu_notification_admin_mobile { get; set; }
        public virtual DbSet<edu_positions> edu_positions { get; set; }
        public virtual DbSet<edu_rank_condition> edu_rank_condition { get; set; }
        public virtual DbSet<edu_relatives_learned> edu_relatives_learned { get; set; }
        public virtual DbSet<edu_relative_student> edu_relative_student { get; set; }
        public virtual DbSet<edu_reserved> edu_reserved { get; set; }
        public virtual DbSet<edu_reserved_sum> edu_reserved_sum { get; set; }
        public virtual DbSet<edu_rollUp> edu_rollUp { get; set; }
        public virtual DbSet<edu_rollUp_student> edu_rollUp_student { get; set; }
        public virtual DbSet<edu_rollUp_teacher> edu_rollUp_teacher { get; set; }
        public virtual DbSet<edu_room> edu_room { get; set; }
        public virtual DbSet<edu_room_purpose> edu_room_purpose { get; set; }
        public virtual DbSet<edu_salary_employee_DT> edu_salary_employee_DT { get; set; }
        public virtual DbSet<edu_salary_employee_HD> edu_salary_employee_HD { get; set; }
        public virtual DbSet<edu_salary_payment_DT> edu_salary_payment_DT { get; set; }
        public virtual DbSet<edu_salary_payment_HD> edu_salary_payment_HD { get; set; }
        public virtual DbSet<edu_salary_teacher_DT> edu_salary_teacher_DT { get; set; }
        public virtual DbSet<edu_salary_teacher_HD> edu_salary_teacher_HD { get; set; }
        public virtual DbSet<edu_salary_teacher_year> edu_salary_teacher_year { get; set; }
        public virtual DbSet<edu_salary_tutor_DT> edu_salary_tutor_DT { get; set; }
        public virtual DbSet<edu_salary_tutor_HD> edu_salary_tutor_HD { get; set; }
        public virtual DbSet<edu_SCCF> edu_SCCF { get; set; }
        public virtual DbSet<edu_school_message> edu_school_message { get; set; }
        public virtual DbSet<edu_schools> edu_schools { get; set; }
        public virtual DbSet<edu_skillmaster> edu_skillmaster { get; set; }
        public virtual DbSet<edu_student> edu_student { get; set; }
        public virtual DbSet<edu_student_absence> edu_student_absence { get; set; }
        public virtual DbSet<edu_student_app> edu_student_app { get; set; }
        public virtual DbSet<edu_student_contact> edu_student_contact { get; set; }
        public virtual DbSet<edu_student_contact_history> edu_student_contact_history { get; set; }
        public virtual DbSet<edu_student_exam> edu_student_exam { get; set; }
        public virtual DbSet<edu_student_history> edu_student_history { get; set; }
        public virtual DbSet<edu_student_transcripts> edu_student_transcripts { get; set; }
        public virtual DbSet<edu_sumBalance_classChange> edu_sumBalance_classChange { get; set; }
        public virtual DbSet<edu_test_beginner> edu_test_beginner { get; set; }
        public virtual DbSet<edu_test_beginner_detail> edu_test_beginner_detail { get; set; }
        public virtual DbSet<edu_token_mobile> edu_token_mobile { get; set; }
        public virtual DbSet<edu_user_mobile> edu_user_mobile { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<common_properties> common_properties { get; set; }

        public virtual DbSet<Jnana_news_cat> Jnana_news_cat { get; set; }
        public virtual DbSet<Jnana_news_article_file> Jnana_news_article_files { get; set; }
        public virtual DbSet<Jnana_news_article> Jnana_news_articles { get; set; }
        public virtual DbSet<Jnana_file> Jnana_file { get; set; }
        public virtual DbSet<EDMSMaterialProduct> EDMSMaterialProducts { get; set; }
        public virtual DbSet<AssetLiquidation> AssetLiquidations { get; set; }

        public virtual DbSet<FundAccEntry> FundAccEntrys { get; set; }
        public virtual DbSet<ParamForWarning> ParamForWarnings { get; set; }
        public virtual DbSet<ParamForWarningModel> ParamForWarningModels { get; set; }
        public virtual DbSet<FundExchagRate> FundExchagRates { get; set; }

        public virtual DbSet<FundCurrency> FundCurrencys { get; set; }
        public virtual DbSet<FundCatReptExps> FundCatReptExpss { get; set; }
        public virtual DbSet<IotWarningSetting> IotWarningSettings { get; set; }
        public virtual DbSet<IotCarInOut> IotCarInOuts { get; set; }
        public virtual DbSet<IotAnalysis_Action> IotAnalysis_Actions { get; set; }
        public virtual DbSet<IotSetUpAlert> IotSetUpAlerts { get; set; }
        public virtual DbSet<IotSensor> IotSensors { get; set; }
        // virtual DbSet<ObeAiRecognitionTracking> ObeAiRecognitionTrackings { get; set; }

        public virtual DbSet<FundFiles> FundFiless { get; set; }

        public virtual DbSet<FundAccEntryTracking> FundAccEntryTrackings { get; set; }

        public virtual DbSet<FcmToken> FcmTokens { get; set; }

        /// CMS_Table
        public virtual DbSet<cms_attachments> cms_attachments { get; set; }
        public virtual DbSet<cms_categories> cms_categories { get; set; }
        public virtual DbSet<cms_comments> cms_comments { get; set; }
        public virtual DbSet<cms_extra_fields> cms_extra_fields { get; set; }
        public virtual DbSet<cms_extra_fields_groups> cms_extra_fields_groups { get; set; }
        public virtual DbSet<cms_extra_fields_value> cms_extra_fields_value { get; set; }
        public virtual DbSet<cms_functions> cms_functions { get; set; }
        public virtual DbSet<cms_function_group> cms_function_group { get; set; }
        public virtual DbSet<cms_function_resource> cms_function_resource { get; set; }
        public virtual DbSet<cms_items> cms_items { get; set; }
        public virtual DbSet<cms_rating> cms_rating { get; set; }
        public virtual DbSet<cms_roles> cms_roles { get; set; }
        public virtual DbSet<cms_setting> cms_setting { get; set; }
        public virtual DbSet<cms_tags> cms_tags { get; set; }
        public virtual DbSet<cms_tags_xref> cms_tags_xref { get; set; }

        public virtual DbSet<UrencoAssetsCategory> UrencoAssetsCategorys { get; set; }
        public virtual DbSet<UrencoMaterialProductGroup> UrencoMaterialProductGroup { get; set; }

        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Builder entity

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50).HasColumnName("RoleId");
                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(255);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.NormalizedName).HasMaxLength(255);

                entity.HasIndex(e => e.NormalizedName).HasName("IX_ROLES_NAME").IsUnique();
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {

                entity.Property(e => e.Id).HasMaxLength(50).HasColumnName("UserId");
                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(50);
                entity.Property(e => e.PasswordHash).HasMaxLength(2000);
                entity.Property(e => e.SecurityStamp).HasMaxLength(50);
                entity.Property(e => e.PhoneNumber).HasMaxLength(100);

                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.UserName).IsRequired().HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).IsRequired().HasMaxLength(256);

                entity.HasIndex(e => e.NormalizedEmail).HasName("IX_USERS_EMAIL");
                entity.HasIndex(e => e.NormalizedUserName).HasName("IX_USERS_USER_NAME").IsUnique();
            
            });

            modelBuilder.Entity<AdApplication>(entity =>
            {
               
            });

            modelBuilder.Entity<AdFunction>(entity =>
            {
                
            });

            modelBuilder.Entity<AdAppFunction>(entity =>
            {

            });

            modelBuilder.Entity<AdGroupUser>(entity =>
            {
        
            });

            modelBuilder.Entity<AdOrganization>(entity =>
            {

            });

            modelBuilder.Entity<AdParameter>(entity =>
            {
            
            });

            modelBuilder.Entity<AdPermission>(entity =>
            {
            
            });

            modelBuilder.Entity<AdPrivilege>(entity =>
            {

            });

            modelBuilder.Entity<AdResource>(entity =>
            {
               
            });

            modelBuilder.Entity<AdUserInGroup>(entity =>
            {
                
            });
            #endregion

            base.OnModelCreating(modelBuilder);

            #region Replace all table, column name to snake case
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase(true);

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.ToSnakeCase(true);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase(true);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase(true);
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase(true);
                }
            }
            #endregion
        }
    }
}