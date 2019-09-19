var ctxfolder = "/views/admin/iotIdentify";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {

    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, dataservice, $filter, $cookies, $translate) {
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


        })
        //$rootScope.validationOptions = {
        //    rules: {
        //        StoreCode: {
        //            required: true,
        //            maxlength: 100
        //        },
        //        StoreName: {
        //            required: true,
        //            maxlength: 250
        //        },
        //        Description: {
        //            maxlength: 500
        //        }
        //    },
        //    messages: {
        //        StoreCode: {
        //            required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MATERIAL_STORE_COL_STORE_CODE),
        //            maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MATERIAL_STORE_LIST_COL_STORE_CODE).replace("{1}", "100")
        //        },
        //        StoreName: {
        //            required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MATERIAL_STORE_LIST_COL_STORE_NAME),
        //            maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MATERIAL_STORE_LIST_COL_STORE_NAME).replace("{1}", "250")
        //        },
        //        Description: {
        //            axlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.MATERIAL_STORE_LBL_DESCRIPTION).replace('{1}', '500')
        //        }
        //    }
        //}
    });

    //$rootScope.DateNow = $filter('date')(new Date(), 'dd/MM/yyyy');
    //$rootScope.DateBeforeSevenDay = $filter('date')(new Date().setDate((new Date()).getDate() + (-7)), 'dd/MM/yyyy');

});
app.config(function ($routeProvider, $validatorProvider, $translateProvider) {
    //$translateProvider.useUrlLoader('/Language/Translation');
    //$translateProvider.preferredLanguage('en-US');
    //caption = $translateProvider.translations();
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
app.controller('index', function ($scope, $rootScope, $confirm, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter, $translate) {
    $scope.model = {
        FromDate: '',
        ToDate: ''
    }
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/IOTIdentify/JTable",
            beforeSend: function (jqXHR, settings) {
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
        .withOption('order', [3, 'asc'])
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);

        })
        .withOption('footerCallback', function (tfoot, data) {
            if (data.length > 0) {
                $scope.$apply(function () {
                    $scope.Total = 0;
                    angular.forEach(data, function (item, index) {
                        $scope.TotalNmber = parseFloat($scope.TotalNmber) + parseFloat(item.Total);
                    });
                });
            }
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.Id] = false;
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', 'tcenter hidden'));

    vm.dtColumns.push(DTColumnBuilder.newColumn('FaceId').withTitle('Mã thiết bị').withOption('sClass', 'dataTable-pr0').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Address').withTitle('Địa chỉ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('NameFace').withTitle('Tên đối tượng').withOption('sClass', 'dataTable-pr0').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ObjType').withTitle('Loại đối tượng').withOption('sClass', 'dataTable-pr0').renderWith(function (data, type) {
        if (data == 'True') {
            return '<img src="/uploads/files/iconNguoi.png" style="color: rgb(106,170,89); aria-hidden="true"  /><span><i style="color: rgb(229, 13, 38);font-size: 10px;">Người</i></span>';
        }
        else {
            //return '<i style="color: rgb(226,165,139);font-size: 15px;" class="fas fa-box-open" aria-hidden="true"> Vật</i>';
            return '<img src="/uploads/files/iconVat.png" style="color: rgb(226,165,139);font-size: 15px;" class="fas fa-box-open" aria-hidden="true" /><span><i style="color: rgb(229, 13, 142);font-size: 10px;">Vật</i></span>';
        }
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Total').withTitle('Tổng số').renderWith(function (data, type) {
    //    return '<span class="text-danger">' + $filter('currency')(data, '', 0) + '</span>';
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Action').withTitle('Hành động').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Image1').withTitle('Ảnh1').renderWith(function (data, type) {
        debugger
        return '<img class="img-circle" src="' + data + '" onerror =' + "'" + 'this.src="' + '/images/default/no_user.png' + '"' + "'" + 'height="30" width="30">';
    }).withOption('sWidth', '50px'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Image2').withTitle('Ảnh2').renderWith(function (data, type) {
        debugger
        return '<img class="img-circle" src="' + data + '" onerror =' + "'" + 'this.src="' + '/images/default/no_user.png' + '"' + "'" + 'height="30" width="30">';
    }).withOption('sWidth', '50px'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Image3').withTitle('Ảnh3').renderWith(function (data, type) {
        debugger
        return '<img class="img-circle" src="' + data + '" onerror =' + "'" + 'this.src="' + '/images/default/no_user.png' + '"' + "'" + 'height="30" width="30">';
    }).withOption('sWidth', '50px'));
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
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    }
    $scope.search = function () {
        reloadData(true);
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
        $("#FromDate").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
            
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#ToDate').datepicker('setStartDate', maxDate);
            debugger
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
        showHideSearch();
    }, 200);
});
app.controller('detail', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
        $rootScope.reload();
    }
    $scope.model1 = {
        ListChoose: []
    };
    $scope.model = {};
    $scope.ListBoxId = [];
    $scope.QR_Code_Req = '';

    $scope.initData = function () {
        $scope.model = para;
    }
    $scope.initData();

    $scope.createReqCode = function (boxCode) {
        dataservice.generatorQRCode(boxCode, function (result) {
            $scope.QR_Code_Req = result;
        });
    };
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);

    setTimeout(function () {
    }, 200);
});