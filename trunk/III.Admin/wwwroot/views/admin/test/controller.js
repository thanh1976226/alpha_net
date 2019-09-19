var ctxfolder = "/views/admin/test";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/test/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/test/update', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/test/delete?id_con=' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/test/getitem?id_con=' + data).success(callback);
        },
        
        gettreedataCoursetype: function(callback) {
            $http.post('/test/gettreedataCoursetype/').success(callback);
        },
        gettreedataLevel: function (callback) {
            $http.post('/task002/gettreedataLevel/').success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };

    $rootScope.validationOptions = {
        rules: {
            course_code: {
                required: true,
                maxlength: 50
            },
            course_name: {
                required: true,
                maxlength: 500
            },
            //price: {
            //    required: true,
            //    maxlength: 500
            //},
        },
        messages: {
            course_code: {
                required: "Nhập mã trình độ",
                maxlength: "Mã địa điểm k vượt quá 50 kí tự"
            },
            course_name: {
                required: "Nhập tên trình độ",
                maxlength: "Tên khu vực k vượt quá 500 kí tự"
            },
            //price: {
            //    required: "Nhập tên trình độ",
            //    maxlength: "Tên khu vực k vượt quá 500 kí tự"
            //},
        }
    }

    $rootScope.StatusData = [{
        Value: 1,
        Name: 'Kích Hoạt'
    }, {
        Value: 2,
        Name: 'Không kích hoạt'
    }];
});

app.config(function ($routeProvider, $validatorProvider) {
    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
        })
        //.when('/edit/:id', {
        //    templateUrl: ctxfolder + '/edit.html',
        //    controller: 'edit'
        //})
        //.when('/add/', {
        //    templateUrl: ctxfolder + '/add.html',
        //    controller: 'add'
        //})
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
});

