var ctxfolder = "/views/admin/task008";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/task008/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/task008/update', data).success(callback);
        },
     
        delete: function (data, callback) {
            $http.post('/Admin/task008/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/task008/getitem/' + data).success(callback);
        },
       
        gettreedataParent_id: function (callback) {
            $http.post('/Admin/task008/gettreedataParent_id/').success(callback);
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
                maxlength: 50
            },
      
        },
        messages: {
            code: {
                required: "Nhập mã kỹ năng",
                maxlength: "Mã kỹ năng không vượt quá 50 kí tự"
            },
            name: {
                required: "Nhập tên kỹ năng",
                maxlength: "Tên kỹ năng không vượt quá 50 kí tự"
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
    $scope.model
    {
        city=''
    };

    $scope.initData = function () {
       
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
            url: "/Admin/task008/jtable",
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
           $(this.api().table().header()).css({'font-size': '8px', 'font-weight': 'bold' });
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("id_skillmaster").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_skillmaster] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id_skillmaster + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        })/*.withOption('sWidth', '30px')*/.withOption('sClass', 'tcenter'));

    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('Mã kỹ năng').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('Tên kỹ năng').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('user').withTitle('Người tạo').renderWith(function (data, type) {
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
            return '<i class="fa fa-edit"></i>Cập nhật';
        }, function ($itemScope, $event, model) {
            $scope.enableedit = true;
            $scope.enableadd = false;
            $scope.show = false;
            $scope.hide = true;
            debugger
            dataservice.getItem($itemScope.data.id_skillmaster, function (rs) {
                console.log("RS: " + $itemScope.data.id_skillmaster);
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
            return '<i class="fa fa-remove"></i> Xóa ';
        }, function ($itemScope, $event, model) {

            $confirm({ text: 'Bạn có chắc chắn xóa: ' + $itemScope.data.code, title: 'Xác nhận', cancel: ' Hủy ' })
                .then(function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                    dataservice.delete($itemScope.data.id_skillmaster, function (result) {
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
                 //   $scope.model.id_skillmaster = null;
                    $scope.model.name = null;
                    $scope.model.code = null;
                    $scope.model.user_id = null;
                   // $scope.model.order = null;
                  
                   
      
                }
            });
        }

    }

    $scope.edit = function () {
        if ($scope.addform.validate()) {

            console.log('Nhung' + $scope.model)
            dataservice.update($scope.model, function (rs) {
                if (rs.Error) {
                    App.notifyDanger(rs.Title);
                } else {
                    App.notifyInfo(rs.Title);
                    $scope.reload();

                  //  $scope.model.id_skillmaster = null;
                    $scope.model.name = null;
                    $scope.model.code = null;
                    $scope.model.user_id = null;
                    //$scope.model.order = null;

                }
                $scope.enableedit = false;
                $scope.enableadd = true;
                $scope.show = true;
                $scope.hide = false;
            });
        }
    }




    $scope.delete = function () {
        $confirm({ text: 'Bạn có chắc chắn xóa: ' + $scope.model.code, title: 'Xác nhận', cancel: ' Hủy ' })
            .then(function () {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
                dataservice.delete($scope.model.id_skillmaster, function (rs) {
                    if (rs.Error) {
                        App.notifyDanger(rs.Title);
                    } else {
                        App.notifyInfo(rs.Title);
                        $scope.reload();
                   //    $scope.model.id_skillmaster = null;
                        $scope.model.name = null;
                        $scope.model.code = null;
                        $scope.model.user_id = null;
              


                    }
                    $scope.enableedit = false;
                    $scope.enableadd = true;
                    $scope.show = true;
                    $scope.hide = false;
                });
            });


        
    }




});


