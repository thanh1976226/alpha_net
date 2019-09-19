var ctxfolder = "/views/admin/hREmployee";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select', 'dynamicNumber', "ngCookies", "pascalprecht.translate"]).
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
    $http.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
    var headers = {
        "Content-Type": "application/json;odata=verbose",
        "Accept": "application/json;odata=verbose",
    }
    var submitFormUploadHD = function (url, data, callback) {
        var config = {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        };

        var formData = new FormData();
        angular.forEach(data, function (value, key) {
            if (value == 'undefined' || value == 'null') {
                data[key] = '';
            }
        });
        formData.append("id", data.id);
        formData.append("Insuarance", data.Insuarance);
        formData.append("Dates_of_pay", data.Dates_of_pay);
        formData.append("Place_Work", data.Place_Work);
        formData.append("Exp_time_work", data.Exp_time_work);
        formData.append("Salary_Ratio", data.Salary_Ratio);
        formData.append("Payment", data.Payment);
        formData.append("Contract_Type", data.Contract_Type);
        formData.append("Signer_Id", data.Signer_Id);
        formData.append("Salary", data.Salary);
        formData.append("Start_Time", data.Start_Time);
        formData.append("End_Time", data.End_Time);
        formData.append("DateOf_LaborBook", data.DateOf_LaborBook);
        formData.append("Work_Content", data.Work_Content);
        formData.append("Allowance", data.Allowance);
        formData.append("Contract_Code", data.Contract_Code);
        formData.append("LaborBook_Code", data.LaborBook_Code);
        formData.append("Other_Agree", data.Other_Agree);
        formData.append("Info_Insuarance", data.Info_Insuarance);
        formData.append("Bonus", data.Bonus);
        formData.append("Tools_Work", data.Tools_Work);
        formData.append("Active", data.Active);
        formData.append("Type_Money", data.Type_Money);
        formData.append("Value_time_work", data.Value_time_work);
        formData.append("Type_Money", data.Type_Money);
        formData.append("File", data.File);

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
    var submitFormUploadFile = function (url, data, callback) {
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
        insert: function (data, callback) {
            $http.post('/Admin/HREmployee/Insert', data).success(callback);
            //submitFormUpload1('/Admin/HREmployee/Insert', data, callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/HREmployee/Update', data).success(callback);
            //submitFormUpload('/Admin/HREmployee/Update', data, callback);
        },

        uploadFile: function (data, callback) {
            submitFormUploadFile('/Admin/HREmployee/UploadFile/', data, callback);
        },
        uploadImage: function (data, callback) {
            submitFormUploadFile('/Admin/HREmployee/UploadImage/', data, callback);
        },

        getPosition: function (callback) {
            $http.post('/Admin/HREmployee/GetPosition').success(callback);
        },
        gettreedataunit: function (callback) {
            $http.post('/Admin/HREmployee/Gettreedataunit').success(callback);
        },

        getItem: function (data, callback) {
            $http.get('/Admin/HREmployee/GetItem/' + data).success(callback);
        },

        deleteItems: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItems', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/HREmployee/Delete/' + data).success(callback);
        },
        getItemT: function (data, callback) {
            $http.get('/Admin/HREmployee/GetItemT/' + data).success(callback);
        },

        setEmployeeId: function (data, callback) {
            $http.post('/Admin/HREmployee/SetEmployeeId/' + data).success(callback);
        },



        insertAddress: function (data, callback) {
            $http.post('/Admin/HREmployee/InsertAddress', data).success(callback);
        },
        updateAddress: function (data, callback) {
            $http.post('/Admin/HREmployee/UpdateAddress', data).success(callback);
        },
        //deleteItemsDC: function (data, callback) {
        //    $http.post('/Admin/HREmployee/DeleteItemsDC', data).success(callback);
        //},
        deleteAddress: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteAddress/' + data).success(callback);
        },
        getItemAddress: function (data, callback) {
            $http.get('/Admin/HREmployee/GetitemAddress/' + data).success(callback);
        },

        insertLH: function (data, callback) {
            $http.post('/Admin/HREmployee/InsertLH', data).success(callback);
        },
        updateLH: function (data, callback) {
            $http.post('/Admin/HREmployee/UpdateLH', data).success(callback);
        },
        deleteItemsLH: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItemsLH', data).success(callback);
        },
        deleteLH: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteLH/' + data).success(callback);
        },
        getItemLH: function (data, callback) {
            $http.get('/Admin/HREmployee/GetitemLH/' + data).success(callback);
        },

        insertQTLV: function (data, callback) {
            $http.post('/Admin/HREmployee/InsertQTLV', data).success(callback);
        },
        updateQTLV: function (data, callback) {
            $http.post('/Admin/HREmployee/UpdateQTLV', data).success(callback);
        },
        deleteItemsQTLV: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItemsQTLV', data).success(callback);
        },
        deleteQTLV: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteQTLV/' + data).success(callback);
        },
        getItemQTLV: function (data, callback) {
            $http.get('/Admin/HREmployee/GetitemQTLV/' + data).success(callback);
        },
        insertQTCV: function (data, callback) {
            $http.post('/Admin/HREmployee/InsertQTCV', data).success(callback);
        },
        updateQTCV: function (data, callback) {
            $http.post('/Admin/HREmployee/UpdateQTCV', data).success(callback);
        },
        deleteItemsQTCV: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItemsQTCV', data).success(callback);
        },
        deleteQTCV: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteQTCV/' + data).success(callback);
        },
        getItemQTCV: function (data, callback) {
            $http.get('/Admin/HREmployee/GetitemQTCV/' + data).success(callback);
        },



        insertBCCC: function (data, callback) {
            $http.post('/Admin/HREmployee/InsertBCCC', data).success(callback);
        },
        updateBCCC: function (data, callback) {
            $http.post('/Admin/HREmployee/UpdateBCCC', data).success(callback);
        },
        deleteItemsBCCC: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItemsBCCC', data).success(callback);
        },
        deleteBCCC: function (data, callback) {
            $http.post('/Admin/HREmployee/deleteBCCC/' + data).success(callback);
        },
        getItemBCCC: function (data, callback) {
            $http.get('/Admin/HREmployee/getitemBCCC/' + data).success(callback);
        },



        insert_HD: function (data, callback) {
            submitFormUploadHD('/Admin/HREmployee/InsertHD', data, callback);
        },
        upload_HD: function (data, callback) {
            submitFormUploadHD('/Admin/HREmployee/UpdateHD', data, callback);
        },
        getItemHD: function (data, callback) {
            $http.get('/Admin/HREmployee/GetItemHD/' + data).success(callback);
        },
        getCurrencyHD: function (callback) {
            $http.post('/Admin/HREmployee/GetCurrencyHD').success(callback);
        },
        deleteItems_HD: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteItemsHD', data).success(callback);
        },
        delete_HD: function (data, callback) {
            $http.post('/Admin/HREmployee/DeleteHD', data).success(callback);
        }
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $filter, dataservice, $cookies, $translate) {
    $rootScope.go = function (path) {
        $location.path(path); return false;
    };

    var culture = $cookies.get('_CULTURE') || 'vi-VN';
    $translate.use(culture);


    $rootScope.$on('$translateChangeSuccess', function () {
        caption = caption[culture];
        $.extend($.validator.messages, {
            min: caption.COM_VALIDATE_VALUE_MIN,
      
        });

    $rootScope.validationOptions = {
        rules: {
            fullname: {
                required: true,
                maxlength: 50
            },
            birthday: {
                required: true,
            },
            nation: {
                required: true,
                maxlength: 50
            },
            phone: {
                required: true,
                maxlength: 50
            },
            permanentresidence: {
                required: true,
                maxlength: 200
            },
            phone: {
                required: true,
                maxlength: 50
            },
            health: {
                required: true,
                maxlength: 50
            },
            identitycard: {
                required: true,
                maxlength: 12
            },
            identitycardplace: {
                maxlength: 100
            },
            socialinsurance: {
                required: true,
                maxlength: 12
            },

            identification: {
                required: true,
                maxlength: 100
            },


            taxcode: {
                required: true,
                maxlength: 50
            },



            bank: {
                required: true,
                maxlength: 100
            },

            accountnumber: {
                required: true,
                maxlength: 50
            },



            Permanent_Address: {
                required: true,
                maxlength: 250
            },
            Now_Address: {
                required: true,
                maxlength: 250
            },
        },
        messages: {
            Permanent_Address: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RESIDENT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_RESIDENT
            },
            birthday: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_DATE_OF_BIRTH,
            },
            Now_Address: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_ADDRESS,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_ADDRESS
            },

            fullname: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_FULL_NAME,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_NAME
            },
            nation: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_NATIONANITY,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_NATIONANITY
            },
            phone: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_PHONE,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_PHONE
            },
            birthday: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_DATE_OF_BIRTH,
                maxlength: caption.HR_HR_MAN_CURD_ERR_HR_MAN_BIRTHDAY
            },
            permanentresidence: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RESIDENT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_RESIDENT
            },
            phone: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_PHONE,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_PHONE
            },
            educationallevel: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_EDUCATION_LEVEL,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_CHARACTER_EDUCATIONAL_LEVEL
            },
            health: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_HEALTH,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_HEALTH
            },
            identitycard: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_PASSPORT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_PASSPORT
            },
            identitycardplace: {
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_ISSUED
            },
            taxcode: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_HEALTH,
                maxlength: caption.HR_HR_MAN_CURD_ERR_HR_MAN_DAY_RANGE
            },
            bank: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_BANK_ACCOUNT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_BANK_ACCOUNT
            },
            accountnumber: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_NUMBER_ACCOUNT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_NUMBER_ACCOUNT
            },


        }
    }
    $rootScope.validationOptionsInfomation = {
        rules: {
            taxcode: {
                digits: true,
            },
            accountnumber: {
                digits: true,
            }
        },
        messages: {
            taxcode: {
                digits: caption.HR_HR_MAN_CURD_ERR_HR_MAN_TAX_CODE,
            },
            accountnumber: {
                digits: caption.HR_HR_MAN_CURD_ERR_HR_MAN_NUMBER_ACCOUNT,
            }
        }
    }
    $rootScope.validationOptionsAddress = {
        rules: {
            Permanent_Address: {
                required: true
            },
            Now_Address: {
                required: true
            }
        },
        messages: {
            Permanent_Address: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RESIDENT
            },
            Now_Address: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_ADDRESS
            }
        }
    }
    $rootScope.validationOptionsContact = {
        rules: {
            Name: {
                required: true,
                maxlength: 250
            },
            Relationship: {
                required: true,
                maxlength: 50
            },
        },
        messages: {
            Name: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RELATIVES,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_NAME_RELATIVES
            },
            Relationship: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RELATIONSHIP,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_RELATIONSHIP
            },
        }
    }
    $rootScope.validationOptionsDegree = {
        rules: {
            Education_Name: {
                required: true,
                maxlength: 250
            },
            Result: {
                required: true,
                maxlength: 50
            },
        },
        messages: {
            Education_Name: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_NAME_TRAINING,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_TRAINING_NAME
            },
            Result: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_RESULT,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_RESULT
            },
        }
    }
    $rootScope.validationOptionsWorkProgress = {
        rules: {
            Start_Time_Workprocess: {
                required: true,
            },
            End_Time_Workprocess: {
                required: true,
            },
            Wage_Level: {
                required: true,
            },
            Salary_Ratio: {
                required: true,
            },
        },
        messages: {
            Start_Time_Workprocess: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_START_DAY,
            },
            End_Time_Workprocess: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_END_DAY,
            },
            Wage_Level: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_SALARY_LEVEL,
            },
            Salary_Ratio: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_COEFFICIENTS,
            },
        }
    }
    $rootScope.validationOptionsContract = {
        rules: {
            Contract_Code: {
                required: true,
                maxlength: 50
            },
            Salary: {
                required: true,
            },
            Insuarance: {
                required: true,
            },
            Start_Time: {
                required: true,
            },

            End_Time: {
                required: true,
            },


        },
        messages: {
            Contract_Code: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CONTRACT_NUMBER,
                maxlength: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_CHARACTER_CONTRACT_NUMBER
            },
            Salary: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_SALALY,
            },
            Insuarance: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_INSURRANCE_LEVEL,
            },
            Start_Time: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_SART_TIME,
            },
            End_Time: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_END_TIME,
            },
        }
    }
    $rootScope.validationOptionsWorkflow = {
        rules: {
            Name_Job: {
                required: true,
                maxlength: 250
            },
            Working_Process: {
                required: true,
                maxlength: 250
            }
        },
        messages: {
            Name_Job: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_NAME_JOB,
                maxlength: " "
            },
            Working_Process: {
                required: caption.HR_HR_MAN_CURD_VALIDATE_HR_MAN_PROCEDURE,
                maxlength: " "
            }
        }
    }

    $rootScope.StatusData = [{
        Value: 1,
        Name: caption.COM_WORKING
    }, {
        Value: 0,
        Name: caption.COM_STOP_WORKING
    }];
    $rootScope.DateNow = $filter('date')(new Date(), 'dd/MM/yyyy');
    $rootScope.DateBeforeSevenDay = $filter('date')(new Date().setDate((new Date()).getDate() + (-7)), 'dd/MM/yyyy');

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
    });
});
app.config(function ($routeProvider, $validatorProvider, $translateProvider, $httpProvider) {

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
        .when('/add_contract/', {
            templateUrl: ctxfolder + '/add_contract.html',
            controller: 'add_contract'
        })
        .when('/edit_contract/:id', {
            templateUrl: ctxfolder + '/edit_contract.html',
            controller: 'edit_contract'
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
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice) {

    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        Key: '',
        FromDate: '',
        ToDate: '',
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/jtable",
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
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(10)
        .withOption('order', [1, 'asc'])
        .withOption('serverSide', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
            //$(this.api().table().header()).css({ 'background-color': '#1a2226', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row).contents())($scope);

            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('fullname').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('gender').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_SEX" | translate}}').renderWith(function (data, type) {
        if (data == 1) {
            return "Nam";
        }
        if (data == 2) {
            return "Nữ"
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('phone').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_PHONE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('permanentresidence').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_NATIVE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('picture').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_AVATAR" | translate}}').renderWith(function (data, type) {
        return '<img class="img-circle" src="' + data + '" onerror =' + "'" + 'this.src="' + '/images/default/no_user.png' + '"' + "'" + 'height="30" width="30">';
    }).withOption('sWidth', '50px'));
    vm.dtColumns.push(DTColumnBuilder.newColumn("null").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable().renderWith(function (data, type, full, meta) {
        return '<button ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }).withOption('sWidth', '50px').withOption('sClass', 'nowrap'));
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

    $scope.initload = function () {
        dataservice.getCurrencyHD(function (result) {
            $rootScope.CurrencyData = result;
        });
    }
    $scope.initload();

    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: true,
            size: '60'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }

    $scope.edit = function (id) {
        //dataservice.setEmployeeId(id, function (rs) { });
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit',
            backdrop: true,
            size: '70',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reloadNoResetPage();
        }, function () {
        });
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
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
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
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
        //$('#datefrom').datepicker('setEndDate', $rootScope.DateNow);
        //$('#dateto').datepicker('setStartDate', $rootScope.DateBeforeSevenDay);
        //$('#datefrom').datepicker('update', $rootScope.DateBeforeSevenDay);
        //$('#dateto').datepicker('update', $rootScope.DateNow);
        $('.end-date').click(function () {
            $('#datefrom').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#dateto').datepicker('setStartDate', null);
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
        loadDate();
        showHideSearch();
    }, 200);

});
app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice) {
    $scope.model = {
        gender: '',
        disciplines: '',
        employeekind: '',
        employeegroup: '',
        unit: '',
        employeetype: '',
    }
    $scope.checkGender = [
        { Name: caption.HR_HR_MAN_CURD_TXT_MEN, gender: 1 },
        { Name: caption.HR_HR_MAN_CURD_TXT_WOMEN, gender: 2 }
    ];
    $scope.tinhtranglamviec = [
        { value: caption.HR_HR_MAN_CURD_TXT_WORKING, disciplines: "1" },
        { value: caption.HR_HR_MAN_CURD_TXT_FURLOUH, disciplines: "2" },
        { value: caption.HR_HR_MAN_CURD_TXT_QUIT_WORK, disciplines: "3" }
    ];
    $scope.loaihinhnhanvien = [
        { value: caption.HR_HR_MAN_CURD_TXT_FULL_TIME, employeekind: "1" },
        { value: caption.HR_HR_MAN_CURD_TXT_PART_TIME, employeekind: "2" }
    ];
    $scope.nhom = [
        { value: caption.HR_HR_MAN_CURD_TXT_MORNING, employeegroup: "1" },
        { value: caption.HR_HR_MAN_CURD_TXT_AFTERNOON, employeegroup: "2" },
        { value: caption.HR_HR_MAN_CURD_TXT_NIGHT, employeegroup: "3" },
    ];


    $scope.kieunhanvien = [
        { value: caption.HR_HR_MAN_CURD_TXT_DRIVER, employeetype: "1" }
    ];

    $scope.initData = function () {
        dataservice.gettreedataunit(function (result) {
            $scope.treeDataunit = result.Object;
        });
        dataservice.getPosition(function (result) {
            $scope.positionData = result;
        });
    }

    $scope.initData();

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.uploadImage = function () {
        var fileuploader = angular.element("#file");
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
                App.toastrError(caption.COM_MSG_INVALID_FORMAT);
                return;
            }
            reader.readAsDataURL(files[0]);
        });
        fileuploader.trigger('click')
    }

    $scope.changleSelect = function (selectType) {
        if (selectType == "gender" && $scope.model.gender != "") {
            $scope.errorGender = false;
        }
        if (selectType == "unit" && $scope.model.unit != "") {
            $scope.errorUnit = false;
        }
        if (selectType == "employeekind" && $scope.model.employeekind != "") {
            $scope.errorEmployeekind = false;
        }
        if (selectType == "employeegroup" && $scope.model.employeegroup != "") {
            $scope.errorEmployeegroup = false;
        }
        if (selectType == "employeetype" && $scope.model.employeetype != "") {
            $scope.errorEmployeetype = false;
        }

        if (selectType == "phone" && $scope.model.phone && $rootScope.partternPhone.test($scope.model.phone)) {
            $scope.errorphone = false;
        } else if (selectType == "phone") {
            $scope.errorphone = true;
        }
    }

    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            //console.log($scope.model);
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            console.log('Name File: ' + extFile);
            if (extFile != "") {
                if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                    App.toastrError(caption.COM_MSG_FORMAT_PNG_JPG_JEG_GIF_BMP);
                } else {
                    var fi = document.getElementById('file');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 1024) {
                        App.toastrError(caption.HR_HR_MAN_CURD_VALIDATE_SIZE_FILE_MAX);
                    } else {
                        var fileUpload = $("#file").get(0);
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
                                if (width > 5000 || height > 5000) {
                                    App.toastrError(caption.HR_HR_MAN_CURD_VALIDATE_SIZE_IMG_MAX);
                                } else {
                                    var data = new FormData();
                                    file = fileUpload.files[0];
                                    data.append("FileUpload", file);
                                    dataservice.uploadImage(data, function (rs) {
                                        if (rs.Error) {
                                            App.toastrError(rs.Title);
                                            return;
                                        }
                                        else {
                                            $scope.model.birthday = convertDatetime($scope.model.birthday);
                                            $scope.model.identitycarddate = convertDatetime($scope.model.identitycarddate);
                                            $scope.model.picture = '/uploads/images/' + rs.Object;
                                            dataservice.insert($scope.model, function (rs) {
                                                if (rs.Error) {
                                                    App.toastrError(rs.Title);
                                                } else {
                                                    App.toastrSuccess(rs.Title);
                                                    $uibModalInstance.close();
                                                }
                                            });
                                        }
                                    })
                                }
                            };
                        }
                    }
                }
            } else {
                $scope.model.birthday = convertDatetime($scope.model.birthday);
                $scope.model.identitycarddate = convertDatetime($scope.model.identitycarddate);
                $scope.model.picture = '/images/default/no_user.png';
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
    }

    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.gender == "") {
            $scope.errorGender = true;
            mess.Status = true;
        } else {
            $scope.errorGender = false;

        }
        if (data.unit == "") {
            $scope.errorUnit = true;
            mess.Status = true;
        } else {
            $scope.errorUnit = false;

        }
        if (data.employeekind == "") {
            $scope.errorEmployeekind = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeekind = false;

        }
        if (data.employeegroup == "") {
            $scope.errorEmployeegroup = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeegroup = false;

        }
        if (data.employeetype == "") {
            $scope.errorEmployeetype = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeetype = false;

        }

        if (data.phone && !$rootScope.partternPhone.test(data.phone)) {
            $scope.errorphone = true;
            mess.Status = true;
        } else {
            $scope.errorphone = false;
        }

        return mess;
    };
    function convertDatetime(date) {
        if (date != null && date != '') {
            var array = date.split('/');
            var result = array[1] + '/' + array[0] + '/' + array[2];
            return result;
        } else {
            return '';
        }
    }
    setTimeout(function () {
        var dateBirthday = new Date();
        dateBirthday.setFullYear(dateBirthday.getFullYear() - 10);
        var dateBirthday2 = new Date();
        dateBirthday2.setFullYear(dateBirthday2.getFullYear() - 10);
        $(".date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: "01/01/1960",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
        });
        $('#date-birthday').datepicker('setEndDate', dateBirthday);
        $('#date-birthday2').datepicker('setEndDate', dateBirthday);
        //setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, DTOptionsBuilder, DTColumnBuilder, DTInstances, $filter, para) {
    $scope.form = {};

    $scope.gioitinh = [
        { value: caption.HR_HR_MAN_CURD_TXT_MEN, gender: 1 },
        { value: caption.HR_HR_MAN_CURD_TXT_WOMEN, gender: 2 }
    ];
    $scope.loaihinhnhanvien = [
        { value: caption.HR_HR_MAN_CURD_TXT_FULL_TIME, employeekind: 1 },
        { value: caption.HR_HR_MAN_CURD_TXT_PART_TIME, employeekind: 2 }
    ];
    $scope.nhom = [
        { value: caption.HR_HR_MAN_CURD_TXT_MORNING, employeegroup: 1 },
        { value: caption.HR_HR_MAN_CURD_TXT_AFTERNOON, employeegroup: 2 },
        { value: caption.HR_HR_MAN_CURD_TXT_NIGHT, employeegroup: 3 },
    ];
    $scope.kieunhanvien = [
        { value: caption.HR_HR_MAN_CURD_TXT_DRIVER, employeetype: 1 }
    ];
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        dataservice.gettreedataunit(function (result) {
            $scope.treeDataunit = result.Object;
        });
        dataservice.getItem(para, function (rs) {
            if (!rs.Error) {
                $scope.model = rs;
                $scope.model.birthday = $scope.model.birthday != null ? $filter('date')($scope.model.birthday, "dd/MM/yyyy") : '';
                $scope.model.identitycarddate = $scope.model.identitycarddate != null ? $filter('date')($scope.model.identitycarddate, "dd/MM/yyyy") : '';
                if ($scope.model.picture == '/images/default/no_image.png') {
                    $scope.model.picture = '/images/default/uploadimg.png';
                }
            }
        });
        dataservice.getPosition(function (result) {
            $scope.positionData = result;
        });
    }
    $scope.initData();
    $scope.loadImage = function () {
        var fileuploader = angular.element("#file");
        fileuploader.on('click', function () {
        });
        fileuploader.on('change', function (e) {
            var reader = new FileReader();
            reader.onload = function () {
                document.getElementById('picture').src = reader.result;
            }
            var files = fileuploader[0].files;
            var idxDot = files[0].name.lastIndexOf(".") + 1;
            var extFile = files[0].name.substr(idxDot, files[0].name.length).toLowerCase();
            if (extFile != "jpg" && extFile != "jpeg" && extFile != "png" && extFile != "gif" && extFile != "bmp") {
                App.toastrError(caption.COM_MSG_INVALID_FORMAT);
                return;
            }
            reader.readAsDataURL(files[0]);
        });
        fileuploader.trigger('click')
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "gender" && ($scope.model.gender != "" || $scope.model.gender != null)) {
            $scope.errorGender = false;
        }
        if (SelectType == "unit" && $scope.model.unit != "") {
            $scope.errorUnit = false;
        }
        if (SelectType == "employeekind" && $scope.model.employeekind != "") {
            $scope.errorEmployeekind = false;
        }
        if (SelectType == "employeegroup" && $scope.model.employeegroup != "") {
            $scope.errorEmployeegroup = false;
        }
        if (SelectType == "employeetype" && $scope.model.employeetype != "") {
            $scope.errorEmployeetype = false;
        }

        if (SelectType == "phone" && $scope.model.phone && $rootScope.partternPhone.test($scope.model.phone)) {
            $scope.errorphone = false;
        } else if (SelectType == "phone") {
            $scope.errorphone = true;
        }
    }


    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.form.editformTT.validate() && $scope.form.editform1.validate() && validationSelect($scope.model).Status == false) {
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            if (extFile !== "") {
                if (extFile !== "jpg" && extFile !== "jpeg" && extFile !== "png" && extFile !== "gif" && extFile !== "bmp") {
                    App.toastrError(caption.COM_MSG_FORMAT_PNG_JPG_JEG_GIF_BMP);
                } else {
                    var fi = document.getElementById('file');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 1024) {
                        App.toastrError(caption.HR_HR_MAN_CURD_VALIDATE_SIZE_FILE_MAX);
                    } else {
                        var fileUpload = $("#file").get(0);
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
                                if (width > 5000 || height > 5000) {
                                    App.toastrError(caption.HR_HR_MAN_CURD_VALIDATE_SIZE_IMG_MAX);
                                } else {
                                    var data = new FormData();
                                    file = fileUpload.files[0];
                                    data.append("FileUpload", file);
                                    dataservice.uploadImage(data, function (rs) {
                                        if (rs.Error) {
                                            App.toastrError(rs.Title);
                                            return;
                                        }
                                        else {
                                            $scope.model.birthday = convertDatetime($scope.model.birthday);
                                            $scope.model.identitycarddate = convertDatetime($scope.model.identitycarddate);
                                            $scope.model.picture = '/uploads/images/' + rs.Object;
                                            dataservice.update($scope.model, function (rs) {
                                                if (rs.Error) {
                                                    App.toastrError(rs.Title);
                                                } else {
                                                    App.toastrSuccess(rs.Title);
                                                    $uibModalInstance.close();
                                                }
                                            });
                                        }
                                    })
                                }
                            };
                        }
                    }
                }
            } else {
                $scope.model.birthday = convertDatetime($scope.model.birthday);
                $scope.model.identitycarddate = convertDatetime($scope.model.identitycarddate);
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

    function convertDatetime(date) {
        if (date != null && date != '') {
            var array = date.split('/');
            var result = array[1] + '/' + array[0] + '/' + array[2];
            return result;
        } else {
            return '';
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.gender == "" || data.gender == null) {
            $scope.errorGender = true;
            mess.Status = true;
        } else {
            $scope.errorGender = false;

        }
        if (data.unit == "" || data.unit == null) {
            $scope.errorUnit = true;
            mess.Status = true;
        } else {
            $scope.errorUnit = false;

        }
        if (data.employeekind == "" || data.employeekind == null) {
            $scope.errorEmployeekind = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeekind = false;

        }
        if (data.employeegroup == "" || data.employeegroup == null) {
            $scope.errorEmployeegroup = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeegroup = false;

        }
        if (data.employeetype == "" || data.employeetype == null) {
            $scope.errorEmployeetype = true;
            mess.Status = true;
        } else {
            $scope.errorEmployeetype = false;

        }

        if (data.phone && !$rootScope.partternPhone.test(data.phone)) {
            $scope.errorphone = true;
            mess.Status = true;
        } else {
            $scope.errorphone = false;
        }

        return mess;
    };
    setTimeout(function () {
        var dateBirthday = new Date();
        dateBirthday.setFullYear(dateBirthday.getFullYear() - 10);
        var dateBirthday2 = new Date();
        dateBirthday2.setFullYear(dateBirthday2.getFullYear() - 10);
        $(".date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            startDate: "01/01/1960",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
        });
        $('#date-birthday').datepicker('setEndDate', dateBirthday);
        $('#date-birthday2').datepicker('setEndDate', dateBirthday);
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 200);

    //bảng địa chỉ
    var vm = $scope;
    vm.dt = {};
    $scope.address = {};
    $scope.selected = [];
    $scope.enableeditDC = false;
    $scope.enableaddDC = true;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAllAddress(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsAddress = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableAddress",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {

            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            //contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row))($scope);
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsAddress = [];
    vm.dtColumnsAddress.push(DTColumnBuilder.newColumn('Now_Address').withTitle('{{"HR_HR_MAN_CURD_LIST_COL_HR_MAN_ADDRESS" | translate}}').withOption('sWidth', '30px').withOption('sClass', 'tcenter').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsAddress.push(DTColumnBuilder.newColumn('Phone').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_PHONE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsAddress.push(DTColumnBuilder.newColumn('action').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id_dc] = full;
        return '<button ng-click="getAddress(selected[' + full.id_dc + '])" class="btn blue btn-icon-only btn-circle btn-outline" style="width: 25px; height: 25px; padding: 0px" title="Cập nhật tài khoản"><i class="fa fa-edit"></i></button>' +
            '<button class="btn btn-icon-only red btn-circle btn-outline" style = "width: 25px; height: 25px; padding: 0px" ng-click="delete_dc(selected[' + full.id_dc + '])" title = "Xóa địa chỉ này"><i class="fa fa-trash"></i></button>';
    }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));

    vm.reloadDataAddress = reloadDataAddress;
    vm.dt.dtInstanceAddress = {};
    function reloadDataAddress(resetPaging) {
        vm.dt.dtInstanceAddress.reloadData(callbackAddress, resetPaging);
    }
    function callbackAddress(json) {

    }
    //function toggleAllAddress(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOneAddress(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    function resetInput() {
        $scope.address.id_dc = null;
        $scope.address.Permanent_Address = null;
        $scope.address.Now_Address = null;
        $scope.address.Phone = null;
        $scope.address.Start_Time = null;
        $scope.address.End_Time = null;
        $scope.enableeditDC = false;
        $scope.enableaddDC = true;
        resetValidateTimeAddress();
    }

    $scope.reloadDataAddress = function () {
        reloadDataAddress(true);
    }
    $scope.getAddress = function (selected) {
        $scope.enableeditDC = true;
        $scope.enableaddDC = false;
        dataservice.getItemAddress(selected.id_dc, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.address = rs;
                $scope.address.Start_Time = $scope.address.Start_Time != null ? $filter('date')($scope.address.Start_Time, "dd/MM/yyyy") : '';
                $scope.address.End_Time = $scope.address.End_Time != null ? $filter('date')($scope.address.End_Time, "dd/MM/yyyy") : '';
            }
        });
    }
    $scope.addDC = function () {
        validationSelectAddress($scope.address);
        if ($scope.form.addformdc.validate() && validationSelectAddress($scope.address).Status == false) {
            dataservice.insertAddress($scope.address, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadDataAddress();
                    resetInput();
                }
            });
        }

    }
    $scope.editDC = function () {
        validationSelectAddress($scope.address);
        if ($scope.form.addformdc.validate() && validationSelectAddress($scope.address).Status == false) {
            dataservice.updateAddress($scope.address, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadDataAddress();
                    resetInput();
                }
            });
        }
    }
    $scope.delete_dc = function (selected) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteAddress(selected.id_dc, function (rs) {
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
            $scope.reloadDataAddress();
        }, function () {
        });
    }


    //Bắt validate cho tab con Address
    $scope.changleSelectAddress = function (SelectType) {
        if (SelectType == "Phone" && $scope.address.Phone && $rootScope.partternPhone.test($scope.address.Phone)) {
            $scope.errorAddressPhone = false;
        } else if (SelectType == "Phone") {
            $scope.errorAddressPhone = true;
        }
    }
    function validationSelectAddress(data) {
        var mess = { Status: false, Title: "" };

        if (data.Phone && !$rootScope.partternPhone.test(data.Phone)) {
            $scope.errorAddressPhone = true;
            mess.Status = true;
        } else {
            $scope.errorAddressPhone = false;
        }
        return mess;
    };

    function loadDateAddress() {
        $("#startTime").datepicker({
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#endTime').datepicker('setStartDate', maxDate);
        });
        $("#endTime").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#startTime').datepicker('setEndDate', maxDate);
        });
        $('.end-date').click(function () {
            $('#startTime').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#endTime').datepicker('setStartDate', null);
        });
    }
    function resetValidateTimeAddress() {
        $('#startTime').datepicker('setEndDate', null);
        $('#endTime').datepicker('setStartDate', null);
    }
    setTimeout(function () {
        loadDateAddress();
    }, 50);



    //bảng  liên hệ
    $scope.contact = {};
    $scope.enableeditLH = false;
    $scope.enableaddLH = true;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAllContact(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsContact = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableLH",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
        .withOption('order', [1, 'asc'])
        .withOption('serverSide', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
            // $(this.api().table().header()).css({ 'background-color': '#1a2226', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            //contextScope.contextMenu = $scope.contextMenu1;
            $compile(angular.element(row))($scope);
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsContact = [];
    vm.dtColumnsContact.push(DTColumnBuilder.newColumn('Name').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_CONTACT_NAME" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsContact.push(DTColumnBuilder.newColumn('Relationship').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_RELATIONSHIP" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsContact.push(DTColumnBuilder.newColumn('Address').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ADDRESS" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsContact.push(DTColumnBuilder.newColumn('Phone1').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_PHONE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsContact.push(DTColumnBuilder.newColumn("null").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id_lh] = full;
        return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="getLH(selected[' + full.id_lh + '])" title="Cập nhật thông tin liên hệ"><i class="fa fa-edit"></i></button>' +
            '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="delete_lh(selected[' + full.id_lh + '])" title="Xóa thông tin liên hệ"><i class="fa fa-trash"></i></button>';
    }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));
    vm.reloadDataContact = reloadDataContact;
    vm.dt.dtInstanceContact = {};
    function reloadDataContact(resetPaging) {
        vm.dt.dtInstanceContact.reloadData(callbackContact, resetPaging);
    }
    function callbackContact(json) {

    }
    //function toggleAllContact(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOneContact(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    function resetInputContact() {
        $scope.contact.Name = null;
        $scope.contact.Relationship = null;
        $scope.contact.Job_Name = null;
        $scope.contact.Birthday = null;
        $scope.contact.Phone = null;
        $scope.contact.Fax = null;
        $scope.contact.Email = null;
        $scope.contact.Address = null;
        $scope.contact.Note = null;
        $scope.enableeditLH = false;
        $scope.enableaddLH = true;
    }
    $scope.reloadDataContact = function () {
        reloadDataContact(true);
    }

    $scope.getLH = function (selected) {
        dataservice.getItemLH(selected.id_lh, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.enableeditLH = true;
                $scope.enableaddLH = false;
                $scope.contact = rs;
                $scope.contact.Birthday = $scope.contact.Birthday != null ? $filter('date')($scope.contact.Birthday, "dd/MM/yyyy") : '';
            }
        });
    }
    $scope.addLH = function () {
        validationSelectContact($scope.contact);
        if ($scope.form.addformlienhe.validate() && validationSelectContact($scope.contact).Status == false) {
            dataservice.insertLH($scope.contact, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadDataContact();
                    resetInputContact();
                }
            });
        }

    }
    $scope.editLH = function () {
        validationSelectContact($scope.contact);
        if ($scope.form.addformlienhe.validate() && validationSelectContact($scope.contact).Status == false) {
            dataservice.updateLH($scope.contact, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadDataContact();
                    resetInputContact();
                }
            });
        }
    }
    $scope.delete_lh = function (selected) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteLH(selected.id_lh, function (rs) {
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
            $scope.reloadDataContact();
        }, function () {
        });
    }


    //Bắt validate cho tab con Contact
    $scope.changleSelectContact = function (SelectType) {
        if (SelectType == "Phone" && $scope.contact.Phone && $rootScope.partternPhone.test($scope.contact.Phone)) {
            $scope.errorContactPhone = false;
        } else if (SelectType == "Phone") {
            $scope.errorContactPhone = true;
        }
    }
    function validationSelectContact(data) {
        var mess = { Status: false, Title: "" };

        if (data.Phone && !$rootScope.partternPhone.test(data.Phone)) {
            $scope.errorContactPhone = true;
            mess.Status = true;
        } else {
            $scope.errorContactPhone = false;
        }
        return mess;
    };


    //Hợp đồng
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAllContract(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsContract = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableHD",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            //contextScope.contextMenu = $scope.contextMenu4;
            $compile(angular.element(row))($scope);
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsContract = [];

    vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Contract_Code').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_CONTRACT_CODE" | translate}}').withOption('sWidth', '10px').withOption('sClass', 'tcenter').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Start_Time').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_START_TIME" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumnsContract.push(DTColumnBuilder.newColumn('End_Time').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_END_TIME" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Salary').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_SALARY" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Insuarance').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_INSUARANCE" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    //vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Type_Money').withTitle('Loại tiền').withOption('sWidth', '30px').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumnsContract.push(DTColumnBuilder.newColumn('Active').withTitle('Trạng thái').renderWith(function (data, type) {
    //    return data == 1 ? '<label class="text-success">Còn hạn</a>' : '<a class="text-danger">Hết hạn</a>';
    //}));
    vm.dtColumnsContract.push(DTColumnBuilder.newColumn("").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id] = full;

            return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="edit_contract(' + full.id + ')" title="Sửa khoản mục"><i class="fa fa-edit"></i></button>' +
                '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="delete_contract(' + full.id + ')" title="Xóa khoản mục"><i class="fa fa-trash"></i></button>';
        }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));
    vm.reloadDataContract = reloadDataContract;
    vm.dt.dtInstanceContract = {};
    function reloadDataContract(resetPaging) {
        vm.dt.dtInstanceContract.reloadData(callbackContract, resetPaging);
    }
    function callbackContract(json) {

    }
    //function toggleAllContract(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOneContract(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}

    $scope.add_contract = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add_contract.html',
            controller: 'add_contract',
            backdrop: true,
            size: '80'
        });
        modalInstance.result.then(function (d) {
            reloadDataContract(true);
        }, function () {

        });
    }
    $scope.edit_contract = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/edit_contract.html',
            controller: 'edit_contract',
            backdrop: 'static',
            size: '80',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadDataContract(true);
        }, function () {
        });
    }
    $scope.delete_contract = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.delete_HD(id, function (rs) {
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
            reloadDataContract(true);
        }, function () {
        });
    }

    //bảng  quá trình làm việc

    //$scope.model = {
    //    Key: ''
    //}
    $scope.workprocess = {};
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAllWorkprocess(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsWorkprocess = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableQTLV",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                //d.Key = $scope.model.Key;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
        .withOption('order', [1, 'asc'])
        .withOption('serverSide', true)
        .withOption('headerCallback', function (header) {
            if (!$scope.headerCompiled) {
                $scope.headerCompiled = true;
                $compile(angular.element(header).contents())($scope);
            }
        })
        .withOption('initComplete', function (settings, json) {
            //$(this.api().table().header()).css({ 'background-color': '#1a2226', 'color': '#f9fdfd', 'font-size': '8px', 'font-weight': 'bold' });
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            //contextScope.contextMenu = $scope.contextMenu7;
            $compile(angular.element(row))($scope);
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsWorkprocess = [];
    vm.dtColumnsWorkprocess.push(DTColumnBuilder.newColumn('Start_Time1').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_START_TIME1" | translate}} ').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumnsWorkprocess.push(DTColumnBuilder.newColumn('End_Date1').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_END_DATE1" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumnsWorkprocess.push(DTColumnBuilder.newColumn('Wage_Level').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_WAGE_LEVEL" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsWorkprocess.push(DTColumnBuilder.newColumn('Salary_Ratio').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_SALARY_RATIO" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsWorkprocess.push(DTColumnBuilder.newColumn("null").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_qt] = full;
            return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="editWorkprocess(selected[' + full.id_qt + '])" title="Cập nhật quá trình"><i class="fa fa-edit"></i></button>' +
                '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="deleteWorkprocess(selected[' + full.id_qt + '])" title=" Xóa địa chỉ này"><i class="fa fa-trash"></i></button>';
        }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));
    vm.reloadDataWorkprocess = reloadDataWorkprocess;
    vm.dt.dtInstanceWorkprocess = {};
    function reloadDataWorkprocess(resetPaging) {
        vm.dt.dtInstanceWorkprocess.reloadData(callbackWorkprocess, resetPaging);
    }
    function callbackWorkprocess(json) {

    }
    function resetInputWorkprocess() {
        $scope.workprocess.Wage_Level = null;
        $scope.workprocess.Salary_Ratio = null;
        $scope.workprocess.Description1 = null;
        $scope.workprocess.End_Date1 = null;
        $scope.workprocess.Start_Time1 = null;
        $scope.workprocess.id_qt = null;
        resetValidateTimeWorkprocess();
    }
    //function toggleAllWorkprocess(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOneWorkprocess(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    $scope.reloadWorkprocess = function () {
        reloadDataWorkprocess(true);
    }
    $scope.enableeditqtlv = false;
    $scope.enableaddqtlv = true;

    $scope.editWorkprocess = function (selected) {
        $scope.enableeditqtlv = true;
        $scope.enableaddqtlv = false;
        dataservice.getItemQTLV(selected.id_qt, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.workprocess = rs.Object;
            }
        });
    }
    $scope.deleteWorkprocess = function (selected) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteQTLV(selected.id_qt, function (rs) {
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
            $scope.reloadWorkprocess();
        }, function () {
        });
    }
    $scope.submitQTLV = function () {
        if ($scope.form.addformqtlv.validate()) {
            dataservice.insertQTLV($scope.workprocess, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadWorkprocess();
                    resetInputWorkprocess();
                }
            });
        }

    }
    $scope.editQTLV = function () {
        if ($scope.form.addformqtlv.validate()) {
            dataservice.updateQTLV($scope.workprocess, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadWorkprocess();
                    resetInputWorkprocess();
                }
                $scope.enableeditqtlv = false;
                $scope.enableaddqtlv = true;
            });
        }
    }
    function loadDateWorkprocess() {
        $("#Start_Time_Workprocess").datepicker({
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#End_Time_Workprocess').datepicker('setStartDate', maxDate);
        });
        $("#End_Time_Workprocess").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#Start_Time_Workprocess').datepicker('setEndDate', maxDate);
        });
    }
    function resetValidateTimeWorkprocess() {
        $('#Start_Time_Workprocess').datepicker('setEndDate', null);
        $('#End_Time_Workprocess').datepicker('setStartDate', null);
    }
    setTimeout(function () {
        loadDateWorkprocess();
    }, 50);


    //bảng  bằng cấp chứng chỉ
    $scope.certificate = {};
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAllCertificate(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsCertificate = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableBCCC",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            //contextScope.contextMenu = $scope.contextMenu3;
            $compile(angular.element(row))($scope);
            //$compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsCertificate = [];
    vm.dtColumnsCertificate.push(DTColumnBuilder.newColumn('Education_Name').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_EDUCATION_NAME" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsCertificate.push(DTColumnBuilder.newColumn('Result').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_RESULT" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsCertificate.push(DTColumnBuilder.newColumn("null").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_ACTION" | translate}}').notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_bccc] = full;
            return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="editCertificate(selected[' + full.id_bccc + '])" title="Cập nhật bằng cấp"><i class="fa fa-edit"></i></button>' +
                '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="deleteCertificate(selected[' + full.id_bccc + '])" title=" Xóa bằng cấp"><i class="fa fa-trash"></i></button>';
        }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));

    vm.reloadDataCertificate = reloadDataCertificate;
    vm.dt.dtInstanceCertificate = {};
    function reloadDataCertificate(resetPaging) {
        vm.dt.dtInstanceCertificate.reloadData(callbackCertificate, resetPaging);
    }
    function callbackCertificate(json) {

    }
    function resetInputCertificate() {
        $scope.certificate.Education_Name = '';
        $scope.certificate.Start_Time3 = null;
        $scope.certificate.End_Time3 = null;
        $scope.certificate.Result = '';
        $scope.certificate.Certificate_Name = '';
        $scope.certificate.Received_Place = '';
        $scope.certificate.Traing_Place = '';
        $scope.certificate.Info_Details1 = '';
    }
    //function toggleAllCertificate(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOneCertificate(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    $scope.reloadCertificate = function () {
        reloadDataCertificate(true);
    }
    $scope.enableeditbccc = false;
    $scope.enableaddbccc = true;

    $scope.editCertificate = function (selected) {
        $scope.enableeditbccc = true;
        $scope.enableaddbccc = false;
        dataservice.getItemBCCC(selected.id_bccc, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.certificate = rs[0];
            }
        });
    }
    $scope.deleteCertificate = function (selected) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteBCCC(selected.id_bccc, function (rs) {
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
            $scope.reloadCertificate();
        }, function () {
        });
    }
    $scope.submitBCCC = function () {
        if ($scope.form.addformBC.validate()) {
            dataservice.insertBCCC($scope.certificate, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadCertificate();
                    resetInputCertificate();
                }
            });
        }

    }
    $scope.editBCCC = function () {
        if ($scope.form.addformBC.validate()) {
            dataservice.updateBCCC($scope.certificate, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadCertificate();
                    resetInputCertificate();
                    $scope.enableeditbccc = false;
                    $scope.enableaddbccc = true;
                }
            });
        }
    }

    //bảng  quy trình công việc
    $scope.workflow = {};
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptionsWorkflow = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/HREmployee/JTableQTCV",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            contextScope.contextMenu = $scope.contextMenu2;
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });

    vm.dtColumnsWorkflow = [];
    vm.dtColumnsWorkflow.push(DTColumnBuilder.newColumn('Name_Job').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_NAME_JOB" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsWorkflow.push(DTColumnBuilder.newColumn('Working_Process').withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_WORKING_PROCESS" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumnsWorkflow.push(DTColumnBuilder.newColumn("id_cv").withTitle('{{"HR_HR_MAN_LIST_COL_HR_MAN_WORKING_PROCESS" | translate}}').notSortable()
        .renderWith(function (data, type, full, meta) {
            $scope.selected[full.id_cv] = full;
            return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="edit_cv(selected[' + full.id_cv + '])" title="Cập nhật quy trình"><i class="fa fa-edit"></i></button>' +
                '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="delete_cv(selected[' + full.id_cv + '])" title=" Xóa quy trình này"><i class="fa fa-trash"></i></button>';
        }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));

    vm.reloadDataWorkflow = reloadDataWorkflow;
    vm.dt.dtInstanceWorkflow = {};
    function reloadDataWorkflow(resetPaging) {
        vm.dt.dtInstanceWorkflow.reloadData(callbackWorkflow, resetPaging);
    }
    function callbackWorkflow(json) {

    }
    function resetInputWorkflow() {
        $scope.workflow.Name_Job = '';
        $scope.workflow.Working_Process = '';
        $scope.workflow.Description2 = '';
        $scope.workflow.Info_Details = '';
    }
    //function toggleAll(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOne(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    $scope.reloadWorkflow = function () {
        reloadDataWorkflow(true);
    }
    $scope.reloadNoResetPageWorkflow = function () {
        reloadDataWorkflow(false);
    }
    $scope.enableeditqtcv = false;
    $scope.enableaddqtcv = true;

    $scope.edit_cv = function (selected) {
        $scope.enableeditqtcv = true;
        $scope.enableaddqtcv = false;
        dataservice.getItemQTCV(selected.id_cv, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.workflow = rs[0];
            }
        });
    }
    $scope.delete_cv = function (selected) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteQTCV(selected.id_cv, function (rs) {
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
            $scope.reloadNoResetPageWorkflow();
        }, function () {
        });
    }

    $scope.submitQTCV = function () {
        if ($scope.form.addformQTCV1.validate()) {
            dataservice.insertQTCV($scope.workflow, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadWorkflow();
                    resetInputWorkflow();
                }
            });
        }

    }
    $scope.editQTCV = function () {
        if ($scope.form.addformQTCV1.validate()) {
            dataservice.updateQTCV($scope.workflow, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reloadNoResetPageWorkflow();
                    resetInputWorkflow();
                    $scope.enableeditqtcv = false;
                    $scope.enableaddqtcv = true;
                }
            });
        }
    }










    ////bảng  hợp đồng



    ////KPI
    //var vm6 = $scope;

    //$scope.model = {
    //    Key: ''
    //}
    //var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    //vm6.dtOptions6 = DTOptionsBuilder.newOptions()
    //    .withOption('ajax', {
    //        url: "",
    //        beforeSend: function (jqXHR, settings) {
    //            App.blockUI({
    //                target: "#contentMain",
    //                boxed: true,
    //                message: 'loading...'
    //            });
    //        },
    //        type: 'POST',
    //        data: function (d) {
    //            d.Key = $scope.model.Key;
    //        },
    //        complete: function () {
    //            App.unblockUI("#contentMain");
    //        }
    //    })
    //    .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
    //    .withDataProp('data').withDisplayLength(5)
    //    .withOption('order', [1, 'asc'])
    //    .withOption('serverSide', true)
    //    .withOption('headerCallback', function (header) {
    //        if (!$scope.headerCompiled) {
    //            $scope.headerCompiled = true;
    //            $compile(angular.element(header).contents())($scope);
    //        }
    //    })
    //    .withOption('initComplete', function (settings, json) {
    //    })
    //    .withOption('createdRow', function (row, data, dataIndex) {
    //        const contextScope = $scope.$new(true);
    //        contextScope.data = data;
    //        contextScope.contextMenu = $scope.contextMenu3;
    //        $compile(angular.element(row))($scope);
    //        $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
    //    });

    //vm6.dtColumns6 = [];
    //vm6.dtColumns6.push(DTColumnBuilder.newColumn('Education_Name').withTitle('Tên khóa đào tạo').withOption('sWidth', '30px').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm6.dtColumns6.push(DTColumnBuilder.newColumn('Result').withTitle('Kết quả').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm6.dtColumns6.push(DTColumnBuilder.newColumn("null").withTitle('Tác vụ').notSortable()
    //    .renderWith(function (data, type, full, meta) {
    //        $scope.selected[full.id_bccc] = full;
    //        return '<button class="btn btn-icon-only btn-circle btn-outline blue" style="width: 25px; height: 25px; padding: 0px" ng-click="edit_bccc(selected[' + full.id_bccc + '])" title="Cập nhật bằng cấp"><i class="fa fa-edit"></i></button>' +
    //            '<button class="btn btn-icon-only btn-circle btn-outline red" style="width: 25px; height: 25px; padding: 0px" ng-click="delete_bccc(selected[' + full.id_bccc + '])" title=" Xóa bằng cấp"><i class="fa fa-trash"></i></button>';
    //    }).withOption('sWidth', '120px').withOption('sClass', 'tcenter'));

    //vm6.reloadData = reloadData6;
    //vm6.dtInstance6 = {};
    //function reloadData6(resetPaging) {
    //    vm4.dtInstance4.reloadData(callback, resetPaging);
    //}
    //function callback(json) {

    //}
    //function toggleAll(selectAll, selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            selectedItems[id] = selectAll;
    //        }
    //    }
    //}
    //function toggleOne(selectedItems) {
    //    for (var id in selectedItems) {
    //        if (selectedItems.hasOwnProperty(id)) {
    //            if (!selectedItems[id]) {
    //                vm.selectAll = false;
    //                return;
    //            }
    //        }
    //    }
    //    vm.selectAll = true;
    //}
    //$scope.reload6 = function () {
    //    reloadData6(true);
    //}


});



