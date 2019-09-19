var ctxfolder = "/views/admin/resource";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose"
    }
    return {
        insert: function (data, callback) {
            $http.post('/Admin/Resource/Insert', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/Resource/Update', data).success(callback);
        },
        deleteItems: function (data, callback) {
            $http.post('/Admin/Resource/DeleteItems', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/Resource/delete', data).success(callback);
        },
        gitem: function (data, callback) {
            $http.post('/Admin/Resource/Getitem', data).success(callback);
        },
        //getall: function (data, callback) {
        //    $http.post('/Admin/Resource/getall', data).success(callback);
        //},
        getByName: function (data, callback) {
            $http.get('/Admin/Resource/GetByNameAddParent/?name=' + data).success(callback);
        },
        resort: function (data, callback) {
            $http.post('/Admin/Resource/Resort', data).success(callback);
        },
        gettreedata: function (data, callback) {
            $http.post('/Admin/Resource/Gettreedata', data).success(callback);
        },
        getbyparent: function (data, callback) {
            $http.post('/Admin/Resource/Getbyparent/', data).success(callback);
        },
        getGroupResource: function (callback) {
            $http.get('/Admin/Resource/GetGroupResource').success(callback);
        },
        insertResAttribute: function (data, callback) {
            $http.post('/Admin/Resource/InsertResAttribute', data).success(callback);
        },
        updateResAttribute: function (data, callback) {
            $http.post('/Admin/Resource/UpdateResAttribute', data).success(callback);
        },
        deleteResAttribute: function (data, callback) {
            $http.post('/Admin/Resource/DeleteResAttribute', data).success(callback);
        },
        getResAttribute: function (data, callback) {
            $http.post('/Admin/Resource/GetResAttribute', data).success(callback);
        },
        getTreeFunction: function (data, callback) {
            $http.post('/Admin/Resource/GetTreeFunction', data).success(callback);
        },
        //getTreeFunctionData: function (data, callback) {
        //    $http.post('/Admin/Resource/GetTreeFunctionData', data).success(callback);
        //},
        addPrivilege: function (data, callback) {
            $http.post('/Admin/Resource/AddPrivilege', data).success(callback);
        },
        deletePrivilege: function (data, callback) {
            $http.post('/Admin/Resource/DeletePrivilege/', data).success(callback);
        },
        GetFunctionItem: function (callback) {
            $http.post('/Admin/Resource/GetTreeFunctionData').success(callback);
        }
    }
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, $confirm, DTColumnBuilder, DTInstances, dataservice, $cookies, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    var culture = $cookies.get('_CULTURE') || 'en-US';
    $translate.use(culture);

    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
        $.extend($.validator.messages, {
            min: caption.COM_VALIDATE_VALUE_MIN,
            //max: 'Max some message {0}'
        });
        $rootScope.checkData = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
            var partternName = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]+[^!@#$%^&*<>?]*$/; //Có chứa được khoảng trắng
            var partternDescription = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@#$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.Code)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace('{0}', caption.ADM_RESOURCE_CURD_LBL_RESOURCE_CODE), "<br/>");
            }
            if (!partternName.test(data.Title)) {
                mess.Status = true;
                mess.Title += " - " + caption.COM_VALIDATE_ITEM_NAME.replace('{0}', caption.ADM_RESOURCE_CURD_LBL_RESOURCE_NAME) + "<br/>";
            }
            if (!partternDescription.test(data.Description)) {
                mess.Status = true;
                mess.Title += " - " + caption.COM_VALIDATE_ITEM.replace('{0}', caption.DESCRIPTION) + "<br/>";
            }
            return mess;
        }
        $rootScope.validationOptions = {

            rules: {
                Title: {
                    required: true,
                    maxlength: 255
                },
                ResourceCode: {
                    required: true,
                    maxlength: 50
                },
                Ord: {
                    required: true
                },
                Path: {
                    required: true,
                    maxlength: 255
                },
                Api: {
                    required: true,
                    maxlength: 255
                },
                Description: {
                    maxlength: 2000
                }
                //GroupResourceId: {
                //    required: true,
                //    maxlength: 50
                //}
            },
            messages: {
                Title: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_NAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_NAME).replace("{1}", "255")
                },
                ResourceCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_CODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_CODE).replace("{1}", "50")
                },
                Ord: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_ORDER)
                },
                Path: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_URL),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_URL).replace("{1}", "255")
                },
                Api: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_API),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.ADM_RESOURCE_CURD_LBL_RESOURCE_API).replace("{1}", "255")
                },
                Description: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace('{0}', caption.ADM_RESOURCE_CURD_LBL_RESOURCE_DESCRIPTION).replace('{1}', '2000')
                }
                //GroupResourceId: {
                //    required: "Yêu cầu chọn nhóm tài nguyên.",
                //    maxlength: "Nhóm tài nguyên không vượt quá 50 ký tự."
                //}
            }
        }
    });
});
app.config(function ($routeProvider, $validatorProvider, $translateProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
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
app.controller('index', function ($scope, $rootScope, $confirm, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice) {



    var vm = $scope;
    //vm.FromDate = '';
    //vm.ToDate = '';
    $scope.model = {
        Name: '',
        Code: '',
        Api: '',
        Function: '',
        Description: '',
        FromDate: '',
        ToDate: ''
    };
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Resource/Jtable",
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
                d.Name = $scope.model.Name;
                d.Code = $scope.model.Code;
                d.Api = $scope.model.Api;
                d.Function = $scope.model.Function;
                d.Description = $scope.model.Description;
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
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
        //.withOption('scrollX', '100%')
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
            return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected, $event)"/><span></span></label>';
        }).withOption('sWidth', '30px').withOption('sClass', ' sorting_disabled'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('_STT').withTitle('#').withOption('sWidth', '30px').notSortable().withOption('sClass', 'sorting_disabled hidden').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Code').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_CODE" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_NAME" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Api').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_API" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Path').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_URL" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Style').withTitle('{{"Style" | translate}}').renderWith(function (data, type) {
    //  return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Scope').withTitle('{{"Scope" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
    //    if (data == "True") return "Private";
    //    return "Public";
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Description').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_DESCRIPTION" | translate}}')/*.notSortable()*/.renderWith(function (data, type) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('CreateDate').withTitle('{{"CREATED_DATE" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Function').withTitle('{{"ADM_FUNC_TITLE_FUNC" | translate}}').notSortable().withOption('sClass', 'w250').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Status').withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_STATUS" | translate}}')/*.notSortable().withOption('sClass', 'sorting_disabled')*/.renderWith(function (data, type) {
        return data == "True" ? '<span class="text-success">' + caption.ACTIVE + ' </span>' : '<span class="text-danger">' + caption.INACTIVE + '</span>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"ADM_RESOURCE_LIST_COL_RESOURCE_ACTION" | translate}}').renderWith(function (data, type, full, meta) {
        return '<button title="Sửa" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Ord').withTitle('{{"ORDER_SORT" | translate}}').withOption('sWidth', '60px').notSortable().withOption('sClass', 'sorting_disabled').withOption('sClass', 'tcenter'));

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
        reloadData(true);
    }
    $scope.reload = function () {
        reloadData(true);
    }
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    };
    $rootScope.reload = function () {
        $scope.reload();
    }

    $rootScope.rootreload = function () {
        $scope.reload();
    }

    $scope.deleteChecked = function () {
        var deleteItems = [];
        for (var id in $scope.selected) {
            if ($scope.selected.hasOwnProperty(id)) {
                if ($scope.selected[id]) {
                    deleteItems.push(id);
                }
            }
        }
        if (deleteItems.length > 0) {
            $confirm({ text: caption.MSG_DELETE_LIST_CONFIRM.replace('{0}', caption.ADM_RESOURCE_LBL_RESOURCE), title: caption.CONFIRM, ok: caption.CONFIRM_OK, cancel: caption.CONFIRM_CANCEL })
                .then(function () {
                    dataservice.deleteItems(deleteItems, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                            $scope.reload();
                        } else {
                            App.toastrSuccess(result.Title);
                            $scope.reloadNoResetPage();
                        }
                        App.unblockUI("#contentMain");
                    });

                });
        } else {
            App.toastrError(caption.COM_ERR_NOT_CHECKED.replace('{0}', caption.ADM_RESOURCE_LBL_RESOURCE));
        }
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit',
            backdrop: 'static',
            size: '50',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reloadNoResetPage();
        }, function () {});
    }


    $scope.contextMenu = [
        //[function ($itemScope) {
        //    return '<i class="fa fa-edit"></i>' + caption.EDIT;
        //}, function ($itemScope, $event, model) {
        //    var modalInstance = $uibModal.open({
        //        animation: true,
        //        templateUrl: ctxfolder + '/edit.html',
        //        controller: 'edit',
        //        backdrop: 'static',
        //        size: 'lg',
        //        resolve: {
        //            para: function () {
        //                return $itemScope.data.Id;
        //            }
        //        }
        //    });
        //    modalInstance.result.then(function (d) {
        //        $scope.reload();
        //    }, function () {
        //    });
        //}, function ($itemScope, $event, model) {
        //    return true;
        //}],
        //[function ($itemScope) {
        //    return '<i class="fa fa-remove"></i>' + caption.DELETE;
        //}, function ($itemScope, $event, model) {

        //    $confirm({ text: caption.MSG_DEL_CONFIRM_WITH_NAME.replace('{0}', caption.ADM_RESOURCE_LBL_RESOURCE).replace('{1}', $itemScope.data.Title), title: caption.CONFIRM, cancel: caption.CONFIRM_CANCEL })
        //        .then(function () {
        //            dataservice.delete($itemScope.data.Id, function (result) {
        //                if (result.Error) {
        //                    App.toastrError(result.Title);
        //                } else {
        //                    App.toastrSuccess(result.Title);
        //                    $scope.reload();
        //                }
        //                App.unblockUI("#contentMain");
        //            });
        //        });
        //}, function ($itemScope, $event, model) {
        //    return true;
        //}],
        [function ($itemScope) {
            return '<i class="fa fa-sitemap"></i> ' + caption.FUNCTION_MANAGEMENT;
        }, function ($itemScope, $event, model) {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: ctxfolder + '/function.html',
                controller: 'function',
                backdrop: 'static',
                size: 'lg',
                resolve: {
                    para: function () {
                        return $itemScope.data;
                    }
                }
            });
            modalInstance.result.then(function (d) {
            }, function () {
            });
        }, function ($itemScope, $event, model) {
            return true;
        }],
        //[function ($itemScope) {
        //    return '<i class="fa fa-list"></i> ' + caption.ATTRIBUTE_MANAGEMENT;
        //}, function ($itemScope, $event, model) {
        //    var modalInstance = $uibModal.open({
        //        animation: true,
        //        templateUrl: ctxfolder + '/manager.html',
        //        controller: 'manager',
        //        backdrop: 'static',
        //        size: 'lg',
        //        resolve: {
        //            para: function () {
        //                return $itemScope.data.Id;

        //            }
        //        }
        //    });
        //    modalInstance.result.then(function (d) {
        //        $scope.reload();
        //    }, function () {
        //    });
        //}, function ($itemScope, $event, model) {
        //    return true;
        //}]
    ];
    $(document).ready(function () {
        $('#FromDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#ToDate').datepicker('setStartDate', minDate);
        });
        $('#ToDate').datepicker({
            format: "dd/mm/yyyy",
            autoclose: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#FromDate').datepicker('setEndDate', maxDate);
        });
        $('.end-date_resoure').click(function () {
            $('#FromDate').datepicker('setEndDate', null);
        });
        $('.start-date_resoure').click(function () {
            $('#ToDate').datepicker('setStartDate', null);
        });

    });



    $scope.add = function (p) {
        $scope.model.Id = 1;
        if (p === null && $scope.model.Id === undefined) {
            App.toastrError(caption.COM_ERR_SELECT_REQUIRED.replace('{0}', caption.ADM_RESOURCE_LBL_RESOURCE));
            return;
        }
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: 'static',
            size: '50',
            resolve: {
                para: function () {
                    if (p === null) {
                        return { check: true, value: $scope.model.Id };
                    }
                    else {
                        return { check: false, value: p }
                    }
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.changeGroup();
        }, function () {
        });
    }

    $scope.sort = function (p) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/sort.html',
            controller: 'sort',
            backdrop: 'static',
            size: '60',
            resolve: {
                para: function () {
                    return { check: true, valueG: (($scope.model.Id === undefined) ? 0 : $scope.model.Id), valueO: p };
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.changeGroup();
        }, function () {
        });
    }
    $scope.manager = function (p) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/manager.html',
            controller: 'manager',
            backdrop: 'static',
            size: '60',
            resolve: {
                para: function () {
                    return p;
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.changeGroup();
        }, function () {
        });
    }
    //$scope.function = function (p) {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/function.html',
    //        controller: 'function',
    //        backdrop: 'static',
    //        size: 'lg',
    //        resolve: {
    //            para: function () {
    //                return p;

    //            }
    //        }
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.changeGroup();
    //    }, function () {
    //    });
    //}
    //#endregion
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
                        $rootScope.reload();
                        //$scope.changeGroup();
                    }
                });
                App.unblockUI("#contentMain");
            });
    }


    //$scope.edit = function (temp) {
    //    var modalInstance = $uibModal.open({
    //        animation: true,
    //        templateUrl: ctxfolder + '/edit.html',
    //        controller: 'edit',
    //        backdrop: 'static',
    //        size: 'lg',
    //        resolve: {
    //            para: function () {
    //                return temp.Id;
    //            }
    //        }
    //    });
    //    modalInstance.result.then(function (d) {
    //        $scope.changeGroup();
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
    }, 50);
});
app.controller('add', function ($scope, $rootScope, dataservice, $uibModal, $uibModalInstance, para, $timeout) {
    $scope.model = {};
    $scope.model.ParentId = null;
    //console.log($scope.model.ParentId);
    //$scope.loadData = function () {
    //    dataservice.gettreedata({ IdS: [$rootScope.selectGResourceId] }, function (result) {
    //        $scope.treeData = result;
    //        if (para.check) {
    //            $scope.model.GroupResourceId = para.value;
    //        } else {
    //            dataservice.gitem(para.value, function (rs) {
    //                $scope.model.GroupResourceId = rs.GroupResourceId;
    //            });
    //        }
    //    });
    //    dataservice.getGroupResource(function (result) {
    //        $scope.GroupResource = result;
    //    });
    //}
    //$scope.loadData();

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
                    $rootScope.reload();
                }
            });
        }
    }

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});

