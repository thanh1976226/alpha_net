var ctxfolder = "/views/admin/supplier";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngCookies", "pascalprecht.translate","ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select', 'ui.tinymce']);
angular.module('number', [])
    .directive('number', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                ctrl.$parsers.push(function (input) {
                    if (input == undefined) return ''
                    var inputNumber = input.toString().replace(/[^0-9]/g, '');
                    if (inputNumber != input) {
                        ctrl.$setViewValue(inputNumber);
                        ctrl.$render();
                    }
                    return inputNumber;
                });
            }
        };
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
        insert: function (data, callback) {
            $http.post('/Admin/Supplier/Insert/', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/Supplier/Update/', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/Supplier/Delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/Supplier/GetItem/' + data).success(callback);
        },
        getItemAdd: function (data, callback) {
            $http.get('/Admin/Supplier/GetItemAdd?code=' + data).success(callback);
        },

        insertFile: function (data, callback) {
            $http.post('/Admin/Supplier/InsertFile/', data).success(callback);
        },
        deleteFile: function (data, callback) {
            $http.post('/Admin/Supplier/DeleteFile/' + data).success(callback);
        },
        updateFile: function (data, callback) {
            $http.post('/Admin/Supplier/UpdateFile/', data).success(callback);
        },
        getFile: function (data, callback) {
            $http.get('/Admin/Supplier/GetFile/' + data).success(callback);
        },
        jtreeRepository: function (callback) {
            $http.post('/Admin/EDMSRepository/JtreeRepository').success(callback);
        },

        insertContact: function (data, callback) {
            $http.post('/Admin/Supplier/InsertContact/', data).success(callback);
        },
        deleteContact: function (data, callback) {
            $http.post('/Admin/Supplier/DeleteContact/' + data).success(callback);
        },
        updateContact: function (data, callback) {
            $http.post('/Admin/Supplier/UpdateContact/', data).success(callback);
        },
        getContact: function (data, callback) {
            $http.get('/Admin/Supplier/GetContact/' + data).success(callback);
        },

        insertExtend: function (data, callback) {
            $http.post('/Admin/Supplier/InsertExtend/', data).success(callback);
        },
        deleteExtend: function (data, callback) {
            $http.post('/Admin/Supplier/DeleteExtend/' + data).success(callback);
        },
        updateExtend: function (data, callback) {
            $http.post('/Admin/Supplier/UpdateExtend/', data).success(callback);
        },
        getExtend: function (data, callback) {
            $http.get('/Admin/Supplier/GetExtend/' + data).success(callback);
        },

        uploadFile: function (data, callback) {
            submitFormUpload('/Admin/Supplier/UploadFile/', data, callback);
        },
        uploadImage: function (data, callback) {
            submitFormUpload('/Admin/Supplier/UploadImage/', data, callback);
        },

        getSupplierGroup: function (callback) {
            $http.post('/Admin/Supplier/GetSupplierGroup/').success(callback);
        },
        getSupplierStatus: function (callback) {
            $http.post('/Admin/Supplier/GetSupplierStatus/').success(callback);
        },


        //CardJob
        getBoards: function (callback) {
            $http.post('/Admin/CardJob/GetBoards/').success(callback);
        },

        //addCardJob: function (data, callback) {
        //    $http.post('/Admin/Supplier/AddCardJob/', data).success(callback);
        //},

        getTeams: function (callback) {
            $http.post('/Admin/CardJob/GetTeams').success(callback);
        },
        getBoardByTeam: function (TeamCode, callback) {
            $http.post('/Admin/Supplier/GetBoards/?TeamCode=' + TeamCode).success(callback);
        },
        getLists: function (BoardCode, callback) {
            $http.post('/Admin/Supplier/GetLists/?BoardCode=' + BoardCode).success(callback);
        },
        getCards: function (ListCode, callback) {
            $http.post('/Admin/Supplier/GetCards/?ListCode=' + ListCode).success(callback);
        },
        addCardRelative: function (data, callback) {
            $http.post('/Admin/Supplier/AddCardRelative/', data).success(callback);
        },

        getCardsByList: function (data, callback) {
            $http.post('/Admin/CardJob/GetCardsByList/?ListCode=' + data).success(callback);
        },
        getCardDetail: function (data, callback) {
            $http.post('/Admin/CardJob/GetCardDetail/?CardCode=' + data).success(callback);
        },
        insertCard: function (data, callback) {
            $http.post('/Admin/CardJob/InsertCard/', data).success(callback);
        },
        deleteCard: function (id, callback) {
            $http.post('/Admin/CardJob/DeleteCard/' + id).success(callback);
        },
        getLevels: function (callback) {
            $http.post('/Admin/CardJob/GetLevels/').success(callback);
        },
        getCardMember: function (CardCode, callback) {
            $http.post('/Admin/CardJob/GetCardMember/?CardCode=' + CardCode).success(callback);
        },
        getCardTeam: function (CardCode, callback) {
            $http.post('/Admin/CardJob/GetCardTeam/?CardCode=' + CardCode).success(callback);
        },
        changeWorkType: function (cardCode, type, callback) {
            $http.post('/Admin/CardJob/ChangeWorkType/?CardCode=' + cardCode + '&Type=' + type).success(callback);
        },
        changeCardStatus: function (cardCode, status, callback) {
            $http.post('/Admin/CardJob/ChangeCardStatus/?CardCode=' + cardCode + '&Status=' + status).success(callback);
        },
        changeCardLevel: function (cardCode, level, callback) {
            $http.post('/Admin/CardJob/ChangeCardLevel/?CardCode=' + cardCode + '&Level=' + level).success(callback);
        },
        getWorkType: function (callback) {
            $http.post('/Admin/CardJob/GetWorkType/').success(callback);
        },
        getStatus: function (callback) {
            $http.post('/Admin/CardJob/GetStatus/').success(callback);
        },
        updateCardName: function (cardId, newName, callback) {
            $http.post('/Admin/CardJob/UpdateCardName/?CardID=' + cardId + '&NewName=' + newName).success(callback);
        },
        updateCardDescription: function (data, callback) {
            $http.post('/Admin/CardJob/UpdateCardDescription', data).success(callback);
        },
        updateCardLabel: function (cardCode, label, callback) {
            $http.post('/Admin/CardJob/UpdateCardLabel/?CardCode=' + cardCode + "&Label=" + label).success(callback);
        },
        changeListCard: function (cardCode, listCode, callback) {
            $http.post('/Admin/CardJob/ChangeListCard/?CardCode=' + cardCode + "&ListCode=" + listCode).success(callback);
        },
        updateDueDate: function (cardCode, duedate, callback) {
            $http.post('/Admin/CardJob/UpdateDuedate/?CardCode=' + cardCode + '&Duedate=' + duedate).success(callback);
        },


        addMember: function (data, callback) {
            $http.post('/Admin/CardJob/AddMember/', data).success(callback);
        },

        addCheckList: function (cardCode, data, callback) {
            $http.post('/Admin/CardJob/AddCheckList/?CardCode=' + cardCode + '&Title=' + data).success(callback);
        },
        getCheckList: function (cardCode, callback) {
            $http.post('/Admin/CardJob/GetCheckLists/?CardCode=' + cardCode).success(callback);
        },
        deleteCheckList: function (CheckCode, callback) {
            $http.post('/Admin/CardJob/DeleteCheckList/?CheckCode=' + CheckCode).success(callback);
        },
        changeCheckTitle: function (checkCode, title, callback) {
            $http.post('/Admin/CardJob/ChangeCheckTitle/?CheckCode=' + checkCode + '&Title=' + title).success(callback);
        },
        sortListByStatus: function (boardCode, orther, callback) {
            $http.post('/Admin/CardJob/SortListByStatus/?BoardCode=' + boardCode + '&Orther=' + orther).success(callback);
        },

        addCheckItem: function (checkCode, data, callback) {
            $http.post('/Admin/CardJob/AddCheckItem/?CheckCode=' + checkCode + '&Title=' + data).success(callback);
        },
        getCheckItem: function (checkCode, callback) {
            $http.post('/Admin/CardJob/GetCheckItem/?CheckCode=' + checkCode).success(callback);
        },
        changeChkItemStatus: function (itemId, callback) {
            $http.post('/Admin/CardJob/ChangeItemStatus/?Id=' + itemId).success(callback);
        },
        changeChkItemTitle: function (itemId, title, callback) {
            $http.post('/Admin/CardJob/ChangeItemTitle/?Id=' + itemId + '&Title=' + title).success(callback);
        },
        deleteCheckItem: function (itemid, callback) {
            $http.post('/Admin/CardJob/DeleteCheckItem/?Id=' + itemid).success(callback);
        },

        addComment: function (cardCode, content, callback) {
            $http.post('/Admin/CardJob/AddComment/?CardCode=' + cardCode + '&Content=' + content).success(callback);
        },
        getComment: function (cardCode, callback) {
            $http.post('/Admin/CardJob/GetComments/?CardCode=' + cardCode).success(callback);
        },
        deleteComment: function (cmtId, callback) {
            $http.post('/Admin/CardJob/DeleteComment/?CommentId=' + cmtId).success(callback);
        },
        updateComment: function (cmtId, content, callback) {
            $http.post('/Admin/CardJob/UpdateComment/?CmtId=' + cmtId + '&Content=' + content).success(callback);
        },

        addAttachment: function (data, callback) {
            $http.post('/Admin/CardJob/AddAttachment/', data).success(callback);
        },
        uploadAttachment: function (data, callback) {
            submitFormUpload('/Admin/CardJob/UploadFile/', data, callback);
        },
        getAttachment: function (cardCode, callback) {
            $http.post('/Admin/CardJob/GetAttachment/?CardCode=' + cardCode).success(callback);
        },
        deleteAttachment: function (fileCode, callback) {
            $http.post('/Admin/CardJob/DeleteAttachment/?FileCode=' + fileCode).success(callback);
        },

        save: function (data, callback) {
            $http.post('/Admin/CardJob/SaveData/', data).success(callback);
        },
        getObjDependency: function (callback) {
            $http.post('/Admin/CardJob/GetObjDependency').success(callback);
        },
        setObjectRelative: function (data, callback) {
            $http.post('/Admin/CardJob/SetObjectRelative/', data).success(callback);
        },
        advanceSearch: function (data, callback) {
            $http.post('/Admin/CardJob/AdvanceSearch/', data).success(callback);
        },
        getObjCode: function (objDepen, callback) {
            $http.post('/Admin/CardJob/GetObjCode/?Dependency=' + objDepen).success(callback);
        },
        insertCardDependency: function (data, callback) {
            $http.post('/Admin/CardJob/InsertCardDependency/', data).success(callback);
        },
        getCardDependency: function (CardCode, callback) {
            $http.post('/Admin/CardJob/GetObjectRelative/?CardCode=' + CardCode).success(callback);
        },
        deleteCardDependency: function (dependencyId, callback) {
            $http.post('/Admin/CardJob/DeleteCardDependency/?Id=' + dependencyId).success(callback);
        },
        getRelative: function (callback) {
            $http.post('/Admin/CardJob/GetRelative/').success(callback);
        },
        getUser: function (callback) {
            $http.post('/Admin/EDMSContract/GetUser').success(callback);
        },

        getSupplierArea: function (callback) {
            $http.post('/Admin/Supplier/getListSupplierArea').success(callback);
        },
        getSupplierType: function (callback) {
            $http.post('/Admin/Supplier/getListSupplierType').success(callback);
        },

        getSupplierRole: function (callback) {
            $http.post('/Admin/Supplier/getListSupplierRole').success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $compile, $uibModal, dataservice, $translate, $cookies) {
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
        $rootScope.checkData = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var partternTelephone = /[0-9]/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.SupCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.SUP_CURD_VALIDATE_CHARACTERS_SPACE_SUPCODE, "<br/>");
            }
            return mess;
        }
        $rootScope.checkDataMore = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.ext_code)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.SUP_CURD_VALIDATE_CHARACTERS_SPACE_EXTCODE, "<br/>");
            }
            return mess;
        }
        $rootScope.checkDataContact = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.ext_code)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.SUP_CURD_VALIDATE_CHARACTERS_SPACE_EXTCODE, "<br/>");
            }
            return mess;
        }
        //$rootScope.checkTelephone = function (data) {
        //    debugger
        //    var partternTelephone = /[0-9]/g;
        //    var mess = { Status: false, Title: "" };
        //    if (!partternTelephone.test(data) && data != null) {
        //        mess.Status = true;
        //        mess.Title = mess.Title.concat(" - ", "Số điện thoại phải là chữ số [0-9]!", "<br/>");
        //    }
        //    return mess;
        //}
        jQuery.extend(jQuery.validator.messages, {

            email: caption.SUP_CURD_VALIDATE_EMAIL,

        });
        $rootScope.validationOptions = {
            rules: {
                SupCode: {
                    required: true,
                    maxlength: 100
                },
                SupName: {
                    required: true,
                    maxlength: 255
                },
                //Email: {
                //    required: true
                //},

                //Mobile: {
                //    required: true
                //},
                Address: {
                    required: true
                },
                TaxCode: {
                    required: true
                }
            },
            messages: {
                SupCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_LBL_CODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_LBL_CODE).replace("{1}", "100")
                },
                SupName: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_LBL_NAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_LBL_NAME).replace("{1}", "255")
                },
                //Email: {
                //    required: 'Nhập địa chỉ Email!'
                //},

                //Mobile: {
                //    required: 'Nhập số điện thoại di động!'
                //},
                Address: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_LBL_ADDRESS),
                },
                TaxCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_LBL_TAX_CODE),
                }

            }
        }
        $rootScope.validationOptionsmore = {
            rules: {
                ext_code: {
                    required: true,
                    maxlength: 100
                },
                ext_value: {
                    required: true,
                    maxlength: 500
                },

            },
            messages: {
                ext_code: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_CODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_CODE).replace("{1}", "100")
                },
                ext_value: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_VALUE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_VALUE).replace("{1}", "500")
                },


            }
        }
        $rootScope.validationOptionsContact = {
            rules: {
                ContactName: {
                    required: true,
                    maxlength: 255
                },
                Email: {
                    required: true
                },
                Mobile: {
                    required: true,
                    maxlength: 100,
                    //minlength: 10,
                },
                Title: {
                    maxlength: 1000
                },
                InChargeOf: {
                    maxlength: 1000
                },
                Address: {
                    maxlength: 500
                },
                Telephone: {
                    maxlength: 100
                },
                Fax: {
                    maxlength: 100
                },
                Facebook: {
                    maxlength: 100
                },
                GooglePlus: {
                    maxlength: 100
                },
                Twitter: {
                    maxlength: 100
                },
                Skype: {
                    maxlength: 100
                },
                Note: {
                    maxlength: 1000
                },
            },
            messages: {
                ContactName: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_CONTACTNAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_CONTACTNAME).replace("{1}", "255")
                },
                Email: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_EMAIL),
                },
                Mobile: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_MOBIEPHONE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_MOBIEPHONE).replace("{1}", "100")
                    //minlength: "Số di động phải có 10 số trở lên!"
                },
                Title: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_POSITION).replace("{1}", "1000")
                },
                InChargeOf: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_INCHARGEOF).replace("{1}", "1000")
                },
                Address: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_ADDRESS).replace("{1}", "500")
                },
                Telephone: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_TELEPHONE).replace("{1}", "100")
                },
                Fax: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_FAX).replace("{1}", "100")
                },
                Facebook: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_FACEBOOK).replace("{1}", "100")
                },
                GooglePlus: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_GOOGLEPLUS).replace("{1}", "100")
                },
                Twitter: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_TWITTER).replace("{1}", "100")
                },
                Skype: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_SKYPE).replace("{1}", "100")
                },
                Note: {
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.SUP_CURD_TAB_CONTACT_CURD_LBL_NOTE).replace("{1}", "1000")
                },
            }
        }
        $rootScope.validationOptionsAttr = {
            rules: {
                Code: {
                    required: true,

                },
                Value: {
                    required: true,

                }
            },
            messages: {
                Code: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_CODE),
                },
                Value: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.SUP_CURD_TAB_MORE_CURD_LBL_NAME),
                }
            }
        }
        $rootScope.validationOptionsFile = {
            rules: {
                Title: {
                    required: true
                },
                File: {
                    required: true
                }
            },
            messages: {
                Title: {
                    required: 'Tiêu đề yêu cầu bắt buộc'
                },
                File: {
                    required: 'Tệp tin yêu cầu bắt buộc'
                }
            }
        }

    });
    

    //Khởi tạo các mảng dùng chung
    $rootScope.Categories = [];
    dataservice.getSupplierGroup(function (rs) {
        $rootScope.SupplierGroup = rs;
    })
    dataservice.getSupplierStatus(function (rs) {
        $rootScope.StatusData = rs;
    })
    dataservice.jtreeRepository(function (result) {
        if (!result.Error) {
            for (var i = 0; i < result.length; i++) {
                var data = {
                    Value: result[i].ReposID,
                    Name: result[i].ReposName,
                }
                $rootScope.Categories.push(data);
            }
        }
    });
    $rootScope.map = [];
    $rootScope.zoomMap = 16;
    $rootScope.latDefault = 21.0277644;
    $rootScope.lngDefault = 105.83415979999995;
    $rootScope.addressDefault = 'Hanoi, Hoàn Kiếm, Hanoi, Vietnam';


    dataservice.getSupplierArea(function (rs) {
        $rootScope.SupplierAreas = rs.Object;
    });
    dataservice.getSupplierType(function (rs) {
        $rootScope.SupplierTypes = rs.Object;
    });
    dataservice.getSupplierRole(function (rs) {
        $rootScope.SupplierRoles = rs.Object;
    });
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
        .when('/add', {
            templateUrl: ctxfolder + '/add.html',
            controller: 'add'
        })
        .when('/edit', {
            templateUrl: ctxfolder + '/edit.html',
            controller: 'edit'
        })
        .when('/contactAdd', {
            templateUrl: ctxfolder + '/contactAdd.html',
            controller: 'contactAdd'
        })
        .when('/fileAdd', {
            templateUrl: ctxfolder + '/fileAdd.html',
            controller: 'fileAdd'
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
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.model = {
        Suppliername: '',
        Supplieremail: '',
        Suppliergroup: '',
        Status: ''
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Supplier/JTable",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.SupplierCode = $scope.model.SupplierCode;
                d.SupplierName = $scope.model.SupplierName;
                d.SupplierEmail = $scope.model.SupplierEmail;
                d.Address = $scope.model.Address;
                d.Phone = $scope.model.Phone;
                d.SupplierGroup = $scope.model.SupplierGroup;
                d.Status = $scope.model.Status;
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
        })
        .withOption('createdRow', function (row, data, dataIndex) {
            const contextScope = $scope.$new(true);
            contextScope.data = data;
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row).contents())($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("SupID").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.SupID] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.SupID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('SupCode').withTitle('{{ "SUP_LIST_COL_SUP_CODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('SupName').withTitle('{{ "SUP_LIST_COL_SUP_NAME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Mobile').withTitle('{{ "SUP_LIST_COL_SUP_PHONE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Email').withTitle('Thư điện tử').renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('SupGroup').withTitle('{{ "SUP_LIST_COL_SUP_GROUP" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Address').withTitle('Địa chỉ').renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Status').withTitle('{{ "SUP_LIST_COL_SUP_STATUS" | translate }}').notSortable().renderWith(function (data, type) {
        if (data == "SUPPLIER_ACTIVE") {
            return '<span class="text-success"> Hoạt động</span>';
        } else if (data == "SUPPLIER_DEACTIVE") {
            return '<span class="text-danger">Không hoạt động</span>';
        } else {
            return '<span class="text-warning">Không xác định</span>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.SupID + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.SupID + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }));
    vm.reloadData = reloadData;
    vm.dtInstance = {};

    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
    };

   
    function callback(json) {

    };
    function toggleAll(selectAll, selectedItems) {
        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                selectedItems[id] = selectAll;
            }
        }
    };
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
    };

    $scope.search = function () {
        reloadData(true);
    };
    $scope.reload = function () {
        reloadData(true);
    };
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    };
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: true,
            size: '70'
        });
        modalInstance.result.then(function (d) {
            reloadData(true);
        }, function () { });
    };
    $scope.edit = function (id) {
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
        }, function () { });
    };
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
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
    };
    $scope.addCardJob = function () {
        var modalInstance = $uibModal.open({
            templateUrl: "/views/admin/cardJob/edit-tag.html",
            controller: 'edit-card',
            size: '60',
            resolve: {
                para: function () {
                    return "";
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.reload();
        }, function () { });
    };
    function initAutocomplete() {
        var defaultBounds = new google.maps.LatLngBounds(new google.maps.LatLng(-33.8902, 151.1759), new google.maps.LatLng(-33.8474, 151.2631));
        var options = {
            bounds: defaultBounds,
            types: ['geocode']
        };

        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('Address'), options);
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
        initAutocomplete();
        showHideSearch();
    }, 200);
});
app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter, $window) {
    $rootScope.SupplierId = '';
    $scope.model = {
        GoogleMap: '',
        SupGroup: '',
        Status: '',
        Address: '',
        CusType: '',
        Role: '',
        Area:''
    }
    $scope.openMap = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/googleMap.html',
            controller: 'googleMap',
            backdrop: true,
            size: '80',
            resolve: {
                para: function () {
                    return '';
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.model.LocationText = $rootScope.map.Address;
            if ($rootScope.map.Lat != '' && $rootScope.map.Lng != '') {
                $scope.model.GoogleMap = $rootScope.map.Lat + ',' + $rootScope.map.Lng;
                $scope.model.Address = $rootScope.map.Address;
            }
            $rootScope.map = [];
        }, function () { });
    }
    $scope.tinymceOptions = {
        plugins: 'print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern help',
        toolbar: "formatselect bold italic strikethrough forecolor link  alignleft aligncenter alignright alignjustify numlist bullist outdent indent removeformat"
    };


    $scope.chkSubTab = function () {
        if ($rootScope.SupplierId == '') {
            App.toastrError("Vui lòng tạo trước nhà cung cấp!");
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Area" && $scope.model.Area != "") {
            $scope.errorArea = false;
        }
        if (SelectType == "SupGroup" && $scope.model.SupGroup != "") {
            $scope.errorSupGroup = false;
        }
        if (SelectType == "Role" && $scope.model.Role != "") {
            $scope.errorRole = false;
        }
        if (SelectType == "CusType" && $scope.model.CusType != "") {
            $scope.errorCusType = false;
        }

        if (SelectType == "Status" && $scope.model.Status != "") {
            $scope.errorStatus = false;
        }

        if (SelectType == "Mobile" && $scope.model.Mobile && $rootScope.partternPhone.test($scope.model.Mobile)) {
            $scope.errorMobile = false;
        } else if (SelectType == "Mobile") {
            $scope.errorMobile = true;
        }
        if (SelectType == "Fax" && $scope.model.Fax && $rootScope.partternPhone.test($scope.model.Fax)) {
            $scope.errorFax = false;
        } else if (SelectType == "Fax") {
            $scope.errorFax = true;
        }

    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            var msg = $rootScope.checkData($scope.model);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            var mmsg = $rootScope.checkTelephone($scope.model.Mobile);
            if (mmsg.Status) {
                App.toastrError(mmsg.Title);
                return;
            }
            var mmsg1 = $rootScope.checkTelephone($scope.model.Telephone);
            if (mmsg1.Status) {
                App.toastrError(mmsg1.Title);
                return;
            }
            dataservice.insert($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $scope.initData = function () {
                        dataservice.getItemAdd($scope.model.SupCode, function (rs) {
                            if (rs.Error) {
                                App.toastrError(rs.Title);
                            } else {
                                $rootScope.SupplierId = rs.SupID;
                                $rootScope.CusSupId = rs.SupID;
                            }
                        });
                    }
                    $scope.initData();
                }
                App.unblockUI("#contentMain");
            });
        }
    }


    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.Area == "") {
            $scope.errorArea = true;
            mess.Status = true;
        } else {
            $scope.errorArea = false;

        }
        if (data.SupGroup == "") {
            $scope.errorSupGroup = true;
            mess.Status = true;
        } else {
            $scope.errorSupGroup = false;

        }
        if (data.Role == "") {
            $scope.errorRole = true;
            mess.Status = true;
        } else {
            $scope.errorRole = false;

        }
        if (data.CusType == "") {
            $scope.errorCusType = true;
            mess.Status = true;
        } else {
            $scope.errorCusType = false;

        }
        if (data.Status == "") {
            $scope.errorStatus = true;
            mess.Status = true;
        } else {
            $scope.errorStatus = false;

        }

        if (data.Mobile && !$rootScope.partternPhone.test(data.Mobile)) {
            $scope.errorMobile = true;
            mess.Status = true;
        } else {
            $scope.errorMobile = false;
        }
        if (data.Fax && !$rootScope.partternPhone.test(data.Fax)) {
            $scope.errorFax = true;
            mess.Status = true;
        } else {
            $scope.errorFax = false;
        }

        return mess;
    };
    function initAutocomplete() {
        var defaultBounds = new google.maps.LatLngBounds(new google.maps.LatLng(-33.8902, 151.1759), new google.maps.LatLng(-33.8474, 151.2631));
        var options = {
            bounds: defaultBounds,
            types: ['geocode']
        };

        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('textAreaAddress'), options);

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            lat = place.geometry.location.lat();
            lng = place.geometry.location.lng();
            $("#locationGPS").val(lat + ',' + lng);
            $scope.model.GoogleMap = lat + ',' + lng
            $scope.model.Address = document.getElementById('textAreaAddress').value;
            console.log(lat + ',' + lng);
        });
    }
    setTimeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
        initAutocomplete();
        $(".product_price").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey == true || e.metaKey == true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    }, 200);
});
app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, para) {
    $rootScope.SupplierId = para;
    $scope.model = {
        GoogleMap: '',
        SupGroup: '',
        Status: '',
        Address: '',

    };

    $scope.initData = function () {
        dataservice.getItem(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;

                console.log(JSON.stringify($scope.model.ActivityStatus));
            }
        });
    }
    $scope.initData();
    $scope.openMap = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/googleMap.html',
            controller: 'googleMap',
            backdrop: true,
            size: '80',
            resolve: {
                para: function () {
                    return $scope.model;
                }
            }
        });
        modalInstance.result.then(function (d) {
            if ($rootScope.map.Lat != '' && $rootScope.map.Lng != '') {
                $scope.model.GoogleMap = $rootScope.map.Lat + ',' + $rootScope.map.Lng;
                $scope.model.Address = $rootScope.map.Address;
            }
            $rootScope.map = [];
        }, function () { });
    }
    $scope.tinymceOptions = {
        plugins: 'print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern help',
        toolbar: "formatselect bold italic strikethrough forecolor link  alignleft aligncenter alignright alignjustify numlist bullist outdent indent removeformat"
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Area" && $scope.model.Area != "") {
            $scope.errorArea = false;
        }
        if (SelectType == "SupGroup" && $scope.model.SupGroup != "") {
            $scope.errorSupGroup = false;
        }
        if (SelectType == "Role" && $scope.model.Role != "") {
            $scope.errorRole = false;
        }
        if (SelectType == "CusType" && $scope.model.CusType != "") {
            $scope.errorCusType = false;
        }

        if (SelectType == "Status" && $scope.model.Status != "") {
            $scope.errorStatus = false;
        }

        if (SelectType == "Mobile" && $scope.model.Mobile && $rootScope.partternPhone.test($scope.model.Mobile)) {
            $scope.errorMobile = false;
        } else if (SelectType == "Mobile") {
            $scope.errorMobile = true;
        }
        if (SelectType == "Fax" && $scope.model.Fax && $rootScope.partternPhone.test($scope.model.Fax)) {
            $scope.errorFax = false;
        } else if (SelectType == "Fax") {
            $scope.errorFax = true;
        }

    }
    $scope.addCardJob = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: "/views/admin/cardJob/edit-tag.html",
            controller: 'edit-card',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return "";
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.reload();
        }, function () { });
    };
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            var msg = $rootScope.checkData($scope.model);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            //var mmsg = $rootScope.checkTelephone($scope.model.Mobile);
            //if (mmsg.Status) {
            //    App.toastrError(mmsg.Title);
            //    return;
            //}
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

    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.Area == "") {
            $scope.errorArea = true;
            mess.Status = true;
        } else {
            $scope.errorArea = false;

        }
        if (data.SupGroup == "") {
            $scope.errorSupGroup = true;
            mess.Status = true;
        } else {
            $scope.errorSupGroup = false;

        }
        if (data.Role == "") {
            $scope.errorRole = true;
            mess.Status = true;
        } else {
            $scope.errorRole = false;

        }
        if (data.CusType == "") {
            $scope.errorCusType = true;
            mess.Status = true;
        } else {
            $scope.errorCusType = false;

        }
        if (data.Status == "") {
            $scope.errorStatus = true;
            mess.Status = true;
        } else {
            $scope.errorStatus = false;

        }

        if (data.Mobile && !$rootScope.partternPhone.test(data.Mobile)) {
            $scope.errorMobile = true;
            mess.Status = true;
        } else {
            $scope.errorMobile = false;
        }
        if (data.Fax && !$rootScope.partternPhone.test(data.Fax)) {
            $scope.errorFax = true;
            mess.Status = true;
        } else {
            $scope.errorFax = false;
        }

        return mess;
    };
    function initAutocomplete() {
        var defaultBounds = new google.maps.LatLngBounds(new google.maps.LatLng(-33.8902, 151.1759), new google.maps.LatLng(-33.8474, 151.2631));
        var options = {
            bounds: defaultBounds,
            types: ['geocode']
        };

        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('textAreaAddress'), options);

        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            lat = place.geometry.location.lat();
            lng = place.geometry.location.lng();
            $("#locationGPS").val(lat + ',' + lng);
            $scope.model.GoogleMap = lat + ',' + lng
            $scope.model.Address = document.getElementById('textAreaAddress').value;
            console.log(lat + ',' + lng);
        });
    }

    setTimeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
        initAutocomplete();
        $(".product_price").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
                (e.keyCode == 65 && (e.ctrlKey == true || e.metaKey == true)) ||
                // Allow: home, end, left, right, down, up
                (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    }, 200);
});


