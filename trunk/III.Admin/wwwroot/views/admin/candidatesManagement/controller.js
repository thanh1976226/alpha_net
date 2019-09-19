var ctxfolder = "/views/admin/candidatesManagement";
var ctxfolderMessage = "/views/message-box";


var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select', 'dynamicNumber', "ngCookies", "pascalprecht.translate"]);
app.factory("interceptors", [function () {
    return {
        // if beforeSend is defined call it
        'request': function (request) {
            if (request.beforeSend)
                request.beforeSend();

            return request;
        },
        // if complete is defined call it
        'response': function (response) {
            if (response.config.complete)
                response.config.complete(response);
            return response;
        }
    };
}]);
app.service('myService', function () {
    var data;
    this.setData = function (d) {
        data = d;
    }
    this.getData = function () {
        return data;
    }
});
app.factory('dataservice', function ($http) {
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
        //index
        getCountCandidateToday: function (callback) {
            $http.post('/Admin/CandidatesManagement/GetCountCandidateToday/').success(callback);
        },
        getItem: function (data, callback) {
            $http.post('/Admin/CandidatesManagement/GetItem/', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/CandidatesManagement/Delete/' + data).success(callback);
        },

        //tab candidate info
        createCandiateCode: function (callback) {
            $http.get('/Admin/CandidatesManagement/CreateCandiateCode/').success(callback);
        },
        updateCandidateInfo: function (data, callback) {
            $http.post('/Admin/CandidatesManagement/UpdateCandidateInfo/', data).success(callback);
        },
        uploadCV: function (data, callback) {
            submitFormUpload('/Admin/CandidatesManagement/UploadFile/', data, callback);
        },
        createCandiateCode: function (callback) {
            $http.get('/Admin/CandidatesManagement/CreateCandiateCode/').success(callback);
        },
        searchCandiateCode: function (data, callback) {
            $http.get('/Admin/CandidatesManagement/SearchCandiateCode?candidateCode=' + data).success(callback);
        },
        //tab candidate info more
        updateCandidateInfoMore: function (data, callback) {
            $http.post('/Admin/CandidatesManagement/UpdateCandidateInfoMore/', data).success(callback);
        },

        //tab candidate event
        getEventCat: function (data, callback) {
            $http.get('/Admin/CandidatesManagement/GetEventCat?candidateCode=' + data).success(callback);
        },
        changeFrameTimeStatus: function (id, frame, callback) {
            $http.post('/Admin/CandidatesManagement/ChangeFrametimeStatus/?id=' + id + "&frame=" + frame).success(callback);
        },
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
    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
        $.extend($.validator.messages, {
            min: caption.COM_VALIDATE_VALUE_MIN,
            //max: 'Max some message {0}'
        });
        $rootScope.partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
        $rootScope.partternName = /^(^[ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.0-9\s]+$)|^(^[0-9]+[ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.\s][ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.0-9\s]*$)/;
        //Miêu tả có thể null, và có chứa được khoảng trắng
        $rootScope.partternDescription = /^[ĂăĐđĨĩŨũƠơƯưÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹẠ-ỹa-zA-Z.0-9]*[^!@#$%^&*<>?]*$/;
        $rootScope.partternDate = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;//Pormat dd/mm/yyyy
        $rootScope.partternEmail = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        $rootScope.partternNumber = /^[0-9]\d*(\\d+)?$/; //Chỉ cho nhập số khong the am
        $rootScope.partternFloat = /^-?\d*(\.\d+)?$/; //Số thực
        $rootScope.partternNotSpace = /^[^\s].*/; //Không chứa khoảng trắng đầu dòng hoặc cuối dòng
        $rootScope.partternPhone = /^(0)+([0-9]{9,10})\b$/; //Số điện thoại 10,11 số bắt đầu bằng số 0

        $rootScope.dateNow = new Date();
        $rootScope.CandidateCode = "";
        $rootScope.validationOptionsBasic = {
            rules: {
                CandidateCode: {
                    required: true
                },
                Fullname: {
                    required: true,
                    maxlength: 255,
                    minlength: 6
                },
                RadioMaried: {
                    required: true,
                },
                Email: {
                    required: true,
                    maxlength: 100,
                    email: true
                },
                Birthday: {
                    required: true,
                },
                MobilePhone: {
                    required: true,
                    maxlength: 11,
                    minlength: 10,
                },
                FileCV: {
                    required: true,
                    maxlength: 255
                },
                Address: {
                    required: true,
                    maxlength: 255,
                    minlength: 15
                },
                Targeting: {
                    maxlength: 500
                },
                Skype: {
                    maxlength: 255
                },
                //MainSkill: {
                //    maxlength: 255
                //},
                //SubSkill: {
                //    maxlength: 255
                //},
                //MainPracticeTime: {
                //    maxlength: 255
                //},
                //SubPracticeTime: {
                //    maxlength: 255
                //},
                //LaptopInfo: {
                //    maxlength: 255
                //},
                //SmartphoneInfo: {
                //    maxlength: 255
                //}
            },
            messages: {
                CandidateCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_FULLNAME),
                },
                Fullname: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_FULLNAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CM_CURD_LBL_CM_FULLNAME).replace("{1}", "255"),
                    minlength: caption.CM_CURD_VALIDATE_NAME_SHORT
                },
                RadioMaried: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_MARRIED),
                },
                Email: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_EMAIL),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CM_CURD_LBL_CM_EMAIL).replace("{1}", "100"),
                    email: caption.CM_CURD_VALIDATE_ERR_MAIL
                },
                Birthday: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_BIRTHDAY),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CM_CURD_LBL_CM_BIRTHDAY).replace("{1}", "100"),
                },
                MobilePhone: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_PHONE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CM_CURD_LBL_CM_PHONE).replace("{1}", "11"),
                    minlength: caption.CM_CURD_VALIDATE_ERR_PHONE,
                },
                FileCV: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_FILECV),
                    maxlength: caption.CM_CURD_VALIDATE_NAME_FILE
                },
                Address: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CM_CURD_LBL_CM_ADDRESS),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CM_CURD_LBL_CM_ADDRESS).replace("{1}", "255"),
                    minlength: caption.CM_CURD_VALIDATE_ADDRESS_SHORT
                },
                Targeting: {
                    maxlength: caption.CM_CURD_VALIDATE_TARGET
                },
                Skype: {
                    maxlength: caption.CM_CURD_VALIDATE_SKYPE
                },
                //MainSkill: {
                //    maxlength: "Kỹ năng chính tối đa 255 ký tự!"
                //},
                //SubSkill: {
                //    maxlength: "Kỹ năng phụ tối đa 255 ký tự!"
                //},
                //MainPracticeTime: {
                //    maxlength: "Thời gian thực tập tối đa 255 ký tự!"
                //},
                //SubPracticeTime: {
                //    maxlength: "Thời gian thực tập tối đa 255 ký tự!"
                //},
                //LaptopInfo: {
                //    maxlength: "Nhập tối đa 255 ký tự!"
                //},
                //SmartphoneInfo: {
                //    maxlength: "Nhập tối đa 255 ký tự!"
                //}
            }
        }
    });
    
    $rootScope.IsEdit = false;
    });
});