app.controller('sort', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para, $timeout) {

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.initData = function () {
        dataservice.getbyparent({ IdI: [para.valueO], IdS: [para.valueG] }, function (rs) {
            $scope.model = rs;
        });
    }
    $scope.initData();
    $scope.resort = function (item, index) {
        $scope.model.splice(index, 1);
        $scope.model.splice(item.Ord - 1, 0, item);
        $.each($scope.model, function (index, item) {
            item.Ord = index + 1;
        });
    }
    $scope.submit = function () {
        dataservice.resort($scope.model, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                App.toastrSuccess(rs.Title);
                $uibModalInstance.close();
            }
        });
    }
});

app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para, $timeout) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        dataservice.gitem(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                //dataservice.gettreedata({ IdS: [$rootScope.selectGResourceId], IdI: [rs.Id] }, function (result) {
                //    $scope.treeData = result;
                //});
                //dataservice.getGroupResource(function (result) {
                //    $scope.GroupResource = result;
                //});
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
            //console.log($scope.model);
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

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});

app.controller('manager', function ($scope, $rootScope, $compile, $uibModal, $confirm, $uibModalInstance, dataservice, para, $timeout) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.liAttributesDel = [];
    $scope.liAttributesUp = [];
    $scope.liAttributes = [];
    $scope.liAttributestemp = [];
    $scope.addAtt = function () {
        $scope.itemp = {
            Id: 0,
            Key: $scope.model.Key,
            Value: $scope.model.Value,
            ResourceId: para.Id,
            Check: 1,
        };
        $scope.liAttributestemp.push($scope.itemp);
        $scope.itemp = {};
    }
    $scope.loadData = function () {
        dataservice.getResAttribute(para.Id, function (rs) {
            for (var i = 0; i < rs.length; i++) {
                rs[i].Check = 0;
            }
            $scope.liAttributestemp = rs;
        });
    }

    $scope.itemChange = function (data) {
        if ($scope.liAttributestemp[data].Check !== 1) {
            $scope.liAttributestemp[data].Check = 2;
        }
    }
    $scope.loadData();
    $scope.itemRemove = function (data) {
        if ($scope.liAttributestemp[data].Check === 0) {
            $scope.liAttributesDel.push($scope.liAttributestemp[data]);
        }
        $scope.liAttributestemp.splice(data, 1);
    }
    $scope.submit = function () {
        for (var i = 0; i < $scope.liAttributestemp.length; i++) {
            if ($scope.liAttributestemp[i].Check === 1) {
                $scope.liAttributes.push($scope.liAttributestemp[i]);
            }
            if ($scope.liAttributestemp[i].Check === 2) {
                $scope.liAttributesUp.push($scope.liAttributestemp[i]);
            }
        }
        dataservice.deleteResAttribute($scope.liAttributesDel, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                dataservice.updateResAttribute($scope.liAttributesUp, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    } else {
                        dataservice.insertResAttribute($scope.liAttributes, function (rs) {
                            if (rs.Error) {
                                App.toastrError(rs.Title);
                            } else {
                                App.toastrSuccess(rs.Title);
                                $uibModalInstance.close();
                            }
                        });
                    }
                });
            }
        });
    }

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);
});

