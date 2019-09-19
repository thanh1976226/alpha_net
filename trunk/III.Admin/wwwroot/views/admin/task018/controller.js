var ctxfolder = "/views/admin/task018";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']).
    directive("filesInput", function () {
        return {
            require: "ngModel",
            link: function postLink(scope, elem, attrs, ngModel) {
                elem.on("change", function (e) {
                    var files = elem[0].files;
                    ngModel.$setViewValue(files);
                });
            }
        }
    });
app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }


   

 


    return {

        insert: function (data, callback) {
            $http.post('/Admin/task018/insert', data).success(callback);
        },
        insertC: function (data, callback) {
            $http.post('/Admin/task018/insertC', data).success(callback);
        },
        gettreedataCurrency: function (callback) {
            $http.post('/Admin/task018/gettreedataCurrency/').success(callback);
        },

    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    $rootScope.validationOptions1 = {
        rules: {
            ticket_code: {
                required: true,
                maxlength: 50
            },
            ticket_title: {
                required: true,
                maxlength: 500
            },
            person_pay: {
                required: true,
                maxlength: 100
            },
        },
        messages: {
            ticket_code: {
                required: "Yêu cầu nhập mã phiếu.",
                maxlength: "Mã phiếu không vượt quá 50 ký tự."
            },
            ticket_title: {
                required: "Yêu cầu nhập nội dung chi trả.",
                maxlength: "Nội dung không vượt quá 500 ký tự."
            },
            person_pay: {
                required: "Yêu cầu nhập người trả tiền",
                maxlength: "Nội dung không vượt quá 500 ký tự."
            },
        


        }
    }

    $rootScope.validationOptions2 = {
        rules: {
            ticket_code: {
                required: true,
                maxlength: 50
            },
            ticket_title: {
                required: true,
                maxlength: 500
            },
            person_pay: {
                required: true,
                maxlength: 100
            },
        },
        messages: {
            ticket_code: {
                required: "Yêu cầu nhập mã phiếu.",
                maxlength: "Mã phiếu không vượt quá 50 ký tự."
            },
            ticket_title: {
                required: "Yêu cầu nhập nội dung chi trả.",
                maxlength: "Nội dung không vượt quá 500 ký tự."
            },

            person_pay: {
                required: "Yêu cầu nhập người thu tiền",
                maxlength: "Nội dung không vượt quá 500 ký tự."
            },


        }
    }

    $rootScope.StatusData = [{
        Value: 1,
        Name: 'Hoạt động'
    }, {
        Value: 0,
        Name: 'Ngừng hoạt động'
    }];

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
    //phiếu thu

    //begin option table
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/task018/jtablePT",
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
                d.Key2 = $scope.model.Key2;
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
            //$(this.api().table().header()).css({ 'background-color': 'rgb(60, 101, 82)', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;

            $compile(angular.element(row).find('input'))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    //end option table 

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("ticket_id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.ticket_id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ticket_id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        })/*.withOption('sWidth', '30px')*/.withOption('sClass', 'tcenter'));


    vm.dtColumns.push(DTColumnBuilder.newColumn('ticket_code').withTitle('Mã phiếu').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('Loại khoản thu').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('createtime').withTitle('Ngày tạo').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ticket_title').withTitle('Nội dung').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('Khách hàng').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('person_pay').withTitle('Người trả tiền').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('payment_mode').withTitle('Hình thức thanh toán').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('Địa điểm').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('Ghi chú').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('pay').withTitle('Tổng tiền').renderWith(function (data, type) {
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: true,
            size: 'lg'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.addChi = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/addChi.html',
            controller: 'addChi',
            backdrop: true,
            size: 'lg'
        });
        modalInstance.result.then(function (d) {
            $scope.reload1();
        }, function () {
        });
    }



    //loại khoản chi
    //begin option table
    vm.dtOptions1 = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/task018/jtablePC",
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
                d.Key7 = $scope.model.Key7;
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
            //$(this.api().table().header()).css({ 'background-color': 'rgb(60, 101, 82)', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;

            $compile(angular.element(row).find('input'))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    //end option table 

    vm.dtColumns1 = [];
    vm.dtColumns1.push(DTColumnBuilder.newColumn("ticket_id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.ticket_id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ticket_id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        })/*.withOption('sWidth', '30px')*/.withOption('sClass', 'tcenter'));


    vm.dtColumns1.push(DTColumnBuilder.newColumn('ticket_code').withTitle('Mã phiếu').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('').withTitle('Loại khoản chi').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('createtime').withTitle('Ngày tạo').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('ticket_title').withTitle('Nội dung').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('').withTitle('Khách hàng').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('person_pay').withTitle('Tên người nhận').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('coursetype').withTitle('Hình thức thanh toán').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('').withTitle('Địa điểm').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('note').withTitle('Ghi chú').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns1.push(DTColumnBuilder.newColumn('pay').withTitle('Tổng tiền').renderWith(function (data, type) {
        return data;
    }));

    vm.reloadData = reloadData1;
    vm.dtInstance1 = {};
    function reloadData1(resetPaging) {
        vm.dtInstance1.reloadData(callback, resetPaging);
    }
    $scope.reload1 = function () {
        reloadData1(true);
    }





});



app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice) {
    //$scope.loaiphieu = [
    //    { value: "Phiếu thu", ticket_type: 1 },
    //    { value: "Phiếu chi", ticket_type: 2 },
    //];
    //$scope.gioitinh = [
    //    { value: "Nam", gender: 1 },
    //    { value: "Nữ", gender: 2 }
    //];
    $scope.initData = function () {
        dataservice.gettreedataCurrency(function (result) {
            $scope.treedataCurrency = result.Object;
        });
    }
    $scope.initData();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {

        console.log(JSON.stringify($scope.model))

        if ($scope.addform.validate()) {
            dataservice.insert($scope.model, function (rs) {

                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    App.notifyInfo(rs.Title);               
                    $uibModalInstance.close();
                    $scope.reload();
                }
            });
        }

    }

   
});


app.controller('addChi', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice) {
    //$scope.loaiphieu = [
    //    { value: "Phiếu thu", ticket_type: 1 },
    //    { value: "Phiếu chi", ticket_type: 2 },
    //];
    //$scope.gioitinh = [
    //    { value: "Nam", gender: 1 },
    //    { value: "Nữ", gender: 2 }
    //];
    $scope.initData = function () {
        dataservice.gettreedataCurrency(function (result) {
            $scope.treedataCurrency = result.Object;
        });
    }
    $scope.initData();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {

        console.log(JSON.stringify($scope.model))

        if ($scope.addform.validate()) {
            dataservice.insertC($scope.model, function (rs) {

                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    App.notifyInfo(rs.Title);
                    $uibModalInstance.close();
                    $scope.reload1();
                }
            });
        }

    }


});

