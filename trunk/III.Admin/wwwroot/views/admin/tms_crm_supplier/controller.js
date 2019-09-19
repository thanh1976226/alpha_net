var ctxfolder = "/views/admin/tms_crm_supplier";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        //getUser: function (callback) {
        //    $http.post('/Admin/AssetActivity/GetUser').success(callback);
        //},
        //getActivityType: function (callback) {
        //    $http.post('/Admin/AssetActivity/GetActivityType').success(callback);
        //},
        //getAsset: function (callback) {
        //    $http.post('/Admin/AssetActivity/GetAsset/').success(callback);
        //},
        //getItem: function (data, callback) {
        //    $http.post('/Admin/AssetActivity/GetItem/', data).success(callback);
        //},
        //insert: function (data, callback) {
        //    $http.post('/Admin/AssetActivity/Insert/', data).success(callback);
        //},
        //update: function (data, callback) {
        //    $http.post('/Admin/AssetActivity/Update/', data).success(callback);
        //},
        //delete: function (data, callback) {
        //    $http.post('/Admin/AssetActivity/Delete/' + data).success(callback);
        //},

    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $cookies, $translate) {
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
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
            var partternName = /^(^[ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.0-9\s]+$)|^(^[0-9]+[ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.\s][ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.0-9\s]*$)/
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.ActCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace('{0}', caption.AA_CURD_LBL_AA_ACTCODE), "<br/>");
            }
            if (!partternName.test(data.ActTitle)) {
                mess.Status = true;
                mess.Title += caption.COM_VALIDATE_ITEM_NAME.replace('{0}', caption.AA_CURD_LBL_AA_ACTTITLE) + "<br/>";
                //mess.Title += " - " + caption.VALIDATE_ITEM_NAME.replace('{0}', caption.USER_USERNAME) + "<br/>";
            }
            return mess;
        }
        $rootScope.validationOptions = {
            rules: {
               Code: {
                    required: true,
                    maxlength: 50
                },
                Name: {
                    required: true,
                    maxlength: 100
                }
            },
            //messages: {
            //    ActCode: {
            //        required: caption.COM_ERR_REQUIRED.replace("{0}", caption.AA_CURD_LBL_AA_ACTCODE),
            //        maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.AA_CURD_LBL_AA_ACTCODE).replace("{1}", "100")
            //    },
            //    ActTitle: {
            //        required: caption.COM_ERR_REQUIRED.replace("{0}", caption.AA_CURD_LBL_AA_ACTTITLE),
            //        maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.AA_CURD_LBL_AA_ACTTITLE).replace("{1}", "100")
            //    }
            //}
        }

        

    });
    
    //dataservice.getActivityType(function (rs) {
    //    $rootScope.ActivityType = rs;
    //})
    //dataservice.getUser(function (rs) {
    //    $rootScope.listUser = rs;
    //});
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
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $translate) {
    var vm = $scope;
    $scope.model = {
        Code: '',
        Name: ''
    };
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/tms_crm_supplier/Jtable",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.Code = $scope.model.ActCode;
                d.Name = $scope.model.ActTitle;
                //d.ActTyp = $scope.model.ActType;
                //d.ActNote = $scope.model.ActNote;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [2, 'asc'])
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
        }).withOption('sWidth', '30px').withOption('sClass', 'tcenter hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Code').withTitle('Mã nhà cung cấp').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Name').withTitle('Tên nhà cung cấp').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Tel').withTitle('Điện thoại').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Address').withTitle('Địa chỉ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('Tác vụ').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;Sửa&quot; | translate}}" ng-click="edit(' + full.ActivityId + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;Xóa&quot; | translate}}" ng-click="delete(' + full.ActivityId + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    }
    //$scope.search = function () {
    //    reloadData(true);
    //}

    //$scope.add = function () {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/add.html',
    //        controller: 'add',
    //        backdrop: 'static',
    //        size: '40'
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.reload();
    //    }, function () {
    //    });
    //}

    //$scope.edit = function (id) {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/edit.html',
    //        controller: 'edit',
    //        backdrop: 'static',
    //        size: '40',
    //        resolve: {
    //            para: function () {
    //                return id;
    //            }
    //        }
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.reloadNoResetPage();
    //    }, function () {
    //    });
    //}

    //$scope.delete = function (id) {
    //    var modalInstance = $uibModal.open({
    //        templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
    //        windowClass: "message-center",
    //        controller: function ($scope, $uibModalInstance) {
    //            $scope.message = "Bạn có chắc chắn muốn xóa ?";
    //            $scope.ok = function () {
    //                dataservice.delete(id, function (rs) {
    //                    if (rs.Error) {
    //                        App.toastrError(rs.Title);
    //                    } else {
    //                        App.toastrSuccess(rs.Title);
    //                        $uibModalInstance.close();
    //                    }
    //                });
    //            };

    //            $scope.cancel = function () {
    //                $uibModalInstance.dismiss('cancel');
    //            };
    //        },
    //        size: '25',
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.reloadNoResetPage();
    //    }, function () {
    //    });
    //}

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
    }, 200);
});
//app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice) {
//    $scope.model = {
//        ActCode: '',
//        ActTitle: '',
//        ActType: '',
//        ActMember: '',
//    }
//    $scope.model1 = {
//        listMember: []
//    }

