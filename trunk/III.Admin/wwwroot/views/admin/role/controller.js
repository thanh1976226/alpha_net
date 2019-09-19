﻿var ctxfolder = "/views/admin/role";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", "ui.select", 'datatables.colreorder', 'angular-confirm', "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/Role/Insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/Role/Update', data).success(callback);
        },
        checkRoleUsed: function (data, callback) {
            $http.post('/Admin/Role/CheckRoleUsed', data).success(callback);
        },
        deleteItems: function (data, callback) {
            $http.post('/Admin/Role/DeleteItems/', data).success(callback);
        },
        //delete: function (data, callback) {
        //    $http.post('/Admin/Role/delete', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/Admin/Role/Delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.post('/Admin/Role/Getitem/', data).success(callback);
        },
        //getItem: function (data, callback) {
        //    $http.post('/Admin/Role/getitem/?id=' + data).success(callback);
        //},
        resort: function (data, callback) {
            $http.post('/Admin/Role/Resort', data).success(callback);
        },
        getAll: function (callback) {
            $http.post('/Admin/Role/GetAll/').success(callback);
        },
        getMapPermission: function (data, callback) {
            $http.post('/Admin/Role/GetMapPermission/', data).success(callback);
        }
    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, $cookies, $translate, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };

    var culture = $cookies.get('_CULTURE') || 'en-US';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];

        $rootScope.checkData = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
            var partternName = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]+[^!@#$%^&*<>?]*$/; //Có chứa được khoảng trắng
            var partternDescription = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@#$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.Code)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_CODE), "<br/>");
            }
            if (!partternName.test(data.Title)) {
                mess.Status = true;
                mess.Title += " - " + caption.COM_VALIDATE_ITEM_NAME.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_NAME) + "<br/>";
            }
            if (!partternDescription.test(data.Description)) {
                mess.Status = true;
                mess.Title += " - " + caption.COM_VALIDATE_ITEM.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_DESCIPTION) + "<br/>";
            }
            return mess;
        }
        $rootScope.validationOptions = {
            rules: {
                Title: {
                    required: true,
                    maxlength: 255
                },
                Code: {
                    required: true,
                    maxlength: 50
                },
                Status: {
                    required: true
                },
                Description: {
                    maxlength: 2000
                }
            },
            messages: {
                Title: {
                    required: caption.COM_ERR_REQUIRED.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_NAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_NAME).replace('{1}', '255')
                },
                Code: {
                    required: caption.COM_ERR_REQUIRED.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_CODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_CODE).replace('{1}', '50')
                },
                Status: {
                    required: caption.COM_ERR_REQUIRED.replace('{0}', caption.Status)
                },
                Description: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_ROLE_CURD_LBL_ROLE_DESCIPTION).replace('{1}', '2000')
                }
            }
        }
    });
    $rootScope.StatusData = [{
        Value: true,
        Name: "Hoạt động"
    }, {
        Value: false,
        Name: "Không hoạt động"
    }];

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

