var ctxfolder = "/views/admin/staffTimeKeeping";
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
        checkIn: function (data, callback) {
            $http.post('/Admin/StaffTimeKeeping/CheckIn', data).success(callback);
        },
        checkOut: function (data, callback) {
            $http.post('/Admin/StaffTimeKeeping/CheckOut', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/StaffTimeKeeping/Delete', data).success(callback);
        },
        //getItem: function (data, callback) {
        //    $http.post('/Admin/StaffTimeKeeping/GetItem/', data).success(callback);
        //},
        getListEmployee: function (callback) {
            $http.post('/Admin/StaffTimeKeeping/GetListEmployee').success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $filter, dataservice, $cookies, $translate) {
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
        $rootScope.validationOptions = {
            rules: {
                ActionTime: {
                    required: true,
                },
            },
            messages: {
                ActionTime: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.STK_LBL_TIME),
                },
            }
        }
    });
    $rootScope.dateNow = $filter('date')(new Date(), 'dd/MM/yyyy');
    $rootScope.statusData = [{
        Code: 'NOTWORK',
        Name: 'Xin nghỉ'
    }, {
        Code: 'GOLATE',
        Name: 'Đến muộn'
    }, {
        Code: 'CHECKIN',
        Name: 'Điểm danh'
        }]
    
});

app.config(function ($routeProvider, $validatorProvider, $translateProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    caption = $translateProvider.translations();
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

app.controller('index', function ($scope, $rootScope, $confirm, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.model = {
        Name: '',
    }
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/StaffTimeKeeping/Jtable",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.Name = $scope.model.Name;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [1, 'desc'])
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
            $compile(angular.element(row).contents())($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    var ad = 0;
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('FullName').withOption('sClass', 'dataTable-pr20').withTitle('{{"STK_LIST_COL_FULL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Gender').withOption('sClass', 'tcenter dataTable-pr20').withTitle('{{"STK_LIST_COL_SEX" | translate}}').renderWith(function (data, type) {
        if (data == 'True') {
            return '<i class="fas fa-male fs20"></i>';
        } else {
            return '<i class="fas fa-female fs20" style="color: #f1204fcf;"></i>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Action').withOption('sClass', 'dataTable-pr20').withTitle('{{"STK_LIST_COL_STATUS" | translate}}').renderWith(function (data, type) {
        if (data == "") {
            return '<span class="text-danger">Chưa chấm công</span>';
        } else {
            if (data == "NOTWORK") {
                return '<span class="text-danger">Xin nghỉ</span>';
            } else if (data == "GOLATE") {
                return '<span class="text-danger">Đi muộn</span>';
            } else if (data == "CHECKIN") {
                return '<span class="text-success">Đã điểm danh</span>';
            } else if (data == "CHECKOUT") {
                return '<span class="text-success">Đã về</span>';
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ActionTime').withOption('sClass', 'dataTable-pr20').withTitle('{{"STK_LIST_COL_TIME" | translate}}').renderWith(function (data, type, full) {
        if (data == "") {
            return '<span class="text-danger">Trống</span>';
        } else {
            if (data != '') {
                if (full.Action == "NOTWORK") {
                    var from = full.ActionTime != "" ? $filter('date')(new Date(full.ActionTime), 'dd/MM/yyyy') : null;
                    var to = full.ActionTo != "" ? $filter('date')(new Date(full.ActionTo), 'dd/MM/yyyy') : null;
                    if (to != null) {
                        return ' <span class="text-danger" >' + from + ' - ' + to + '</span>';
                    } else {
                        return ' <span class="text-danger" >' + from + '</span>';
                    }
                }
                var createDate = $filter('date')(new Date(data), 'dd/MM/yyyy');
                if ($rootScope.dateNow == createDate) {
                    var hrs = new Date(data).getHours();
                    var mins = new Date(data).getMinutes();
                    if (hrs <= 8) {
                        if (mins > 30) {
                            return ' <span class="text-danger" >' + hrs + 'h ' + mins + 'p </span>';
                        } else {
                            return ' <span class="text-success" >' + hrs + 'h ' + mins + 'p </span>';
                        }
                    } else {
                        return ' <span class="text-danger" >' + hrs + 'h ' + mins + 'p </span>';
                    }
                } else {
                    return createDate;
                }
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Note').withTitle('{{"STK_LIST_COL_DESCRIPTION" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{"STK_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button ng-click="editTimekeeping(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="deleteTimekeeping(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }));
    vm.reloadData = reloadData;
    vm.dtInstance = {};
    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
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
    $scope.reload = function () {
        reloadData(true);
    }

    $scope.addTimekeeping = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/addTimekeeping.html',
            controller: 'addTimekeeping',
            backdrop: true,
            size: '35'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }

    $scope.editTimekeeping = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/editTimekeeping.html',
            controller: 'editTimekeeping',
            backdrop: true,
            size: '40',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }

    $scope.deleteTimekeeping = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/deleteTimekeeping.html',
            controller: 'deleteTimekeeping',
            size: '35',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () { });
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
    }, 50);
});

app.controller('addTimekeeping', function ($scope, $rootScope, dataservice, $uibModal, $uibModalInstance) {
    $scope.model = {
        UserId: '',
        Note: ''
    }
    $scope.checkIn = false;
    $scope.checkOut = false;

    $scope.initload = function () {
        dataservice.getListEmployee(function (rs) {
            $scope.listEmployeeData = rs;
        });
    }
    $scope.initload();

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "UserId" && $scope.model.UserId != "") {
            $scope.errorUserId = false;
        }
        if (SelectType == "Action" && $scope.model.Action != "") {
            $scope.errorAction = false;
        }
    }
    $scope.uploadImage = function () {
        var fileuploader = angular.element("#File");
        fileuploader.on('click', function () {
        });
        fileuploader.on('change', function (e) {
            var reader = new FileReader();
            reader.onload = function () {
                document.getElementById('imageId').src = reader.result;
            }
            var files = fileuploader[0].files;
            var idxDot = files[0].name.lastIndexOf(".") + 1;
            var extFile = files[0].name.substr(idxDot, files[0].name.length).toLowerCase();
            if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                App.toastrError(caption.STK_MSG_IMG_FORMAT);
                return;
            } else {
                $scope.model.Picture = files[0];
            }
            reader.readAsDataURL(files[0]);
        });
        fileuploader.trigger('click')
    }
    $scope.showCheckIn = function () {
        $("#checkIn").removeClass("hidden");
        $("#checkOut").addClass("hidden");
        $scope.checkIn = true;
        $scope.checkOut = false;
    }
    $scope.hideCheckIn = function () {
        $("#checkIn").addClass("hidden");
        $scope.checkIn = false;
    }
    $scope.showCheckOut = function () {
        $("#checkOut").removeClass("hidden");
        $("#checkIn").addClass("hidden");
        $scope.checkOut = true;
        $scope.checkIn = false;
    }
    $scope.hideCheckOut = function () {
        $("#checkOut").addClass("hidden");
        $scope.checkOut = false;
    }

    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && !validationSelect($scope.model).Status) {
            if ($scope.checkIn == false && $scope.checkOut == false) {
                App.toastrError(caption.STK_MSG_CHECKIN_CHECKOUT);
            } else {
                if ($scope.checkIn == true) {
                    dataservice.checkIn($scope.model, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                        } else {
                            App.toastrSuccess(rs.Title);
                            $uibModalInstance.close();
                        }
                    })
                } else {
                    dataservice.checkOut($scope.model, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                        } else {
                            App.toastrSuccess(rs.Title);
                            $uibModalInstance.close();
                        }
                    })
                }
            }
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    function loadDate() {
        $("#ActionTime").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#ActionTo').datepicker('setStartDate', maxDate);
        });
        $("#ActionTo").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#ActionTime').datepicker('setEndDate', maxDate);
        });
        //$('#DateTo').datepicker('update', $rootScope.DateNow);
        $('.end-date').click(function () {
            $('#ActionTime').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#ActionTo').datepicker('setStartDate', null);
        });
    }

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
        loadDate();
    }, 200);

    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        //Check null user
        if (data.UserId == "") {
            $scope.errorUserId = true;
            mess.Status = true;
        } else {
            $scope.errorUserId = false;
        }
        return mess;
    };
});