//    $scope.initData = function () {
//        dataservice.getAsset(function (rs) {
//            $scope.listAsset = rs;
//        })
//    }
//    $scope.initData();

//    $scope.cancel = function () {
//        $uibModalInstance.dismiss('cancel');
//    }
//    $scope.submit = function () {
//        if ($scope.addform.validate()) {
//            var temp = $rootScope.checkData($scope.model);
//            if (temp.Status) {
//                App.toastrError(temp.Title);
//                return;
//            }
//            $scope.model.ActMember = $scope.model1.listMember.join(',');
//            dataservice.insert($scope.model, function (rs) {
//                if (rs.Error) {
//                    App.toastrError(rs.Title);
//                }
//                else {
//                    App.toastrSuccess(rs.Title);
//                    $uibModalInstance.close();
//                }
//            })
//        }
//    }
//    setTimeout(function () {
//        setModalDraggable('.modal-dialog');
//    }, 200);
//});
//app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
//    $scope.model1 = {
//        listMember: []
//    }
//    $scope.cancel = function () {
//        $uibModalInstance.close();
//    }
//    $scope.initData = function () {
//        dataservice.getAsset(function (rs) {
//            $scope.listAsset = rs;
//        })
//        dataservice.getItem(para, function (rs) {
//            if (rs.Error) {
//                App.toastrError(rs.Title);
//            }
//            else {
//                $scope.model = rs;
//                var listMember = rs.ActMember.split(',');
//                for (var i = 0; i < listMember.length; i++) {
//                    for (var j = 0; j < $rootScope.listUser.length; j++) {
//                        if (listMember[i] == $rootScope.listUser[j].Id) {
//                            $scope.model1.listMember.push($rootScope.listUser[j]);
//                            break;
//                        }
//                    }
//                }
//            }
//        });
//    }
//    $scope.initData();
//    $scope.submit = function () {
//        if ($scope.addform.validate()) {
//            var temp = $rootScope.checkData($scope.model);
//            if (temp.Status) {
//                App.toastrError(temp.Title);
//                return;
//            }
//            $scope.model.ActMember = $scope.model1.listMember.join(',');
//            dataservice.update($scope.model, function (rs) {
//                if (rs.Error) {
//                    App.toastrError(rs.Title);
//                }
//                else {
//                    App.toastrSuccess(rs.Title);
//                    $uibModalInstance.close();
//                }
//            })
//        }
//    }
//    setTimeout(function () {
//        setModalDraggable('.modal-dialog');
//    }, 200);
//});