var ctxfolder = "/views/admin/accountLogin";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate"]);
app.factory('dataservice', function ($http) {
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    var submitFormUpload = function (url, data, callback) {
        var req = {
            method: 'POST',
            url: url,
            headers: {
                'Content-Type': undefined
            },
            data: data
        }

        $http(req).success(callback);
    };
    return {
        changeImage: function (data, callback) {
            submitFormUpload('/Admin/AccountLogin/ChangeImage', data, callback);
        },
        changePassword: function (data, callback) {
            $http.post('/Admin/AccountLogin/ChangePassword', data).success(callback);
        },
        getItem: function (callback) {
            $http.post('/Admin/AccountLogin/GetItem').success(callback);
        },
    }
});
app.factory('httpResponseInterceptor', ['$q', '$rootScope', '$location', function ($q, $rootScope, $location) {
    return {
        responseError: function (rejection) {
            if (rejection.status === 401) {
                var url = "/Home/Logout";
                location.href = url;
            }
            return $q.reject(rejection);
        }
    };
}]);
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, $cookies, $translate, dataservice) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };
    $rootScope.validationOptions = {
        rules: {
            PasswordOld: {
                required: true,
                minlength: 6
            },
            PasswordNew: {
                required: true,
                minlength: 6,
            },
            InputPasswordNew: {
                required: true,
                minlength: 6,
            }
        },
        messages: {
            PasswordOld: {
                required: "Mật khẩu cũ không được để trống",
                minlength: "Mật khẩu cũ lớn hơn 6 kí tự",
            },
            PasswordNew: {
                required: "Mật khẩu mới không được để trống",
                minlength: "Mật khẩu mới lớn hơn 6 kí tự",
            },
            InputPasswordNew: {
                required: "Nhập lại mật khẩu mới không được để trống",
                minlength: "Nhập lại mật khẩu mới lớn hơn 6 kí tự",
            }
        }
    }
    $rootScope.checkData = function (data) {
        var partternPass = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{6,})/;
        var mess = { Status: false, Title: "" }
        if (data.PasswordNew.length < 6) {
            mess.Status = true;
            mess.Title += "Mật khẩu mới phải có ít nhất 6 ký tự" + "<br/>";
        }
        return mess;
    }

});
app.config(function ($routeProvider, $validatorProvider, $httpProvider) {
    $routeProvider
        .when('/infoAccount', {
            templateUrl: ctxfolder + '/infoAccount.html',
            controller: 'infoAccount'
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
    $httpProvider.interceptors.push('httpResponseInterceptor');
});

app.controller('infoAccount', function ($scope, $rootScope, $compile, $uibModal, $confirm, dataservice) {
    $scope.initLoad = function () {
        dataservice.getItem(function (rs) {
            $scope.model = rs;
        });
    }
    $scope.initLoad();

    //image
    $scope.uploadImage = function () {
        var fileuploader = angular.element("#File");
        fileuploader.on('click', function () {
        });
        fileuploader.on('change', function (e) {
            var reader = new FileReader();
            reader.onload = function () {
                document.getElementById('imageId').src = reader.result;
            }
            var files = fileuploader[0].files;
            var idxDot = files[0].name.lastIndexOf(".") + 1;
            var extFile = files[0].name.substr(idxDot, files[0].name.length).toLowerCase();
            if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                App.toastrError("Định dạng ảnh không hợp lệ!");
                return;
            }
            reader.readAsDataURL(files[0]);
        });
        fileuploader.trigger('click')
    }
    $scope.changeImage = function () {
        var file = document.getElementById("File").files[0]
        if (file != undefined) {
            var idxDot = file.name.lastIndexOf(".") + 1;
            var extFile = file.name.substr(idxDot, file.name.length).toLowerCase();
            if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                App.toastrError("Yêu cầu định dạng is png, jpg, jpeg, gif, bmp!");
                return;
            }
            var formData = new FormData();
            formData.append("image", file != undefined ? file : null);
            formData.append("Id", $scope.model.Id);
            var modalInstance = $uibModal.open({
                templateUrl: ctxfolderMessage + '/messageConfirmAdd.html',
                windowClass: "message-center",
                resolve: {
                    para: function () {
                        return formData;
                    }
                },
                controller: function ($scope, $uibModalInstance, para) {
                    $scope.message = "Bạn muốn chỉnh sửa ảnh đại diện hay không ?";
                    $scope.ok = function () {
                        dataservice.changeImage(para, function (rs) {
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
                size: '30',
            });
            modalInstance.result.then(function (d) {
                $scope.initLoad();
            }, function () {
            });

        } else {
            App.toastrError("Vui lòng chọn ảnh!");
        }
    }

    //password
    $scope.changePassword = function () {
        if ($scope.changePasswordform.validate()) {
            if ($scope.model.PasswordNew == $scope.model.InputPasswordNew) {
                var temp = $rootScope.checkData($scope.model);
                if (temp.Status) {
                    App.toastrError(temp.Title);
                    return;
                }
                var modalInstance = $uibModal.open({
                    templateUrl: ctxfolderMessage + '/messageConfirmAdd.html',
                    windowClass: "message-center",
                    resolve: {
                        para: function () {
                            return $scope.model;
                        }
                    },
                    controller: function ($scope, $uibModalInstance, para) {
                        $scope.message = "Bạn muốn cập nhập mật khẩu hay không ?";
                        $scope.ok = function () {
                            dataservice.changePassword(para, function (rs) {
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
                    size: '30',
                });
                modalInstance.result.then(function (d) {
                   
                }, function () {
                });
            } else {
                App.toastrError("Mật khẩu mới nhập không trùng nhau")
            }
        }
    }

    setTimeout(function () {
        $(".toggle-passwordOld").click(function () {
            $(this).toggleClass("fa-eye fa-eye-slash");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });
        $(".toggle-passwordNew").click(function () {
            $(this).toggleClass("fa-eye fa-eye-slash");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });
        $(".toggle-inputPasswordNew").click(function () {
            $(this).toggleClass("fa-eye fa-eye-slash");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });
    }, 200);
});
