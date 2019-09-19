var ctxfolder = "/views/admin/staffCalendar";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select', "pascalprecht.translate", "ngCookies"]);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    var submitFormUpload = function (url, data, callback) {
        var req = {
            method: 'POST',
            url: url,
            headers: {
                'Content-Type': undefined
            },
            data: data
        }
        $http(req).success(callback);
    };
    return {
        getListEmployee: function (callback) {
            $http.post('/Admin/StaffCalendar/GetListEmployee').success(callback);
        },
        getEventCat: function (data, callback) {
            $http.get('/Admin/StaffCalendar/GetEventCat?memberId=' + data).success(callback);
        },
        changeFrameTimeStatus: function (id, frame, callback) {
            $http.post('/Admin/StaffCalendar/ChangeFrametimeStatus/?id=' + id + "&frame=" + frame).success(callback);
        },
        //getMemberCode: function (data, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/GetMemberCode/?MemberType=' + data).success(callback);
        //},
        //notPresent: function (code, frame, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/NotPresent/?Event=' + code + '&Frame=' + frame).success(callback);
        //},
        //getInterviewDate: function (data, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/GetInterviewDate/', data).success(callback);
        //},
        //setInterviewDate: function (data, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/SetInterviewDate/', data).success(callback);
        //},
        //getEventCat: function (data, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/GetEventCat/', data).success(callback);
        //},
        //getEventCatToday: function (callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/GetEventCatToday').success(callback);
        //},
        //changeFrameTimeStatus: function (id, frame, callback) {
        //    $http.post('/Admin/EDMSCalendarManagement/ChangeFrametimeStatus/?id=' + id + "&frame=" + frame).success(callback);
        //},
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice, $cookies, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    var culture = $cookies.get('_CULTURE') || 'vi-VN';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
        $.extend($.validator.messages, {
            min: caption.COM_VALIDATE_VALUE_MIN,
            //max: 'Max some message {0}'
        });
    });
    //$rootScope.MemberType = [
    //    { id: 0, title: "Ứng viên" },
    //    { id: 1, title: "Người dùng" },
    //    { id: 2, title: "Nhân viên" }
    //];

    //$rootScope.CandidateCode = [];
    //dataservice.getMemberCode(0, function (rs) {
    //    angular.forEach(rs, function (value, key) {
    //        $rootScope.CandidateCode.push(value);
    //    });
    //})
    //$rootScope.validationOptions = {
    //    rules: {
    //        CandidateCode: {
    //            required: true
    //        },
    //        Fullname: {
    //            required: true,
    //            maxlength: 255,
    //            minlength: 6
    //        },
    //        Email: {
    //            required: true,
    //            maxlength: 100,
    //            email: true
    //        },
    //        MobilePhone: {
    //            required: true,
    //            maxlength: 11,
    //            minlength: 10,
    //        },
    //        FileCV: {
    //            required: true,
    //            maxlength: 255
    //        },
    //        Address: {
    //            maxlength: 255,
    //            minlength: 15
    //        },
    //        Targeting: {
    //            maxlength: 500
    //        },
    //        Skype: {
    //            maxlength: 255
    //        },
    //        MainSkill: {
    //            maxlength: 255
    //        },
    //        SubSkill: {
    //            maxlength: 255
    //        },
    //        MainPracticeTime: {
    //            maxlength: 255
    //        },
    //        SubPracticeTime: {
    //            maxlength: 255
    //        },
    //        LaptopInfo: {
    //            maxlength: 255
    //        },
    //        SmartphoneInfo: {
    //            maxlength: 255
    //        }
    //    },
    //    messages: {
    //        CandidateCode: {
    //            required: "Lấy mã candidate!"
    //        },
    //        Fullname: {
    //            required: "Nhập họ tên!",
    //            maxlength: "Tên tối đa 255 ký tự!",
    //            minlength: "Tên quá ngắn!"
    //        },
    //        Email: {
    //            required: "Nhập email!",
    //            maxlength: "Email tối đa 100 ký tự!",
    //            email: "Địa chỉ email không hợp lệ!"
    //        },
    //        MobilePhone: {
    //            required: "Nhập số điện thoại!",
    //            maxlength: "Số điện thoại tối đa 11 số!",
    //            minlength: "Số điện thoại tối thiểu 10 số!",
    //        },
    //        FileCV: {
    //            required: "Chọn tệp tin!",
    //            maxlength: "Tên tệp tin quá dài!"
    //        },
    //        Address: {
    //            maxlength: "Địa chỉ tối đa 255 ký tự!",
    //            minlength: "Địa chỉ quá ngắn!"
    //        },
    //        Targeting: {
    //            maxlength: "Mục tiêu tối đa 500 ký tự!"
    //        },
    //        Skype: {
    //            maxlength: "Skype tối đa 255 ký tự!"
    //        },
    //        MainSkill: {
    //            maxlength: "Kỹ năng chính tối đa 255 ký tự!"
    //        },
    //        SubSkill: {
    //            maxlength: "Kỹ năng phụ tối đa 255 ký tự!"
    //        },
    //        MainPracticeTime: {
    //            maxlength: "Thời gian thực tập tối đa 255 ký tự!"
    //        },
    //        SubPracticeTime: {
    //            maxlength: "Thời gian thực tập tối đa 255 ký tự!"
    //        },
    //        LaptopInfo: {
    //            maxlength: "Nhập tối đa 255 ký tự!"
    //        },
    //        SmartphoneInfo: {
    //            maxlength: "Nhập tối đa 255 ký tự!"
    //        }
    //    }
    //}
});