//app.controller('editTimekeeping', function ($scope, $rootScope, dataservice, $uibModal, $uibModalInstance, para) {
//    $scope.initload = function () {
//        dataservice.getListEmployee(function (rs) {
//            $scope.listEmployeeData = rs;
//        });
//        dataservice.getItem(para, function (rs) {
//            $scope.model = rs.Object;
//        });
//    }
//    $scope.initload();

//    $scope.changleSelect = function (SelectType) {
//        if (SelectType == "UserId" && $scope.model.UserId != "") {
//            $scope.errorUserId = false;
//        }
//        if (SelectType == "Action" && $scope.model.Action != "") {
//            $scope.errorAction = false;
//        }
//    }

//    $scope.submit = function () {
//        validationSelect($scope.model);
//        if ($scope.addform.validate() && !validationSelect($scope.model).Status) {
//            dataservice.updateStaffTime($scope.model, function (rs) {
//                if (rs.Error) {
//                    App.toastrError(rs.Title);
//                } else {
//                    App.toastrSuccess(rs.Title);
//                    $uibModalInstance.close();
//                }
//            })
//        }
//    }

//    $scope.cancel = function () {
//        $uibModalInstance.dismiss('cancel');
//    }

//    function loadDate() {
//        $("#ActionTime").datepicker({
//            inline: false,
//            autoclose: true,
//            format: "dd/mm/yyyy",
//            fontAwesome: true,
//        }).on('changeDate', function (selected) {
//            var maxDate = new Date(selected.date.valueOf());
//            $('#ActionTo').datepicker('setStartDate', maxDate);
//        });
//        $("#ActionTo").datepicker({
//            inline: false,
//            autoclose: true,
//            format: "dd/mm/yyyy",
//            fontAwesome: true,
//        }).on('changeDate', function (selected) {
//            var maxDate = new Date(selected.date.valueOf());
//            $('#ActionTime').datepicker('setEndDate', maxDate);
//        });
//        //$('#DateTo').datepicker('update', $rootScope.DateNow);
//        $('.end-date').click(function () {
//            $('#ActionTime').datepicker('setEndDate', null);
//        });
//        $('.start-date').click(function () {
//            $('#ActionTo').datepicker('setStartDate', null);
//        });
//    }
//    function validationSelect(data) {
//        var mess = { Status: false, Title: "" }
//        //Check null user
//        if (data.UserId == "") {
//            $scope.errorUserId = true;
//            mess.Status = true;
//        } else {
//            $scope.errorUserId = false;
//        }
//        //Check null status
//        if (data.Action == "") {
//            $scope.errorAction = true;
//            mess.Status = true;
//        } else {
//            $scope.errorAction = false;
//        }
//        return mess;
//    };

//    setTimeout(function () {
//        setModalDraggable('.modal-dialog');
//        loadDate();
//    }, 200);
//});

app.controller('deleteTimekeeping', function ($scope, $rootScope, dataservice, para, $uibModalInstance) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submit = function () {
        dataservice.delete(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $uibModalInstance.close();
            }
        });
    }
});