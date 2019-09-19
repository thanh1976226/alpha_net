var ctxfolder = "/views/Admin/edu_student";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/edu_student/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/edu_student/update', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/Admin/edu_student/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/edu_student/getitem/' + data).success(callback);
        },
        getData: function (data, callback) {
            $http.get('/Admin/edu_student/GetData/' + data).success(callback);
        },
        gettreedataCoursetype: function(callback) {
            $http.post('/Admin/edu_student/gettreedataCoursetype/').success(callback);
        },
        gettreedataLevel: function (callback) {
            $http.post('/Admin/edu_student/gettreedataLevel/').success(callback);
        },


        update_MXH: function (data, callback) {
            $http.post('/Admin/edu_student/update_MXH', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        //delete: function (data, callback) {
        //    $http.post('/task011/delete/' + data).success(callback);
        //},
        getItem_MXH: function (data, callback) {
            $http.get('/Admin/edu_student/getItem_MXH/' + data).success(callback);
        },


        gettreedataParent_idKHV: function (callback) {
            $http.post('/Admin/edu_student/gettreedataParent_idKHV/').success(callback);
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
        .when('/addMXH/', {
            templateUrl: ctxfolder + '/addMXH.html',
            controller: 'addMXH'
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
            url: "/Admin/edu_student/jtable",
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
        //.withOption('createdRow', function (row, data, dataIndex) {
        //    const contextScope = $scope.$new(true);
        //    contextScope.data = data;
        //    contextScope.contextMenu = $scope.contextMenu;
        //    $compile(angular.element(row).find('input'))($scope);
        //    $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        //    $(row).find('td:not(:has(label.mt-checkbox))').on('dblclick', function (evt) {
        //        if (evt.target.localName == 'input' && evt.target.type == 'checkbox') {

        //        } else {
        //            var row = $(evt.target).closest('tr');
        //            // data key value
        //            var key = row.attr("data-id");
        //            // cell values
        //            var Id = row.find('td:eq(1)').text();
        //            if (evt.target.localName == 'input' && evt.target.type == 'checkbox') {
        //                $scope.selected[data.id_st] = !$scope.selected[data.id_st];
        //            }
        //            else {
        //                var self = $(this).parent();
        //                $scope.selected[data.id] = true;
        //                //dataservice.getItem(data.Id, function (rs) {
        //                //    if (rs.Error) {
        //                //        App.toastrError(rs.Title);
        //                //    } else {
        //                //        myService.setData(rs.Object);
        //                //        $location.path('/index/');
        //                //    }
        //                //});
        //                debugger
        //                dataservice.getItem(data.id_st, function (rs) {
        //                    if (rs.Error) {
        //                        App.toastrError(rs.Title);
        //                    }
        //                    else {
        //                        //myService.setData(rs.Object);

        //                        $scope.model = rs.Object;
        //                    }
        //                })
        //            }
        //            //else {
        //            //    var self = $(this).parent();
        //            //    $('#tblData').DataTable().$('tr.selected').removeClass('selected');
        //            //    $scope.selected.forEach(function (obj, index) {
        //            //        if ($scope.selected[index])
        //            //            $scope.selected[index] = false;
        //            //    });
        //            //    $(self).addClass('selected');
        //            //    $scope.selected[data.Id] = true;
        //            //}
        //        }
        //    });
        //});
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row).find('input'))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    //end option table 

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("id_st").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_st] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id_st + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        })/*.withOption('sWidth', '30px')*/.withOption('sClass'));

    vm.dtColumns.push(DTColumnBuilder.newColumn('firstname').withTitle('Họ và tên học sinh').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('Mã học sinh').renderWith(function (data, type) {
        return data;
    }));
    $scope.editingListTitle = function (id) {
        if ($scope.acticeDrag == false) {
            $scope.acticeDrag = true;
            document.getElementById("listName_" + id).focus();
        }
    }
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
    $scope.addMXH = function () {
        debugger
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/addMXH.html',
            controller: 'addMXH',
            backdrop: true,
            size: 'lg'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.addKHV = function () {
       debugger
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/addKHV.html',
            controller: 'addKHV',
            backdrop: true,
            size: '80'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
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

    setTimeout(function () {
        $('#FromDate').datetimepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            minView: 2
        });
        $('#ToDate').datetimepicker({
            autoclose: true,
            format: 'dd/mm/yyyy',
            minView: 2
        });
    }, 200);

    $scope.cancel = function () {
        this.close();
    };
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
    // //hide,show menu
    //function openNavCard() {
    //    if (!$rootScope.open) {
    //        $rootScope.open = true;
    //        document.getElementById("mySidenav").style.width = "200px";
    //        document.getElementById("BoardDetail").style.paddingLeft = "200px";
    //    }
    //    else {
    //        closeNavCard();
    //    }
    //}
    //function closeNavCard() {
    //    $rootScope.open = false;
    //    document.getElementById("mySidenav").style.width = "0";
    //    document.getElementById("BoardDetail").style.paddingLeft = "15px";
    //    document.body.style.backgroundColor = "white";
    //}
    //function openMenu() {
    //    $.app.menu.expanded = false;
    //    $.app.menu.expand();
    //}
    //function closeMenu() {
    //    $.app.menu.collapsed = false;
    //    $.app.menu.toggle();
    //}
    //function removeFooter() {
    //    var el = document.querySelector('footer.footer');
    //    el.parentNode.removeChild(el);
    //}

    //setTimeout(function () {
    //    $('.content-wrapper').css("height", "100%");
    //    $('#contentMain').css("height", "100%");
    //    //$('.ng-scope').not('.boards').not('.card').not('.search-bar').not('.board-detail').css("height", "100%");
    //    $('.container-fluid').not('.board-detail').css("height", "94%");
    //    $('.row').css("height", "100%");
    //    $('.testimonial-group').css("height", "100%");
    //    $.app.menu.expanded = true;
    //    $.app.menu.collapsed = false;
    //    $.app.menu.toggle();
    //    document.getElementById("mySidenav").style.width = "200px";
    //    document.getElementById("BoardDetail").style.paddingLeft = "200px";
    //    removeFooter();
    //}, 400);
    //$(document).ready(function (e) {
    //    $(".menu-toggle").click(function (e) {
    //        if ($.app.menu.collapsed) {
    //            $.app.menu.expanded = false;
    //            $.app.menu.expand();
    //            closeNavCard();
    //        } else {
    //            $.app.menu.collapsed = false;
    //            $.app.menu.toggle();
    //            closeNavCard();
    //        }
    //        e.stopImmediatePropagation();
    //    });
    //    $("#btnOpenTrello").click(function (e) {
    //        e.preventDefault();
    //        if ($.app.menu.expanded) {
    //            $.app.menu.toggle();
    //        }
    //        openNavCard();
    //        e.stopImmediatePropagation();
    //    });

    //});

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
    //hide,show menu
    function openNavCard() {
        if (!$rootScope.open) {
            $rootScope.open = true;
            document.getElementById("listds").style.width = "200px";
            document.getElementById("mySidenav").style.paddingLeft = "300px";
        }
        else {
            closeNavCard();
        }
    }
    function closeNavCard() {
        $rootScope.open = false;
        document.getElementById("listds").style.width = "400px";
        document.getElementById("mySidenav").style.paddingLeft = "15px";
        document.body.style.backgroundColor = "white";
    }
    function openMenu() {
        $.app.menu.expanded = false;
        $.app.menu.expand();
    }
    function closeMenu() {
        $.app.menu.collapsed = false;
        $.app.menu.toggle();
    }
    function removeFooter() {
        var el = document.querySelector('footer.footer');
        el.parentNode.removeChild(el);
    }
    setTimeout(function () {
        $('.content-wrapper').css("height", "100%");
        $('#contentMain').css("height", "100%");
        //$('.ng-scope').not('.boards').not('.card').not('.search-bar').not('.board-detail').css("height", "100%");
        $('.container-fluid').not('.board-detail').css("height", "94%");
        $('.row').css("height", "100%");
        $('.testimonial-group').css("height", "100%");
        $.app.menu.expanded = true;
        $.app.menu.collapsed = false;
        $.app.menu.toggle();
        document.getElementById("listds").style.width = "300px";
        document.getElementById("mySidenav").style.paddingLeft = "10px";
        removeFooter();
    }, 400);
    $(document).ready(function (e) {
        $(".menu-toggle").click(function (e) {
            if ($.app.menu.collapsed) {
                $.app.menu.expanded = false;
                $.app.menu.expand();
                closeNavCard();
            } else {
                $.app.menu.collapsed = false;
                $.app.menu.toggle();
                closeNavCard();
            }
            e.stopImmediatePropagation();
        });
        $("#btnOpenTrello").click(function (e) {
            e.preventDefault();
            if ($.app.menu.expanded) {
                $.app.menu.toggle();
            }
            openNavCard();
            e.stopImmediatePropagation();
        });
    });

});