app.controller('function', function ($scope, $rootScope, $confirm, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $uibModalInstance, para, $timeout) {
    $scope.Api = para.Api;
    var vm = $scope;
    $scope.model = {
        Id: para.Id,
        ResourceCode: para.Code
    };

    $scope.selected = [];
    $scope.ListResFunctionDefault = [];
    $scope.ListResFunction = [];
    $scope.ListFunctionAdd = [];
    $scope.ListFunctionDel = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Resource/JTableFunctionByResourceId",
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
                d.Id = $scope.model.Id;
                d.Code = $scope.model.ResourceCode;
            },
            complete: function (rs) {
                $scope.ListResFunction = rs.responseJSON.data;
                $scope.ListResFunctionDefault = angular.copy(rs.responseJSON.data);

                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        //.withOption('order', [1, 'asc'])
        .withOption('serverSide', false)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            //const contextScope = $scope.$new(true);
            //contextScope.data = data;
            //contextScope.contextMenu = $scope.contextMenu;
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
            $compile(angular.element(row).contents())($scope);
            $compile(angular.element(row).find('input'))($scope);
            //$compile(angular.element(row).find('button'))($scope);
        });

    vm.dtColumns = [];
    //vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable()
    //    .renderWith(function (data, type, full, meta) {
    //        $scope.selected[full.Id] = false;
    //        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //    }).withOption('sWidth', '30px').withOption('sClass', 'tcenter'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('_STT').withTitle('#').notSortable().withOption('sClass', 'sorting_disabled hidden').withOption('sWidth', '60px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle(caption.FUNC_NAME).notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type, full, meta) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('FunctionCode').withTitle(caption.FUNC_CODE).notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type, full, meta) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('ParentId').withTitle('ParentId').renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Description').withTitle(caption.DESCRIPTION).notSortable().withOption('sClass', 'sorting_disabled').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn("FunctionCode").withTitle("").notSortable().renderWith(function (data, type, full, meta) {
        //$scope.selected[full.Id] = false;
        //return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
        return "<button class='btn btn-icon-only btn-round  btn-danger' ng-click='removeResFunction(\"" + data + "\")'><i class='fa fa-times white'></i></button>";
    }).withOption('sWidth', '30px').withOption('sClass', 'tcenter'));

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
    $scope.reload = function () {
        reloadData(true);
        $scope.loadData();
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
        $rootScope.rootreload();
    }
    $scope.loadData = function () {
        dataservice.GetFunctionItem(function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.liFunction = rs;
            }
        });
    }
    $scope.loadData();

    $scope.addResFunction = function () {
        if ($scope.model.ResourceCode != null && $scope.model.Function != undefined) {
            var index = $scope.ListResFunction.map(function (e) { return e.FunctionCode; }).indexOf($scope.model.Function.Code);
            if (index > -1) {
                App.toastrError(caption.COM_ERR_EXIST.replace('{0}', caption.ADM_FUNC_TITLE_FUNC));
            } else {
                index = $scope.ListResFunction.map(function (e) { return e.FunctionCode; }).indexOf($scope.model.Function.ParentCode);
                //if ($scope.model.Function.ParentCode == null || index > -1) {
                // Define the row to insert (using your method of choice)
                var rowInsert = {
                    "Id": $scope.model.Function.Id,
                    "_STT": "1",
                    //"Title": $scope.model.Function.Title.replace(/- /g, '. . . '),
                    "Title": $scope.model.Function.Title,
                    "FunctionCode": $scope.model.Function.Code,
                    "Description": $scope.model.Function.Description,
                    "ParentCode": $scope.model.Function.ParentCode,
                    "Ord": "0"
                };

                $scope.ListResFunction.splice(index + 1, 0, rowInsert);
                var indexAdd = $scope.ListResFunctionDefault.map(function (e) { return e.FunctionCode; }).indexOf($scope.model.Function.Code);
                if (indexAdd < 0) {
                    $scope.ListFunctionAdd.push($scope.model.Function.Code);
                }
                var indexDel = $scope.ListFunctionDel.map(function (e) { return e; }).indexOf($scope.model.Function.Code);
                if (indexDel > -1) {
                    $scope.ListFunctionDel.splice(indexDel, 1);
                }

                // Get table reference - note: dataTable() not DataTable()
                var table = $('#tblData').dataTable();
                // Get api
                var dt = table.api();
                // Insert row (inserted as the last element in aiDisplayMaster array)
                dt.row.add(rowInsert);
                // Get the array holding the rows
                var aiDisplayMaster = table.fnSettings()['aiDisplayMaster'];
                // Remove the last element in the array
                var moveRow = aiDisplayMaster.pop();
                // EITHER add row to the beginning of the array (uncomment)
                //aiDisplayMaster.unshift(moveRow);
                // OR add row to a specific index (in this case to index 3)
                aiDisplayMaster.splice(index + 1, 0, moveRow);
                // Redraw Table
                dt.draw();
                //} else {
                //    App.toastrError(caption.MSG_ADD_PARENT_FIRST.replace('{0}', caption.ADM_FUNC_TITLE_FUNC));
                //}
            }
        } else {
            App.toastrError(caption.COM_ERR_NOT_CHECKED.replace('{0}', caption.ADM_FUNC_TITLE_FUNC));
        }
    }
    $scope.refreshResFunction = function () {
        $scope.model.Function = undefined;
        $scope.ListFunctionAdd = [];
        $scope.ListFunctionDel = [];
        $scope.reload();
    }
    $scope.removeResFunction = function (temp) {
        if ($scope.ListResFunction.length > 0) {
            var index = $scope.ListResFunction.map(function (e) { return e.FunctionCode; }).indexOf(temp);
            if (index > -1) {
                var indexChild = $scope.ListResFunction.map(function (e) { return e.ParentCode; }).indexOf($scope.ListResFunction[index].Code);
                if (indexChild > -1) {
                    App.toastrError(caption.MSG_DELETE_CHILD.replace('{0}', caption.ADM_FUNC_TITLE_FUNC));
                } else {
                    var indexDel = $scope.ListResFunctionDefault.map(function (e) { return e.FunctionCode; }).indexOf(temp);
                    if (indexDel > -1) {
                        $scope.ListFunctionDel.push(temp);
                    }
                    var indexAdd = $scope.ListFunctionAdd.map(function (e) { return e; }).indexOf(temp);
                    if (indexAdd > -1) {
                        $scope.ListFunctionAdd.splice(indexAdd, 1);
                    }
                    $scope.ListResFunction.splice(index, 1);

                    var table = $('#tblData').dataTable();
                    var row = table.find('tr').eq(index + 1);
                    table.fnDeleteRow(row[0]);
                }
            }
        }
    }
    $scope.saveResFunction = function () {
        $scope.model.ResourceCode = para.Code;
        if ($scope.model.ResourceCode != null) {
            if ($scope.ListFunctionAdd.length > 0 || $scope.ListFunctionDel.length > 0) {
                var modelUpdate = {
                    ResourceCode: $scope.model.ResourceCode,
                    FunctionAdd: $scope.ListFunctionAdd,
                    FunctionDel: $scope.ListFunctionDel
                }
                dataservice.addPrivilege(modelUpdate, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    } else {
                        $scope.model.Function = undefined;
                        $scope.reload();
                        App.toastrSuccess(rs.Title);
                    }
                });
            } else {
                App.toastrError('Please add new function or delete old function to Resource!');
            }
        } else {
            App.toastrError('Resource is not exists!');
        }
    }

    $timeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 100);

    //$scope.model = {
    //    ResourceId: "",
    //    FunctionId : undefined,
    //}
    //$scope.cancel = function () {
    //    $uibModalInstance.dismiss('cancel');
    //}
    //$scope.initData = function () {
    //    //alert(JSON.stringify(para.Id));
    //    dataservice.getTreeFunction(para.Id, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        } else {
    //            $scope.tree_data = buildJtree(rs);
    //            dataservice.getTreeFunctionData(para.Id, function (result) {
    //                $scope.treeData = result;
    //            });
    //        }
    //    });

    //}
    //$scope.initData();
    //$scope.tree_data = [];
    //$scope.my_tree = tree = {};
    //function buildJtree(data) {
    //    var tree = [];
    //    if (!data || data.length === 0) return [];
    //    $.each(data, function (index, item) {
    //        if (!item.ParentId) {
    //            var treeObjs = item;
    //            var sub = subJtree(data, item.Id);
    //            if (sub.length > 0) {
    //                treeObjs.children = sub;
    //            }
    //            treeObjs.expanded = true;
    //            tree.push(treeObjs);
    //        }
    //    });
    //    return tree;
    //}
    //function subJtree(data, parentVal) {
    //    var subTree = [];
    //    $.each(data, function (index, item) {
    //        if (item.ParentId && item.ParentId === parentVal) {
    //            var treeObjs = item;
    //            var sub = subJtree(data, item.Id);
    //            if (sub.length > 0) {
    //                treeObjs.children = sub;
    //            }
    //            treeObjs.expanded = true;
    //            subTree.push(treeObjs);
    //        }
    //    });
    //    return subTree;
    //}
    //$scope.my_tree_handler = function (branch) {
    //    $rootScope.selectedItem = branch;
    //}
    //$scope.expanding_property = {
    //    field: "Title",
    //    width: "200px",
    //    displayName: caption.FUNC_NAME,
    //    sortable: true,
    //    filterable: true
    //};
    //var contextMenu = [];
    //contextMenu.push("<li  ><a  ng-click='cellTemplateScope.deletePrivilege(row.branch)' title='" + '{{"DELETE" | translate }}' + "'><i class='fa fa-remove'></i> " + '{{"DELETE" | translate }}' + "</a></li>");
    //$scope.col_defs = [
    //    {
    //        field: "Code", displayName: caption.FUNC_CODE, width: "150px",
    //        sortable: tree,
    //        cellTemplate: "<span>{{row.branch['Code']}}</span>",
    //    },
    //    {
    //        field: "Description", displayName: caption.DESCRIPTION, width: "250px",
    //        sortable: tree,
    //        cellTemplate: "<span>{{row.branch['Description']}}</span>",
    //    },
    //    {
    //        field: "",
    //        sortable: false, width: '45px',
    //        displayName: "",
    //        colclass: "action",
    //        cellTemplate: "<div class='btn-group  '>"
    //            + "<a class='btn btn-icon-only btn-rectange btn-danger' ng-click='cellTemplateScope.deletePrivilege(row.branch)'><i class='fa fa-times'></i></a>"
    //            + "</div>",
    //        cellTemplateScope: {
    //            deletePrivilege: function (data) {

    //                $scope.delete(data);

    //            }
    //        }
    //    }
    //    //{
    //    //    field: "",
    //    //    sortable: false, width: '45px',
    //    //    displayName: "",
    //    //    colclass: "action",
    //    //    cellTemplate: "<div class='btn-group  '>"
    //    //    + "<a   class='btn btn-icon-xs btn-icon-only ' data-toggle='dropdown'  ><i class='fa fa-list-ul'></i></a>"
    //    //    + "<ul class='dropdown-menu  pull-right' >"
    //    //    + contextMenu.join("")
    //    //    + "</ul>"
    //    //    + "</div>",
    //    //    cellTemplateScope: {
    //    //        deletePrivilege: function (data) {

    //    //            $scope.delete(data);

    //    //        }
    //    //    }
    //    //}
    //];

    //$scope.addFuntion = function () {
    //    $scope.model.ResourceId = para.Id;
    //    dataservice.addFunction($scope.model, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        } else {
    //            $scope.initData();
    //            $scope.model.FunctionId = undefined;
    //            App.toastrSuccess(rs.Title);
    //        }
    //    });
    //}
    //$scope.addFunction = function () {
    //    $scope.model.ResourceCode = para.Code;
    //    if ($scope.model.ResourceCode != null && $scope.model.FunctionCode != undefined) {
    //        dataservice.addPrivilege($scope.model, function (rs) {
    //            if (rs.Error) {
    //                App.toastrError(rs.Title);
    //            } else {
    //                $scope.model.FunctionCode = undefined;
    //                $scope.reload();
    //                App.toastrSuccess(rs.Title);
    //            }
    //        });
    //    } else {
    //        App.toastrError(caption.COM_ERR_NOT_CHECKED.replace('{0}', caption.ADM_FUNC_TITLE_FUNC));
    //    }
    //}
    //$scope.deleteFunction = function (temp) {
    //    $scope.model.ResourceCode = para.Code;
    //    $scope.model.FunctionCode = temp;
    //    $confirm({ text: caption.MSG_DELETE_CONFIRM.replace('{0}', caption.ADM_FUNC_TITLE_FUNC), title: caption.CONFIRM, ok: caption.CONFIRM_OK, cancel: caption.CONFIRM_CANCEL })
    //        .then(function () {
    //            dataservice.deletePrivilege($scope.model, function (rs) {
    //                if (rs.Error) {
    //                    App.toastrError(rs.Title);
    //                } else {
    //                    App.toastrSuccess(rs.Title);
    //                    $scope.reload();
    //                }
    //            });
    //        });
    //}
});