app.controller('index', function ($scope, $rootScope, $compile, $confirm, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $translate) {

    var vm = $scope;
    $scope.model = {
        Code: '',
        Name: '',
        Status: true
    };

    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liRole = [];
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Role/Jtable",
            beforeSend: function (jqXHR, settings) {
                resetCheckbox();
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
                $scope.liRole = [];
            },
            type: 'POST',
            data: function (d) {
                d.Code = $scope.model.Code;
                d.Name = $scope.model.Name;
                d.Status = $scope.model.Status;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [2, 'asc'])
        .withOption('serverSide', true)
        //.withOption('autoWidth', true)
        //.withOption('scrollY', $(window).height() - 220)
        //.withOption('scrollX', false)
        //.withOption('scrollCollapse', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            $scope.liRole.push(data);

            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row).contents())($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("_STT").withTitle(titleHtml).notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full._STT] = false;

            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full._STT + ']" ng-click="toggleOne(selected, $event)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Code').withTitle('{{"ADM_ROLE_LIST_COL_ROLE_CODE" | translate}}').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"ADM_ROLE_LIST_COL_ROLE_NAME" | translate}}').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Description').withTitle('{{"ADM_ROLE_LIST_COL_ROLE_DESCIPTION" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Status').withTitle('{{"ADM_ROLE_LIST_COL_ROLE_STATUS" | translate}}').renderWith(function (data, type) {
        return data == "True" ? '<span class="text-success">Hoạt động</span>' : '<span class="text-danger">Không hoạt động</span>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"ADM_ROLE_LIST_COL_ROLE_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button ng-click="edit(' + full._STT + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>'
           /* '<button ng-click="delete(' + full._STT + ')" style = "width: 25px; height: 25px; padding: 0px;;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>'*/;
    }));
    vm.reloadData = reloadData;
    vm.dtInstance = {};
    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
    }
    function callback(json) {

    }
    function toggleAll(selectAll, selectedItems) {
        //if (selectAll)
        //    $('#tblData').DataTable().$('tr:not(.selected)').addClass('selected');
        //else
        //    $('#tblData').DataTable().$('tr.selected').removeClass('selected');

        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                selectedItems[id] = selectAll;
            }
        }
    }
    function toggleOne(selectedItems, evt) {
        //$(evt.target).closest('tr').toggleClass('selected');

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
            size: '50'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.edit = function (stt) {
        var userModel = {};
        var listdata = $('#tblData').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i]._STT == stt) {
                userModel = listdata[i];
                break;
            }
        }

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit',
            backdrop: 'static',
            size: '50',
            resolve: {
                para: function () {
                    return userModel;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reloadNoResetPage();
        }, function () {
        });
    }
    //$scope.showMapPermission = function () {
    //    var editItems = [];
    //    for (var id in $scope.selected) {
    //        if ($scope.selected.hasOwnProperty(id)) {
    //            if ($scope.selected[id]) {
    //                editItems.push(id);
    //            }
    //        }
    //    }
    //    if (editItems.length > 0) {
    //        var listRoleId = $scope.liRole.filter(function (obj, index) {
    //            var index1 = editItems.indexOf(index + 1 + '');
    //            return index1 > -1;
    //        });

    //        var modalInstance = $uibModal.open({
    //            animation: true,
    //            templateUrl: ctxfolder + '/permission.html',
    //            controller: 'permission',
    //            backdrop: 'static',
    //            size: '',
    //            resolve: {
    //                para: function () {
    //                    return listRoleId;
    //                }
    //            }
    //        });
    //        modalInstance.result.then(function (d) {
    //        }, function () { });
    //    } else {
    //        App.toastrError(caption.ERR_NOT_CHECKED.replace('{0}', caption.ROLE));
    //    }
    //}
    //$scope.exportPermission = function () {
    //    var editItems = [];
    //    for (var id in $scope.selected) {
    //        if ($scope.selected.hasOwnProperty(id)) {
    //            if ($scope.selected[id]) {
    //                editItems.push(id);
    //            }
    //        }
    //    }
    //    if (editItems.length > 0) {
    //        bootbox.confirm({
    //            title: 'Confirm',
    //            message: 'Do you want to export map permission of selected Role?',
    //            callback: function (result) {
    //                if (result) {
    //                    var listRoleId = $scope.liRole.filter(function (obj, index) {
    //                        var index1 = editItems.indexOf(index + 1 + '');
    //                        return index1 > -1;
    //                    });

    //                    var objCode = listRoleId.map(function (obj, index) { return 'code=' + obj.Code; }).join('&');
    //                    location.href = "/Admin/Role/ExportMapPermision?" + objCode;
    //                }
    //            }
    //        });
    //    } else {
    //        App.toastrError(caption.ERR_NOT_CHECKED.replace('{0}', caption.ROLE));
    //    }
    //}
    //$scope.sort = function () {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/sort.html',
    //        controller: 'sort',
    //        backdrop: 'static',
    //        size: '60'
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.reload();
    //    }, function () {
    //    });
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
    //    var deleteItemsId = [];
    //    var deleteRolesId = [];
    //    for (var i = 0; i < deleteItems.length; i++) {
    //        deleteItemsId.push($scope.liRole[parseInt(deleteItems[i]) - 1]);
    //    }
    //    if (deleteItemsId.length > 0) {
    //        $confirm({ text: caption.MSG_DELETE_LIST_CONFIRM.replace('{0}', caption.ROLE.toLowerCase()), title: caption.CONFIRM, ok: caption.CONFIRM_OK, cancel: caption.CONFIRM_CANCEL })
    //            .then(function () {
    //                //App.blockUI({
    //                //    target: "#contentMain",
    //                //    boxed: true,
    //                //    message: 'loading...'
    //                //});
    //                for (var j = 0; j < deleteItemsId.length; j++) {
    //                    deleteRolesId.push(deleteItemsId[j].Id);
    //                }
    //                dataservice.deleteItems(deleteRolesId, function (result) {
    //                    if (result.Error) {
    //                        App.toastrError(result.Title);
    //                        $scope.reload();
    //                    } else {
    //                        App.toastrSuccess(result.Title);
    //                        $scope.reload();
    //                    }
    //                    App.unblockUI("#contentMain");
    //                });
    //            });
    //    } else {
    //        App.toastrError(caption.ERR_NOT_CHECKED.replace('{0}', caption.ROLE.toLowerCase()));
    //    }
    //}

    //$scope.contextMenu = [
    //    [function ($itemScope) {
    //        return '<i class="fa fa-edit"></i> ' + caption.EDIT_ITEM.replace('{0}', caption.ROLE.toLowerCase());
    //    }, function ($itemScope, $event, model) {
    //        var modalInstance = $uibModal.open({
    //            animation: true,
    //            templateUrl: ctxfolder + '/edit.html',
    //            controller: 'edit',
    //            backdrop: 'static',
    //            size: '80',
    //            resolve: {
    //                para: function () {
    //                    return $itemScope.data;
    //                }
    //            }
    //        });
    //        modalInstance.result.then(function (d) {
    //            $scope.reload();
    //        }, function () {
    //        });
    //    }, function ($itemScope, $event, model) {
    //        return true;
    //    }],
    //    //[function ($itemScope) {
    //    //    return '<i class="fa fa-remove"></i> ' + caption.DELETE_ITEM.replace('{0}', caption.ROLE.toLowerCase());
    //    //}, function ($itemScope, $event, model) {

    //    //    $confirm({ text: caption.MSG_DEL_CONFIRM_WITH_NAME.replace('{0}', caption.ROLE.toLowerCase()).replace('{1}', $itemScope.data.Title), title: caption.CONFIRM, cancel: caption.CONFIRM_CANCEL })
    //    //        .then(function () {
    //    //            //App.blockUI({
    //    //            //    target: "#contentMain",
    //    //            //    boxed: true,
    //    //            //    message: 'loading...'
    //    //            //});
    //    //            dataservice.delete($itemScope.data.Id, function (result) {
    //    //                if (result.Error) {
    //    //                    App.toastrError(result.Title);
    //    //                } else {
    //    //                    App.toastrSuccess(result.Title);
    //    //                    $scope.reload();
    //    //                }
    //    //                App.unblockUI("#contentMain");
    //    //            });
    //    //        });
    //    //}, function ($itemScope, $event, model) {
    //    //    return true;
    //    //}]
    //];
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
    }, 50);
});

