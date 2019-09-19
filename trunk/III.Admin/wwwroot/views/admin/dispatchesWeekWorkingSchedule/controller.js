﻿var ctxfolder = "/views/admin/dispatchesWeekWorkingSchedule";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('httpResponseInterceptor', ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {
    return {
        responseError: function (rejection) {
            if (rejection.status === 401) {
                var url = "/Home/Logout";
                location.href = url;
            }
            return $q.reject(rejection);
        }
    };
}]);
app.factory("interceptors", [function () {
    return {
        // if beforeSend is defined call it
        'request': function (request) {
            if (request.beforeSend)
                request.beforeSend();

            return request;
        },
        // if complete is defined call it
        'response': function (response) {
            if (response.config.complete)
                response.config.complete(response);
            return response;
        }
    };
}]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        getItem: function (data, callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/GetItem/', data, {
                beforeSend: function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                },
                complete: function () {
                    App.unblockUI("#contentMain");
                }
            }).success(callback);
        },

        getAll: function (callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/GetAll').success(callback);
        },
        getEventToday: function (callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/GetEventToday').success(callback);
        },
        checkIsSecretary: function (callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/CheckIsSecretary').success(callback);
        },
        insert: function (data, callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/Insert/', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/Update/', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/DispatchesWeekWorkingSchedule/Delete/', data, {
                beforeSend: function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                },
                complete: function () {
                    App.unblockUI("#contentMain");
                }
            }).success(callback);
        },
    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, dataservice, $filter, $cookies, $location, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    var culture = $cookies.get('_CULTURE') || 'vi-VN';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
    });
    $rootScope.validationOptions = {
        rules: {
            Chair: {
                required: true
            },
            CreatedTime: {
                required: true,
            },
            Room: {
                required: true,
            },

            Composition: {
                required: true,
            },

            Content: {
                required: true
            },


        },
        messages: {
            Chair: {
                required: caption.DWWS_MSG_NO_BLANK_CHAIR
            },
            CreatedTime: {
                required: caption.DWWS_MSG_NO_BLANK_CREATED_TIME
            },
            Room: {
                required: caption.DWWS_MSG_NO_BLANK_ROOM
            },
            Composition: {
                required: caption.DWWS_MSG_NO_BLANK_PARTICIPANTS
            },
            Content: {
                required: caption.DWWS_MSG_NO_BLANK_CONTENT
            },

        }
    }
    dataservice.checkIsSecretary(function (rs) {
        $rootScope.IsSecretary = rs;
    })
    $rootScope.today = $filter('date')(new Date(), 'dd/MM/yyyy');
});
app.service('myService', function () {
    var data;
    this.setData = function (d) {
        data = d;
    }
    this.getData = function () {
        return data;
    }
});
app.config(function ($routeProvider, $validatorProvider, $locationProvider, $httpProvider, $translateProvider) {
     $translateProvider.useUrlLoader('/Admin/Language/Translation');
    caption = $translateProvider.translations();
    $routeProvider
        .when('/',
            {
                templateUrl: ctxfolder + '/index.html',
                controller: 'index'
            }).when('/manager/', {
                templateUrl: ctxfolder + '/manager.html',
                controller: 'manager'
            })
    $validatorProvider.setDefaults({
        errorElement: 'span',
        errorClass: 'help-block',
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
    $httpProvider.interceptors.push('interceptors');
    $httpProvider.interceptors.push('httpResponseInterceptor');
});
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $location, $filter, myService, $timeout) {
    $scope.monday = "";
    $scope.saturday = "";
    $scope.countEventToday = 0;
    $scope.initLoad = function () {
        dataservice.getAll(function (rs) {
            $scope.model = rs.ListWeek;
            $scope.weekNumber = rs.WeekNumber;
            $scope.monday = rs.FromDate;
            $scope.saturday = rs.ToDate;
        })
        dataservice.getEventToday(function (rs) {
            $scope.countEventToday = rs;
        })
    }
    $scope.initLoad();
    $scope.addEvent = function () {
        $location.path('/manager/');
    }
});

