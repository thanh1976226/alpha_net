var ctxfolder = "/views/admin/calendarInterview";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select',"pascalprecht.translate", "ngCookies"]);

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
        getMemberCode: function (data, callback) {
            $http.post('/Admin/CalendarInterview/GetMemberCode/?MemberType=' + data).success(callback);
        },
        notPresent: function (code, frame, callback) {
            $http.post('/Admin/CalendarInterview/NotPresent/?Event=' + code + '&Frame=' + frame).success(callback);
        },
        getEventCat: function (data, callback) {
            $http.post('/Admin/CalendarInterview/GetEventCat/', data).success(callback);
        },
        getInterviewDate: function (data, callback) {
            $http.post('/Admin/CalendarInterview/GetInterviewDate/', data).success(callback);
        },
        setInterviewDate: function (data, callback) {
            $http.post('/Admin/CalendarInterview/SetInterviewDate/', data).success(callback);
        },
        getEventCatToday: function (callback) {
            $http.post('/Admin/CalendarInterview/GetEventCatToday').success(callback);
        }
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice, $cookies, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    //$rootScope.MemberType = [
    //    { id: 0, title: "Ứng viên" },
    //    { id: 1, title: "Người dùng" },
    //    { id: 2, title: "Nhân viên" }
    //];
    var culture = $cookies.get('_CULTURE') || 'vi-VN';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
        $.extend($.validator.messages, {
            min: caption.COM_VALIDATE_VALUE_MIN,
            //max: 'Max some message {0}'
        });
    });
    $rootScope.CandidateCode = [];
    dataservice.getMemberCode(0, function (rs) {
        angular.forEach(rs, function (value, key) {
            $rootScope.CandidateCode.push(value);
        });
    })
    $rootScope.date = new Date();
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
app.directive('noFloat', function () {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs, ctrl) {
            elm.on('keydown', function (event) {
                if ([110, 190].indexOf(event.which) > -1) {
                    // dot and numpad dot
                    event.preventDefault();
                    return false;
                }
                else {
                    return true;
                }
            });
        }
    }
});