app.config(function ($routeProvider, $validatorProvider, $httpProvider, $translateProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    caption = $translateProvider.translations();
    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
        }).when('/add/', {
            templateUrl: ctxfolder + '/add.html',
            controller: 'add'
        }).when('/edit/', {
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit'
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
    $httpProvider.interceptors.push('interceptors');
});
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $location, $filter, myService) {
    var vm = $scope;
    $scope.model = {
        FromDate: '',
        ToDate: ''
    }
    $scope.countCandidatesToday = 0;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/CandidatesManagement/JTable",
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
        .withOption('order', [5, 'desc'])
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
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ' hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Fullname').withTitle('{{"CM_LIST_COL_CM_FULL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Sex').withTitle('{{"CM_LIST_COL_CM_SEX" | translate}}').renderWith(function (data, type) {
        if (data == 1) {
            return '<i class="fas fa-male fs20"></i>';
        } else {
            return '<i class="fas fa-female fs20" style="color: #f1204fcf;"></i>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Birthday').withTitle('{{"CM_LIST_COL_CM_BIRTHDAY" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Phone').withTitle('{{"CM_LIST_COL_CM_PHONE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"CM_LIST_COL_CM_CREATEDTIME" | translate}}').renderWith(function (data, type) {
        var dateNow = $filter('date')(new Date($rootScope.dateNow), 'dd/MM/yyyy');
        if (data != '') {
            var createDate = $filter('date')(new Date(data), 'dd/MM/yyyy');
            if (dateNow == createDate) {
                var today = new Date();
                var created = new Date(data);
                var diffMs = (today - created);
                var diffHrs = Math.floor((diffMs % 86400000) / 3600000);
                var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000);
                return '<span class="badge badge-success">Mới </span> <span class="time">' + diffHrs + 'h ' + diffMins + 'p </span>';
            } else {
                return createDate;
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').notSortable().withOption('sClass', 'nowrap').withTitle('{{"CM_LIST_COL_CM_ACTION" | translate}}').renderWith(function (data, type, full, meta) {
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

    $scope.search = function () {
        reloadData(true);
    }
    $scope.initLoad = function () {
        dataservice.getCountCandidateToday(function (rs) {
            $scope.countCandidatesToday = rs;
        });
    }
    $scope.initLoad();
    $scope.add = function () {
        $location.path('/add/');
    }
    $scope.edit = function (id) {
        dataservice.getItem(id, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                myService.setData(rs.Object);
                $location.path('/edit/');
            }
        });
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.CM_MSG_CONFIRM_DELETE_CANDIDATE;
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
            size: '30',
        });
        modalInstance.result.then(function (d) {
            reloadData(true);
        }, function () {
        });
    }
    function loadDate() {
        $("#FromTo").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#DateTo').datepicker('setStartDate', maxDate);
        });
        $("#DateTo").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#FromTo').datepicker('setEndDate', maxDate);
        });
        $('.end-date').click(function () {
            $('#FromTo').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#DateTo').datepicker('setStartDate', null);
        });
    }
    setTimeout(function () {
        loadDate();
    }, 200);
});
app.controller('add', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $location, $filter) {
    var vm = $scope;
    $scope.forms = {}
    $scope.type = 0;
    $scope.modelInfo = {
        CandidateCode: '',
        Fullname: '',
        Sex: 1,
        Married: 0,
        Address: '',
        Phone: '',
        Email: '',
        Birthday: '',
        Skype: '',
        FileCv_1: '',
        Targeting: ''
    }
    $scope.modelInfoMore = {
        Ability: '',
        Targeting: '',
        MainSkill: '',
        MainPracticeTime: '',
        SubSkill: '',
        SubPracticeTime: '',
        LanguageUse: '',
        SalaryHope: '',
        CanJoinDate: '',
        LaptopInfo: '',
        SmartphoneInfo: ''
    }
    $scope.modelCalendar = {
        FromDate: '',
        ToDate: '',
        FullTime: false,
        Morning: false,
        Afternoon: false,
        Evening: false,
        Sunday: false,
        Saturday: false,
        Worktype: -1,
        MemberCode: '',
        Membertype: ''
    };

    //var events = [];
    //$scope.invalidBirthday = false;
    //$scope.Type = 0;
    //$scope.GoogleSuggest = [];
    $scope.initLoad = function () {
        $rootScope.IsEdit = false;
    }
    $scope.initLoad();
    $scope.cancel = function () {
        $location.path('/');
    }



    //tab candidate info,info more
    $scope.addMainSkill = function () {
        var str = $("#mainSkill").val();
        $("#mainSkill").val("");
        $('#main-skill input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#main-skill input").keypress(function () {
        }).trigger(e);
        //$scope.model.MainSkill += str + ',';
    }
    $scope.addSubSkill = function () {
        var str = $("#subSkill").val();
        $("#subSkill").val("");
        console.log(str);
        $('#sub-skill input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#sub-skill input").keypress(function () {
        }).trigger(e);
        //$scope.model.SubSkill += str + ',';
    }
    $scope.addLanguage = function () {
        var str = $("#language").val();
        $("#language").val("");
        console.log(str);
        $('#language-use input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#language-use input").keypress(function () {
        }).trigger(e);
    }
    $scope.googleSearch = function (id) {
        var str = document.getElementById(id).value;
        $.getJSON("http://suggestqueries.google.com/complete/search?callback=?",
            {
                "hl": "vi",
                "q": str,
                "client": "chrome"
            }
        )
            .done(function (data) {
                //$scope.GoogleSuggest = data[1];
                data[1].length = 10;
                $('#' + id).autocomplete({
                    source: data[1]
                });
            });
    }
    $scope.submitNextTab = function (form) {
        validationSelect($scope.modelInfo);
        if ($scope.forms.basicForm.validate() && validationSelect($scope.modelInfo).Status == false) {
            if (form == 1) {
                var file = document.getElementById("File").files[0];
                if (file == null || file == undefined) {
                    App.toastrError(caption.CM_CURD_VALIDATE_FILE);
                } else {
                    var fileName = $('input[type=file]').val();
                    var idxDot = fileName.lastIndexOf(".") + 1;
                    var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
                    if (extFile != "docx" && extFile != "doc" && extFile != "pdf") {
                        App.toastrError(caption.CM_MSG_DOCX_DOC_PDF);
                        return;
                    };
                    var data = new FormData();
                    data.append("FileUpload", file);
                    dataservice.uploadCV(data, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                            return;
                        }
                        else {
                            debugger
                            $scope.modelInfo.FileCv_1 = '/uploads/files/' + rs.Object;
                            $scope.modelInfo.CandidateCode = $rootScope.CandidateCode;
                            dataservice.updateCandidateInfo($scope.modelInfo, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                }
                                else {
                                    App.toastrSuccess(rs.Title);
                                    nextTab();
                                }
                            })
                        }
                    });
                }
            }
            if (form == 2) {
                var ability = document.getElementsByName("Ability");
                for (var i = 0; i < ability.length; i++) {
                    if (ability[i].checked) {
                        $scope.modelInfoMore.Ability += ability[i].value + ",";
                    }
                }
                $scope.modelInfoMore.CandidateCode = $rootScope.CandidateCode;

                dataservice.updateCandidateInfoMore($scope.modelInfoMore, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    }
                    else {
                        App.toastrSuccess(rs.Title);
                        nextTab();
                    }
                })
            }
        }
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Phone" && $scope.modelInfo.Phone && $rootScope.partternPhone.test($scope.modelInfo.Phone)) {
            $scope.errorPhone = false;
        } else if (SelectType == "Phone") {
            $scope.errorPhone = true;
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.Phone && !$rootScope.partternPhone.test(data.Phone)) {
            $scope.errorPhone = true;
            mess.Status = true;
        } else {
            $scope.errorPhone = false;
        }

        return mess;
    };
    $scope.prevTab = function () {
        prevTab();
    }
    $scope.createdSearchCandiateCode = function (type) {
        if (type == 0) {
            dataservice.createCandiateCode(function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    $("#CandidateCode").val(rs.Title);
                    $rootScope.CandidateCode = rs.Title;
                }
            })
        } else {
            if ($scope.modelInfo.CandidateCode == '') {
                App.toastrError(caption.CM_MSG_CODE_TO_SEARCH);
            } else {
                dataservice.searchCandiateCode($scope.modelInfo.CandidateCode, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    }
                    else {
                        $scope.modelInfo = rs.Object.CandidatesInfo;
                        $scope.modelInfoMore = rs.Object.CandidatesInfoMore;
                        $rootScope.CandidateCode = $scope.modelInfo.CandidateCode;
                        var tab = $('.nav-tabs li');
                        tab.removeClass('disabled');
                        $('div[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                            $('#calendar').fullCalendar('render');
                        });
                        reloadData(true);
                    }
                })
            }
        }
    }
    function deleteFrameTime(frameTime, eventCode, event) {
        $confirm({ text: caption.CM_MSG_OFF, title: caption.COM_CONFIRM, cancel: caption.COM_CONFIRM_CANCEL })
            .then(function () {
                dataservice.deleteFrameTime(eventCode, frameTime, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    }
                    else {
                        offEvent(event);
                    }
                });
            });
    }
    function loadCalendar(id) {
        $('#' + id).fullCalendar({
            defaultView: 'month',
            selectable: true,
            editable: true,
            eventLimit: true,
            header: {
                left: 'prev,next, today',
                center: 'title',
            },
            dayNames: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            monthNames: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 -', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            monthNamesShort: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 ', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            dayNamesShort: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            buttonText: {
                today: 'Hôm nay',
                icon: 'far fa-calendar-check'
            },
            events: function (start, end, timezone, callback) {
                dataservice.getEventCat($rootScope.candidateCode, function (rs) {
                    var event = [];
                    angular.forEach(rs, function (value, key) {
                        var morning = {
                            title: "1.Sáng",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Morning == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 0
                        }
                        var afternoon = {
                            title: "2.Chiều",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Afternoon == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 1
                        }
                        var evening = {
                            title: "3.Tối",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Evening == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 2
                        }
                        event.push(morning);
                        event.push(afternoon);
                        event.push(evening);
                    })
                    callback(event);
                })
            },
            eventClick: function (calEvent) {
                calEvent.color = '#bdc3c7';
                deleteFrameTime(calEvent.frameTime, calEvent.eventCode, calEvent);
                $("a.fc-day-grid-event.fc-h-event.fc-event.fc-start.fc-end.fc-draggable").click(function () {
                    var stt = $(this).hasClass("true");
                    if (stt == true) {
                        $(this).removeClass("true");
                        $(this).css('background', 'silver');
                    } else {
                        $(this).addClass(" true");
                        $(this).css('background', 'green');

                    }
                });
            },
            dayClick: function (date) {

            },
        })
    }
    function loadDate() {
        var date = new Date();
        date.setDate(date.getDate() + 1);
        $("#date-birthday").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: "01/01/1960",
            fontAwesome: true,
        });
        $("#can-join-date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: date,
            fontAwesome: true,
        });
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: date,
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#dateto').datepicker('setStartDate', maxDate);
        });
        $("#dateto").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefrom').datepicker('setEndDate', maxDate);
        });
    }
    function nextTab() {
        var $active = $('.nav-tabs li.active');
        $active.next().removeClass('disabled');
        $($active).next().find('div[data-toggle="tab"]').click();
    }
    function prevTab() {
        var $active = $('.nav-tabs li.active');
        $($active).prev().find('div[data-toggle="tab"]').click();
    }



    //tab candidate event
    $scope.changeStatusFrameTime = function (eventId, frameTime) {
        dataservice.changeFrameTimeStatus(eventId, frameTime, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                element = document.getElementsByClassName('sorting')[0];
                element.click();
            }
        });
    }
    $scope.submit = function () {
        App.blockUI({
            target: "#contentMain",
            boxed: true,
            message: 'loading...'
        });
        if ($("#morning:checked").length > 0) {
            $scope.modelCalendar.Morning = true;
        }
        else {
            $scope.modelCalendar.Morning = false;
        }
        if ($("#afternoon:checked").length > 0) {
            $scope.modelCalendar.Afternoon = true;
        }
        else {
            $scope.modelCalendar.Afternoon = false;
        }
        if ($("#evening:checked").length > 0) {
            $scope.modelCalendar.Evening = true;
        }
        else {
            $scope.modelCalendar.Evening = false;
        }
        if ($("#fulltime:checked").length > 0) {
            $scope.modelCalendar.FullTime = true;
        }
        else {
            $scope.modelCalendar.FullTime = false;
        }

        if ($scope.modelCalendar.FullTime == true) {
            $scope.modelCalendar.Morning = true;
            $scope.modelCalendar.Afternoon = true;
            $scope.modelCalendar.Evening = false;
        }
        dataservice.insertEventCat($scope.modelCalendar, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
                App.unblockUI("#contentMain");
            }
            else {
                App.toastrSuccess(rs.Title);
                //$scope.model = $scope.currentScope;
                //$scope.loadDate($scope.calendar);
                element = document.getElementsByClassName('sorting')[0];
                element.click();
            }
        })
    };
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/CandidatesManagement/GetEventCatGrid",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.CandidateCode = $rootScope.CandidateCode;
                //d.Worktype = $scope.calendar.Worktype;
                //d.Membertype = $scope.calendar.Membertype;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(10)
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn('CandidateCode').withTitle('{{"CM_LIST_COL_CM_CANDIDATE_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Fullname').withTitle('{{"CM_LIST_COL_CM_FULL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('DatetimeEvent').withTitle('{{"CM_LIST_COL_CM_DATETIME_EVENT" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));

    vm.dtColumns.push(DTColumnBuilder.newColumn('Morning').withTitle('{{"CM_LIST_COL_CM_MORNING" | translate}}').renderWith(function (data, type, full) {
        return data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Afternoon').withTitle('{{"CM_LIST_COL_CM_AFTERNOON" | translate}}').renderWith(function (data, type, full) {
        return data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Evening').withTitle('{{"CM_LIST_COL_CM_EVENING" | translate}}').renderWith(function (data, type, full) {
        return data == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.reloadData = reloadData;
    vm.dt = {
        dtInstance: {}
    }

    function reloadData(resetPaging) {
        vm.dt.dtInstance.reloadData(callback, resetPaging);
    }
    function callback(json) {

    }
    function loadTagInput() {
        $('.tag-input').tagsinput();
    }
    function renderCalenderInTab() {
        $('div[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $('#calendar').fullCalendar('render');
        });
    }
    setTimeout(function () {
        $('div[data-toggle="tab"]').on('show.bs.tab', function (e) {
            var $target = $(e.target);
            if ($target.parent().hasClass('disabled')) {
                return false;
            }
        });
        loadDate();
        loadCalendar("calendar");
        loadTagInput();
        renderCalenderInTab();
        setModalDraggable('.modal-dialog');
    }, 300);
});
app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $location, $filter, myService) {

    var vm = $scope;
    $scope.forms = {};

    //var events = [];
    //$scope.modelInfo = {};
    //$scope.modelInfoMore = {};
    //$scope.modelCalendar = {
    //    FromDate: '',
    //    ToDate: '',
    //    FullTime: false,
    //    Morning: false,
    //    Afternoon: false,
    //    Evening: false,
    //    Sunday: false,
    //    Saturday: false,
    //    Worktype: -1,
    //    MemberCode: '',
    //    Membertype: ''
    //};

    //$scope.invalidBirthday = false;
    //$scope.Type = 0;
    //$scope.GoogleSuggest = [];

    $scope.initLoad = function () {
        var data = myService.getData();
        if (data != undefined) {
            $scope.modelInfo = data.CandidatesInfo;
            $scope.modelInfoMore = data.CandidatesInfoMore;
            $rootScope.CandidateCode = $scope.modelInfo.CandidateCode;
            $rootScope.IsEdit = true;
        } else {
            $location.path('/');
        }
    }
    $scope.initLoad();
    $scope.cancel = function () {
        $location.path('/');
    }

    //tab candidate info,info more
    $scope.addMainSkill = function () {
        var str = $("#mainSkill").val();
        $("#mainSkill").val("");
        $('#main-skill input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#main-skill input").keypress(function () {
        }).trigger(e);
        //$scope.model.MainSkill += str + ',';
    }
    $scope.addSubSkill = function () {
        var str = $("#subSkill").val();
        $("#subSkill").val("");
        console.log(str);
        $('#sub-skill input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#sub-skill input").keypress(function () {
        }).trigger(e);
        //$scope.model.SubSkill += str + ',';
    }
    $scope.addLanguage = function () {
        var str = $("#language").val();
        $("#language").val("");
        console.log(str);
        $('#language-use input').val(str);

        e = jQuery.Event("keypress");
        e.which = 13;
        $("#language-use input").keypress(function () {
        }).trigger(e);
    }
    $scope.googleSearch = function (id) {
        var str = document.getElementById(id).value;
        $.getJSON("http://suggestqueries.google.com/complete/search?callback=?",
            {
                "hl": "vi",
                "q": str,
                "client": "chrome"
            }
        )
            .done(function (data) {
                //$scope.GoogleSuggest = data[1];
                data[1].length = 10;
                $('#' + id).autocomplete({
                    source: data[1]
                });
            });
    }
    $scope.submitNextTab = function (form) {
        validationSelect($scope.modelInfo);
        if ($scope.forms.basicForm.validate() && validationSelect($scope.modelInfo).Status == false) {
            if (form == 1) {
                var file = document.getElementById("File").files[0];
                if (file == null || file == undefined) {
                    App.toastrError(caption.CM_CURD_VALIDATE_FILE);
                } else {
                    var fileName = $('input[type=file]').val();
                    var idxDot = fileName.lastIndexOf(".") + 1;
                    var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
                    if (extFile != "docx" && extFile != "doc" && extFile != "pdf") {
                        App.toastrError(caption.CM_MSG_DOCX_DOC_PDF);
                        return;
                    };
                    var data = new FormData();
                    data.append("FileUpload", file);
                    dataservice.uploadCV(data, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                            return;
                        }
                        else {
                            $scope.modelInfo.FileCv_1 = '/uploads/files/' + rs.Object;
                            $scope.modelInfo.CandidateCode = $rootScope.CandidateCode;
                            dataservice.updateCandidateInfo($scope.modelInfo, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                }
                                else {
                                    App.toastrSuccess(rs.Title);
                                    nextTab();
                                }
                            })
                        }
                    });
                }
            }
            if (form == 2) {
                var ability = document.getElementsByName("Ability");
                for (var i = 0; i < ability.length; i++) {
                    if (ability[i].checked) {
                        $scope.modelInfoMore.Ability += ability[i].value + ",";
                    }
                }
                $scope.modelInfoMore.CandidateCode = $rootScope.CandidateCode;

                dataservice.updateCandidateInfoMore($scope.modelInfoMore, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    }
                    else {
                        App.toastrSuccess(rs.Title);
                        nextTab();
                    }
                })
            }
        }
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Phone" && $scope.modelInfo.Phone && $rootScope.partternPhone.test($scope.modelInfo.Phone)) {
            $scope.errorPhone = false;
        } else if (SelectType == "Phone") {
            $scope.errorPhone = true;
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.Phone && !$rootScope.partternPhone.test(data.Phone)) {
            $scope.errorPhone = true;
            mess.Status = true;
        } else {
            $scope.errorPhone = false;
        }

        return mess;
    };

    $scope.prevTab = function () {
        prevTab();
    }
    function deleteFrameTime(frameTime, eventCode, event) {
        $confirm({ text: caption.CM_MSG_OFF, title: caption.COM_BTN_CONFIRM, cancel: caption.COM_CONFIRM_CANCEL })
            .then(function () {
                dataservice.deleteFrameTime(eventCode, frameTime, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                    }
                    else {
                        offEvent(event);
                    }
                });
            });
    }
    function loadCalendar(id) {
        $('#' + id).fullCalendar({
            defaultView: 'month',
            selectable: true,
            editable: true,
            eventLimit: true,
            header: {
                left: 'prev,next, today',
                center: 'title',
            },
            dayNames: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            monthNames: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 -', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            monthNamesShort: ['Tháng 1 -', 'Tháng 2 -', 'Tháng 3 -', 'Tháng 4 -', 'Tháng 5 -', 'Tháng 6 -', 'Tháng 7 -', 'Tháng 8 ', 'Tháng 9 -', 'Tháng 10 -', 'Tháng 11 -', 'Tháng 12 -'],
            dayNamesShort: ['Chủ nhật', 'Thứ hai', 'Thứ ba', 'Thứ tư', 'Thứ năm', 'Thứ sáu', 'Thứ bảy'],
            buttonText: {
                today: 'Hôm nay',
                icon: 'far fa-calendar-check'
            },
            events: function (start, end, timezone, callback) {
                dataservice.getEventCat($rootScope.candidateCode, function (rs) {
                    var event = [];
                    angular.forEach(rs, function (value, key) {
                        var morning = {
                            title: "1.Sáng",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Morning == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 0
                        }
                        var afternoon = {
                            title: "2.Chiều",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Afternoon == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 1
                        }
                        var evening = {
                            title: "3.Tối",
                            start: value.DatetimeEvent,
                            allDay: true,
                            color: value.Evening == "True" ? '#16a085' : '#bdc3c7',
                            id: value.Id,
                            frameTime: 2
                        }
                        event.push(morning);
                        event.push(afternoon);
                        event.push(evening);
                    })
                    callback(event);
                })
            },
            eventClick: function (calEvent) {
                calEvent.color = '#bdc3c7';
                deleteFrameTime(calEvent.frameTime, calEvent.eventCode, calEvent);
                $("a.fc-day-grid-event.fc-h-event.fc-event.fc-start.fc-end.fc-draggable").click(function () {
                    var stt = $(this).hasClass("true");
                    if (stt == true) {
                        $(this).removeClass("true");
                        $(this).css('background', 'silver');
                    } else {
                        $(this).addClass(" true");
                        $(this).css('background', 'green');

                    }
                });
            },
            dayClick: function (date) {

            },
        })
    }
    function loadDate() {
        var date = new Date();
        date.setDate(date.getDate() + 1);
        $("#date-birthday").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: "01/01/1960",
            fontAwesome: true,
        });
        $("#can-join-date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: date,
            fontAwesome: true,
        });
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: date,
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#dateto').datepicker('setStartDate', maxDate);
        });
        $("#dateto").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefrom').datepicker('setEndDate', maxDate);
        });
    }
    function nextTab() {
        var $active = $('.nav-tabs li.active');
        $active.next().removeClass('disabled');
        $($active).next().find('div[data-toggle="tab"]').click();
    }
    function prevTab() {
        var $active = $('.nav-tabs li.active');
        $($active).prev().find('div[data-toggle="tab"]').click();
    }



    //tab candidate event
    $scope.changeStatusFrameTime = function (eventId, frameTime) {
        dataservice.changeFrameTimeStatus(eventId, frameTime, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                element = document.getElementsByClassName('sorting')[0];
                element.click();
            }
        });
    }
    $scope.submit = function () {
        App.blockUI({
            target: "#contentMain",
            boxed: true,
            message: 'loading...'
        });
        if ($("#morning:checked").length > 0) {
            $scope.calendar.Morning = true;
        }
        else {
            $scope.calendar.Morning = false;
        }
        if ($("#afternoon:checked").length > 0) {
            $scope.calendar.Afternoon = true;
        }
        else {
            $scope.calendar.Afternoon = false;
        }
        if ($("#evening:checked").length > 0) {
            $scope.calendar.Evening = true;
        }
        else {
            $scope.calendar.Evening = false;
        }
        if ($("#fulltime:checked").length > 0) {
            $scope.calendar.FullTime = true;
        }
        else {
            $scope.calendar.FullTime = false;
        }

        if ($scope.calendar.FullTime == true) {
            $scope.calendar.Morning = true;
            $scope.calendar.Afternoon = true;
            $scope.calendar.Evening = false;
        }
        dataservice.insertEventCat($scope.calendar, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
                App.unblockUI("#contentMain");
            }
            else {
                App.toastrSuccess(rs.Title);
                //$scope.model = $scope.currentScope;
                //$scope.loadDate($scope.calendar);
                element = document.getElementsByClassName('sorting')[0];
                element.click();
            }
        })
    };
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/CandidatesManagement/GetEventCatGrid",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.CandidateCode = $rootScope.CandidateCode;
                //d.Worktype = $scope.calendar.Worktype;
                //d.Membertype = $scope.calendar.Membertype;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(10)
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn('CandidateCode').withTitle('{{"CM_LIST_COL_CM_CANDIDATE_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Fullname').withTitle('{{"CM_LIST_COL_CM_FULL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('DatetimeEvent').withTitle('{{"CM_LIST_COL_CM_DATETIME_EVENT" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));

    vm.dtColumns.push(DTColumnBuilder.newColumn('Morning').withTitle('{{"CM_LIST_COL_CM_MORNING" | translate}}').renderWith(function (data, type, full) {
        return full.FrameTime.split(';')[0] == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 1 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Afternoon').withTitle('{{"CM_LIST_COL_CM_AFTERNOON" | translate}}').renderWith(function (data, type, full) {
        return full.FrameTime.split(';')[1] == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 2 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Evening').withTitle('{{"CM_LIST_COL_CM_EVENING" | translate}}').renderWith(function (data, type, full) {
        return full.FrameTime.split(';')[2] == "True" ? '<button class="btn btn-circle btn-icon-only green" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-check"></i></button>' :
            '<button class="btn btn-circle btn-icon-only red" ng-click="changeStatusFrameTime(' + full.Id + ',' + 3 + ')" style="padding: 0px; width: 25px; height: 25px"><i class="fa fa-times" style="color: #ffffff"></i></button>';
    }));
    vm.reloadData = reloadData;
    vm.dt = {
        dtInstance: {}
    }

    function reloadData(resetPaging) {
        vm.dt.dtInstance.reloadData(callback, resetPaging);
    }
    function callback(json) {

    }
    function loadTagInput() {
        $('.tag-input').tagsinput();
    }
    function renderCalenderInTab() {
        $('div[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            $('#calendar').fullCalendar('render');
        });
    }
    setTimeout(function () {
        loadDate();
        loadCalendar("calendar");
        loadTagInput();
        renderCalenderInTab();
        setModalDraggable('.modal-dialog');
    }, 300);

});