app.controller('contact', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        ContactName: '',
        ContactEmail: '',
        ContactTelephone: '',
        ContactMobilePhone: '',
        SupplierId: ''
    }


    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Supplier/JTableContact",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contact-main",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.SupplierId = $rootScope.SupplierId;
                d.ContactName = $scope.model.ContactName;
                d.ContactEmail = $scope.model.ContactEmail;
                d.ContactTelephone = $scope.model.ContactTelephone;
                d.ContactMobilePhone = $scope.model.ContactMobilePhone;
            },
            complete: function () {
                App.unblockUI("#contact-main");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('contactName').withTitle('{{ "SUP_CURD_TAB_CONTACT_LIST_COL_CONTACTNAME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('contactEmail').withTitle('{{ "SUP_CURD_TAB_CONTACT_LIST_COL_CONTACTEMAIL" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('contactAddress').withTitle('{{ "SUP_CURD_TAB_CONTACT_LIST_COL_CONTACTADDRESS" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('contactTelephone').withTitle('{{ "SUP_CURD_TAB_CONTACT_LIST_COL_CONTACTTELEPHONE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('contactMobilePhone').withTitle('{{ "SUP_CURD_TAB_CONTACT_LIST_COL_CONTACTMOBIPHONE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.search = function () {
        reloadData(true);
    }
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contactAdd.html',
            controller: 'contactAdd',
            backdrop: true,
            size: '60'
        });
        modalInstance.result.then(function (d) {
            reloadData();
        }, function () { });
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contactEdit.html',
            controller: 'contactEdit',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData();
        }, function () {
        });
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteContact(id, function (rs) {
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
            $scope.reload();
        }, function () {
        });
    }
});
app.controller('contactAdd', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.model = {
        FileName: ''
    };

    $scope.cancel = function () {
        $uibModalInstance.close();
    };
    $scope.changleSelect = function (SelectType) {

        if (SelectType == "MobilePhone" && $scope.model.MobilePhone && $rootScope.partternPhone.test($scope.model.MobilePhone) || $scope.model.MobilePhone == "") {
            $scope.errorMobilePhone = false;
        } else if (SelectType == "MobilePhone") {
            $scope.errorMobilePhone = true;
        }
        if (SelectType == "Fax" && $scope.model.Fax && $rootScope.partternPhone.test($scope.model.Fax) || $scope.model.Fax == "") {
            $scope.errorFax = false;
        } else if (SelectType == "Fax") {
            $scope.errorFax = true;
        }
        if (SelectType == "Telephone" && $scope.model.Telephone && $rootScope.partternNumber.test($scope.model.Telephone) || $scope.model.Telephone == "") {
            $scope.errorTelephone = false;
        } else if (SelectType == "Telephone") {
            $scope.errorTelephone = true;
        }

    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addForm.validate() && validationSelect($scope.model).Status == false) {
            var files = $('#File').get(0);
            var file = files.files[0];
            var data = new FormData();
            var fileName = '';

            data.append("FileUpload", file);
            dataservice.uploadImage(data, function (rs) {
                if (rs.Error) {
                    App.toastrError(result.Title);
                    return;
                }
                else {

                    $scope.model.FilePath = '/uploads/images/' + rs.Object;
                    if ($scope.model.FilePath == '/images/default/uploadimg.png' || $scope.model.FilePath == '') {
                        $scope.model.FilePath = '/images/default/no_image.png';
                    }
                    $scope.model.CusSupId = $rootScope.SupplierId;
                    dataservice.insertContact($scope.model, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close();
                        }
                    });
                }
            });
        }
    };
    $scope.loadImage = function () {
        var fileuploader = angular.element("#File");
        fileuploader.on('click', function () {
        });
        fileuploader.on('change', function (e) {
            var reader = new FileReader();
            reader.onload = function () {
                document.getElementById('image').src = reader.result;
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
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };
        if (data.MobilePhone && !$rootScope.partternPhone.test(data.MobilePhone)) {
            $scope.errorMobilePhone = true;
            mess.Status = true;
        } else {
            $scope.errorMobilePhone = false;
        }
        if (data.Fax && !$rootScope.partternPhone.test(data.Fax)) {
            $scope.errorFax = true;
            mess.Status = true;
        } else {
            $scope.errorFax = false;
        }
        if (data.Telephone && !$rootScope.partternNumber.test(data.Telephone)) {
            $scope.errorTelephone = true;
            mess.Status = true;
        } else {
            $scope.errorTelephone = false;
        }

        return mess;
    };
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contactEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.model = {
        FileName: ''
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.changleSelect = function (SelectType) {

        if (SelectType == "MobilePhone" && $scope.model.MobilePhone && $rootScope.partternPhone.test($scope.model.MobilePhone) || $scope.model.MobilePhone == "") {
            $scope.errorMobilePhone = false;
        } else if (SelectType == "MobilePhone") {
            $scope.errorMobilePhone = true;
        }
        if (SelectType == "Fax" && $scope.model.Fax && $rootScope.partternPhone.test($scope.model.Fax) || $scope.model.Fax == "") {
            $scope.errorFax = false;
        } else if (SelectType == "Fax") {
            $scope.errorFax = true;
        }
        if (SelectType == "Telephone" && $scope.model.Telephone && $rootScope.partternNumber.test($scope.model.Telephone) || $scope.model.Telephone == "") {
            $scope.errorTelephone = false;
        } else if (SelectType == "Telephone") {
            $scope.errorTelephone = true;
        }

    }
    $scope.initData = function () {
        dataservice.getContact(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                if ($scope.model.FilePath == '' || $scope.model.FilePath == '/uploads/images/null') {
                    $scope.model.FilePath = '/images/default/uploadimg.png';
                }
            }
        });
    }
    $scope.initData();
    $scope.loadImage = function () {
        var fileuploader = angular.element("#File");
        fileuploader.on('click', function () {
        });
        fileuploader.on('change', function (e) {
            var reader = new FileReader();
            reader.onload = function () {
                document.getElementById('image').src = reader.result;
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
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addForm.validate() && validationSelect($scope.model).Status == false) {
            var files = $('#File').get(0);
            var file = files.files[0];
            var data = new FormData();
            var fileName = '';

            if (file == null) {
                $scope.model.CusSupId = $rootScope.SupplierId;
                dataservice.updateContact($scope.model, function (result) {
                    if (result.Error) {
                        App.toastrError(result.Title);
                    } else {
                        App.toastrSuccess(result.Title);
                        $uibModalInstance.close();
                    }
                });
            }
            else {
                data.append("FileUpload", file);
                dataservice.uploadImage(data, function (rs) {
                    if (rs.Error) {
                        App.toastrError(result.Title);
                        return;
                    }
                    else {
                        $scope.model.FilePath = '/uploads/images/' + rs.Object;
                        $scope.model.CusSupId = $rootScope.SupplierId;
                        dataservice.updateContact($scope.model, function (result) {
                            if (result.Error) {
                                App.toastrError(result.Title);
                            } else {
                                App.toastrSuccess(result.Title);
                                $uibModalInstance.close();
                                //$uibModalInstance.dismiss('cancel');
                            }
                        });
                    }
                });

                //setTimeout(function () {
                //    App.inputFile();
                //}, 200);
            }
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };
        if (data.MobilePhone && !$rootScope.partternPhone.test(data.MobilePhone)) {
            $scope.errorMobilePhone = true;
            mess.Status = true;
        } else {
            $scope.errorMobilePhone = false;
        }
        if (data.Fax && !$rootScope.partternPhone.test(data.Fax)) {
            $scope.errorFax = true;
            mess.Status = true;
        } else {
            $scope.errorFax = false;
        }
        if (data.Telephone && !$rootScope.partternNumber.test(data.Telephone)) {
            $scope.errorTelephone = true;
            mess.Status = true;
        } else {
            $scope.errorTelephone = false;
        }

        return mess;
    };
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
        setModalMaxHeight('.modal');
    }, 200)
});


