var ctxfolder = "/views/admin/task010";
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

    var submitFormUpload = function (url, data, callback) {
        debugger
        var config = {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        };

        var formData = new FormData();

        formData.append("id", data.id);
        formData.append("name", data.name);
   
        formData.append("priority", data.priority);
        formData.append("note", data.note);
        formData.append("cat_code", data.cat_code);
        formData.append("picture", data.picture != null && data.picture.length > 0 ? data.picture[0] : null);

        var req = {
            method: 'POST',
            url: url,
            headers: {
                'Content-Type': undefined
            },
            data: formData
        }
        $http(req).success(callback);
    };


    return {
        insert: function (data, callback) {
            submitFormUpload('/Admin/task010/Insert', data, callback);
        },
        update: function (data, callback) {
            submitFormUpload('/Admin/task010/Update', data, callback);
        },


        //insert: function (data, callback) {
        //    $http.post('/Admin/task010/insert', data).success(callback);
        //},
        //update: function (data, callback) {
        //    $http.post('/Admin/task010/update', data).success(callback);
        //},
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        delete: function (data, callback) {
            $http.post('/Admin/task010/delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/task010/getitem/' + data).success(callback);
        },
        
        //gettreedataCoursetype: function(callback) {
        //    $http.post('/Admin/task010/gettreedataCoursetype/').success(callback);
        //},
        //gettreedataLevel: function (callback) {
        //    $http.post('/task002/gettreedataLevel/').success(callback);
        //},
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };

    $rootScope.validationOptions = {
        rules: {
            name: {
                required: true,
                maxlength: 50
            },
            //course_name: {
            //    required: true,
            //    maxlength: 500
            //},
            //price: {
            //    required: true,
            //    maxlength: 500
            //},
        },
        messages: {
            name: {
                required: "Nhập tên xếp hạng",
                maxlength: "Tên k vượt quá 250 kí tự"
            },
            //course_name: {
            //    required: "Nhập tên trình độ",
            //    maxlength: "Tên khu vực k vượt quá 500 kí tự"
            //},
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
    $scope.initData = function () {
        debugger
        //dataservice.gettreedataCoursetype(function (result) {
        //    $scope.treedataCoursetype = result.Object;
        //});
        //dataservice.gettreedataLevel(function (result) {
        //    $scope.treedataLevel = result.Object;
        //});
        
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
            url: "/Admin/task010/jtable",
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


    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('Tên').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('picture').withTitle('Hình ảnh').renderWith(function (data, type) {
        return '<img src="' + data + '" class="img-responsive " style="height:40px;width:200px">';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('Ghi chú').renderWith(function (data, type) {
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
            return '<i class="fa fa-edit"></i>Cập nhật trình độ ';
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


    $scope.clear = function () {
        angular.element("input[type='file']").val(null);
    };
    $scope.start_Filter = function () {
        debugger
        // This is in the upload function post all your logic
        $(":file").val('');
    };


    $scope.submit = function () {
        debugger
        if ($scope.addform.validate()) {
            debugger
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            console.log('Name File: ' + extFile);
            if (extFile != "") {
                if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                    App.toastrError("Format required is png, jpg, jpeg, gif, bmp!");
                } else {
                    var fi = document.getElementById('file');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 1024) {
                        App.toastrError("Maximum allowed file size is 1 MB !");
                    } else {
                        var fileUpload = $("#file")[0];
                        var reader = new FileReader();
                        reader.readAsDataURL(fileUpload.files[0]);
                        reader.onload = function (e) {
                            //Initiate the JavaScript Image object.
                            var image = new Image();
                            //Set the Base64 string return from FileReader as source.
                            image.src = e.target.result;
                            image.onload = function () {
                                //Determine the Height and Width.
                                var height = this.height;
                                var width = this.width;
                                if (width > 1000 || height > 1000) {
                                    App.toastrError("Maximum allowed file size is (1000px x 1000px)!");
                                } else {
                                    console.log('Click')

                                    dataservice.insert($scope.model, function (rs) {
                                        if (rs.Error) {
                                            App.notifyDanger(rs.Title);
                                        } else {
                                            App.notifyInfo(rs.Title);
                                            $scope.reload();
                                            $scope.model.id = null;
                                            $scope.model.name = null;
                                            $scope.model.picture = null;
                                            $scope.model.priority = null;
                                            $scope.model.note = null;
                                            $scope.model.file = null;
                                            $('#Image').attr('src', null);
                                            $(":file").filestyle('clear');
                                           
                                            $scope.start_Filter();
                                            console.log($scope.model)
                                            console.log($('input[type=file]').val().stringify);
                                        }
                                    });
                                }
                            };
                          
                        }
                    }
                }
            } else {
                console.log('Click else')


                dataservice.insert($scope.model, function (rs) {
                    if (rs.Error) {
                        App.notifyDanger(rs.Title);
                    } else {
                        App.notifyInfo(rs.Title);
                        $scope.reload();
                        $scope.model.id = null;
                        $scope.model.name = null;
                        $scope.model.picture = null;
                        $scope.model.priority = null;
                        $scope.model.note = null;
                        $scope.model.file = null;
                        $(":file").filestyle('clear');
                        console.log($('input[type=file]').val());
                    }
                });
            }
           
        }
        $scope.enableedit = false;
        $scope.enableadd = true;
        $scope.show = true;
        $scope.hide = false;
      
    }

    $scope.edit = function () {
        if ($scope.addform.validate()) {
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            console.log('Name File: ' + extFile);
            if (extFile != "") {
                if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                    App.toastrError("Format required is png, jpg, jpeg, gif, bmp!");
                } else {
                    var fi = document.getElementById('file');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 1024) {
                        App.toastrError("Maximum allowed file size is 1 MB !");
                    } else {
                        var fileUpload = $("#file")[0];
                        var reader = new FileReader();
                        reader.readAsDataURL(fileUpload.files[0]);
                        reader.onload = function (e) {
                            //Initiate the JavaScript Image object.
                            var image = new Image();
                            //Set the Base64 string return from FileReader as source.
                            image.src = e.target.result;
                            image.onload = function () {
                                //Determine the Height and Width.
                                var height = this.height;
                                var width = this.width;
                                if (width > 1000 || height > 1000) {
                                    App.toastrError("Maximum allowed file size is (1000px x 1000px)!");
                                } else {
                                    console.log('Click')

                                    dataservice.update($scope.model, function (rs) {
                                        if (rs.Error) {
                                            App.notifyDanger(rs.Title);
                                        } else {
                                            App.notifyInfo(rs.Title);
                                            $scope.reload();
                                            $scope.model.id = null;
                                            $scope.model.name = null;
                                            $scope.model.picture = null;
                                            $scope.model.priority = null;
                                            $scope.model.note = null;
                                            $('#Image1').attr('src', null);
                                            $(":file").filestyle('clear');
                                            $scope.start_Filter();
                                        }
                                    });
                                }
                            };
                        }
                    }
                }
            } else {
                console.log('Click else')


                dataservice.update($scope.model, function (rs) {
                    if (rs.Error) {
                        App.notifyDanger(rs.Title);
                    } else {
                        App.notifyInfo(rs.Title);
                        $scope.reload();
                        $scope.model.id = null;
                        $scope.model.name = null;
                        $scope.model.picture = null;
                        $scope.model.priority = null;
                        $scope.model.note = null;
                        $('#Image1').attr('src', null);
                        $(":file").filestyle('clear');
                        $scope.start_Filter();
                    }
                });
            }
             $scope.enableedit = false;
             $scope.enableadd = true;
             $scope.show = true;
             $scope.hide = false;
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
                        $scope.model.id = null;
                        $scope.model.name = null;
                        $scope.model.picture = null;
                        $scope.model.priority = null;
                        $scope.model.note = null;
                        // $scope.model.order = null;
                        $('#Image1').attr('src', null);
                        $scope.start_Filter();

                    }
                    $scope.enableedit = false;
                    $scope.enableadd = true;
                    $scope.show = true;
                    $scope.hide = false;
                });
            });

    }


});