app.controller('add_contract', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice) {
    $scope.contract = {
        Contract_Code: '',
        LaborBook_Code: '',
        Insuarance: '',
        Dates_of_pay: '',
        Place_Work: '',
        Exp_time_work: '',
        Salary_Ratio: '',
        Payment: '',
        Contract_Type: '',
        Work_Content: '',
        Other_Agree: '',
        Signer_Id: '',
        Salary: '',
        Start_Time: '',
        End_Time: '',
        Allowance: '',
        Bonus: '',
        Tools_Work: '',
        Type_Money: '',
        Info_Insuarance: '',
        File: ''
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Branch" && $scope.model.BranchId != "") {
            $scope.errorBranch = false;
        }
    }
    $scope.submitHD = function () {
        if ($scope.addformhd.validate()) {
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            if (extFile != "") {
                if (extFile != "docx" && extFile != "pdf") {
                    App.toastrError("Định dạng bắt buộc là: docx,pdf!");
                } else {
                    var fi = document.getElementById('fileContract');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 102400) {
                        App.toastrError("Kích thước tối đa của tệp là: 1 MB !");
                    } else {
                        var fileUpload = $("#fileContract").get(0);
                        var reader = new FileReader();
                        reader.readAsDataURL(fileUpload.files[0]);
                        reader.onloadend = function (e) {
                            var data = new FormData();
                            file = fileUpload.files[0];
                            data.append("FileUpload", file);
                            dataservice.uploadFile(data, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                    return;
                                }
                                else {
                                    $scope.contract.File = '/uploads/files/' + rs.Object;
                                    dataservice.insert_HD($scope.model, function (rs) {
                                        if (rs.Error) {
                                            App.toastrError(rs.Title);
                                        } else {
                                            App.toastrSuccess(rs.Title);
                                            $uibModalInstance.close();
                                        }
                                    });
                                }
                            })
                        }
                    }
                }
            } else {
                dataservice.insert_HD($scope.contract, function (rs) {
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
    setTimeout(function () {
        $(".date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {

        });
        setModalDraggable('.modal-dialog');
    }, 200);

});
app.controller('edit_contract', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        dataservice.getItemHD(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.contract = rs;
            }
        });
    }
    $scope.initData();
    $scope.submitHD = function () {
        if ($scope.editformhd.validate()) {
            var fileName = $('input[type=file]').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            console.log('Name File: ' + extFile);
            if (extFile != "") {
                if (extFile != "docx" && extFile != "pdf") {
                    App.toastrError(caption.COM_MSG_FORMAT_DOCX_PDF);
                } else {
                    var fi = document.getElementById('fileContract');
                    var fsize = (fi.files.item(0).size) / 1024;
                    if (fsize > 1024) {
                        App.toastrError(caption.HR_HR_MAN_CURD_VALIDATE_SIZE_FILE_MAX);
                    } else {
                        var fileUpload = $("#fileContract").get(0);
                        var reader = new FileReader();
                        reader.readAsDataURL(fileUpload.files[0]);
                        reader.onloadend = function (e) {
                            var data = new FormData();
                            file = fileUpload.files[0];
                            data.append("FileUpload", file);
                            dataservice.uploadFile(data, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                    return;
                                }
                                else {
                                    $scope.contract.File = '/uploads/files/' + rs.Object;
                                    dataservice.upload_HD($scope.model, function (rs) {
                                        if (rs.Error) {
                                            App.toastrError(rs.Title);
                                        } else {
                                            App.toastrSuccess(rs.Title);
                                            $uibModalInstance.close();
                                        }
                                    });
                                }
                            })
                        }
                    }
                }
            } else {
                dataservice.upload_HD($scope.model, function (rs) {
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
    setTimeout(function () {
        $(".date").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
        });
        setModalDraggable('.modal-dialog');
    }, 200);
});