app.controller('file', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        SupplierId: '',
        Title: '',
        Type: '',
        FromDate: '',
        ToDate: ''
    }



    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Supplier/JTableFile",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.SupplierId = $rootScope.SupplierId;
                d.Title = $scope.model.Title;
                d.Type = $scope.model.Type;
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
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
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('title').withTitle('{{ "SUP_CURD_TAB_FILE_LIST_COL_TITLE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('type').withTitle('{{ "SUP_CURD_TAB_FILE_LIST_COL_TYPE" | translate }}').renderWith(function (data, type) {
        var excel = ['.xlsm', '.xlsx', '.xlsb', '.xltx', '.xltm', '.xls', '.xlt', '.xls', '.xml', '.xml', '.xlam', '.xla', '.xlw', '.xlr'];
        var document = ['.txt'];
        var word = ['.docx', '.doc'];
        var pdf = ['.pdf'];
        var powerPoint = ['.pps', '.pptx'];
        var image = ['.jpg', '.png', '.PNG'];

        if (excel.indexOf(data) !== -1) {
            return '<i style="color: rgb(106,170,89);font-size: 15px;" class="fa fa-file-excel-o" aria-hidden="true"></i>';
        } else if (word.indexOf(data) !== -1) {
            return '<i style="color: rgb(13,118,206);font-size: 15px;" class="fa fa-file-word-o" aria-hidden="true"></i>';
        } else if (document.indexOf(data) !== -1) {
            return '<i style="color: rgb(0,0,0);font-size: 15px;" class="fa fa-file-text-o" aria-hidden="true"></i>';
        } else if (pdf.indexOf(data) !== -1) {
            return '<i style="color: rgb(226,165,139);font-size: 15px;" class="fa fa-file-pdf-o" aria-hidden="true"></i>';
        } else if (powerPoint.indexOf(data) !== -1) {
            return '<i style="color: rgb(226,165,139);font-size: 15px;" class="fa fa-file-powerpoint-o" aria-hidden="true"></i>';
        } else if (image.indexOf(data) !== -1) {
            return '<i style="color: rgb(42,42,42);font-size: 15px;" class="fa fa-picture-o" aria-hidden="true"></i>';
        } else {
            return data;
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('description').withTitle('{{ "SUP_CURD_TAB_FILE_LIST_COL_NOTE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('createdTime').withTitle('{{ "SUP_CURD_TAB_FILE_LIST_COL_CREATETIME" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('updatedTime').withTitle('Ngày sửa').renderWith(function (data, type) {
    //    return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.search = function () {
        vm.dtInstance.reloadData();
    }
    $scope.add = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/fileAdd.html',
            controller: 'fileAdd',
            backdrop: true,
            size: '50',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData()
        }, function () { });
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/fileEdit.html',
            controller: 'fileEdit',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData()
        }, function () { });
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteFile(id, function (rs) {
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
            $scope.reload();
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
        $('.end-date').click(function () {
            $('#datefrom').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#dateto').datepicker('setStartDate', null);
        });
    }
    setTimeout(function () {
        loadDate();
    }, 200);
});
app.controller('fileAdd', function ($scope, $rootScope, $compile, $uibModalInstance, dataservice, para) {
    $scope.treeData = [];
    $scope.model = {
        FileName: '',
        Category: ''
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.readyCB = function () {
        App.blockUI({
            target: "#contentMainRepository",
            boxed: true,
            message: 'loading...'
        });
        dataservice.jtreeRepository(function (result) {
            var root = {
                id: 'root',
                parent: "#",
                text: "Tất cả kho dữ liệu",
                state: { selected: false, opened: true, checkbox_disabled: true, disabled: true }
            }
            $scope.treeData.push(root);
            for (var i = 0; i < result.length; i++) {
                if (result[i].Parent == '#') {
                    var data = {
                        id: result[i].ReposID,
                        parent: 'root',
                        text: result[i].ReposCode + ' - ' + result[i].ReposName,
                        state: { selected: false, opened: true }
                    }
                    $scope.treeData.push(data);
                } else {
                    var data = {
                        id: result[i].ReposID,
                        parent: result[i].Parent,
                        text: result[i].ReposCode + ' - ' + result[i].ReposName,
                        state: { selected: false, opened: true }
                    }
                    $scope.treeData.push(data);
                }
            }
            App.unblockUI("#contentMainRepository");
        });
    }
    $scope.searchRepository = function (search) {
        if (search != '' && search != undefined) {
            $("#treeDiv").jstree("close_all");
            $('#treeDiv').jstree(true).search(search);
        }
    }
    $scope.searchTreeRepository = function (e, data) {
        if (data.res.length === 0) {
            App.toastrWarning('Không tìm thấy kho lưu trữ');
        };
    }
    $scope.selectNodeRepository = function (node, selected, event) {
        $scope.model.Category = selected.node.original.id;
    }
    $scope.treeConfig = {
        core: {
            multiple: false,
            animation: true,
            error: function (error) {
                $log.error('treeCtrl: error from js tree - ' + angular.toJson(error));
            },
            check_callback: true,
            worker: true,

        },
        types: {
            default: {
                icon: 'fa fa-folder icon-state-warning'
            }
        },
        version: 1,
        plugins: ['checkbox', 'types', 'search', 'state'],
        checkbox: {
            "three_state": false,
            "whole_node": true,
            "keep_selected_style": true,
            "cascade": "undetermined",
        },
        types: {
            valid_children: ["selected"],
            types: {
                "selected": {
                    "select_node": false
                }
            },
            "default": {
                "icon": "fa fa-folder icon-state-warning icon-lg"
            },
            "file": {
                "icon": "fa fa-file icon-state-warning icon-lg"
            }
        },
    };
    $scope.treeEvents = {
        'ready': $scope.readyCB,
        'select_node': $scope.selectNodeRepository,
        'search': $scope.searchTreeRepository,
    }
    $scope.submit = function () {
        var file = document.getElementById("File").files[0];
        if (file == null || file == undefined) {
            App.toastrError("Vui lòng chọn tệp tin");
        } else {
            var listNoteSelect = $scope.treeInstance.jstree(true).get_checked(true);
            if (listNoteSelect.length == 0) {
                App.toastrError("Vui lòng chọn kho dữ liệu");
            } else {
                var data = new FormData();
                data.append("FileUpload", file);
                dataservice.uploadFile(data, function (rs) {
                    if (rs.Error) {
                        App.toastrError(rs.Title);
                        return;
                    }
                    else {
                        $scope.model.SupplierId = $rootScope.SupplierId;
                        $scope.model.FileName = rs.Object;
                        //$scope.model.CustomerID = $rootScope.CustomerId;
                        $scope.model.FileUrl = '/uploads/files/' + $scope.model.FileName;
                        $scope.model.FilePath = '~/upload/files/' + $scope.model.FileName;
                        dataservice.insertFile($scope.model, function (result) {
                            if (result.Error) {
                                App.toastrError(result.Title);
                            } else {
                                App.toastrSuccess(result.Title);
                                $uibModalInstance.close();
                            }
                        });
                    }
                });
            }
        }
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('fileEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.model = {
        FileName: '',

    };
    //document.getElementById("File").innerHTML = "ạkdhasjkdhasjkdhasjkdashdjk";
    $scope.model.Category = '';
    $scope.treeData = [];
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.initData = function () {
        dataservice.getFile(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
            }
        });
    }
    $scope.initData();
    $scope.readyCB = function () {
        App.blockUI({
            target: "#contentMainRepository",
            boxed: true,
            message: 'loading...'
        });
        dataservice.jtreeRepository(function (result) {
            debugger
            var root = {
                id: 'root',
                parent: "#",
                text: "Tất cả kho dữ liệu",
                state: { selected: false, opened: true, checkbox_disabled: true, disabled: true }
            }
            $scope.treeData.push(root);
            for (var i = 0; i < result.length; i++) {
                if (result[i].Parent == '#') {
                    var data = {
                        id: result[i].ReposID,
                        parent: 'root',
                        text: result[i].ReposCode + ' - ' + result[i].ReposName,
                        state: { selected: false, opened: true }
                    }
                    $scope.treeData.push(data);
                } else {
                    var data = {
                        id: result[i].ReposID,
                        parent: result[i].Parent,
                        text: result[i].ReposCode + ' - ' + result[i].ReposName,
                        state: { selected: false, opened: true }
                    }
                    $scope.treeData.push(data);
                }
            }
            App.unblockUI("#contentMainRepository");
        });
    }
    
    $scope.searchRepository = function () {
        $("#treeDiv").jstree("close_all");
        $('#treeDiv').jstree(true).search($scope.model.Name);
    }
    $scope.searchTreeRepository = function (e, data) {
        if (data.res.length === 0) {
            App.toastrWarning('Không tìm thấy kho lưu trữ');
        };
    }
    $scope.selectNodeRepository = function (node, selected, event) {
        $scope.model.Category = selected.node.original.id;
    }
    $scope.treeConfig = {
        core: {
            multiple: false,
            animation: true,
            error: function (error) {
                $log.error('treeCtrl: error from js tree - ' + angular.toJson(error));
            },
            check_callback: true,
            worker: true,

        },
        types: {
            default: {
                icon: 'fa fa-folder icon-state-warning'
            }
        },
        version: 1,
        plugins: ['checkbox', 'types', 'search', 'state'],
        checkbox: {
            "three_state": false,
            "whole_node": true,
            "keep_selected_style": true,
            "cascade": "undetermined",
        },
        types: {
            valid_children: ["selected"],
            types: {
                "selected": {
                    "select_node": false
                }
            },
            "default": {
                "icon": "fa fa-folder icon-state-warning icon-lg"
            },
            "file": {
                "icon": "fa fa-file icon-state-warning icon-lg"
            }
        },
    };
    $scope.treeEvents = {
        'ready': $scope.readyCB,
        'select_node': $scope.selectNodeRepository,
        'search': $scope.searchTreeRepository,
    }
    $scope.submit = function () {
        var files = $("#File").get(0);
        var file = files.files[0];
        var data = new FormData();
        var fileName = '';

        if (file == null) {
            $scope.model.SupplierId = $rootScope.SupplierId;
            dataservice.updateFile($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
            });
        }
        else {
            data.append("FileUpload", file);
            dataservice.uploadFile(data, function (rs) {
                if (rs.Error) {
                    App.toastrError(result.Title);
                    return;
                }
                else {
                    $scope.model.SupplierId = $rootScope.SupplierId;
                    $scope.model.FileName = rs.Object;
                    //$scope.model.CustomerID = $rootScope.CustomerId;
                    $scope.model.FileUrl = '/uploads/files/' + $scope.model.FileName;
                    $scope.model.FilePath = '~/upload/files/' + $scope.model.FileName;
                    dataservice.updateFile($scope.model, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close();
                        }
                    });
                }
            });
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
    //$scope.submit = function () {
    //    var file = document.getElementById("File").files[0];
    //    if (file == null || file == undefined) {
    //        App.toastrError("Vui lòng chọn tệp tin");
    //    } else {
    //        var listNoteSelect = $scope.treeInstance.jstree(true).get_checked(true);
    //        if (listNoteSelect.length == 0) {
    //            App.toastrError("Vui lòng chọn kho dữ liệu");
    //        } else {
    //            var data = new FormData();
    //            data.append("FileUpload", file);
    //            dataservice.uploadFile(data, function (rs) {
    //                if (rs.Error) {
    //                    App.toastrError(rs.Title);
    //                    return;
    //                }
    //                else {
    //                    $scope.model.SupplierId = $rootScope.SupplierId;
    //                    $scope.model.FileName = rs.Object;
    //                    $scope.model.CustomerID = $rootScope.CustomerId;
    //                    $scope.model.FileUrl = '/uploads/files/' + $scope.model.FileName;
    //                    $scope.model.FilePath = '~/upload/files/' + $scope.model.FileName;
    //                    dataservice.updateFile($scope.model, function (result) {
    //                        if (result.Error) {
    //                            App.toastrError(result.Title);
    //                        } else {
    //                            App.toastrSuccess(result.Title);
    //                            $uibModalInstance.close();
    //                        }
    //                    });
    //                }
    //            });
    //        }
    //    }
    //    setTimeout(function () {
    //        App.inputFile();
    //    }, 200);
    //};

    //$scope.submit = function () {
    //    /*var files = $('#FileInput').get(0);*/
    //    var file = document.getElementById("File").files[0];
    //    //var file = files.files[0];
    //    var data = new FormData();
    //    var fileName = '';

    //    if (file == null) {
    //        $scope.model.SupplierId = $rootScope.SupplierId;
    //        console.log($scope.model);
    //        dataservice.updateFile($scope.model, function (result) {
    //            if (result.Error) {
    //                App.toastrError(result.Title);
    //            } else {
    //                App.toastrSuccess(result.Title);
    //                $uibModalInstance.close();
    //            }
    //        });
    //    }
    //    else {
    //        data.append("FileUpload", file);
    //        dataservice.uploadFile(data, function (rs) {
    //            if (rs.Error) {
    //                App.toastrError(result.Title);
    //                return;
    //            }
    //            else {
    //                $scope.model.FileName = rs.Object;
    //                $scope.model.SupplierId = $rootScope.SupplierId;
    //                $scope.model.FileUrl = '/uploads/files/' + $scope.model.FileName;
    //                $scope.model.FilePath = '~/upload/files/' + $scope.model.FileName;

    //                console.log($scope.model);
    //                dataservice.updateFile($scope.model, function (result) {
    //                    if (result.Error) {
    //                        App.toastrError(result.Title);
    //                    } else {
    //                        App.toastrSuccess(result.Title);
    //                        $uibModalInstance.close();
    //                    }
    //                });
    //            }
    //        });

    //        setTimeout(function () {
    //            App.inputFile();
    //        }, 200);
    //    }
    //}
    //setTimeout(function () {
    //    App.inputFile();
    //}, 200);
});


