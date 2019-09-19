var ctxfolder = "/views/admin/task003";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select','angularjs-dropdown-multiselect']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/task003/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/task003/update', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/Admin/task003/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/task003/getitem/' + data).success(callback);
        },
        
        gettreedataCourse: function(callback) {
            $http.post('/Admin/task003/gettreedataCourse/').success(callback);
        },
        gettreedataLocation: function (callback) {
            $http.post('/Admin/task003/gettreedataLocation/').success(callback);
        },
        
        gettreedataStatus: function(callback) {
            $http.post('/Admin/task003/gettreedataStatus/').success(callback);
        },
        gettreedataSkills: function (callback) {
            $http.post('/Admin/task003/gettreedataSkills/').success(callback);
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
    //$scope.name = 'World';
    //$scope.cars = [{ id: 1, name: 'Audi' }, { id: 2, name: 'BMW' }, { id: 1, name: 'Honda' }];
    //$scope.selectedCar = [];

    $scope.cancel = function () {
        
        $uibModalInstance.close();
    }

        $scope.example13model = [];
        $scope.example13data = [{
            id: 2,
            label: "Thứ 2"
        }, {
            id: 3,
            label: "Thứ 3"
        }, {
            id: 4,
            label: "Thứ 4"
        }, {
            id: 5,
            label: "Thứ 5"
        }, {
            id: 6,
            label: "Thứ 6"
        }, {
            id: 7,
            label: "Thứ 7"
        }, {
                id: 8,
                label: "Chủ nhật"
        }];

        $scope.example13settings = {
            smartButtonMaxItems: 3,
            smartButtonTextConverter: function (itemText, originalItem) {
                if (itemText === 'Jhon') {
                    return 'Jhonny!';
                }vcxz

                return itemText;
            }
        };

        $scope.initData = function () {

            dataservice.gettreedataSkills(function (result) {
                $scope.treedataSkills = result.Object;
            });



        }
        $scope.initData();
    

    $scope.initData = function () {
        debugger
        dataservice.gettreedataCourse(function (result) {
            $scope.treedataCourse = result.Object;
        });
        dataservice.gettreedataLocation(function (result) {
            $scope.treedataLocation = result.Object;
        });
        
        dataservice.gettreedataStatus(function (result) {
            $scope.treedataStatus = result.Object;
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
            url: "/Admin/task003/jtable",
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
        .withDataProp('data').withDisplayLength(3)
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
        })/*.withOption('sWidth', '30px')*/.withOption('sClass', 'tcenter'));


    vm.dtColumns.push(DTColumnBuilder.newColumn('course_code').withTitle('Mã trình độ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('course_name').withTitle('Tên trình độ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('coursetype').withTitle('Kiểu trình độ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('price').withTitle('Học phí').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('Ghi chú').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('userid').withTitle('Người tạo').renderWith(function (data, type) {
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
            return '<i class="fa fa-remove"></i> Xóa trình độ này';
        }, function ($itemScope, $event, model) {

            $confirm({ text: 'Bạn có chắc chắn xóa: ' + $itemScope.data.course_name, title: 'Xác nhận', cancel: ' Hủy ' })
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
                    //$scope.model.id = '';
                    $scope.model.code = null;
                    $scope.model.name = null;
                    $scope.model.idcrouse = null;
                    $scope.model.numberstudent = null;
                    $scope.model.userid = null;
                    $scope.model.schoolstart = null;
                    $scope.model.schoolend = null;
                    $scope.model.schoolday = null;


                    $scope.model.status = null;
                    $scope.model.classroom_id = null;
                    $scope.model.teacher_id = null;
                    $scope.model.fee = null;
                    $scope.model.datestart = null;
                    $scope.model.dateend = null;
                    $scope.model.sale_off_price = null;
                    $scope.model.start_sale_off = null;

                    $scope.model.end_sale_off = null;
                    $scope.model.sale_off_note = null;
                    $scope.model.location_id = null;
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
                    $scope.model.code = null;
                    $scope.model.name = null;
                    $scope.model.idcrouse = null;
                    $scope.model.numberstudent = null;
                    $scope.model.userid = null;
                    $scope.model.schoolstart = null;
                    $scope.model.schoolend = null;
                    $scope.model.schoolday = null;


                    $scope.model.status = null;
                    $scope.model.classroom_id = null;
                    $scope.model.teacher_id = null;
                    $scope.model.fee = null;
                    $scope.model.datestart = null;
                    $scope.model.dateend = null;
                    $scope.model.sale_off_price = null;
                    $scope.model.start_sale_off = null;

                    $scope.model.end_sale_off = null;
                    $scope.model.sale_off_note = null;
                    $scope.model.location_id = null;
                }
                $scope.enableedit = false;
                $scope.enableadd = true;
            });
        }
    }


    $scope.addGV = function () {
        debugger
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/addGV.html',
            controller: 'addGV',
            backdrop: true,
            size: '80'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }



});

//function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para, DTOptionsBuilder, DTColumnBuilder, DTInstances)

app.controller('addGV', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $filter, DTOptionsBuilder, DTColumnBuilder, DTInstances) {

    $scope.initData = function () {

        dataservice.gettreedataSkills(function (result) {
            $scope.treedataSkills = result.Object;
        });



    }
    $scope.initData();

    $scope.cancel = function () {
        $uibModalInstance.close();
    }
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
            url: "/Admin/task003/jtableGV",
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
                d.Key5 = $scope.model.Key5;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(8)
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
            // $(this.api().table().header()).css({ 'background-color': 'rgb(60, 101, 82)', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
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
        }).withOption('sWidth', '30px')/*.withOption('sClass', 'tcenter')*/);


    vm.dtColumns.push(DTColumnBuilder.newColumn('teacher_id').withTitle('Giáo viên').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('starttime').withTitle('Từ ngày').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('endtime').withTitle('Tới ngày').renderWith(function (data, type) {
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
    $scope.show = true;
    $scope.hide = false;
    $scope.contextMenu = [
        [function ($itemScope) {
            return '<i class="fa fa-edit"></i>Cập nhật cấp học ';
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
            return '<i class="fa fa-remove"></i> Xóa cấp học';
        }, function ($itemScope, $event, model) {

            $confirm({ text: 'Bạn có chắc chắn xóa: ' + $itemScope.data.value, title: 'Xác nhận', cancel: ' Hủy ' })
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
                    //$scope.model.id = '';
                    $scope.model.value = '';
                    $scope.model.code = '';
                    $scope.model.parent_id = null;
                    $scope.model.order = null;



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
                    $scope.model.value = '';
                    $scope.model.code = '';
                    $scope.model.parent_id = null;
                    $scope.model.order = null;

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
                        $scope.model.value = '';
                        $scope.model.code = '';
                        $scope.model.parent_id = null;
                        $scope.model.order = null;


                    }
                    $scope.enableedit = false;
                    $scope.enableadd = true;
                    $scope.show = true;
                    $scope.hide = false;
                });
            });


    }




});


