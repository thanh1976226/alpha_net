var ctxfolder = "/views/admin/GroupUser";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/GroupUser/insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/GroupUser/update', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/GroupUser/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.post('/Admin/GroupUser/Getitem/' + data).success(callback);
        },
        resort: function (data, callback) {
            $http.post('/Admin/GroupUser/resort', data).success(callback);
        },
        getAll: function (callback) {
            $http.post('/Admin/GroupUser/getAll/').success(callback);
        },
        getGroupUser: function (data, callback) {
            $http.post('/Admin/GroupUser/GetTreeGroupUser', data).success(callback);
        },


        getUserOutGroup: function (data, callback) {
            $http.post('/Admin/GroupUser/GetUserOutGroup', data).success(callback);
        },
        getUserInGroup: function (data, callback) {
            $http.post('/Admin/GroupUser/GetUserInGroup', data).success(callback);
        },
        getGRole: function (callback) {
            $http.post('/Admin/GroupUser/GetGRole').success(callback);
        },
        updateGroupUser: function (data, callback) {
            $http.post('/Admin/GroupUser/UpdateGroupUserAsync', data).success(callback);
        },
        gettreedata: function (data, callback) {
            $http.post('/Admin/GroupUser/gettreedata/' + data).success(callback);
        },
        getListUserInGroup: function (data, callback) {
            $http.post('/Admin/GroupUser/GetListUserInGroup?deparment=' + data).success(callback);
        },
    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice, $cookies, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    var culture = $cookies.get('_CULTURE') || 'vi-VN';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess',
        function () {
            caption = caption[culture];
            $rootScope.checkData = function (data) {
                var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
                var partternName = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]+[^!@#$%^&*<>?]*$/; //Có chứa được khoảng trắng
                var partternDescription = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@#$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng
                var mess = { Status: false, Title: "" }
                if (!partternCode.test(data.GroupUserCode)) {
                    mess.Status = true;
                    mess.Title = mess.Title.concat(" - ", caption.VALIDATE_ITEM_CODE.replace('{0}', caption.G_USERS_CODE), "<br/>");
                }
                if (!partternName.test(data.Title)) {
                    mess.Status = true;
                    mess.Title += " - " + caption.VALIDATE_ITEM_NAME.replace('{0}', caption.G_USERS_NAME) + "<br/>";
                }
                if (!partternDescription.test(data.Description)) {
                    mess.Status = true;
                    mess.Title += " - " + caption.VALIDATE_ITEM.replace('{0}', caption.DESCRIPTION) + "<br/>";
                }
                return mess;
            }
            $rootScope.validationOptions = {
                rules: {
                    Title: {
                        required: true,
                        maxlength: 255
                    },
                    GroupUserCode: {
                        required: true,
                        maxlength: 50
                    },
                    Description: {
                        maxlength: 500
                    }
                },
                messages: {
                    GroupUserCode: {
                        required: caption.COM_ERR_REQUIRED.replace('{0}', caption.ADM_DEPARTMENT_CURD_LBL_DEPT_CODE),
                        maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_DEPARTMENT_CURD_LBL_DEPT_CODE).replace('{1}', '50')
                    },
                    Title: {
                        required: caption.COM_ERR_REQUIRED.replace('{0}', caption.ADM_DEPARTMENT_CURD_LBL_DEPT_NAME),
                        maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_DEPARTMENT_CURD_LBL_DEPT_NAME).replace('{1}', '255')
                    },
                    Description: {
                        maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_DEPARTMENT_CURD_LBL_DEPT_DESCIPTION).replace('{1}', '500')
                    }
                }
            }
            $rootScope.StatusData = [{
                Value: true,
                Name: caption.ADM_DEPARTMENT_LIST_LBL_DEPT_ACTIVE
            }, {
                Value: false,
                Name: caption.ADM_DEPARTMENT_LIST_LBL_DEPT_INACTIVE
            }];
        });


});
app.config(function ($routeProvider, $validatorProvider, $translateProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    //$translateProvider.preferredLanguage('en-US');
    caption = $translateProvider.translations();
    debugger
    $routeProvider
        .when('/',
            {
                templateUrl: ctxfolder + '/index.html',
                controller: 'index'
            })
        .when('/edit/:id',
            {
                templateUrl: ctxfolder + '/edit.html',
                controller: 'edit'
            })
        .when('/user/:id',
            {
                templateUrl: ctxfolder + '/user.html',
                controller: 'user'
            })
        .when('/add/',
            {
                templateUrl: ctxfolder + '/add.html',
                controller: 'add'
            });
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
app.controller('index', function ($scope, $rootScope, $compile, $confirm, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $translate) {

    var vm = $scope;
    $scope.model = {
        GroupName: '',
        GroupCode: '',
        Status: ''
    };
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.liGroupUser = [];
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/GroupUser/jtable",
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
                d.GroupCode = $scope.model.GroupCode;
                d.GroupName = $scope.model.GroupName;
                d.Status = $scope.model.Status;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [1, 'asc'])
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("_STT").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full._STT] = false;
        $scope.addId(full);
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full._STT + ']" ng-click="toggleOne(selected,$event)"/><span></span></label>';
    }).withOption('sWidth', '30px').withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Id').withTitle('ID').withOption('sWidth', '60px').notSortable().withOption('sClass', 'sorting_disabled hidden').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"ADM_DEPARTMENT_LIST_COL_DEPT_NAME" | translate}}').notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type, full, meta) {
        return data.replace(/. . . /g, '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;');
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Code').withTitle('{{"ADM_DEPARTMENT_LIST_COL_DEPT_CODE" | translate}}').notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Description').withTitle('{{"ADM_DEPARTMENT_LIST_COL_DEPT_DESCIPTION" | translate}}').notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('IsEnabled').withTitle('{{"ADM_DEPARTMENT_LIST_COL_DEPT_STATUS" | translate}}').notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type) {
        return data == "True" ? '<span class="text-success">{{"ADM_DEPARTMENT_LIST_LBL_DEPT_ACTIVE" | translate}}</span>' : '<span class="text-danger">{{"ADM_DEPARTMENT_LIST_LBL_DEPT_INACTIVE" | translate}}</span>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{"ADM_DEPARTMENT_LIST_COL_DEPT_ACTION" | translate}}').renderWith(function (data, type, full, meta) {
        return '<button title="Sửa" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    function toggleOne(selectedItems, evt) {
        $(evt.target).closest('tr').toggleClass('selected');
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
    $scope.addId = function (data) {
        for (var i = 0; i < $scope.liGroupUser.length; i++) {
            if ($scope.liGroupUser[i] == data.Id) {
                return;
            }
        }
        $scope.liGroupUser.push(data);
    }
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

    $scope.edit = function (id) {
        var userModel = {};
        var listdata = $('#tblData').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i].Id == id) {
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
    $scope.delete = function (id) {
        $confirm({ text: caption.MSG_DELETE_CONFIRM.replace('{0}', ''), title: caption.CONFIRM, ok: caption.CONFIRM_OK, cancel: caption.CONFIRM_CANCEL })
            .then(function () {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
                dataservice.delete(id, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    } else {
                        App.toastrSuccess(rs.Title);
                        $scope.reloadNoResetPage();
                    }
                });
                App.unblockUI("#contentMain");
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
    }, 50);
});

app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice) {
    $scope.model = {
        ParentId: null
    };
    $scope.loadData = function () {
        dataservice.gettreedata(null, function (result) {
            $scope.treeData = result;
        });
    }
    $scope.loadData();

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
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
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        debugger
        dataservice.getItem(para.Id, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                dataservice.getListUserInGroup(para.Code, function (rs) {
                    debugger
                    $scope.listUserInGroupData = rs;
                });
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
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});