app.controller('more', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        SupplierId: ''
    }



    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';

    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Supplier/JTableExtend",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.SupplierId = $rootScope.SupplierId;
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
            contextScope.contextMenu = $scope.contextMenu;
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    //end option table
    //Tạo các cột của bảng để đổ dữ liệu vào
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('id').withTitle('ID').renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{ "SUP_CURD_TAB_MORE_LIST_COL_CODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('value').withTitle('{{ "SUP_CURD_TAB_MORE_LIST_COL_VALUE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/moreAdd.html',
            controller: 'moreAdd',
            backdrop: true,
            size: '40'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/moreEdit.html',
            controller: 'moreEdit',
            backdrop: true,
            size: '40',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteExtend(id, function (rs) {
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
            $scope.reload();
        }, function () {
        });
    }

});
app.controller('moreAdd', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        ext_code: '',
        ext_value: '',
        supplier_code: '',
    }
    $scope.submit = function () {
        if ($scope.addform.validate()) {
            var msg = $rootScope.checkDataMore($scope.model);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            $scope.model.supplier_code = $rootScope.SupplierId;
            dataservice.insertExtend($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
                App.unblockUI("#contentMain");
            });
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('moreEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.initData = function () {
        dataservice.getExtend(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
            }
        });
    }
    $scope.initData();

    $scope.submit = function () {
        if ($scope.addform.validate()) {
            dataservice.updateExtend($scope.model, function (rs) {
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
app.controller('googleMap', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter, para) {
    var lat = '';
    var lng = '';
    var address = '';
    $rootScope.map = {
        Lat: '',
        Lng: '',
        Address: ''
    }
    $scope.model = {
        Lat: '',
        Lng: ''
    }
    $scope.initLoad = function () {
        fields_vector_source = new ol.source.Vector({});
        var center = ol.proj.transform([$rootScope.lngDefault, $rootScope.latDefault], 'EPSG:4326', 'EPSG:3857');
        map = new ol.Map({
            target: $('#map')[0],

            layers: [
                new ol.layer.Tile({
                    source: new ol.source.OSM({
                        url: 'http://mt{0-3}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}'


                    })
                }),
                new ol.layer.Vector({
                    source: fields_vector_source
                })
            ],

            view: new ol.View({
                center: center,
                zoom: 15

            }),

            controls: ol.control.defaults({
                attribution: false,
                zoom: false,
            })
        });
        var pathGG = $('#pathGG').html();
        var id = $("#ID").html();
        var aaa = parseInt(id);
        if (pathGG != "" && pathGG != null) {
            pathSourceVector = new ol.source.Vector({
                features: []
            });
            pathLayerMarker = new ol.layer.Vector({
                source: pathSourceVector
            });
            var path = polyline.decode(pathGG);

            pathLayerMarker = renderLinePathLayer(path);
            map.addLayer(pathLayerMarker);

            var styles3 = [

                new ol.style.Style({
                    stroke: new ol.style.Stroke({
                        color: '#64c936',
                        width: 3
                    }),
                    fill: new ol.style.Fill({
                        color: 'rgba(100, 201, 54,1)'
                    })
                }),
            ];

            var iconStyleStart = new ol.style.Style({
                image: new ol.style.Icon(({
                    anchor: [0.5, 26],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'pixels',
                    src: 'https://i.imgur.com/pjZYQLJ.png'
                })),
                zIndex: 11
            });
            var iconStyleEnd = new ol.style.Style({
                image: new ol.style.Icon(({
                    anchor: [0.5, 26],
                    anchorXUnits: 'fraction',
                    anchorYUnits: 'pixels',
                    src: 'https://i.imgur.com/3g07NhB.png'
                })),
                zIndex: 11
            });

            var pathLenght = path.length - 1;
            var iconFeatureStart = new ol.Feature({
                geometry: new ol.geom.Point(ol.proj.transform([parseFloat(path[0][1]), parseFloat(path[0][0])], 'EPSG:4326', 'EPSG:3857')),
                type: "valve"
            });

            iconFeatureStart.setId(1);
            iconFeatureStart.setStyle(iconStyleStart);
            var iconFeatureEnd = new ol.Feature({
                geometry: new ol.geom.Point(ol.proj.transform([parseFloat(path[pathLenght][1]), parseFloat(path[pathLenght][0])], 'EPSG:4326', 'EPSG:3857')),
                type: "valve"
            });
            iconFeatureEnd.setId(2);
            iconFeatureEnd.setStyle(iconStyleEnd);
            var vectorIcon = new ol.source.Vector({});
            vectorIcon.addFeature(iconFeatureStart);
            vectorIcon.addFeature(iconFeatureEnd);

            var vectorLayer = new ol.layer.Vector({
                source: vectorIcon,
                style: styles3
            });

            map.addLayer(vectorLayer);


            //pathSource = new ol.source.Vector({});


            pathSource.addFeature(renderLineStringFeature(path))
            var field_location = pathSource.getFeatureById(aaa).getProperties();
            var field_extent = field_location.geometry.getExtent();
            map.getView().fit(field_extent, map.getSize());
            map.getView().setZoom(12);
        }
    }
    $scope.initLoad();
    $scope.cancel = function () {
        $uibModalInstance.close();
    };
    $scope.submit = function () {
        //$rootScope.map.Address = $("#Address").val();
        $rootScope.map.Lat = lat;
        $rootScope.map.Lng = lng;
        $rootScope.map.Address = address;
        $uibModalInstance.close();
    }
    function initMap() {
        if (para) {
            if (para.GoogleMap) {
                lat = parseFloat(para.GoogleMap.split(',')[0]);
                lng = parseFloat(para.GoogleMap.split(',')[1]);
                address = para.Address;
                document.getElementById("startPlace").value = para.Address;
            }
        } else {
            lat = $rootScope.latDefault;
            lng = $rootScope.lngDefault;
            address = $rootScope.adressDefault;
            document.getElementById("startPlace").value = $rootScope.addressDefault;
        }

        var centerPoint = { lat: lat == '' ? $rootScope.latDefault : lat, lng: lng == '' ? $rootScope.lngDefault : lng };
        var maps = new google.maps.Map(
            document.getElementById('map'), { zoom: $rootScope.zoomMap, center: centerPoint });
        maps.controls[google.maps.ControlPosition.TOP_LEFT].push(document.getElementById('startPlace'));
        var marker = new google.maps.Marker({
            position: centerPoint,
            map: maps
        });

        //maps.addListener('zoom_changed', function () {
        //    console.log('Zoom: ' + maps.getZoom());
        //});

        var defaultBounds = new google.maps.LatLngBounds(new google.maps.LatLng(-33.8902, 151.1759), new google.maps.LatLng(-33.8474, 151.2631));
        var options = {
            bounds: defaultBounds,
            types: ['geocode']
        };
        var autocomplete = new google.maps.places.Autocomplete(document.getElementById('startPlace'), options);
        google.maps.event.addListener(autocomplete, 'place_changed', function () {
            var place = autocomplete.getPlace();
            lat = place.geometry.location.lat();
            lng = place.geometry.location.lng();
            address = document.getElementById("startPlace").value;
            console.log(lat + ',' + lng);
            maps.setCenter({ lat: lat, lng: lng });
            if (marker) {
                marker.setPosition({ lat: lat, lng: lng });
            }
            else {
                marker = new google.maps.Marker({
                    position: { lat: lat, lng: lng },
                    map: maps
                });
            }
            maps.setZoom($rootScope.zoomMap);
        });

        maps.addListener('click', function (event) {
            var point = { lat: event.latLng.lat(), lng: event.latLng.lng() }
            var str = 'https://maps.googleapis.com/maps/api/geocode/json?latlng=' + point.lat + ',' + point.lng + '&sensor=true&key=AIzaSyAn-5Fd7KH4e78m1X7SNj5gayFcJKDoUow';
            lat = point.lat;
            lng = point.lng;

            $.getJSON(str, function (data) {
                address = data.results[0].formatted_address;
                document.getElementById("startPlace").value = address;
            });
            if (marker) {
                marker.setPosition(point);
            }
            else {
                marker = new google.maps.Marker({
                    position: point,
                    map: maps
                });
            }
            maps.setZoom($rootScope.zoomMap);
        })
    }
    setTimeout(function () {
        setModalMaxHeight('.modal');
        initMap();
        setModalDraggable('.modal-dialog');
    }, 200)
});

app.controller('cardJob', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Supplier/JTableCardJob",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contact-main",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.SupplierId = $rootScope.SupplierId;
            },
            complete: function () {
                App.unblockUI("#contact-main");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Supplier').withTitle('{{ "SUP_CURD_TAB_CARD_JOB_LIST_COL_SUPPLIER" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardCode').withTitle('{{ "SUP_CURD_TAB_CARD_JOB_LIST_COL_CARDCODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardName').withTitle('{{ "SUP_CURD_TAB_CARD_JOB_LIST_COL_CARDNAME" | translate }}').renderWith(function (data, type) {
        return data;
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
    $scope.search = function () {
        reloadData(true);
    }
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contactAdd.html',
            controller: 'contactAdd',
            backdrop: true,
            size: '60'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () { });
    }
    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contactEdit.html',
            controller: 'contactEdit',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteContact(id, function (rs) {
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
            $scope.reload();
        }, function () {
        });
    }
});

app.controller('edit-card', function ($scope, $http, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    var dataSet = [];

    $scope.model = {};
    $scope.checkitem = {
        Title: ''
    };
    $scope.comment = {
        Content: ''
    };

    $scope.members = [];
    $scope.checkList = [];
    $scope.show = {
        SelectCard: true,
        Options: false
    };
    $scope.addCardRelative = function (cardCode) {
        var data = {
            ObjCode: $rootScope.SupplierId,
            CatObjCode: "SUPPLIER",
            CardCode: cardCode,
            Relative: "MAIN"
        };
        dataservice.addCardRelative(data, function (rs) {
            if (rs.Error) {
                //App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.reload();
            }
        });
    };
    $scope.cancel = function () {
        $uibModalInstance.close();
    };

    $scope.initData = function () {
        if (para === "") {
            console.log("Another");
            $scope.ParamNull = true;
        }
        else {
            $scope.ParamNull = false;
        }
        dataservice.getLevels(function (rs) {
            $scope.CardLevels = rs;
        });
        dataservice.getWorkType(function (rs) {
            $scope.WorkTypes = rs;
        });
        dataservice.getCardDetail(para, function (rs) {
            console.log(rs);
            $scope.model = rs;
        });
        dataservice.getAttachment(para, function (rs) {
            $scope.attachments = rs;
        });
        dataservice.getCheckList(para, function (rs) {
            console.log(rs);
            $scope.checklists = rs;
            angular.forEach(rs, function (value, key) {
                dataservice.getCheckItem(value.ChkListCode, function (result) {
                    $scope.checkList[value.ChkListCode] = result;
                })
            })
        });
        dataservice.getComment(para, function (rs) {
            $scope.comments = rs;
        });
        dataservice.getCardDependency(para, function (rs) {
            dataSet = [];
            angular.forEach(rs, function (value, key) {
                var obj = [];
                obj.push(value.CatObjCode);
                obj.push(value.ObjCode);
                obj.push(value.Relative);
                obj.push('<button title="Xoá" ng-click="deleteObjReletive(' + value.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i style="color:red" class="fa fa-trash"></i></button>');
                obj.push(value.Id);
                dataSet.push(obj);
            });

            setTimeout(function () {
                refrestTable();
            }, 500);
        });
        dataservice.getStatus(function (rs) {
            $scope.CardStatus = rs;
        });
        dataservice.getBoards(function (rs) {
            $scope.Boards = rs;
        });
    };
    $scope.initData();

    $scope.boardSelect = function (boardCode) {
        dataservice.getLists(boardCode, function (rs) {
            $scope.Lists = rs;
        });
    };
    $scope.listSelect = function (listCode) {
        dataservice.getCards(listCode, function (rs) {
            $scope.Cards = rs;
        });
        $scope.show.Options = true;
        $scope.model = {};
        $scope.model.ListCode = listCode;
    };

    $scope.cardSelect = function (CardCode) {
        para = CardCode;
        $scope.addCardRelative(para);
        $scope.initData();
    };

    $scope.deleteObjReletive = function (code) {
        console.log(code);
        dataservice.deleteCardDependency(code, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        });
    };

    $scope.addCard = function (event) {
        var data = {
            CardName: event.target.value,
            ListCode: $scope.model.ListCode
        };
        console.log(data);
        dataservice.insertCard(data, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                para = rs.Object.CardCode;
                $scope.addCardRelative(para);
                $scope.initData();
            }
        });
    };

    $scope.editDescription = function () {
        if ($scope.model.Description === "" || $scope.model.Description == null || $scope.model.Description == undefined) {
            return;
        }
        var obj = {
            CardCode: para,
            Description: $scope.model.Description
        }
        dataservice.updateCardDescription(obj, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
            }
        });
    };
    $scope.updateCardName = function (id) {
        console.log(id);
        var element = $('#card_' + id);
        var newName = element.val();
        var currentName = element.attr('data-currentvalue');
        console.log('NewName: ' + newName);
        console.log("CurrentName: " + currentName);
        if (newName != currentName) {
            console.log("Change name");
            dataservice.updateCardName(id, newName, function (rs) {
                if (rs.Error) {
                    //App.toastrError(rs.Title);
                }
                else {
                    //App.toastrSuccess(rs.Title);
                    //$scope.initData();
                    element.attr('data-currentvalue', newName)
                }
                console.log(rs.Title);
            })
        }
    }
    $scope.addMember = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/add-member.html',
            controller: 'add-member',
            size: '50',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    };
    $scope.addDueDate = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/add-duedate.html',
            controller: 'add-duedate',
            size: '20',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    }

    $scope.changeWorkType = function (type) {
        dataservice.changeWorkType(para, type, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
            }
        });
    };
    $scope.changeCardStatus = function (status) {
        dataservice.changeCardStatus(para, status, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
            }
        });
    };
    $scope.changeCardLevel = function (level) {
        dataservice.changeCardLevel(para, level, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
            }
        });
    };

    $scope.objectRelative = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/object-relative.html',
            controller: 'object-relative',
            size: '60',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    };

    $scope.addAttachment = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/attachment.html',
            controller: 'attachment',
            size: '35',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    }
    $scope.deleteAttachment = function (fileCode) {
        dataservice.deleteAttachment(fileCode, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        });
    }

    $scope.addComment = function () {
        if ($scope.comment.Content == "") {
            App.toastrError("Nhập bình luận!");
            return;
        }
        dataservice.addComment(para, $scope.comment.Content, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.comment.Content = '';
                $scope.initData();
            }
        })
    }
    $scope.deleteComment = function (CmtId) {
        dataservice.deleteComment(CmtId, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        });
    }
    $scope.updateComment = function (e) {
        console.log(e.target.getAttribute('cmtid'));
        $scope.show.editComment[e.target.getAttribute('cmtid')] = false;
        dataservice.updateComment(e.target.getAttribute('cmtid'), e.target.value, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        });
    };

    $scope.addLabel = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/label.html',
            controller: 'label',
            size: '35',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    }

    $scope.addCheckList = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/add-checklist.html',
            controller: 'add-checklist',
            size: '20',
            resolve: {
                cardCode: function () {
                    return para;
                }
            }
            //backdrop: true,
        });
        modalInstance.result.then(function (d) {
            $scope.initData();
        }, function () {
        });
    }
    $scope.deleteCheckList = function (CheckCode) {
        dataservice.deleteCheckList(CheckCode, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        })
    }
    $scope.editCheckTitle = function (e) {
        var checkTitle = e.target.value;
        var checkItem = e.target.getAttribute("checkcode");
        console.log(checkTitle);
        console.log(checkItem);
        $scope.show.editCheckTitle[e.target.getAttribute('checkcode')] = false;
        dataservice.changeCheckTitle(checkItem, checkTitle, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        })
    }
    $scope.deleteCheckItem = function (id) {
        dataservice.deleteCheckItem(id, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        })
    }

    $scope.checkItemClick = function (itemId) {
        dataservice.changeChkItemStatus(itemId, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);

                //$scope.initData();
            }
        });
    }
    $scope.addCheckItem = function (checkCode) {
        console.log($scope.checkitem);
        dataservice.addCheckItem(checkCode, $scope.checkitem.Title, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.checkitem.Title = '';
                $scope.initData();
                $scope.show.addCheckItem = false;
            }
        })
    };
    $scope.editCheckItemTitle = function (e) {
        var itemTitle = e.target.value;
        var itemId = e.target.getAttribute("itemid");
        //console.log(itemTitle);
        //console.log(itemId);
        $scope.show.editCheckItem[e.target.getAttribute('itemid')] = false;
        dataservice.changeChkItemTitle(itemId, itemTitle, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.initData();
            }
        })

    }

    $scope.deleteCard = function (id) {
        dataservice.deleteCard(id, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.cancel();
            }
        })
    };

    $scope.submit = function () {
        //$scope.model.Background = $('#background-color').val();
        console.log($scope.model);
        //dataservice.updateBoard($scope.model, function (rs) {
        //    if (rs.Error) {
        //        App.toastrError(rs.Title);
        //    }
        //    else {
        //        App.toastrSuccess(rs.Title);
        //        $uibModalInstance.close();
        //    }
        //})
    };

    function refrestTable() {
        var datatable = $('#data-table').DataTable({
            columns: [
                { title: '<i class="fa fa-info-circle mr5"></i>Loại phụ thuộc' },
                { title: '<i class="fa fa-code mr5"></i>Mã đối tượng' },
                { title: '<i class="fa fa-thumbtack mr5"></i>Quan hệ' },
                { title: '<i class="fa fa-location-arrow mr5"></i>Thao tác' }
            ],
            "createdRow": function (row, data, dataIndex) {
                $compile(angular.element(row).contents())($scope);
            },
            "searching": false,
            "lengthChange": false,
            "stripeClasses": [],
            "ordering": false,
            "bPaginate": false,
            "info": false
        });
        datatable.clear();
        datatable.rows.add(dataSet);
        datatable.draw();
    }
    setTimeout(function () {
        setModalMaxHeight('.modal');
        setModalDraggable(".modal-dialog");

    }, 400);
});

