﻿var ctxfolder = "/views/Admin/Location";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/Location/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/Location/update', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/Admin/Location/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/Location/getitem/' + data).success(callback);
        },
        gettreedataParent_id: function (callback) {
            $http.post('/Admin/Location/gettreedataParent_id/').success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };

    $rootScope.validationOptions = {
        rules: {
            code: {
                required: true,
                maxlength: 50
            },
            name: {
                required: true,
                maxlength: 500
            },
        },
        messages: {
            code: {
                required: "Nhập mã địa điểm",
                maxlength: "Mã địa điểm k vượt quá 50 kí tự"
            },
            name: {
                required: "Nhập tên địa điểm",
                maxlength: "Tên khu vực k vượt quá 500 kí tự"
            },
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
        dataservice.gettreedataParent_id(function (result) {
            $scope.treedataParent_id = result.Object;
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
            url: "/Admin/Location/jtable",
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', 'tcenter'));


    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('Địa điểm').renderWith(function (data, type) {
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
    //$scope.add = function () {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/add.html',
    //        controller: 'add',
    //        backdrop: true,
    //        size: 'lg'
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.reload();
    //    }, function () {
    //    });
    //}

    //$scope.edit = function () {
    //    var editItems = [];
    //    for (var id in $scope.selected) {
    //        if ($scope.selected.hasOwnProperty(id)) {
    //            if ($scope.selected[id]) {
    //                editItems.push(id);
    //            }
    //        }
    //    }
    //    if (editItems.length > 0) {
    //        if (editItems.length == 1) {
    //            var modalInstance = $uibModal.open({
    //                animation: true,
    //                templateUrl: ctxfolder + '/edit.html',
    //                controller: 'edit',
    //                backdrop: 'static',
    //                size: 'lg',
    //                resolve: {
    //                    para: function () {
    //                        return editItems[0];
    //                    }
    //                }
    //            });
    //            modalInstance.result.then(function (d) {
    //                $scope.reload();
    //            }, function () {
    //            });
    //        } else {
    //            App.toastrError(caption.ONLY_SELECT.replace('{0}', caption.FUNCTION));
    //        }
    //    } else {
    //        App.toastrError(caption.ERR_NOT_CHECKED.replace('{0}', caption.FUNCTION));
    //    }
    //}

    //$scope.deleteChecked = function () {
    //    var deleteItems = [];
    //    for (var id in $scope.selected) {
    //        if ($scope.selected.hasOwnProperty(id)) {
    //            if ($scope.selected[id]) {
    //                deleteItems.push(id);
    //            }
    //        }
    //    }
    //    if (deleteItems.length > 0) {
    //        $confirm({ text: 'Bạn có chắc chắn muốn khoá các khoản mục đã chọn này ?', title: 'Xác nhận', ok: 'Chắc chắn', cancel: ' Hủy ' })
    //            .then(function () {
    //                App.blockUI({
    //                    target: "#contentMain",
    //                    boxed: true,
    //                    message: 'loading...'
    //                });

    //                dataservice.deleteItems(deleteItems, function (result) {
    //                    if (result.Error) {
    //                        App.notifyDanger(result.Title);
    //                    } else {
    //                        App.notifyInfo(result.Title);
    //                        $scope.reload();
    //                    }
    //                    App.unblockUI("#contentMain");
    //                });

    //            });
    //    } else {
    //        App.notifyDanger("Không có khoản mục nào được chọn!");
    //    }
    //}
    $scope.enableedit = false;
    $scope.enableadd = true;
    $scope.show = true;
    $scope.hide = false;
    $scope.contextMenu = [
        [function ($itemScope) {
            return '<i class="fa fa-edit"></i>Cập nhật tài khoản ';
        }, function ($itemScope, $event, model) {
            $scope.enableedit = true;
            $scope.enableadd = false;
            $scope.show = false;
            $scope.hide = true;
            dataservice.getItem($itemScope.data.id, function (rs) {
                console.log("RS: " + $itemScope.data.id);
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
            return '<i class="fa fa-remove"></i> Xóa địa chỉ này';
        }, function ($itemScope, $event, model) {

            $confirm({ text: 'Bạn có chắc chắn xóa: ' + $itemScope.data.name, title: 'Xác nhận', cancel: ' Hủy ' })
                .then(function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                    dataservice.delete($itemScope.data.id, function (result) {
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
                    $scope.model.code = null;
                    $scope.model.name = null;
                    $scope.model.parent_id = null;
                    $scope.model.description = null;
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
                    $scope.model.code = null;
                    $scope.model.name = null;
                    $scope.model.parent_id = null;
                    $scope.model.description = null;
                }
                $scope.enableedit = false;
                $scope.enableadd = true;
                $scope.show = true;
                $scope.hide = false;
            });
        }
    }






    $scope.delete = function () {
        $confirm({ text: 'Bạn có chắc chắn xóa: ' + $scope.model.name, title: 'Xác nhận', cancel: ' Hủy ' })
            .then(function () {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
                dataservice.delete($scope.model.id, function (rs) {
                    if (rs.Error) {
                        App.notifyDanger(rs.Title);
                    } else {
                        App.notifyInfo(rs.Title);
                        $scope.reload();
                        //$scope.model.id = '';
                        $scope.model.code = null;
                        $scope.model.name = null;
                        $scope.model.parent_id = null;
                        $scope.model.description = null;


                    }
                    $scope.enableedit = false;
                    $scope.enableadd = true;
                    $scope.show = true;
                    $scope.hide = false;
                });
            });

    }




});