app.controller('addMXH', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $filter) {

    $scope.initData = function () {
        dataservice.getItem(4, function (rs) {
            if (rs.Error) {
                App.notifyDanger(rs.Title);
            } else {
                $scope.model = rs[0];
            }
        });
    }
    $scope.initData();



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

                }
            });
        }
    }


});


app.controller('addKHV', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $filter, DTOptionsBuilder, DTColumnBuilder, DTInstances) {

    $scope.model
    {
        city = ''
    };

    $scope.initData = function () {
        debugger
        dataservice.gettreedataParent_idKHV(function (result) {
            $scope.treedataParent_idKHV = result.Object;
        });
        //dataservice.gettreedataDistrict(function (result) {
        //    $scope.treedataDistrict = result.Object;
        //});
        //dataservice.gettreedataIdstudentb(function (result) {
        //    $scope.treedataIdstudentb = result.Object;
        //});
        //dataservice.gettreedataCareer(function (result) {
        //    $scope.treedataCareer = result.Object;
        //});

    }
    $scope.initData();
    //$scope.loadHuyen = function () {
    //    dataservice.gettreedataDistrict($scope.model.city, function (result) {
    //        $scope.treedataDistrict = result.Object;
    //    });
    //}
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
            url: "/task012/jtable",
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


    vm.dtColumns.push(DTColumnBuilder.newColumn('value').withTitle('Loại').renderWith(function (data, type) {
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