app.controller('object-relative', function ($scope, $rootScope, $cookies, $cookieStore, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    var vm = $scope;
    var dataSet = [];
    var listDependency = [];
    var customSetting = {};

    $scope.listTeam = [];
    $scope.listUser = [];
    $scope.cardMember = {
        listTeam: [],
        listMember: []
    };
    $scope.setting = {
        Id: -1,
        ObjDependency: '',
        ObjCode: '',
        Relative: '',
        Member: '',
        Team: ''
    };
    $scope.objDependency = [];
    $scope.listObjCode = [];

    $scope.useDefaultSetting = function () {
        var DfSetting = $cookies.getObject('DefaultCardSetting');
        if (DfSetting !== undefined) {
            dataSet = [];
            $scope.cardMember.listTeam = DfSetting.Team;
            $scope.cardMember.listMember = DfSetting.Member;
            angular.forEach(DfSetting.ListDependency, function (value, key) {
                var obj = [];
                obj.push(value.Dependency);
                obj.push(value.ObjCode);
                obj.push(value.Relative);
                dataSet.push(obj);
            });

            refrestTable();
        }
    };
    $scope.initData = function () {
        dataservice.getObjDependency(function (rs) {
            $scope.objDependency = rs;
        });
        dataservice.getCardDependency(cardCode, function (rs) {
            angular.forEach(rs, function (value, key) {
                var obj = [];
                obj.push(value.CatObjCode);
                obj.push(value.ObjCode);
                obj.push(value.Relative);
                obj.push('<button title="Xoá" ng-click="deleteObjReletive(' + value.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i style="color:red" class="fa fa-trash"></i></button>');
                obj.push(value.Id);
                dataSet.push(obj);
            });
            setTimeout(function () {
                refrestTable();
            }, 400);
        });
        dataservice.getRelative(function (rs) {
            $scope.relative = rs;
        })

    };
    $scope.initData();
    $uibModalInstance.opened.then(function () {
        setTimeout(function () {
            refrestTable();
        }, 100);
    });

    $scope.objDependencyChange = function (code) {
        console.log(code);
        dataservice.getObjCode(code, function (rs) {
            $scope.listObjCode = rs;
        });
    };
    $scope.objCodeChange = function (code) {
        console.log(code);
    };
    $scope.addDependency = function () {
        var dependency = $scope.setting.ObjDependency;
        var objCode = $scope.setting.ObjCode;
        var relative = $scope.setting.Relative;
        var id = $scope.setting.Id--;
        if (objCode === "" || relative === "" || dependency === "") {
            App.toastrError("Chọn đầy đủ thông tin");
            return;
        }
        var obj = [];

        for (var i = 0; i < dataSet.length; i++) {
            for (var j = 0; j < dataSet[i].length; j++) {
                if (dataSet[i][j] === objCode) {
                    App.toastrError("Mã đối tượng đã tồn tại");
                    return;
                }
            }
        }

        obj.push(dependency);
        obj.push(objCode);
        obj.push(relative);
        obj.push('<button title="Xoá" ng-click="deleteObjReletive(' + id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i style="color:red" class="fa fa-trash"></i></button>');
        obj.push(id);
        dataSet.push(obj);

        refrestTable();

        $scope.setting.ObjDependency = '';
        $scope.setting.ObjCode = '';
        $scope.setting.Relative = '';
        listObjCode = [];
    };
    $scope.deleteObjReletive = function (id) {
        for (var i = 0; i < dataSet.length; i++) {
            if (dataSet[i][4] == id) {
                dataSet.splice(i, 1);
                refrestTable();
                break;
            }
        }
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.clearData = function () {
        dataSet = [];
        refrestTable();
        console.log("Clear");
    };
    $scope.submit = function () {
        listDependency = [];
        for (var i = 0; i < dataSet.length; i++) {
            var data = { Dependency: dataSet[i][0], ObjCode: dataSet[i][1], Relative: dataSet[i][2] };
            listDependency.push(data);
        }
        console.log(listDependency);
        //if (listDependency.length > 0) {
        dataservice.setObjectRelative({ CardCode: cardCode, listDependency }, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $uibModalInstance.close();
            }
        });
        //}
        //$scope.cancel();
    };

    function refrestTable() {
        var datatable = $('#obj-data-table').DataTable({
            columns: [
                { title: '<i class="fa fa-info-circle mr5"></i>Loại phụ thuộc' },
                { title: '<i class="fa fa-code mr5"></i>Mã đối tượng' },
                { title: '<i class="fa fa-thumbtack mr5"></i>Quan hệ' },
                { title: '<i class="fa fa-location-arrow mr5"></i>Thao tác' }
            ],
            "createdRow": function (row, data, dataIndex) {
                $compile(angular.element(row).contents())($scope);
            },
            "searching": false,
            "lengthChange": false,
            "stripeClasses": [],
            "ordering": false,
            "bPaginate": false,
            "info": false
        });
        datatable.clear();
        datatable.rows.add(dataSet);
        datatable.draw();
    }
    setTimeout(function () {
        //$('#data-table').DataTable({
        //    columns: [
        //        { title: "Loại phụ thuộc" },
        //        { title: "Mã đối tượng" },
        //        { title: "Quan hệ" },
        //    ],
        //    "searching": false,
        //    "lengthChange": false,
        //    "stripeClasses": [],
        //    "ordering": false
        //});
    }, 100);
});
app.controller('add-member', function ($scope, $rootScope, $cookies, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    $scope.model = {};
    $scope.listTeam = [];
    $scope.listUser = [];
    $scope.cardMember = {
        listTeam: [],
        listMember: []
    };
    $scope.DfSetting = {};
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.initData = function () {
        dataservice.getCardTeam(cardCode, function (rs) {
            $scope.cardMember.listTeam = rs;
        });
        dataservice.getCardMember(cardCode, function (rs) {
            for (var i = 0; i < rs.length; i++) {
                $scope.cardMember.listMember[i] = {};
                $scope.cardMember.listMember[i].UserName = rs[i].MemberId;
            }
        });
        dataservice.getTeams(function (rs) {
            $scope.listTeam = rs;
        });
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    };
    $scope.initData();

    $scope.teamSelect = function (item) {
        for (var i = 0; i < $scope.cardMember.listTeam.length; i++) {
            if ($scope.cardMember.listTeam[i].TeamCode === item.TeamCode) {
                App.toastrError("Team đã tồn tại!");
                return;
            }
        }
        dataservice.getTeamMember(item.TeamCode, function (rs) {
            for (var i = 0; i < rs.split(';').length; i++) {
                for (var j = 0; j < $scope.listUser.length; j++) {
                    if ($scope.listUser[j].UserName === rs.split(';')[i]) {
                        $scope.listUser.splice(j, 1);
                        break;
                    }
                }
            }
        });
        $scope.cardMember.listTeam.push(item);
        //remove member in list
        for (var i = 0; i < $scope.listTeam.length; i++) {
            if ($scope.listTeam[i].TeamCode === item.TeamCode) {
                $scope.listTeam.splice(i, 1);
                break;
            }
        }
    };
    $scope.removeTeam = function (id) {
        for (var i = 0; i < $scope.cardMember.listTeam.length; i++) {
            if ($scope.cardMember.listTeam[i].TeamCode == id) {
                //if ($scope.listTeam.length != 0) {
                $scope.listTeam.push($scope.cardMember.listTeam[i]);
                //}
                $scope.cardMember.listTeam.splice(i, 1);
                break;
            }
        }
    };
    $scope.memberSelect = function (item) {
        for (var i = 0; i < $scope.cardMember.listMember.length; i++) {
            if ($scope.cardMember.listMember[i].UserName === item.UserName) {
                App.toastrError("Member đã tồn tại!");
                return;
            }
        }
        $scope.cardMember.listMember.push(item);
        for (var i = 0; i < $scope.listUser.length; i++) {
            if ($scope.listUser[i].UserName == item.UserName) {
                $scope.listUser.splice(i, 1);
                break;
            }
        }
    };
    $scope.removeMember = function (id) {
        for (var i = 0; i < $scope.cardMember.listMember.length; i++) {
            if ($scope.cardMember.listMember[i].UserName == id) {
                $scope.listUser.push($scope.cardMember.listMember[i]);
                $scope.cardMember.listMember.splice(i, 1);
                break;
            }
        }
    };

    $scope.useDefaultSetting = function () {
        $scope.DfSetting = $cookies.getObject("DefaultCardSetting");
        if ($scope.DfSetting === undefined) {
            App.toastrError("Chưa cài đặt mặt định!");
            return;
        }
        $scope.cardMember.listTeam = $scope.DfSetting.Team;
        $scope.cardMember.listMember = $scope.DfSetting.Member;
    };

    $scope.submit = function () {
        var data = { cardcode: cardCode, listteam: $scope.cardMember.listTeam, listmember: $scope.cardMember.listMember };
        console.log(data);
        dataservice.addMember(data, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.cancel();
            }
        });
    };
});
app.controller('add-checklist', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    $scope.model = {};
    $scope.validationOptions = {
        rules: {
            title: {
                required: true,
                maxlength: 255,
            }
        },
        messages: {
            title: {
                required: 'Nhập tiêu đề!',
                maxlength: 'Cho phép tối đa 255 ký tự!'
            }
        }
    }
    $scope.cancel = function () {
        $uibModalInstance.close();
    }
    $scope.initData = function () {
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initData();
    $scope.submit = function () {
        if ($scope.checklist.validate()) {
            console.log($scope.model);
            dataservice.addCheckList(cardCode, $scope.model.Title, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $scope.cancel();
                }
            })
        }
        //console.log(cardCode);
        //console.log(member);
    }
});
app.controller('attachment', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    $scope.model = {
        CardCode: '',
        FileCode: '',
        MemberId: '',
        CreatedTime: '',
        UpdatedTime: '',
        FileName: '',
        FileUrl: ''
    };
    $scope.validationOptions = {
        rules: {

        },
        messages: {

        }
    }
    $scope.cancel = function () {
        $uibModalInstance.close();
    }
    $scope.initData = function () {

    }
    $scope.initData();
    $scope.submit = function () {
        var file = document.getElementById("FileAttachment").files[0];

        if (file == null || file == undefined) {
            App.toastrError("Vui lòng chọn tệp tin");
        } else {
            var fileName = $('#FileAttachment').val();
            var idxDot = fileName.lastIndexOf(".") + 1;
            var extFile = fileName.substr(idxDot, fileName.length).toLowerCase();
            console.log(extFile);

            //if (extFile != "docx" && extFile != "doc" && extFile != "pdf") {
            //    App.toastrError("Chỉ tải lên tệp docx, doc hoặc pdf!");
            //    return;
            //};

            var data = new FormData();
            data.append("FileUpload", file);
            dataservice.uploadAttachment(data, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                    return;
                }
                else {
                    var file = fileName.split('\\');
                    $scope.model.FileName = file[file.length - 1];
                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                    $scope.model.CardCode = cardCode;
                    console.log($scope.model);

                    dataservice.addAttachment($scope.model, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                        }
                        else {
                            App.toastrSuccess(rs.Title);
                            $scope.cancel();
                        }
                    })
                }
            });
        }
    }
});
app.controller('label', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    $scope.model = {

    };
    $scope.validationOptions = {
        rules: {

        },
        messages: {

        }
    }
    $scope.cancel = function () {
        $uibModalInstance.close();
    }

    $scope.initData = function () {

    }
    $scope.initData();



    $scope.submit = function () {
        var label = '';
        var element = $('.label-checkbox:checked');
        angular.forEach(element, function (value, key) {
            label += value.value + ';';
        })
        label = label.substring(0, label.length - 1);
        console.log(label);
        if (label != '') {
            debugger
            dataservice.updateCardLabel(cardCode, label, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $scope.cancel();
                }
            })
        }
        else {
            App.toastrError("Chọn nhãn!");
        }
    }
});
app.controller('add-duedate', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, cardCode) {
    $scope.duedate = {
        DueDate: ''
    };
    $scope.cancel = function () {
        $uibModalInstance.close();
    }
    $scope.initData = function () {

    }
    $scope.initData();
    $scope.submit = function () {
        if ($scope.duedate.DueDate != '') {
            dataservice.updateDueDate(cardCode, $scope.duedate.DueDate, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $scope.cancel();
                }
            })
        }
    }
});