app.config(function ($routeProvider, $validatorProvider, $translateProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    caption = $translateProvider.translations();
    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
        })
        .when('/search', {
            templateUrl: ctxfolder + '/duration-search.html',
            controller: 'duration-search'
        })
        .when('/interview', {
            templateUrl: ctxfolder + '/interview.html',
            controller: 'duration-search'
        })
        .when('/candidates', {
            templateUrl: ctxfolder + '/Candidate-grid.html',
            controller: 'candidate-grid'
        })


    $validatorProvider.setDefaults({
        errorElement: 'span',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else if (element.prop('type') === 'radio' && element.parent('.radio-inline').length) {
                error.insertAfter(element.parent().parent());
            } else if (element.prop('type') === 'checkbox' || element.prop('type') === 'radio') {
                error.appendTo(element.parent().parent());
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) {
            $(element).closest('.form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.form-group').removeClass('has-error');
        },
        success: function (label) {
            label.closest('.form-group').removeClass('has-error');
        }
    });
});


app.controller('index', function ($scope, $rootScope, $compile, $confirm, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    //$scope.table = {};
    //$scope.eventToday = [];
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var vm = $scope;
    $scope.model = {
        MemberId: '',
        FromDate: '',
        ToDate: '',
        //Worktype: -1,
        //Membertype: ''
    };
    //$scope.interview = {
    //    CandidateCode: '',
    //    InterviewDate: ''
    //};
    //$scope.WorkType = [
    //    { id: 0, title: "Full time" },
    //    { id: 1, title: "Morning" },
    //    { id: 2, title: "Affternoon" },
    //    { id: 3, title: "Evening" }
    //];


    $scope.initLoad = function () {
        dataservice.getListEmployee(function (rs) {
            $scope.listEmployeeData = rs;
        });
    }
    $scope.initLoad();

    //tab calender
    $scope.reloadCalender = function () {
        $('#calendar').fullCalendar('refetchEvents');
    }
    function loadCalendar(id) {
        $('#' + id).fullCalendar({
            defaultView: 'month',
            selectable: true,
            editable: true,
            eventLimit: true,
            header: {
                center: 'title',
                left: 'prev',
                right: 'next'
            },
            dayNames: [caption.SC_LIST_COL_DAY_NAME_SUNDAY, caption.SC_LIST_COL_DAY_NAME_MONDAY, caption.SC_LIST_COL_DAY_NAME_TUESDAY, caption.SC_LIST_COL_DAY_NAME_WEDNESDAY, caption.SC_LIST_COL_DAY_NAME_THURSDAY, caption.SC_LIST_COL_DAY_NAME_FRIDAY, caption.SC_LIST_COL_DAY_NAME_SATURDAY],
            monthNames: [caption.SC_LBL_MONTH_NAME_JAN + ' - ', caption.SC_LBL_MONTH_NAME_FEB + ' - ', caption.SC_LBL_MONTH_NAME_MAR + ' - ', caption.SC_LBL_MONTH_NAME_APR + ' - ', caption.SC_LBL_MONTH_NAME_MAY + ' - ', caption.SC_LBL_MONTH_NAME_JUNE + ' - ', caption.SC_LBL_MONTH_NAME_JULY + ' - ', caption.SC_LBL_MONTH_NAME_AUG + ' - ', caption.SC_LBL_MONTH_NAME_SEPT + ' - ', caption.SC_LBL_MONTH_NAME_OCT + ' - ', caption.SC_LBL_MONTH_NAME_NOV + ' - ', caption.SC_LBL_MONTH_NAME_DEC + ' - '],
            monthNamesShort: [caption.SC_LBL_MONTH_NAME_JAN + ' - ', caption.SC_LBL_MONTH_NAME_FEB + ' - ', caption.SC_LBL_MONTH_NAME_MAR + ' - ', caption.SC_LBL_MONTH_NAME_APR + ' - ', caption.SC_LBL_MONTH_NAME_MAY + ' - ', caption.SC_LBL_MONTH_NAME_JUNE + ' - ', caption.SC_LBL_MONTH_NAME_JULY + ' - ', caption.SC_LBL_MONTH_NAME_AUG + ' - ', caption.SC_LBL_MONTH_NAME_SEPT + ' - ', caption.SC_LBL_MONTH_NAME_OCT + ' - ', caption.SC_LBL_MONTH_NAME_NOV + ' - ', caption.SC_LBL_MONTH_NAME_DEC + ' - '],
            dayNamesShort: [caption.SC_LIST_COL_DAY_NAME_SUNDAY, caption.SC_LIST_COL_DAY_NAME_MONDAY, caption.SC_LIST_COL_DAY_NAME_TUESDAY, caption.SC_LIST_COL_DAY_NAME_WEDNESDAY, caption.SC_LIST_COL_DAY_NAME_THURSDAY, caption.SC_LIST_COL_DAY_NAME_FRIDAY, caption.SC_LIST_COL_DAY_NAME_SATURDAY],

            //eventRender: function (event, element) {
            //    element.bind('dblclick', function () {
            //        if ($("#change-calendar:checked").length == 1) {
            //            deleteFrameTime(event.frameTime, event.eventCode, event);
            //        }
            //        if ($("#timekeeping:checked").length == 1) {
            //            console.log("Chấm công");
            //            if (event.on == "True") {
            //                notPresent(event);
            //            }
            //        }

            //    });
            //},
            events: function (start, end, timezone, callback) {
                dataservice.getEventCat($scope.model.MemberId, function (rs) {
                    var event = [];
                    angular.forEach(rs, function (value, key) {
                        var morning = {
                            title: "1.Sáng",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Morning == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 0
                        }
                        var afternoon = {
                            title: "2.Chiều",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Afternoon == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 1
                        }
                        var evening = {
                            title: "3.Tối",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Evening == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 2
                        }
                        event.push(morning);
                        event.push(afternoon);
                        event.push(evening);
                    })
                    callback(event);
                })
            },
            eventClick: function (calEvent) {
                //calEvent.color = '#bdc3c7';
                //deleteFrameTime(calEvent.frameTime, calEvent.eventCode, calEvent);
                //$("a.fc-day-grid-event.fc-h-event.fc-event.fc-start.fc-end.fc-draggable").click(function () {
                //    var stt = $(this).hasClass("true");
                //    if (stt == true) {
                //        $(this).removeClass("true");
                //        $(this).css('background', 'silver');
                //    } else {
                //        $(this).addClass(" true");
                //        $(this).css('background', 'green');
                //    }
                //});
            },
            dayClick: function (date) {
            },
            customButtons: {
            }
        })
    }
    function showHideSearch() {
        $(".btnSearch").click(function () {
            $(".input-search").removeClass('hidden');
            $(".btnSearch").hide();
        });
        $(".close-input-search").click(function () {
            $(".input-search").addClass('hidden');
            $(".btnSearch").show();
        });
    }
    setTimeout(function () {
        loadCalendar("calendar");
        showHideSearch();
    }, 100);



    //function createEvent(date, title, on, present, eventCode, frameTime) {
    //    $('#calendar').fullCalendar('renderEvent', {
    //        title: title + (present == "False" ? " - Vắng" : ""),
    //        textColor: present == "False" ? "#c0392b" : "#ffffff",
    //        start: date,
    //        allDay: true,
    //        color: on == "True" ? '#16a085' : '#bdc3c7',
    //        on: on,
    //        eventCode: eventCode,
    //        frameTime: frameTime
    //    });
    //}
    //function showEvent(rs) {
    //    angular.forEach(rs, function (value, key) {
    //        // var date = moment(value.DatetimeEvent);
    //        var frameTime = value.FrameTime.split(';');
    //        //if (date.isValid()) {
    //        createEvent(value.DatetimeEvent, "1.Sáng", frameTime[0], value.MorningPresent, value.EventCatCode, 0);
    //        createEvent(value.DatetimeEvent, "2.Chiều", frameTime[1], value.AfternoonPresent, value.EventCatCode, 1);
    //        createEvent(value.DatetimeEvent, "3.Tối", frameTime[2], value.EveningPresent, value.EventCatCode, 2);
    //        //}
    //    })
    //}
    //function deleteFrameTime(frameTime, eventCode, event) {
    //    //console.log("Delete frametime: " + frameTime + " event code: " + eventCode);
    //    //$confirm({ text: 'Bạn có chắc chắn off?', title: 'Xác nhận', cancel: ' Hủy ' })
    //    //    .then(function () {
    //    dataservice.deleteFrameTime(eventCode, frameTime, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        }
    //        else {
    //            //$scope.loadDate();
    //            event.color = '#bdc3c7';
    //            offEvent(event);
    //        }
    //    });
    //    //});
    //}
    //function offEvent(event) {
    //    $('#calendar').fullCalendar('updateEvent', event);
    //}
    //function addNotPresentToEvent(event) {
    //    event.color = "#16a085";
    //    event.title = event.title + ' - Vắng';
    //    event.textColor = "#c0392b";
    //    $('#calendar').fullCalendar('updateEvent', event);
    //}
    //function notPresent(event) {
    //    dataservice.notPresent(event.eventCode, event.frameTime, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        }
    //        else {
    //            App.toastrSuccess(rs.Title);
    //            addNotPresentToEvent(event);
    //        }
    //    })
    //}



    //tab today
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/StaffCalendar/GetEventCatGridToday",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.MemberId = $scope.model.MemberId;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [2, 'desc'])
        .withOption('serverSide', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('fullname').withTitle('{{"SC_LIST_COL_FULL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('DatetimeEvent').withTitle('{{"SC_LIST_COL_DATE" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Morning').withTitle('{{"SC_LIST_COL_MORNING" | translate}}').renderWith(function (data, type, full) {
        return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.MorningPresent == "False" ? "Vắng" : "");

    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Afternoon').withTitle('{{"SC_LIST_COL_AFTERNOON" | translate}}').renderWith(function (data, type, full) {
        return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.AfternoonPresent == "False" ? "Vắng" : "");
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Evening').withTitle('{{"SC_LIST_COL_EVENING" | translate}}').renderWith(function (data, type, full) {
        return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.EveningPresent == "False" ? "Vắng" : "");
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('').notSortable().withTitle('Thao tác').renderWith(function (data, type, full, meta) {
    //    return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
    //        '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    //}));
    vm.reloadData = reloadData;
    vm.dt = {
        dtInstance: {}
    }
    function reloadData(resetPaging) {
        vm.dt.dtInstance.reloadData(callback, resetPaging);
    }
    function callback(json) {

    }
    function toggleAll(selectAll, selectedItems) {
        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                selectedItems[id] = selectAll;
            }
        }
    }
    function toggleOne(selectedItems) {
        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                if (!selectedItems[id]) {
                    vm.selectAll = false;
                    return;
                }
            }
        }
        vm.selectAll = true;
    }
    $scope.reloadTable = function () {
        reloadData(true);
    };
    $scope.changeStatusFrameTime = function (eventId, frameTime) {
        dataservice.changeFrameTimeStatus(eventId, frameTime, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                element = document.getElementsByClassName('sorting')[0];
                element.click();
                $scope.reloadCalender();
            }
        });
    };
    //var events = [];
    //$scope.initData = function () {
    //    dataservice.getEventCatToday(function (rs) {
    //        $scope.eventToday = rs;
    //    });
    //};
    ////$scope.initData();
    //$scope.search = function () {
    //    $scope.loadDate($scope.model);
    //    $scope.reload();
    //};
    //$scope.getUserId = function (item) {
    //    //console.log(item);
    //    dataservice.getMemberCode(item.id, function (rs) {
    //        $scope.MemberCode = rs;
    //    })
    //}
    //$scope.loadDate = function (data) {
    //    App.blockUI({
    //        target: "#contentMain",
    //        boxed: true,
    //        message: 'loading...'
    //    });
    //    $('#calendar').fullCalendar('removeEvents');
    //    dataservice.getEventCat(data, function (rs) {
    //        //console.log(rs);
    //        events = rs;
    //        showEvent(rs);
    //        App.unblockUI("#contentMain");
    //    });
    //    dataservice.getInterviewDate(data, function (rs) {
    //        $('#calendar-interview').fullCalendar('removeEvents');
    //        angular.forEach(rs, function (value, key) {
    //            $('#calendar-interview').fullCalendar('renderEvent', {
    //                title: value.InterviewDate.split('T')[1] + "-" + value.Fullname,
    //                color: '#16a085',
    //                start: value.InterviewDate,
    //                allDay: true,
    //            })
    //        });
    //    });
    //}
    //$scope.changeStatusFrameTime = function (eventId, frameTime) {
    //    console.log(eventId, frameTime);
    //    dataservice.changeFrameTimeStatus(eventId, frameTime, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        }
    //        else {
    //            App.toastrSuccess(rs.Title);
    //            reloadData(false);
    //            reloadData1(false);
    //            //element = document.getElementsByClassName('sorting')[0];
    //            //element.click();
    //        }
    //    });
    //};
    //$scope.saveInterviewDate = function () {
    //    console.log($scope.interview);
    //    dataservice.setInterviewDate($scope.interview, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        }
    //        else {
    //            App.toastrSuccess(rs.Title);
    //        }
    //    });
    //}
    //$scope.showCalendar = function () {
    //    $('.fc-next-button').click();
    //    $('.fc-prev-button').click();
    //}







    //vm.dtOptions1 = DTOptionsBuilder.newOptions()
    //    .withOption('ajax', {
    //        url: "/Admin/EDMSCalendarManagement/GetGridToday",
    //        beforeSend: function (jqXHR, settings) {
    //            App.blockUI({
    //                target: "#contentMain",
    //                boxed: true,
    //                message: 'loading...'
    //            });
    //        },
    //        type: 'POST',
    //        data: function (d) {

    //        },
    //        complete: function () {
    //            App.unblockUI("#contentMain");
    //        }
    //    })
    //    .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
    //    .withDataProp('data').withDisplayLength(10)
    //    .withOption('order', [1, 'asc'])
    //    .withOption('serverSide', true)
    //    .withOption('headerCallback', function (header) {
    //        if (!$scope.headerCompiled) {
    //            $scope.headerCompiled = true;
    //            $compile(angular.element(header).contents())($scope);
    //        }
    //    })
    //    .withOption('initComplete', function (settings, json) {
    //    })
    //    .withOption('createdRow', function (row, data, dataIndex) {
    //        const contextScope = $scope.$new(true);
    //        contextScope.data = data;
    //        contextScope.contextMenu = $scope.contextMenu;
    //        $compile(angular.element(row))($scope);
    //        $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
    //    });

    //vm.dtColumns1 = [];
    //vm.dtColumns1.push(DTColumnBuilder.newColumn("").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.id] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', ''));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('MemberId').withTitle('Mã nhân viên').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('Fullname').withTitle('Họ tên').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('DatetimeEvent').withTitle('Ngày').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('Morning').withTitle('Sáng').renderWith(function (data, type, full) {
    //    return (data === "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.MorningPresent == "False" ? "Vắng" : "");

    //}));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('Afternoon').withTitle('Chiều').renderWith(function (data, type, full) {
    //    return (data === "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.AfternoonPresent == "False" ? "Vắng" : "");
    //}));
    //vm.dtColumns1.push(DTColumnBuilder.newColumn('Evening').withTitle('Tối').renderWith(function (data, type, full) {
    //    return (data === "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.EveningPresent == "False" ? "Vắng" : "");
    //}));
    //vm.reloadData1 = reloadData1;
    //vm.dtInstance1 = {};

    //function reloadData1(resetPaging) {
    //    vm.dtInstance1.reloadData(callback, resetPaging);
    //}

    setTimeout(function () {
        //$("#FromTo").datepicker({
        //    inline: false,
        //    autoclose: true,
        //    format: "dd/mm/yyyy",
        //    fontAwesome: true,
        //}).on('changeDate', function (selected) {
        //    var maxDate = new Date(selected.date.valueOf());
        //    $('#DateTo').datepicker('setStartDate', maxDate);
        //});
        //$("#DateTo").datepicker({
        //    inline: false,
        //    autoclose: true,
        //    format: "dd/mm/yyyy",
        //    fontAwesome: true,
        //}).on('changeDate', function (selected) {
        //    var maxDate = new Date(selected.date.valueOf());
        //    $('#FromTo').datepicker('setEndDate', maxDate);
        //});
        //$('.end-date').click(function () {
        //    $('#FromTo').datepicker('setEndDate', null);
        //});
        //$('.start-date').click(function () {
        //    $('#DateTo').datepicker('setStartDate', null);
        //});


        //$(".date").not(".interview-date").datepicker({
        //    inline: false,
        //    autoclose: true,
        //    format: "dd/mm/yyyy",
        //    fontAwesome: true,
        //});
        //createCalendar("calendar-interview");
        //$(".fc-prev-button").click(function () {
        //    showEvent(events);
        //});
        //$(".fc-next-button").click(function () {
        //    showEvent(events);
        //});

        //setTimeout(function () {
        //    angular.forEach($scope.eventToday, function (value, key) {
        //        var present = (value.MorningPresent === "False" ? " - S:Vắng" : "") + (value.AfternoonPresent === "False" ? " - T:Vắng" : "") + (value.EveningPresent === "False" ? " - C:Vắng" : "");
        //        $('#calendar').fullCalendar('renderEvent', {
        //            title: value.FullName + "(" + value.MemberId + ") " + present,
        //            textColor: present !== "" ? "#000000" : "#ffffff",
        //            start: value.DatetimeEvent,
        //            allDay: true,
        //            color: present !== "" ? '#fff947' : '#16a085'
        //            //on: on,
        //            //eventCode: eventCode,
        //            //frameTime: frameTime
        //        });
        //    });
        //}, 3000);
    }, 600);
});