app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    //$scope.table = {};
    //var vm = $scope.table;
    //$scope.selected = [];
    $scope.interview = {
        CandidateCode: '',
        InterviewDate: ''
    };
    //$scope.selectAll = false;
    //$scope.toggleAll = toggleAll;
    //$scope.toggleOne = toggleOne;
    $scope.eventToday = [];
    $scope.model = {
        CandidateCode: '',
        FromDate: '',
        ToDate: '',
        //Worktype: -1,
        //Membertype: ''
    };

    //$scope.WorkType = [
    //    { id: 0, title: "Full time" },
    //    { id: 1, title: "Morning" },
    //    { id: 2, title: "Affternoon" },
    //    { id: 3, title: "Evening" }
    //];

    //var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    //vm.dtOptions = DTOptionsBuilder.newOptions()
    //    .withOption('ajax', {
    //        url: "/Admin/CalendarInterview/GetEventCatGrid",
    //        beforeSend: function (jqXHR, settings) {
    //            App.blockUI({
    //                target: "#contentMain",
    //                boxed: true,
    //                message: 'loading...'
    //            });
    //        },
    //        type: 'POST',
    //        data: function (d) {
    //            d.MemberCode = $scope.model.MemberCode;
    //            d.FromDate = $scope.model.FromDate;
    //            d.ToDate = $scope.model.ToDate;
    //            d.Worktype = $scope.model.Worktype;
    //            d.Membertype = $scope.model.Membertype;
    //        },
    //        complete: function () {
    //            App.unblockUI("#contentMain");
    //        }
    //    })
    //    .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
    //    .withDataProp('data').withDisplayLength(15)
    //    .withOption('order', [2, 'desc'])
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

    //vm.dtColumns = [];
    //vm.dtColumns.push(DTColumnBuilder.newColumn("").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.id] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', 'hidden'));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('MemberId').withTitle('Mã nhân viên').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Fullname').withTitle('Họ tên').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('DatetimeEvent').withTitle('Ngày').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Morning').withTitle('Sáng').renderWith(function (data, type, full) {
    //    return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.MorningPresent == "False" ? "Vắng" : "");

    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Afternoon').withTitle('Chiều').renderWith(function (data, type, full) {
    //    return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.AfternoonPresent == "False" ? "Vắng" : "");
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Evening').withTitle('Tối').renderWith(function (data, type, full) {
    //    return (data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
    //        '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>') + (full.EveningPresent == "False" ? "Vắng" : "");
    //}));
    ////vm.dtColumns.push(DTColumnBuilder.newColumn('').notSortable().withTitle('Thao tác').renderWith(function (data, type, full, meta) {
    ////    return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
    ////        '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    ////}));
    //vm.reloadData = reloadData;
    //vm.dtInstance = {};
    //function reloadData(resetPaging) {
    //    vm.dtInstance.reloadData(callback, resetPaging);
    //}
    //function callback(json) {

    //}
    //function toggleAll(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOne(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    //$scope.reload = function () {
    //    reloadData(true);
    //};

    var events = [];
    $scope.initData = function () {
        dataservice.getEventCatToday(function (rs) {
            $scope.eventToday = rs;
        });
    };
    //$scope.initData();
    $scope.search = function () {
        $('#calendar-interview').fullCalendar('refetchEvents');
    };
    $scope.getUserId = function (item) {
        dataservice.getMemberCode(item.id, function (rs) {
            $scope.MemberCode = rs;
        })
    }
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
    $scope.saveInterviewDate = function () {
        dataservice.setInterviewDate($scope.interview, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $('#calendar-interview').fullCalendar('refetchEvents');
            }
        });
    }
    //$scope.showCalendar = function () {
    //    $('.fc-next-button').click();
    //    $('.fc-prev-button').click();
    //}
    function createEvent(date, title, on, present, eventCode, frameTime) {
        $('#calendar').fullCalendar('renderEvent', {
            title: title + (present == "False" ? " - Vắng" : ""),
            textColor: present == "False" ? "#c0392b" : "#ffffff",
            start: date,
            allDay: true,
            color: on == "True" ? '#16a085' : '#bdc3c7',
            on: on,
            eventCode: eventCode,
            frameTime: frameTime
        });
    }
    function showEvent(rs) {
        angular.forEach(rs, function (value, key) {
            // var date = moment(value.DatetimeEvent);
            var frameTime = value.FrameTime.split(';');
            //if (date.isValid()) {
            createEvent(value.DatetimeEvent, "1.Sáng", frameTime[0], value.MorningPresent, value.EventCatCode, 0);
            createEvent(value.DatetimeEvent, "2.Chiều", frameTime[1], value.AfternoonPresent, value.EventCatCode, 1);
            createEvent(value.DatetimeEvent, "3.Tối", frameTime[2], value.EveningPresent, value.EventCatCode, 2);
            //}
        })
    }
    function deleteFrameTime(frameTime, eventCode, event) {
        dataservice.deleteFrameTime(eventCode, frameTime, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                //$scope.loadDate();
                event.color = '#bdc3c7';
                offEvent(event);
            }
        });
    }
    function offEvent(event) {
        $('#calendar').fullCalendar('updateEvent', event);
    }
    function addNotPresentToEvent(event) {
        event.color = "#16a085";
        event.title = event.title + ' - Vắng';
        event.textColor = "#c0392b";
        $('#calendar').fullCalendar('updateEvent', event);
    }
    function notPresent(event) {
        dataservice.notPresent(event.eventCode, event.frameTime, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                addNotPresentToEvent(event);
            }
        })
    }
    function createCalendar(id) {
        $('#' + id).fullCalendar({
            defaultView: 'month',
            selectable: true,
            editable: true,
            eventLimit: true,
            header: {
                center: 'title',
                left: 'prev,today',
                right: 'next'
            },
            buttonText: {
                today: caption.CI_BTN_TODAY,
                icon: 'far fa-calendar-check'
            },
            dayNames: [caption.CI_COL_DAY_NAME_SUNDAY, caption.CI_COL_DAY_NAME_MONDAY, caption.CI_COL_DAY_NAME_TUESDAY, caption.CI_COL_DAY_NAME_WEDNESDAY, caption.CI_COL_DAY_NAME_THURSDAY, caption.CI_COL_DAY_NAME_FRIDAY, caption.CI_COL_DAY_NAME_SATURDAY],
            monthNames: [caption.CI_LBL_MONTH_NAME_JAN + ' - ', caption.CI_LBL_MONTH_NAME_FEB + ' - ', caption.CI_LBL_MONTH_NAME_MAR + ' - ', caption.CI_LBL_MONTH_NAME_APR + ' - ', caption.CI_LBL_MONTH_NAME_MAY + ' - ', caption.CI_LBL_MONTH_NAME_JUNE + ' - ', caption.CI_LBL_MONTH_NAME_JULY + ' - ', caption.CI_LBL_MONTH_NAME_AUG + ' - ', caption.CI_LBL_MONTH_NAME_SEPT + ' - ', caption.CI_LBL_MONTH_NAME_OCT + ' - ', caption.CI_LBL_MONTH_NAME_NOV + ' - ', caption.CI_LBL_MONTH_NAME_DEC + ' - '],
            monthNamesShort: [caption.CI_LBL_MONTH_NAME_JAN + ' - ', caption.CI_LBL_MONTH_NAME_FEB + ' - ', caption.CI_LBL_MONTH_NAME_MAR + ' - ', caption.CI_LBL_MONTH_NAME_APR + ' - ', caption.CI_LBL_MONTH_NAME_MAY + ' - ', caption.CI_LBL_MONTH_NAME_JUNE + ' - ', caption.CI_LBL_MONTH_NAME_JULY + ' - ', caption.CI_LBL_MONTH_NAME_AUG + ' - ', caption.CI_LBL_MONTH_NAME_SEPT + ' - ', caption.CI_LBL_MONTH_NAME_OCT + ' - ', caption.CI_LBL_MONTH_NAME_NOV + ' - ', caption.CI_LBL_MONTH_NAME_DEC + ' - '],
            dayNamesShort: [caption.CI_COL_DAY_NAME_SUNDAY, caption.CI_COL_DAY_NAME_MONDAY, caption.CI_COL_DAY_NAME_TUESDAY, caption.CI_COL_DAY_NAME_WEDNESDAY, caption.CI_COL_DAY_NAME_THURSDAY, caption.CI_COL_DAY_NAME_FRIDAY, caption.CI_COL_DAY_NAME_SATURDAY],
            
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
                dataservice.getInterviewDate($scope.model, function (rs) {
                    var event = [];
                    angular.forEach(rs, function (value, key) {
                        var obj = {
                            title: value.Fullname,
                            start: value.InterviewDate,
                            className: new Date(value.InterviewDate) >= $rootScope.date ? 'fc-event-event-pink' : 'fc-event-event-default',
                            //color: value.Morning == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                        }
                        event.push(obj);
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
        })
    }
    function loadDate() {
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#dateto').datepicker('setStartDate', maxDate);
        });
        $("#dateto").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefrom').datepicker('setEndDate', maxDate);
        });
        $('.end-date').click(function () {
            $('#datefrom').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#dateto').datepicker('setStartDate', null);
        });
        $(".interview-date").datetimepicker({
            startDate: $rootScope.date,
            autoclose: true
        });
    }
    setTimeout(function () {
        $("#register-interview").hide();
        $("#btnShowInterview").click(function () {
            var isHidden = $("#register-interview").is(":hidden");
            if (isHidden == true) {
                $("#btnShowInterview").html(caption.COM_BTN_CLOSE);
            }
            else {
                $("#btnShowInterview").html(caption.COM_BTN_ADD_CALENDAR);
            }
            $("#register-interview").toggle();
        });
        loadDate();
        createCalendar("calendar-interview");

        //$(".date").not(".interview-date").datepicker({
        //    inline: false,
        //    autoclose: true,
        //    format: "dd/mm/yyyy",
        //    fontAwesome: true,
        //});
        //createCalendar("calendar");
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
    }, 300);
    //vm.dtOptions1 = DTOptionsBuilder.newOptions()
    //    .withOption('ajax', {
    //        url: "/Admin/CalendarInterview/GetGridToday",
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


});