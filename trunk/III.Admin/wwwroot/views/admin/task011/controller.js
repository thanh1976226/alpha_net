var ctxfolder = "/views/admin/task011";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select']);

app.factory('dataservice', function ($http) {
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    return {
        //insert: function (data, callback) {
        //    $http.post('/Admin/task011/insert', data).success(callback);
        //},
        update: function (data, callback) {
            $http.post('/Admin/task011/update', data).success(callback);
        },
        //deleteItems: function (data, callback) {
        //    $http.post('/LgVendor/deleteItems', data).success(callback);
        //},
        //delete: function (data, callback) {
        //    $http.post('/Admin/task011/delete/' + data).success(callback);
        //},
        getItem: function (data, callback) {
            $http.get('/Admin/task011/getitem/' + data).success(callback);
        },
        
        //gettreedataCoursetype: function(callback) {
        //    $http.post('/Admin/task011/gettreedataCoursetype/').success(callback);
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
    $rootScope.checkData = function (data) {
        var parttern = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
        if (data.google == "" || data.google == undefined || parttern.test(data.google)) {
            return { Status: false, Title: '' };
        } else {
            return { Status: true, Title: 'Định dạng Email chưa đúng vui lòng nhập lại' };
        }
    }

    $rootScope.validationOptions = {
        rules: {
            google: {
                required: true,
                maxlength: 255,
             //   minlength: number,
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
            google: {
                required: "Yêu cầu nhập Email",
                maxlength: "Email không vượt quá 255 ký tự."
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