//app.controller('calendar-overview', function ($scope, $rootScope, $compile, dataservice) {
//    $scope.eventToday = [];
//    $scope.initData = function () {
//        dataservice.getEventCatToday(function (rs) {
//            $scope.eventToday = rs;
//        });
//    };
//    $scope.initData();
//    $scope.loadData = function () {
//        App.blockUI({
//            target: "#contentMain",
//            boxed: true,
//            message: 'loading...'
//        });
//        $('#calendar').fullCalendar('removeEvents');
//        $("#calendar-overview .fc-left button").click();
//        $("#calendar-overview .fc-right button").click();

//        angular.forEach($scope.eventToday, function (value, key) {
//            var present = (value.MorningPresent === "False" ? " - S:Vắng" : "") + (value.AfternoonPresent === "False" ? " - T:Vắng" : "") + (value.EveningPresent === "False" ? " - C:Vắng" : "");
//            $('#calendar-overview').fullCalendar('renderEvent', {
//                title: value.FullName + "(" + value.MemberId + ") " + present,
//                textColor: present !== "" ? "#000000" : "#ffffff",
//                start: value.DatetimeEvent,
//                allDay: true,
//                color: present !== "" ? '#fff947' : '#16a085'
//                //on: on,
//                //eventCode: eventCode,
//                //frameTime: frameTime
//            });
//        });
//        App.unblockUI("#contentMain");
//    };
//    function createCalendar(id) {
//        $('#' + id).fullCalendar({
//            defaultView: 'month',
//            defaultDate: new Date(),
//            selectable: true,
//            editable: false,
//            selectHelper: true,
//            eventLimit: true,
//            header: {
//                center: 'title',
//                left: 'prev',
//                right: 'next'
//            },
//            eventRender: function (event, element) {
//                element.bind('dblclick', function () {

//                });
//            },
//            eventClick: function (calEvent) {

//            },
//            dayClick: function (date) {
//            },
//            customButtons: {
//            }
//        });
//    }
//    setTimeout(function () {
//        createCalendar("calendar-overview");
//    }, 600);
//});