app.controller('manager', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $location, $filter, myService) {
    var vm = $scope;
    $scope.model = {
        FromDate: '',
        ToDate: ''
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liFunction = [];
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/DispatchesWeekWorkingSchedule/JTable",
            beforeSend: function (jqXHR, settings) {
                resetCheckbox();
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [0, 'desc'])
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.Id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', ' hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"DWWS_TAB_MANAGER_LIST_COL_CREATED_TIME" | translate}}').withOption('sClass', 'tcenter dataTable-pr20').renderWith(function (data, type, full, meta) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Chair').withTitle('{{"DWWS_TAB_MANAGER_LIST_COL_CHAIR" | translate}}').withOption('sClass', 'tcenter dataTable-pr20').renderWith(function (data, type, full, meta) {
        return data;
    }));

    vm.dtColumns.push(DTColumnBuilder.newColumn('Composition').withTitle('{{"DWWS_TAB_MANAGER_LIST_COL_COMPOSITION" | translate}}').withOption('sClass', 'tcenter dataTable-pr20').renderWith(function (data, type) {
        return data;
    }));

    vm.dtColumns.push(DTColumnBuilder.newColumn('Content').withTitle('{{"DWWS_TAB_MANAGER_LIST_COL_CONTENT" | translate}}').withOption('sClass', 'tcenter dataTable-pr20').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"DWWS_TAB_MANAGER_LIST_COL_ACTION" | translate}}').withOption('sClass', 'nowrap').renderWith(function (data, type, full) {
        return '<button ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fas fa-pencil-alt"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45);" class="btn btn-icon-only btn-circle btn-outline red"><i class="fas fa-trash-alt"></i></button>';
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
    function resetCheckbox() {
        $scope.selected = [];
        vm.selectAll = false;
    }
    function loadDate() {
        $.fn.datepicker.defaults.language = 'vi';
        $("#FromDate").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#ToDate').datepicker('setStartDate', maxDate);
        });
        $("#ToDate").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#FromDate').datepicker('setEndDate', maxDate);
        });
        $('.end-date').click(function () {
            $('#FromDate').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#ToDate').datepicker('setStartDate', null);
        });
    }


    $scope.search = function () {
        reloadData(true);
    }

    $scope.reload = function () {
        reloadData(true);
    }
    $scope.cancel = function () {
        $location.path('/');
    }
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: 'static',
            size: '40'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.edit = function (id) {
        dataservice.getItem(id, function (rs) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: ctxfolder + '/edit.html',
                controller: 'edit',
                backdrop: 'static',
                size: '40',
                resolve: {
                    para: function () {
                        return rs.Object;
                    }
                }
            });
            modalInstance.result.then(function (d) {
                $scope.reload();
            }, function () {
            });
        });
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.DWWS_MSG_DELETE;
                $scope.ok = function () {
                    dataservice.delete(id, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                        } else {
                            App.toastrSuccess(rs.Title);
                            $uibModalInstance.close();
                        }
                    });
                };
                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };
            },
            size: '30',
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }

    setTimeout(function () {
        loadDate();
    }, 200);
});

app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice) {
    $scope.model = {
        Title: '',
        CreatedTime: '',
    }
    $scope.submit = function () {
        if ($scope.addform.validate()) {
            dataservice.insert($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            })
        }
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
        $("#CreatedTime").datetimepicker({
            format: 'DD/MM/YYYY',
            locale: "vi",
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
        $('#TimeStart').datetimepicker({
            format: 'LT'
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
        $('#TimeEnd').datetimepicker({
            format: 'LT'
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
    }, 10);
});

app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.initLoad = function () {
        $scope.model = para;
    }
    $scope.initLoad();
    $scope.submit = function () {
        if ($scope.editform.validate()) {
            dataservice.update($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            })
        }
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
        $("#CreatedTime").datetimepicker({
            format: 'DD/MM/YYYY',
            locale: "vi",
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
        $('#TimeStart').datetimepicker({
            format: 'LT'
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
        $('#TimeEnd').datetimepicker({
            format: 'LT'
        }).on("dp.change", function (e) {
            angular.element($(this)).triggerHandler('input');
        });
    }, 10);
});
