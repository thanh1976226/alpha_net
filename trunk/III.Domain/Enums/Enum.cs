﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace III.Domain.Enums
{
    public enum WpStatus
    {
        [Display(Name = "WP_PENDING")]
        [Description("WP_PENDING")]
        WpPending = 1,

        [Display(Name = "WP_APPROVED")]
        [Description("WP_APPROVED")]
        WpApproved = 2,

        [Display(Name = "WP_PROCESSING")]
        [Description("WP_PROCESSING")]
        WpProcessing = 3,

        [Display(Name = "WP_DONE")]
        [Description("WP_DONE")]
        WpDone = 4,

        [Display(Name = "WP_CANCEL")]
        [Description("WP_CANCEL")]
        WpCancel = 5,

        [Display(Name = "WP_REJECT")]
        [Description("WP_REJECT")]
        WpReject = 6,

        [Display(Name = "WP_WAITING")]
        [Description("WP_WAITING")]
        WpWaiting = 7,
    }
    public enum RouteStatus
    {
        [Display(Name = "ROUTE_PENDING")]
        [Description("ROUTE_PENDING")]
        RoutePending = 1,

        [Display(Name = "ROUTE_PROCESSING")]
        [Description("ROUTE_PROCESSING")]
        RouteProcessing = 2,

        [Display(Name = "ROUTE_DONE")]
        [Description("ROUTE_DONE")]
        RouteDone = 3,

        [Display(Name = "ROUTE_CANCEL")]
        [Description("ROUTE_CANCEL")]
        RouteCancel = 4,
    }
    public enum StaffStauts
    {
        /// <summary>
        /// Check in for staff
        /// </summary>
        [Display(Name = "CHECKIN")]
        [Description("CheckIn")]
        CheckIn = 1,

        ///<summary>
        ///Check out for staff
        ///</summary>
        [Display(Name = "CHECKOUT")]
        [Description("CheckOut")]
        CheckOut = 2,

        /// <summary>
        /// No Working
        /// </summary>
        [Display(Name = "NOTWORK")]
        [Description("Báo nghỉ")]
        NoWork = 3,


        [Display(Name = "GOLATE")]
        [Description("Đến muộn")]
        GoLate = 4,

        [Display(Name = "QUITWORK")]
        [Description("Nghỉ việc")]
        QuitWork = 5,
    }


    /// <summary>
    /// Dispatches
    /// </summary>
    public enum DocumentStatusEnum
    {
        [Display(Name = "CREATED")]
        [Description("CREATED")]
        Created = 1,

        [Display(Name = "UPDATED")]
        [Description("UPDATED")]
        Updated = 2,

        [Display(Name = "DONE")]
        [Description("DONE")]
        Done = 3,

        [Display(Name = "REVIEW")]
        [Description("REVIEW")]
        Review = 4,

        [Display(Name = "PENDING")]
        [Description("PENDING")]
        Pending = 5,

        [Display(Name = "SEND")]
        [Description("SEND")]
        Send = 6,

        [Display(Name = "ADD_SEND")]
        [Description("ADD_SEND")]
        AddSend = 7,

        [Display(Name = "COORDINATED")]
        [Description("COORDINATED")]
        Coordinated = 8,

        [Display(Name = "SEND_COORDINATED")]
        [Description("SEND_COORDINATED")]
        SendCoordinated = 9,

        [Display(Name = "NODONE")]
        [Description("NODONE")]
        NoDone = 10,

        [Display(Name = "PROCESSING")]
        [Description("PROCESSING")]
        Processing = 11,

        [Display(Name = "NOREVIEW")]
        [Description("NOREVIEW")]
        NoReview = 12,

        [Display(Name = "RECOVER")]
        [Description("RECOVER")]
        Recover = 13,
    }
    public enum DocumentTypeEnum
    {
        [Display(Name = "IBT")]
        [Description("IBT")]
        InBoxType = 1,

        [Display(Name = "DM")]
        [Description("DM")]
        DM = 2,

        [Display(Name = "DK")]
        [Description("DK")]
        DK = 3,

        [Display(Name = "DQT")]
        [Description("DQT")]
        DQT = 4,

        [Display(Name = "LVVB")]
        [Description("LVVB")]
        LVVB = 5,

        [Display(Name = "LVB")]
        [Description("LVB")]
        LVB = 6,

        [Display(Name = "SVB")]
        [Description("SVB")]
        SVB = 7,

        [Display(Name = "KHVB")]
        [Description("KHVB")]
        KHVB = 8,

        [Display(Name = "QTXLVB")]
        [Description("QTXLVB")]
        QTXLVB = 9,

        [Display(Name = "GET_METHOD")]
        [Description("GET_METHOD")]
        GET_METHOD = 10,

        [Display(Name = "OBT")]
        [Description("OBT")]
        OBT = 11,

        [Display(Name = "IBT")]
        [Description("IBT")]
        IBT = 12,
    }
    public enum DocumentRoleEnum
    {
        [Display(Name = "Main")]
        [Description("Main")]
        Main = 1,

        [Display(Name = "Support")]
        [Description("Support")]
        Support = 2,

        [Display(Name = "ReView")]
        [Description("ReView")]
        ReView = 3,
    }
    public enum GroupUserEnum
    {
        [Display(Name = "LANHDAO")]
        [Description("LANHDAO")]
        LD = 2,
        [Display(Name = "CHUYEN_VIEN")]
        [Description("CHUYEN_VIEN")]
        CV = 3,
        [Display(Name = "VC_BRAND")]
        [Description("VC_BRAND")]
        VC_BRAND = 4,
        [Display(Name = "VC_TRANSPOTER_WEIGHT")]
        [Description("VC_TRANSPOTER_WEIGHT")]
        TRANSPOTER_WEIGHT = 5


    }
    public enum RoleEnum
    {
        [Display(Name = "VT")]
        [Description("VT")]
        VT = 1,
        [Display(Name = "TK")]
        [Description("TK")]
        TK = 2,
        [Display(Name = "CHUYEN_VIEN")]
        [Description("CHUYEN_VIEN")]
        CV = 3,
        [Display(Name = "VC_SHOP")]
        [Description("VC_SHOP")]
        SHOP = 4,
        [Display(Name = "VC_AGENT")]
        [Description("VC_AGENT")]
        AGENT = 5,
        [Display(Name = "VC_DISTRIBUTOR")]
        [Description("VC_DISTRIBUTOR")]
        DISTRIBUTOR = 6

    }
    ///<summary>
    ///Store
    ///</summary>
    public enum MapType
    {
        [Display(Name = "STORE")]
        [Description("STORE")]
        Store = 1,
        [Display(Name = "CUSTOMER")]
        [Description("CUSTOMER")]
        Customer = 2,
        [Display(Name = "SUPPLIER")]
        [Description("SUPPLIER")]
        Supplier = 3,
    }
    ///<summary>
    ///ACB
    ///</summary>
    public enum GroupUserCode
    {
        [Display(Name = "G_AD")]
        [Description("G_AD")]
        GroupAdmin = 1,
        [Display(Name = "KETOAN")]
        [Description("KETOAN")]
        GroupWareHouse = 2,
        [Display(Name = "G_BR")]
        [Description("G_BR")]
        GroupBranch = 3,
    }

    ///<summary>
    ///Project
    ///</summary>
    public enum ProjectStatusEnum
    {
        [Display(Name = "ACTIVE")]
        [Description("Hoạt động")]
        Active = 1,

        [Display(Name = "DONE")]
        [Description("Hoàn thành")]
        Done = 2,
    }
    ///<summary>
    ///Card
    ///</summary>
    public enum CardAction
    {
        [Display(Name = "REVIEW")]
        [Description("Đã xem")]
        Review = 0,

        [Display(Name = "REJECT")]
        [Description("Từ chối")]
        Reject = 1,

        [Display(Name = "ACCEPT")]
        [Description("Đồng ý")]
        Accept = 2,
    }

    ///<summary>
    ///REQUEST STATUS
    ///</summary>
    public enum RequestStatus
    {
        [Display(Name = "RECEIPTPROFILE_STATUS_PENDING")]
        [Description("Khởi tạo")]
        Pending = 1,

        [Display(Name = "RECEIPTPROFILE_STATUS_WAITING")]
        [Description("Đang chờ")]
        Waiting = 2,

        [Display(Name = "RECEIPTPROFILE_STATUS_APPROVED")]
        [Description("Đã duyệt")]
        Approved = 3,

        [Display(Name = "RECEIPTPROFILE_STATUS_REJECTED")]
        [Description("Từ chối")]
        Rejected = 4,
    }

    ///<summary>
    ///OBJECT SHARE
    ///</summary>
    public enum FileObjectShareObj
    {
        [Display(Name = "FILE_OBJ_CONTRACT")]
        [Description("Hợp đồng")]
        Contract = 1,

        [Display(Name = "FILE_OBJ_PROJECT")]
        [Description("Dự án")]
        Project = 2,

        [Display(Name = "FILE_OBJ_SUPPLIER")]
        [Description("Nhà cung cấp")]
        Supplier = 3,

        [Display(Name = "FILE_OBJ_CUSTOMMER")]
        [Description("Khách hàng")]
        Custommer = 4,
    }
    /// <summary>
    ///log status
    /// </summary>
    public enum TypeLogStatus
    {
        [Display(Name = "STATUS_LINE")]
        [Description("Dòng trạng thái phiếu nhập/xuất")]
        StatusLine = 1,

        [Display(Name = "STATUS_LINE_PAY")]
        [Description("Dòng trạng thái phiếu trả")]
        StatusLinePay = 2,

        [Display(Name = "STATUS_RECEIPT")]
        [Description("Trạng thái phiếu")]
        StatusReceipt = 3,
    }
}