app.controller('index', function ($scope, $rootScope, $compile, $confirm, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.initData = function () {
        debugger
        dataservice.gettreedataCoursetype(function (result) {
            $scope.treedataCoursetype = result.Object;
        });
        dataservice.gettreedataLevel(function (result) {
            $scope.treedataLevel = result.Object;
        });
        
    }
    $scope.initData();
    $scope.DateNow = $filter("date")(new Date(), "dd/MM/yyyy");
    var vm = $scope;

    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    $scope.model = {
        Key: ''
    }

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';


    //begin option table
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/test/jtable",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.Key = $scope.model.Key;
                d.Key4 = $scope.model.Key4;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(15)
        //.withOption('scrollY', '100vh')
        //.withOption('scrollCollapse', true)
        .withOption('order', [1, 'asc'])
        .withOption('serverSide', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
            //$(this.api().table().header()).css({ 'background-color': 'rgb(60, 101, 82)', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row).find('input'))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    //end option table 

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("id_con").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_con] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id_con + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        })/*.withOption('sWidth', '30px')*/.withOption('sClass', 'tcenter'));


    vm.dtColumns.push(DTColumnBuilder.newColumn('firstname').withTitle('Tên sinh viên').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('Mã sinh viên').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('birthday').withTitle('Ngày sinh').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('sex').withTitle('Giới tính').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('Tên người thân').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('relationship').withTitle('Quan hệ').renderWith(function (data, type) {
        return data;
    }));
  
    vm.reloadData = reloadData;
    vm.dtInstance = {};
    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
    }
    $scope.reload = function () {
        reloadData(true);
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

    $scope.enableedit = false;
    $scope.enableadd = true;
    $scope.contextMenu = [
        [function ($itemScope) {
            return '<i class="fa fa-edit"></i>Cập nhật trình độ ';
        }, function ($itemScope, $event, model) {
            $scope.enableedit = true;
            $scope.enableadd = false;
            dataservice.getItem($itemScope.data.id_con, function (rs) {
                console.log("RS1: " + $itemScope.data.id_con);
                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    $scope.model = rs[0];
                    console.log('Data details: ' + JSON.stringify(rs))
                }
            });

        }, function ($itemScope, $event, model) {
            return true;
        }],
        [function ($itemScope) {
            return '<i class="fa fa-remove"></i> Xóa trình độ này';
        }, function ($itemScope, $event, model) {

            $confirm({ text: 'Bạn có chắc chắn xóa: ' + $itemScope.data.name, title: 'Xác nhận', cancel: ' Hủy ' })
                .then(function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                    dataservice.delete($itemScope.data.id_con, function (result) {
                        if (result.Error) {
                            App.notifyDanger(result.Title);
                            //alert(result.Title)
                        } else {
                            App.notifyInfo(result.Title);
                            //alert(result.Title)
                            $scope.reload();
                        }
                        App.unblockUI("#contentMain");
                    });
                });
        }, function ($itemScope, $event, model) {
            return true;
        }]
    ];


    $scope.submit = function () {

        console.log(JSON.stringify($scope.model))

        if ($scope.addform.validate()) {
            dataservice.insert($scope.model, function (rs) {

                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    App.notifyInfo(rs.Title);
                    $scope.reload();
                    //$scope.model.id = '';
                    $scope.model.course_name = null;
                    $scope.model.note = null;
                    $scope.model.coursetype = null;
                    $scope.model.price = null;
                    $scope.model.userid = null;
                    $scope.model.course_code = null;
                    $scope.model.totalday = null;
                    $scope.model.level = null;
                }
            });
        }

    }

    $scope.edit = function () {
        if ($scope.addform.validate()) {

            console.log('Nga' + $scope.model)
            dataservice.update($scope.model, function (rs) {
                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    App.notifyInfo(rs.Title);
                    $scope.reload();
                    //$scope.model.id = '';
                    $scope.model.course_name = null;
                    $scope.model.note = null;
                    $scope.model.coursetype = null;
                    $scope.model.price = null;
                    $scope.model.userid = null;
                    $scope.model.course_code = null;
                    $scope.model.totalday = null;
                    $scope.model.level = null;
                }
                $scope.enableedit = false;
                $scope.enableadd = true;
            });
        }
    }

});

//app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $filter) {
//    $scope.names1 = [
//        { value: "Hợp tác", active_code: 1 },
//        { value: "Không hợp tác", active_code: 0 },
//    ];
//    $scope.cancel = function () {
//        $uibModalInstance.dismiss('cancel');
//    }
//    $scope.submit = function () {

//        console.log(JSON.stringify($scope.model))

//        if ($scope.addform.validate()) {
//            dataservice.insert($scope.model, function (rs) {
//                if (rs.Error) {
//                    App.notifyDanger(rs.Title);
//                } else {
//                    App.notifyInfo(rs.Title);
//                    $uibModalInstance.close();
//                }
//            });
//        }

//    }

//});
//app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para) {
//    $scope.names1 = [
//        { value: "Hợp tác", active_code: 1 },
//        { value: "Không hợp tác", active_code: 0 },
//    ];
//    $scope.loadData = function () {

//    }
//    $scope.loadData();
//    $scope.cancel = function () {
//        $uibModalInstance.dismiss('cancel');
//    }
//    $scope.initData = function () {
//        dataservice.getItem(para, function (rs) {
//            console.log("RS: " + para);
//            if (rs.Error) {
//                App.notifyDanger(rs.Title);
//            } else {
//                $scope.model = rs[0];
//                console.log('Data details: ' + JSON.stringify(rs))
//            }
//        });
//    }
//    $scope.initData();
//    $scope.submit = function () {
//        // if ($scope.editform.validate()) {
//        dataservice.update($scope.model, function (rs) {
//            if (rs.Error) {
//                App.notifyDanger(rs.Title);
//            } else {
//                App.notifyInfo(rs.Title);
//                $uibModalInstance.close();
//            }
//        });
//        // }
//    }
//});