app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $translate, $timeout) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.model = {
        status: 'true'
    };
    $scope.submit = function () {
        if ($scope.addform.validate()) {
            var temp = $rootScope.checkData($scope.model);
            if (temp.Status) {
                App.toastrError(temp.Title);
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

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});

app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para, $translate, $timeout) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        dataservice.getItem(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
            }
        });
    }
    $scope.initData();
    $scope.submit = function () {
        if ($scope.editform.validate()) {
            var temp = $rootScope.checkData($scope.model);
            if (temp.Status) {
                App.toastrError(temp.Title);
                return;
            }
            if ($scope.model.Status === false) {
                dataservice.checkRoleUsed($scope.model, function (rs) {
                    if (rs) {
                        $confirm({ text: caption.DISABLE_ROLE, title: caption.CONFIRM, cancel: caption.CONFIRM_CANCEL })
                            .then(function () {
                                dataservice.update($scope.model, function (rs1) {
                                    if (rs1.Error) {
                                        App.toastrError(rs1.Title);
                                    } else {
                                        App.toastrSuccess(rs1.Title);
                                        $uibModalInstance.close();
                                    }
                                });
                            });
                    } else {
                        dataservice.update($scope.model, function (rs1) {
                            if (rs1.Error) {
                                App.toastrError(rs1.Title);
                            } else {
                                App.toastrSuccess(rs1.Title);
                                $uibModalInstance.close();
                            }
                        });
                    }
                });
            } else {
                dataservice.update($scope.model, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    } else {
                        App.toastrSuccess(rs.Title);
                        $uibModalInstance.close();
                    }
                });

            }
        }
    }

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});

app.controller('permission', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, $translate, $timeout, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.para = para;
    $scope.listPermission = [];

    $scope.initData = function () {
        var listCode = para.map(function (obj, index) { return obj.Code; });
        dataservice.getMapPermission(listCode, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.listPermission = rs;
            }
        });
    }
    $scope.initData();

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});
