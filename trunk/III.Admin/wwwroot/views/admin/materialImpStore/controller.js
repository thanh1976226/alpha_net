var ctxfolder = "/views/admin/materialImpStore";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate", 'dynamicNumber'])
    .directive('customOnChange', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                var onChangeHandler = scope.$eval(attrs.customOnChange);
                element.on('change', onChangeHandler);
                element.on('$destroy', function () {
                    element.off();
                });

            }
        };
    });
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        getUnit: function (data, callback) {
            $http.get('/Admin/MaterialImpStore/GetUnit?impCode=' + data).success(callback);
        },
        getStore: function (callback) {
            $http.post('/Admin/MaterialImpStore/GetStore').success(callback);
        },
        getUser: function (callback) {
            $http.post('/Admin/MaterialImpStore/GetUser').success(callback);
        },
        getSupplier: function (callback) {
            $http.post('/Admin/MaterialImpStore/Getsupplier').success(callback);
        },
        getItem: function (data, callback) {
            $http.post('/Admin/MaterialImpStore/GetItem', data).success(callback);
        },
        insert: function (data, callback) {
            $http.post('/Admin/MaterialImpStore/Insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/MaterialImpStore/Update', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/MaterialImpStore/Delete', data).success(callback);
        }
    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, $cookies, $translate, dataservice) {
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
        $rootScope.checkData = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data)) {
                mess.Status = true;
                //mess.Title = mess.Title.concat(" - ", "Mã nhà cung cấp không chứa ký tự đặc biệt hoặc khoảng trắng!", "<br/>");
            }
            return mess;
        }
        $rootScope.validationOptions = {
            rules: {
                ImpCode: {
                    required: true,
                    maxlength: 100
                },
                CreatedTime: {
                    required: true,
                },
                Note: {
                    maxlength: 1000
                }
            },
            messages: {
                ImpCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MIS_CURD_LBL_MIS_CODE).replace("{1}", "100")
                },
                CreatedTime: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CREATE),
                    //required: caption.ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CREATE)
                },
                Note: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MIS_CURD_LBL_MIS_NOTE).replace("{1}", "1000"),
                }
            }
        }
        //$rootScope.checkData = function (data) {
        //    var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
        //    var partternName = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]+[^!@#$%^&*<>?]*$/; //Có chứa được khoảng trắng
        //    //var partternDescription = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@#$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng
        //    var partternUrl = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng & dấu #
        //    var mess = { Status: false, Title: "" }
        //    if (!partternCode.test(data.ApplicationCode)) {
        //        mess.Status = true;
        //        mess.Title = mess.Title.concat(" - ", caption.VALIDATE_ITEM_CODE.replace('{0}', caption.APP_CODE), "<br/>");
        //    }
        //    if (!partternName.test(data.Title)) {
        //        mess.Status = true;
        //        mess.Title += " - " + caption.VALIDATE_ITEM_NAME.replace('{0}', caption.APP_NAME) + "<br/>";
        //    }
        //    if (!partternUrl.test(data.AppUrl)) {
        //        mess.Status = true;
        //        mess.Title += " - " + caption.VALIDATE_ITEM.replace('{0}', caption.URL_APP) + "<br/>";
        //    }
        //    return mess;
        //}
        //$rootScope.validationOptions = {
        //    rules: {
        //        Title: {
        //            required: true,
        //            maxlength: 255
        //        },
        //        ApplicationCode: {
        //            required: true,
        //            maxlength: 50
        //        },
        //        Ord: {
        //            required: true,
        //            maxlength: 5
        //        },
        //        AppUrl: {
        //            required: true,
        //            maxlength: 255
        //        }
        //    },
        //    messages: {
        //        Title: {
        //            required: caption.ERR_REQUIRED.replace('{0}', caption.APP_NAME),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace('{0}', caption.APP_NAME).replace('{1}', '255')
        //        },
        //        ApplicationCode: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.APP_CODE),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace("{0}", caption.APP_CODE).replace("{1}", "50")
        //        },
        //        Ord: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.ORDER),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace('{0}', caption.ORDER_SORT).replace('{1}', '5')
        //        },
        //        AppUrl: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.URL_APP),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace("{0}", caption.URL_APP).replace("{1}", "255")
        //        }
        //    }
        //}

    });
    //$rootScope.checkData = function (data) {
    //    var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
    //    var mess = { Status: false, Title: "" }
    //    if (!partternCode.test(data)) {
    //        mess.Status = true;
    //        //mess.Title = mess.Title.concat(" - ", "Mã nhà cung cấp không chứa ký tự đặc biệt hoặc khoảng trắng!", "<br/>");
    //    }
    //    return mess;
    //}
    //$rootScope.validationOptions = {
    //    rules: {
    //        ImpCode: {
    //            required: true,
    //            maxlength:50
    //        },
    //        CreatedTime: {
    //            required: true,
    //        },
    //        Note: {
    //            maxlength:1000
    //        }
    //    },
    //    messages: {
    //        ImpCode: {
    //            required: "Mã nhập hàng yêu cầu bắt buộc!"
    //        },
    //        CreatedTime: {
    //            required: "Ngày tạo yêu cầu bắt buộc!"
    //        },
    //        Note: {
    //            maxlength:"Ghi chú không vượt quá 1000 kí tự!"
    //        }
    //    }
    //}
    $rootScope.ImpCode = '';
});
app.config(function ($routeProvider, $validatorProvider, $translateProvider) {

    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    //$translateProvider.preferredLanguage('en-US');
    caption = $translateProvider.translations();

    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
        })
        .when('/edit/:id', {
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit'
        })
        .when('/add/', {
            templateUrl: ctxfolder + '/add.html',
            controller: 'add'
        }).when('/function/', {
            templateUrl: ctxfolder + '/function.html',
            controller: 'function'
        });

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
    var vm = $scope;
    $scope.model = {
        ImpCode: '',
        StoreName: '',
        FromTo: '',
        DateTo: ''
    };
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liFunction = [];
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/MaterialImpStore/jtable",
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
                d.ImpCode = $scope.model.ImpCode;
                d.StoreName = $scope.model.StoreName;
                d.FromTo = $scope.model.FromTo;
                d.DateTo = $scope.model.DateTo;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [0, 'asc'])
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
        }).withOption('sWidth', '30px').withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ImpCode').withTitle('{{ "MIS_LIST_COL_MIS_CODE" | translate }}').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('StoreName').withTitle('{{ "MIS_LIST_COL_MIS_NAME_WAREHOUSE" | translate }}').renderWith(function (data) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Total').withTitle('{{ "MIS_LIST_COL_MIS_TOTAL" | translate }}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('TotalPayment').withTitle('{{ "MIS_LIST_COL_MIS_TOTALPAYMENT" | translate }}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Vat').withTitle('{{ "MIS_LIST_COL_MIS_VAT" | translate }}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) + "%" : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('TotalPayed').withTitle('{{ "MIS_LIST_COL_MIS_TOTALPAYED" | translate }}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Debt').withTitle('{{ "MIS_LIST_COL_MIS_DEBT" | translate }}').renderWith(function (data, type, full) {
        var result = "";
        if (data == "") {
            result = "Không xác định";
        } else if (data == "True") {
            result = (full.Total - full.TotalPayed);
            result = result != "" ? $filter('currency')(result, '', 0) : null
        } else if (data == "False") {
            result = "Không xác định";
        }
        return result;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{ "MIS_LIST_COL_MIS_CREATE" | translate }}').renderWith(function (data) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "MIS_LIST_COL_MIS_ACTION" | translate }}').renderWith(function (data, type, full) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45);" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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

    $scope.search = function () {
        reloadData(true);
    }
    $scope.reload = function () {
        reloadData(true);
    }
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    };

    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: 'static',
            size: '70'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.edit = function (id) {
        var userModel = {};
        var listdata = $('#tblData').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i].Id == id) {
                userModel = listdata[i];
                break;
            }
        }
        $rootScope.ImpCode = userModel.ImpCode;
        dataservice.getItem(userModel.Id, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: ctxfolder + '/edit.html',
                    controller: 'edit',
                    backdrop: 'static',
                    size: '70',
                    resolve: {
                        para: function () {
                            return rs.Object;
                        }
                    }
                });
                modalInstance.result.then(function (d) {
                    $scope.reloadNoResetPage();
                }, function () {
                });
            }
        });
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn muốn xóa lô hàng ?";
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
            size: '25',
        });
        modalInstance.result.then(function (d) {
            $scope.reloadNoResetPage();
        }, function () {
        });

    }
    function loadDate() {
        $("#FromDate").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#DateTo').datepicker('setStartDate', maxDate);
        });
        $("#ToDate").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#FromTo').datepicker('setEndDate', maxDate);
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
        loadDate();
    }, 200);
});

