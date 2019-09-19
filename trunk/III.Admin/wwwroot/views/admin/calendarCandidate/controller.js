var ctxfolder = "/views/admin/calendarCandidate";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

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
        getListCandidate: function (callback) {
            $http.post('/Admin/CalendarCandidate/GetListCandidate').success(callback);
        },
        getEventCat: function (data, callback) {
            $http.post('/Admin/CalendarCandidate/GetEventCat', data).success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $filter) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    $rootScope.dateNow = $filter('date')(new Date(), 'dd/MM/yyyy');
});

app.config(function ($routeProvider, $validatorProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
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


app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.model = {
        MemberId: '',
        FromDate: '',
        ToDate: '',
    };
    $scope.initLoad = function () {
        dataservice.getListCandidate(function (rs) {
            $scope.listCandidateData = rs;
        });
        $scope.model.FromDate = $rootScope.dateNow;
        $scope.model.ToDate = $rootScope.dateNow;
    }
    $scope.initLoad();

    //tab calender
    $scope.reloadCalender = function () {
        $('#calendar').fullCalendar('refetchEvents');
    }
    function loadCalendar(id) {
        $('#' + id).fullCalendar({
            //defaultView: 'month',
            //selectable: true,
            //editable: true,
            eventLimit: true,
            header: {
                center: 'title',
                left: 'prev',
                right: 'next'
            },
            dayNames: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            monthNames: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 -', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            monthNamesShort: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 ', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            dayNamesShort: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            events: function (start, end, timezone, callback) {
                dataservice.getEventCat($scope.model, function (rs) {
                    var event = [];
                    angular.forEach(rs, function (value, key) {
                        var obj = {
                            title: value.Fullname,
                            start: value.DatetimeEvent,
                            className: 'fc-event-event-pink',
                            allDay: true,
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
            //dayClick: function (date) {
            //},
            //customButtons: {
            //}
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
        showHideSearch();
        loadCalendar("calendar");
        loadDate();
    }, 100);
});