app.controller('add', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, $uibModalInstance, $filter, dataservice) {
    var vm = $scope;
    $scope.model = {
        ImpCode: '',
        SupplierId: '',
        UserId: '',
        Total: '',
        TotalPayment: '',
        Vat: '',
        TotalPayed: '',
        ShipperId: '',
        StoreId: '',
        CreatedTime: '',
        Debt: false,
        Note: '',
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liFunction = [];


    $scope.chkSubTab = function () {
        if ($rootScope.ImpCode == '') {
            App.toastrError('{{"MIS_CURD_VALIDATE_TAB_CLICK" | translate}}');
        }
    }

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/MaterialImpStore/JTableDetail",
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
                //d.Code = $scope.model.Code;
                //d.Name = $scope.model.Name;
                //d.FromTo = $scope.model.FromTo;
                //d.DateTo = $scope.model.DateTo;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [0, 'asc'])
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
        }).withOption('sWidth', '30px').withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ProductName').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_NAME_ATTRIBUTE" | translate}}').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Quantity').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_QUANTITY" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Unit').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_UNIT" | translate}}').renderWith(function (data) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Cose').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_COSE" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Saleoff').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_SALEOFF" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) + "%" : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('BarCode').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_BARCODE" | translate}}').renderWith(function (data) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ExpireDate').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_EXPIREDATE" | translate}}').renderWith(function (data, type, full) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45);" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && !validationSelect($scope.model).Status) {
            var msg = $rootScope.checkData($scope.model.ImpCode);
            if (msg.Status) {

                App.toastrError(caption.MIS_CURD_VALIDATE_CHARACTERS_SPACE);
                return;
            }
            dataservice.insert($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            });
        }
    }
    $scope.initLoad = function () {
        dataservice.getUser(function (rs) {
            $scope.UserData = rs;
        });
        dataservice.getStore(function (rs) {
            $scope.StoreData = rs;
        });
        dataservice.getSupplier(function (rs) {
            $scope.SupplierData = rs;
        });
    }
    $scope.initLoad();
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "SupplierId" && $scope.model.SupplierId != "") {
            $scope.errorSupplierId = false;
        }
        if (SelectType == "UserId" && $scope.model.UserId != "") {
            $scope.errorUserId = false;
        }
        if (SelectType == "StoreId" && $scope.model.StoreId != "") {
            $scope.errorStoreId = false;
        }
    }
    setTimeout(function () {
        $("#CreatedTime").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        });
        var today = new Date(new Date());
        $('#CreatedTime').datepicker('setStartDate', today);
        $('#CreatedTime').datepicker('update', new Date());
        $scope.model.CreatedTime = $filter('date')(new Date(today), 'dd/MM/yyyy');
        setModalDraggable('.modal-dialog');
    }, 100);
    //Validate UiSelect
    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        //Check null tên nhà cung cấp
        if (data.SupplierId == "") {
            $scope.errorSupplierId = true;
            mess.Status = true;
        } else {
            $scope.errorSupplierId = false;
        }
        //Check null người đại diện
        if (data.UserId == "") {
            $scope.errorUserId = true;
            mess.Status = true;
        } else {
            $scope.errorUserId = false;
        }

        //Check null Tên kho
        if (data.StoreId == "") {
            $scope.errorStoreId = true;
            mess.Status = true;
        } else {
            $scope.errorStoreId = false;
        }
        return mess;
    };
});

app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, $uibModalInstance, $filter, dataservice, para) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liFunction = [];
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/MaterialImpStore/JTableDetail",
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
                d.ImpCode = $rootScope.ImpCode;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [0, 'asc'])
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
        }).withOption('sWidth', '30px').withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ProductName').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_NAME_ATTRIBUTE" | translate}}').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Quantity').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_QUANTITY" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Unit').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_UNIT" | translate}}').renderWith(function (data) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Cose').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_COSE" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Saleoff').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_SALEOFF" | translate}}').renderWith(function (data) {
        return data != "" ? $filter('currency')(data, '', 0) + "%" : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('BarCode').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_BARCODE" | translate}}').renderWith(function (data) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ExpireDate').withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_EXPIREDATE" | translate}}').renderWith(function (data, type, full) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"MIS_CURD_TAB_DETAIL_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45);" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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

    $scope.search = function () {
        if ($scope.model.Name === '' || $scope.model.Name == null) {
            App.toastrWarning('Nhập điều kiện tìm kiếm');
        } else {
            reloadData(true);
        }
    }
    $scope.reload = function () {
        reloadData(false);
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "ProductCode" && $scope.model.ProductCode != "") {
            $scope.errorProductCode = false;
        }
        if (SelectType == "StoreId" && $scope.model.StoreId != "") {
            $scope.errorStoreId = false;
        }
        if (SelectType == "Unit" && $scope.model.Unit != "") {
            $scope.errorUnit = false;
        }
    }
    $scope.initLoad = function () {
        dataservice.getUnit($rootScope.ImpCode, function (rs) {
            $scope.UnitData = rs;
        });
        dataservice.getUser(function (rs) {
            $scope.UserData = rs;
        });
        dataservice.getStore(function (rs) {
            $scope.StoreData = rs;
        });
        dataservice.getSupplier(function (rs) {
            $scope.SupplierData = rs;
        });
        $scope.model = para;
    }
    $scope.initLoad();
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.editform.validate() && !validationSelect($scope.model).Status) {
            dataservice.update($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
            });
        }
    }
    setTimeout(function () {
        $("#CreatedTime").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        });
        var today = new Date(new Date());
        $('#CreatedTime').datepicker('setStartDate', today);
        setModalDraggable('.modal-dialog');
    }, 200);
    //validate UiSelect
    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        //Check null tên sản phẩm 
        if (data.ProductCode == "") {
            $scope.errorProductCode = true;
            mess.Status = true;
        } else {
            $scope.errorProductCode = false;
        }
        //Check null tên kho
        if (data.StoreId == "") {
            $scope.errorStoreId = true;
            mess.Status = true;
        } else {
            $scope.errorStoreId = false;
        }

        //Check null tên kho
        if (data.Unit == "") {
            $scope.errorUnit = true;
            mess.Status = true;
        } else {
            $scope.errorUnit = false;
        }
        return mess;
    };
});
