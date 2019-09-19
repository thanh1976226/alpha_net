var ctxfolder = "/views/admin/project";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngCookies", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", "pascalprecht.translate", 'datatables.colreorder', 'angular-confirm', 'ui.select', 'dynamicNumber']);
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

        getItemAdd: function (data, callback) {
            $http.get('/Admin/Project/GetItemAdd?code=' + data).success(callback);
        },
        insert: function (data, callback) {
            $http.post('/Admin/Project/Insert/', data).success(callback);
        },
        insertProjectTabCustomer: function (data, callback) {
            $http.post('/Admin/Project/InsertProjectTabCustomer/', data).success(callback);
        },
        insertProjectTabMember: function (data, callback) {
            $http.post('/Admin/Project/InsertProjectTabMember/', data).success(callback);
        },
        updateProjectTabMember: function (data, callback) {
            $http.post('/Admin/Project/UpdateProjectTabMember/', data).success(callback);
        },
        insertProjectTabNote: function (data, callback) {
            $http.post('/Admin/Project/InsertProjectTabNote/', data).success(callback);
        },
        deleteprojectTabNote: function (data, callback) {
            $http.post('/Admin/Project/DeleteprojectTabNote/' + data).success(callback);
        },
        updateProjectTabNote: function (data, callback) {
            $http.post('/Admin/Project/UpdateProjectTabNote/', data).success(callback);
        },
        insertProjectFile: function (data, callback) {
            $http.post('/Admin/Project/InsertProjectFile/', data).success(callback);
        },
        updateProjectFile: function (data, callback) {
            $http.post('/Admin/Project/UpdateProjectFile/', data).success(callback);
        },
        deleteProjectFile: function (data, callback) {
            $http.post('/Admin/Project/DeleteProjectFile/' + data).success(callback);
        },

        insertProjectPayment: function (data, callback) {
            $http.post('/Admin/MaterialPaymentTicket/Insert', data).success(callback);
        },

        //getpayment
        getPaymentObjType: function (callback) {
            $http.post('/Admin/MaterialPaymentTicket/GetPaymentObjTypeProject').success(callback);
        },
        getPaymentType: function (callback) {
            $http.post('/Admin/Contract/GetPaymentType').success(callback);
        },
        getObjPayment: function (callback) {
            $http.post('/Admin/Contract/GetObjPayment').success(callback);
        },
        getUnit: function (callback) {
            $http.post('/Admin/MaterialPaymentTicket/GetUnit').success(callback);
        },
        getUser: function (callback) {
            $http.post('/Admin/Contract/GetUser').success(callback);
        },
        getProject: function (callback) {
            $http.post('/Admin/Project/GetProject').success(callback);
        },
        checkProject: function (data, callback) {
            $http.get('/Admin/MaterialPaymentTicket/CheckProject?contractCode=' + data).success(callback);
        },

        update: function (data, callback) {
            $http.post('/Admin/Project/Update/', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/Project/Delete/' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/Project/GetItem/' + data).success(callback);
        },
        deleteProjectTabCustomer: function (data, callback) {
            $http.post('/Admin/Project/DeleteProjectTabCustomer/' + data).success(callback);
        },
        deleteProjectTabMember: function (data, callback) {
            $http.post('/Admin/Project/DeleteProjectTabMember/' + data).success(callback);
        },
        getUser: function (callback) {
            $http.post('/Admin/Project/GetUser').success(callback);
        },
        getCustomers: function (callback) {
            $http.post('/Admin/Project/GetCustomers/').success(callback);
        },
        getItemCustomers: function (data, callback) {
            $http.post('/Admin/Project/GetItemCustomers/' + data).success(callback);
        },
        getMember: function (data, callback) {
            $http.get('/Admin/Project/GetMember/' + data).success(callback);
        },
        getNote: function (data, callback) {
            $http.get('/Admin/Project/GetNote/' + data).success(callback);
        },
        getRepository: function (callback) {
            $http.post('/Admin/EDMSRepository/JtreeRepository').success(callback);
        },

        insertFile: function (data, callback) {
            $http.post('/Admin/Project/InsertFile/', data).success(callback);
        },
        deleteFile: function (data, callback) {
            $http.post('/Admin/Project/DeleteFile/' + data).success(callback);
        },
        updateFile: function (data, callback) {
            $http.post('/Admin/Project/UpdateFile/', data).success(callback);
        },
        getFile: function (data, callback) {
            $http.get('/Admin/Project/GetFile/' + data).success(callback);
        },
        jtreeRepository: function (callback) {
            $http.post('/Admin/Project/JtreeRepository').success(callback);
        },

        insertContact: function (data, callback) {
            $http.post('/Admin/Project/InsertContact/', data).success(callback);
        },
        deleteContact: function (data, callback) {
            $http.post('/Admin/Project/DeleteContact/' + data).success(callback);
        },
        updateContact: function (data, callback) {
            $http.post('/Admin/Project/UpdateContact/', data).success(callback);
        },
        getContact: function (data, callback) {
            $http.get('/Admin/Project/GetContact/' + data).success(callback);
        },

        insertExtend: function (data, callback) {
            $http.post('/Admin/Project/InsertExtend/', data).success(callback);
        },
        deleteExtend: function (data, callback) {
            $http.post('/Admin/Project/DeleteExtend/' + data).success(callback);
        },
        updateExtend: function (data, callback) {
            $http.post('/Admin/Project/UpdateExtend/', data).success(callback);
        },
        getExtend: function (data, callback) {
            $http.get('/Admin/Project/GetExtend/' + data).success(callback);
        },

        uploadFile: function (data, callback) {
            submitFormUpload('/Admin/Project/UploadFile/', data, callback);
        },
        uploadImage: function (data, callback) {
            submitFormUpload('/Admin/Project/UploadImage/', data, callback);
        },

        getProjectUnit: function (callback) {
            $http.post('/Admin/Project/GetProjectUnit/').success(callback);
        },
        getProjectLanguage: function (callback) {
            $http.post('/Admin/Project/GetProjectLanguage/').success(callback);
        },
        getProjectStatus: function (callback) {
            $http.post('/Admin/Project/GetProjectStatus/').success(callback);
        },

        //CardJob
        getBoards: function (callback) {
            $http.post('/Admin/CardJob/GetBoards/').success(callback);
        },

        addCardJob: function (data, callback) {
            $http.post('/Admin/Project/AddCardJob/', data).success(callback);
        },

        getTeams: function (callback) {
            $http.post('/Admin/CardJob/GetTeams').success(callback);
        },
        getBoardByTeam: function (TeamCode, callback) {
            $http.post('/Admin/Project/GetBoards/?TeamCode=' + TeamCode).success(callback);
        },
        getLists: function (BoardCode, callback) {
            $http.post('/Admin/Project/GetLists/?BoardCode=' + BoardCode).success(callback);
        },
        getCards: function (ListCode, callback) {
            $http.post('/Admin/Project/GetCards/?ListCode=' + ListCode).success(callback);
        },
        addCardRelative: function (data, callback) {
            $http.post('/Admin/Project/AddCardRelative/', data).success(callback);
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
            $http.post('/Admin/Contract/GetUser').success(callback);
        },
    }
});

app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $cookies, $filter, dataservice, $translate) {
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
            if (!partternCode.test(data)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.PROJECT_CURD_VALIDATE_CHARACTERS_SPACE, "<br/>");
            }

            return mess;
        }
        $rootScope.checkDatafile = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var partternTelephone = /[0-9]/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.PROJECT_CURD_TAB_FILE_VALIDATE_CHARACTERS_SPACE, "<br/>");
            }

            return mess;
        }
        $rootScope.checkDatapayment = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;

            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.PayCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.PROJECT_CURD_TAB_PAYMENT_VALIDATE_CHARACTERS_SPACE, "<br/>");
            }

            return mess;
        }
        $rootScope.validationOptions = {
            rules: {
                ProjectCode: {
                    required: true,
                    maxlength: 100
                },
                ProjectTitle: {
                    required: true,
                    maxlength: 255
                },
                Budget: {
                    required: true
                },
                FromTo: {
                    required: true
                },
                DateTo: {
                    required: true
                }
            },
            messages: {
                ProjectCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_CODE_PROJECT),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_CODE_PROJECT).replace("{1}", "255")
                },
                ProjectTitle: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_NAME_PROJECT),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_NAME_PROJECT).replace("{1}", "255")
                },
                Budget: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_BUDGET),
                },
                FromTo: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_STARTTIME),
                },
                DateTo: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_LBL_PROJECT_ENDTIME),
                }
            }
        }
        $rootScope.validateMember = {
            rules: {
                Position: {
                    required: true
                },

            },
            messages: {
                Position: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_MEMBER_LBL_POSITION),
                },

            }
        }
        $rootScope.validationOptionsFile = {
            rules: {
                FileCode: {
                    required: true
                },

            },
            messages: {
                FileCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_FILE_CURD_LBL_FILECODE),
                },

            }
        }

        $rootScope.validateNote = {
            rules: {
                Note: {
                    required: true,
                    maxlength: 500
                },
                Title: {
                    required: true,
                    maxlength: 100
                },

            },
            messages: {
                Note: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_NOTE_CURD_LBL_NOTE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_TAB_NOTE_CURD_LBL_NOTE).replace("{1}", "500")
                },
                Title: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_NOTE_CURD_LBL_TITLE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_TAB_NOTE_CURD_LBL_TITLE).replace("{1}", "100")
                },

            }
        }

        $rootScope.validationOptionsContact = {
            rules: {
                ContractName: {
                    required: true
                },
                Email: {
                    required: true
                },
                Mobile: {
                    required: true
                }
            },
            messages: {
                ContractName: {
                    required: 'Bắt buộc'
                },
                Email: {
                    required: 'Bắt buộc'
                },
                Mobile: {
                    required: 'Bắt buộc'
                }
            }
        }
        $rootScope.validationOptionsAttr = {
            rules: {
                Code: {
                    required: true
                },
                Value: {
                    required: true
                }
            },
            messages: {
                Code: {
                    required: 'Bắt buộc'
                },
                Value: {
                    required: 'Bắt buộc'
                }
            }
        }
        $rootScope.validationOptionsPayment = {
            rules: {
                PayCode: {
                    required: true,
                    maxlength: 100
                },
                PayTitle: {
                    required: true,
                    maxlength: 255
                },
                MoneyTotal: {
                    required: true
                }
            },
            messages: {
                PayCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_PAYMENT_CURD_LBL_PAYCODE),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_TAB_PAYMENT_CURD_LBL_PAYCODE).replace("{1}", "100")
                },
                PayTitle: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_PAYMENT_CURD_LBL_PAYNAME),
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.PROJECT_CURD_TAB_PAYMENT_CURD_LBL_PAYNAME).replace("{1}", "255")
                },
                MoneyTotal: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.PROJECT_CURD_TAB_PAYMENT_CURD_LBL_TOTAL),
                }
            }
        }
        //$rootScope.checkData = function (data) {
        //    var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
        //    var mess = { Status: false, Title: "" }
        //    if (!partternCode.test(data)) {
        //        mess.Status = true;
        //        //mess.Title = mess.Title.concat(" - ", "Mã nhà cung cấp không chứa ký tự đặc biệt hoặc khoảng trắng!", "<br/>");
        //    }
        //    return mess;
        //}
        //$rootScope.validationOptions = {
        //    rules: {
        //        ImpCode: {
        //            required: true,
        //            maxlength: 100
        //        },
        //        CreatedTime: {
        //            required: true,
        //        },
        //        Note: {
        //            maxlength: 1000
        //        }
        //    },
        //    messages: {
        //        ImpCode: {
        //            required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CODE),
        //            maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MIS_CURD_LBL_MIS_CODE).replace("{1}", "100")
        //        },
        //        CreatedTime: {
        //            required: caption.COM_ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CREATE),
        //            //required: caption.ERR_REQUIRED.replace("{0}", caption.MIS_CURD_LBL_MIS_CREATE)
        //        },
        //        Note: {
        //            maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.MIS_CURD_LBL_MIS_NOTE).replace("{1}", "1000"),
        //        }
        //    }
        //}
        //$rootScope.checkData = function (data) {
        //    var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/;
        //    var partternName = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]+[^!@#$%^&*<>?]*$/; //Có chứa được khoảng trắng
        //    //var partternDescription = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@#$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng
        //    var partternUrl = /^[ĂăĐđĨĩŨũƠơƯưẠ-ỹa-zA-Z0-9]*[^!@$%^&*<>?]*$/; //Có thể null, và có chứa được khoảng trắng & dấu #
        //    var mess = { Status: false, Title: "" }
        //    if (!partternCode.test(data.ApplicationCode)) {
        //        mess.Status = true;
        //        mess.Title = mess.Title.concat(" - ", caption.VALIDATE_ITEM_CODE.replace('{0}', caption.APP_CODE), "<br/>");
        //    }
        //    if (!partternName.test(data.Title)) {
        //        mess.Status = true;
        //        mess.Title += " - " + caption.VALIDATE_ITEM_NAME.replace('{0}', caption.APP_NAME) + "<br/>";
        //    }
        //    if (!partternUrl.test(data.AppUrl)) {
        //        mess.Status = true;
        //        mess.Title += " - " + caption.VALIDATE_ITEM.replace('{0}', caption.URL_APP) + "<br/>";
        //    }
        //    return mess;
        //}
        //$rootScope.validationOptions = {
        //    rules: {
        //        Title: {
        //            required: true,
        //            maxlength: 255
        //        },
        //        ApplicationCode: {
        //            required: true,
        //            maxlength: 50
        //        },
        //        Ord: {
        //            required: true,
        //            maxlength: 5
        //        },
        //        AppUrl: {
        //            required: true,
        //            maxlength: 255
        //        }
        //    },
        //    messages: {
        //        Title: {
        //            required: caption.ERR_REQUIRED.replace('{0}', caption.APP_NAME),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace('{0}', caption.APP_NAME).replace('{1}', '255')
        //        },
        //        ApplicationCode: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.APP_CODE),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace("{0}", caption.APP_CODE).replace("{1}", "50")
        //        },
        //        Ord: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.ORDER),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace('{0}', caption.ORDER_SORT).replace('{1}', '5')
        //        },
        //        AppUrl: {
        //            required: caption.ERR_REQUIRED.replace("{0}", caption.URL_APP),
        //            maxlength: caption.ERR_EXCEED_CHARACTERS.replace("{0}", caption.URL_APP).replace("{1}", "255")
        //        }
        //    }
        //}

    });

    
    

    $rootScope.Categories = [];
    dataservice.getRepository(function (result) {
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

    $rootScope.PaymentType = [{
        Value: 0,
        Name: "Phiếu chi"
    }, {
        Value: 1,
        Name: "Phiếu thu"
    }]

    $rootScope.DateNow = $filter('date')(new Date(), 'dd/MM/yyyy');
    $rootScope.DateBeforeSevenDay = $filter('date')(new Date().setDate((new Date()).getDate() + (-7)), 'dd/MM/yyyy');
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
        .when('/contact-add', {
            templateUrl: ctxfolder + '/contact_add.html',
            controller: 'contact_add'
        })
        .when('/file-add', {
            templateUrl: ctxfolder + '/file_add.html',
            controller: 'file_add'
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
app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.model = {
        ProjectCode: '',
        ProjectTitle: '',
        Budget: '',
        StartTime: '',
        EndTime: '',
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTable",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectCode = $scope.model.ProjectCode;
                d.ProjectTitle = $scope.model.ProjectTitle;
                d.Budget = $scope.model.Budget;
                d.StartTime = $scope.model.StartTime;
                d.EndTime = $scope.model.EndTime;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(pageLength)
        .withOption('order', [1, 'desc'])
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Id').withTitle('Id').withOption('sClass', 'hidden').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ProjectCode').withTitle('{{ "PROJECT_LIST_COL_PROJECT_CODE_PROJECT" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ProjectTitle').withTitle('{{ "PROJECT_LIST_COL_PROJECT_NAME_PROJECT" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Currency').withTitle('{{ "PROJECT_LIST_COL_PROJECT_CURRENCY" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Budget').withTitle('{{ "PROJECT_LIST_COL_PROJECT_BUDGET" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('StartTime').withTitle('{{ "PROJECT_LIST_COL_PROJECT_STARTTIME" | translate }}').renderWith(function (data, type) {
        var date = $filter('date')(new Date(data), 'MM/dd/yyyy');
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('EndTime').withTitle('{{ "PROJECT_LIST_COL_PROJECT_ENDTIME" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withOption('sClass', 'nowrap').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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

    $scope.search = function () {
        validationSelect($scope.model);
        if (validationSelect($scope.model).Status == false) {
            reloadData(true);
        }
    };
    $scope.reload = function () {
        validationSelect($scope.model);
        if (validationSelect($scope.model).Status == false) {
            reloadData(true);
        }
    }
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
            $scope.reload();
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
            $scope.reload();
        }, function () {
        });
    };
    $scope.addCardJob = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: "/views/Admin/CardJob/edit-tag.html",
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
        //$('#FromTo').datepicker('setEndDate', $rootScope.DateNow);
        //$('#DateTo').datepicker('setStartDate', $rootScope.DateBeforeSevenDay);
        //$('#FromTo').datepicker('update', $rootScope.DateBeforeSevenDay);
        //$('#DateTo').datepicker('update', $rootScope.DateNow);
        $('.end-date').click(function () {
            $('#FromTo').datepicker('setEndDate', null);
        });
        $('.start-date').click(function () {
            $('#DateTo').datepicker('setStartDate', null);
        });
    }
    setTimeout(function () {
        loadDate();
        showHideSearch();
    }, 200);


    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Budget" && ($scope.model.Budget == "" || $scope.model.Budget && $rootScope.partternFloat.test($scope.model.Budget))) {
            $scope.errorBudget = false;
        } else if (SelectType == "Budget") {
            $scope.errorBudget = true;
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.Budget && !$rootScope.partternFloat.test(data.Budget)) {
            $scope.errorBudget = true;
            mess.Status = true;
        } else {
            $scope.errorBudget = false;
        }


        return mess;
    };
});
app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $rootScope.ProjectCode = '';
    $rootScope.ProjectId = '';
    $scope.model = {

        ProjectCode: '',
        ProjectTitle: '',
        Currency: '',
        Budget: '',
        PrjSkillKeyword: '',
        StartTime: '',
        EndTime: '',
        PrjMode: '',
        SetPriority: '',
        PrjStatus: '',
    }
    $scope.initData = function () {
        dataservice.getProjectUnit(function (rs) {
            if (!rs.Error) {
                $scope.projectUnit = rs;
            }
        });
        dataservice.getProjectLanguage(function (rs) {
            if (!rs.Error) {
                $scope.projectLanguage = rs;
            }
        });
        dataservice.getProjectStatus(function (rs) {
            if (!rs.Error) {
                $scope.projectStatus = rs;
            }
        });
    }
    $scope.initData();

    $scope.chkProject = function () {
        if ($rootScope.ProjectCode == '') {
            App.toastrError(caption.PROJECT_CHECK_CLICK_OPEN_TAB);
        }
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && !validationSelect($scope.model).Status) {
            var msg = $rootScope.checkData($scope.model.ProjectCode);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            dataservice.insert($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $scope.dimiss = "";
                    $scope.initData = function () {
                        dataservice.getItemAdd($scope.model.ProjectCode, function (rs) {
                            if (rs.Error) {
                                App.toastrError(rs.Title);
                            } else {
                                $rootScope.ProjectId = rs.Id;
                                $rootScope.ProjectCode = rs.ProjectCode;

                                //$rootScope.CusSupId = rs.SupID;
                            }
                        });
                    }
                    $scope.initData();
                    App.unblockUI("#contentMain");
                    //$uibModalInstance.close();
                }
            });
        }
    }


    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Currency" && $scope.model.Currency != "") {
            $scope.errorCurrency = false;
        }
        if (SelectType == "PrjStatus" && $scope.model.PrjStatus != "") {
            $scope.errorPrjStatus = false;
        }

    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        if (data.Currency == "" || data.Currency == null) {
            $scope.errorCurrency = true;
            mess.Status = true;
        } else {
            $scope.errorCurrency = false;
        }
        if (data.PrjStatus == "" || data.PrjStatus == null) {
            $scope.errorPrjStatus = true;
            mess.Status = true;
        } else {
            $scope.errorPrjStatus = false;
        }
        return mess;
    };
    function initDateTime() {
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

    }

    //Converto datetime dd/MM/yyyy to MM/dd/yyyy
    function convertDatetime(date) {
        var array = date.split('/');
        var result = array[1] + '/' + array[0] + '/' + array[2];
        return result;
    }
    setTimeout(function () {
        initDateTime();
        setModalMaxHeight('.modal');

        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter, para) {
    $rootScope.ProjectId = para;
    $scope.initData = function () {
        dataservice.getItem(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                $rootScope.ProjectCode = $scope.model.ProjectCode;
                $scope.model.StartTime = $filter('date')(new Date($scope.model.StartTime), 'dd/MM/yyyy');
                $scope.model.EndTime = $filter('date')(new Date($scope.model.EndTime), 'dd/MM/yyyy');
            }
        });
        dataservice.getProjectUnit(function (rs) {
            if (!rs.Error) {
                $scope.projectUnit = rs;

            }
        });
        dataservice.getProjectLanguage(function (rs) {
            if (!rs.Error) {
                $scope.projectLanguage = rs;
            }
        });
        dataservice.getProjectStatus(function (rs) {
            if (!rs.Error) {
                $scope.projectStatus = rs;
            }
        });
    }
    $scope.initData();

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.editform.validate() && !validationSelect($scope.model).Status) {
            $scope.model.StartTime = convertDatetime($scope.model.StartTime);
            $scope.model.EndTime = convertDatetime($scope.model.EndTime);
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
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Currency" && $scope.model.Currency != "") {
            $scope.errorCurrency = false;
        }
        if (SelectType == "PrjStatus" && $scope.model.PrjStatus != "") {
            $scope.errorPrjStatus = false;
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
    function initDateTime() {
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

    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        if (data.Currency == "" || data.Currency == null) {
            $scope.errorCurrency = true;
            mess.Status = true;
        } else {
            $scope.errorCurrency = false;
        }
        if (data.PrjStatus == "" || data.PrjStatus == null) {
            $scope.errorPrjStatus = true;
            mess.Status = true;
        } else {
            $scope.errorPrjStatus = false;
        }
        return mess;
    };
    //Converto datetime dd/MM/yyyy to MM/dd/yyyy
    function convertDatetime(date) {
        var array = date.split('/');
        var result = array[1] + '/' + array[0] + '/' + array[2];
        return result;
    }
    setTimeout(function () {
        initDateTime();
        setModalMaxHeight('.modal');

        setModalDraggable('.modal-dialog');
    }, 200);
});


app.controller('projectTabSupplier', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
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
            url: "/Admin/Project/JTableSupplier",
            beforeSend: function (jqXHR, settings) {
                //App.blockUI({
                //    target: "#contact-main",
                //    boxed: true,
                //    message: 'loading...'
                //});
            },
            type: 'POST',
            data: function (d) {
                //d.SupplierId = $rootScope.SupplierId;
                //d.ContactName = $scope.model.ContactName;
                //d.ContactEmail = $scope.model.ContactEmail;
                //d.ContactTelephone = $scope.model.ContactTelephone;
                //d.ContactMobilePhone = $scope.model.ContactMobilePhone;
            },
            complete: function () {
                //App.unblockUI("#contact-main");
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('SupplierCode').withTitle('Mã nhà cung cấp').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('SupName').withTitle('Tên nhà cung cấp').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Address').withTitle('Địa chỉ').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Email').withTitle('Thư điện tử').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('Thao tác').renderWith(function (data, type, full, meta) {
        return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
            templateUrl: ctxfolder + '/contact_add.html',
            controller: 'contact_add',
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
            templateUrl: ctxfolder + '/contact_edit.html',
            controller: 'contact_edit',
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
                    dataservice.deleteContact(id, function (result) {
                        console.log(result);
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
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
    setTimeout(function () {
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
    }, 200);
});
app.controller('contact_add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.model = {
        FileName: ''
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.submit = function () {
        if ($scope.addForm.validate()) {
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
                    $scope.model.CusSupId = $rootScope.SupplierId
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
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contact_edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.model = {
        FileName: ''
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.initData = function () {
        dataservice.getContact(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
            }
        });
    }
    $scope.initData();

    $scope.submit = function () {
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
                        }
                    });
                }
            });
        }
    }
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
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});


app.controller('projectTabCustomer', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $rootScope.abc = $rootScope.ProjectId;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        CustomerCode: '',
        FromDate: '',
        ToDate: '',
        ProjectId: '',

    }

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableCustomer",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
                //$scope.initData = function () {
                //    var a = $rootScope.ProjectId;
                //    dataservice.getItemCustomers(a, function (rs) {
                //        if (rs.Error) {
                //            App.toastrError(rs.Title);
                //        } else {
                //            $rootScope.ProjectCode = rs.ProjectCode;

                //        }
                //    });
                //}
                //$scope.initData();
                //d.ProjectCode = $rootScope.ProjectId;
                d.CustomerCode = $scope.model.CustomerCode;
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CustomerCode').withTitle('{{ "PROJECT_CURD_TAB_CUSTOMER_LIST_COL_CUSCODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CusName').withTitle('{{ "PROJECT_CURD_TAB_CUSTOMER_LIST_COL_CUSNAME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Address').withTitle('{{ "PROJECT_CURD_TAB_CUSTOMER_LIST_COL_ADDRESS" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Email').withTitle('{{ "PROJECT_CURD_TAB_CUSTOMER_LIST_COL_EMAIL" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';

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
    $scope.add = function () {

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabCustomerAdd.html',
            controller: 'projectTabCustomerAdd',
            backdrop: true,
            size: '40',
        });
        modalInstance.result.then(function (d) {
            reloadData()
        }, function () { });
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/file_edit.html',
            controller: 'file_edit',
            backdrop: true,
            size: '90',
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
                    dataservice.deleteProjectTabCustomer(id, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close();
                        }
                        App.unblockUI("#contentMain");
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
    setTimeout(function () {
        $("#datefromCustomer").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datetoCustomer').datepicker('setStartDate', maxDate);
        });
        $("#datetoCustomer").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefromCustomer').datepicker('setEndDate', maxDate);
        });
    }, 200);
});
app.controller('projectTabCustomerAdd', function ($scope, $rootScope, $compile, $uibModalInstance, dataservice) {
    $scope.model = {
        ProjectCode: '',

    };
    $scope.model.Category = '';
    $scope.treeData = [];
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.initData = function () {
        $scope.dimiss = "disabled-element";
        dataservice.getCustomers(function (rs) {
            $scope.Customers = rs;
        })
    }

    $scope.initData();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };


        if (data.CusCode == "" || data.CusCode == null) {
            $scope.errorCusCode = true;
            mess.Status = true;
        } else {
            $scope.errorCusCode = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "CusCode" && $scope.model.CusCode != "") {
            $scope.errorCusCode = false;
        }
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if (validationSelect($scope.model).Status == false) {
            $scope.model.ProjectCode = $rootScope.ProjectCode;
            dataservice.insertProjectTabCustomer($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
            });
        };
    };
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('file_edit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.model = {
        FileName: ''
    };
    $scope.model.Category = '';
    $scope.treeData = [];
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.readyCB = function () {
        dataservice.jtreeRepository(function (result) {
            dataservice.getFile(para, function (rs) {
                $scope.model = rs;
                for (var i = 0; i < result.length; i++) {
                    var data = {
                        id: result[i].ReposCode,
                        data: result[i].ReposID,
                        parent: result[i].Parent,
                        text: result[i].ReposName,
                        state: { selected: result[i].ReposID == rs.Category, opened: true }
                    }
                    $scope.treeData.push(data);
                }
            });
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
        $scope.model.Category = selected.node.data;
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
        'select_node': $scope.selectNodeRepository,
        'search': $scope.searchTreeRepository,
    }
    $scope.submit = function () {
        var files = $('#FileInput').get(0);
        var file = files.files[0];
        var data = new FormData();
        var fileName = '';

        if (file == null) {
            $scope.model.SupplierId = $rootScope.SupplierId;
            console.log($scope.model);
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
                    $scope.model.FileName = rs.Object;
                    $scope.model.SupplierId = $rootScope.SupplierId;
                    $scope.model.FileUrl = '/uploads/files/' + $scope.model.FileName;
                    $scope.model.FilePath = '~/upload/files/' + $scope.model.FileName;

                    console.log($scope.model);
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

            setTimeout(function () {
                App.inputFile();
            }, 200);
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
    //setTimeout(function () {
    //    App.inputFile();
    //}, 200);
});


app.controller('projectTabMember', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        fullname: '',
        Position: '',
        ProjectId: '',
        //ProjectCode:'',
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableMember",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
                d.fullname = $scope.model.fullname;
                d.Position = $scope.model.Position;
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', ''));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Name').withTitle('{{ "PROJECT_CURD_TAB_MEMBER_LIST_COL_FULLNAME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Position').withTitle('{{ "PROJECT_CURD_TAB_MEMBER_LIST_COL_POSITION" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Email').withTitle('{{ "PROJECT_CURD_TAB_MEMBER_LIST_COL_EMAIL" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Active').withTitle('{{ "PROJECT_CURD_TAB_MEMBER_LIST_COL_STATUS" | translate }}').renderWith(function (data, type) {
        return data == "True" ? '<span class="text-success">Hoạt động</span>' : '<span class="text-danger">Không hoạt động</span>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Action').notSortable().withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full, meta) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>';
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
            templateUrl: ctxfolder + '/projectTabMemberAdd.html',
            controller: 'projectTabMemberAdd',
            backdrop: true,
            size: '50'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }
    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabMemberEdit.html',
            controller: 'projectTabMemberEdit',
            backdrop: true,
            size: '50',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData();
        }, function () { });
    };
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteProjectTabMember(id, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close();
                        }
                        App.unblockUI("#contentMain");
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
app.controller('projectTabMemberAdd', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        Position: '',
        ProjectCode: '',

    };
    $scope.initLoad = function () {

        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initLoad();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };


        if (data.MemberCode == "" || data.MemberCode == null) {
            $scope.errorMemberCode = true;
            mess.Status = true;
        } else {
            $scope.errorMemberCode = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "MemberCode" && $scope.model.MemberCode != "") {
            $scope.errorMemberCode = false;
        }
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            $scope.model.ProjectCode = $rootScope.ProjectCode;
            dataservice.insertProjectTabMember($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
                App.unblockUI("#contentMain");
            });
        };
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('projectTabMemberEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        Member: '',
    };
    $scope.initData = function () {
        dataservice.getMember(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                $scope.model.Member = $scope.model.MemberCode;
            }
        });
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initData();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };


        if (data.MemberCode == "" || data.MemberCode == null) {
            $scope.errorMemberCode = true;
            mess.Status = true;
        } else {
            $scope.errorMemberCode = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "MemberCode" && $scope.model.MemberCode != "") {
            $scope.errorMemberCode = false;
        }
    }

    $scope.submit = function () {

        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            $scope.model.ProjectCode = $rootScope.ProjectCode;
            dataservice.updateProjectTabMember($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            });
        };
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});

app.controller('projectTabFile', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        FileName: '',
        FromDate: '',
        ToDate: '',
        ProjectId: '',
        //ProjectCode:'',
    }

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableFile",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
                d.FileName = $scope.model.FileName;
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(5)
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('FileName').withTitle('{{ "PROJECT_CURD_TAB_FILE_LIST_COL_FILENAME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ReposName').withTitle('{{ "PROJECT_CURD_TAB_FILE_LIST_COL_REPOSNAME" | translate }}').renderWith(function (data, type, full) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{ "PROJECT_CURD_TAB_FILE_LIST_COL_CREATETIME" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Tags').withTitle('{{ "PROJECT_CURD_TAB_FILE_LIST_COL_TAGS" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Description').withTitle('{{ "PROJECT_CURD_TAB_FILE_LIST_COL_NOTE" | translate }}').notSortable().renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<a href="' + full.FileUrl + '" target="_blank" style="width: 25px; height: 25px; padding: 0px" title="Tải xuống - ' + full.FileName + '" class="btn btn-icon-only btn-circle btn-outline green " download><i class="fa fa-download pt5"></i></a>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    //$scope.submit = function () {
    //    $confirm({ text: 'Thêm mới tệp tin?', title: 'Xác nhận', cancel: ' Hủy ' })
    //        .then(function () {
    //            var file = $('#FileInput').get(0).files[0];
    //            var data = new FormData();
    //            data.append("FileUpload", file);
    //            dataservice.uploadFile(data, function (rs) {
    //                if (rs.Error) {
    //                    App.toastrError(rs.Title);
    //                }
    //                else {
    //                    $scope.model.ContractCode = $rootScope.ContractHeaderId;
    //                    $scope.model.FileName = rs.Object;
    //                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
    //                    $scope.model.Category = $scope.model.Category.Value;
    //                    dataservice.insertContractFile($scope.model, function (result) {
    //                        if (result.Error) {
    //                            App.toastrError(result.Title);
    //                        } else {
    //                            App.toastrSuccess(result.Title);
    //                            $scope.reload();
    //                        }
    //                    });
    //                }
    //            })
    //        });
    //}

    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabFileAdd.html',
            controller: 'projectTabFileAdd',
            backdrop: true,
            size: '50'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }

    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabFileEdit.html',
            controller: 'projectTabFileEdit',
            backdrop: true,
            size: '50',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData();
        }, function () { });
    };

    $scope.save = function () {
        var files = $("#FileInput").get(0);
        var file = files.files[0];
        var data = new FormData();

        if (file == null) {
            $scope.model.Category = $scope.model.Category.Value;
            dataservice.updateContractFile($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $scope.model = [];
                    $scope.model = $scope.currentData;
                    $("#btnAddFile").removeClass("hidden");
                    $("#btnEditFile").addClass("hidden");
                    $scope.reload();
                }
            });
        }
        else {
            data.append("FileUpload", file);
            dataservice.uploadFile(data, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    $scope.model.FileName = rs.Object;
                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                    $scope.model.Category = $scope.model.Category.Value;
                    dataservice.updateContractFile($scope.model, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $scope.model = [];
                            $scope.model = $scope.currentData;
                            $("#btnAddFile").removeClass("hidden");
                            $("#btnEditFile").addClass("hidden");
                            $scope.reload();
                        }
                    });
                }
            });
            setTimeout(function () {
                App.inputFile();
            }, 200);
        }
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ?";
                $scope.ok = function () {
                    dataservice.deleteProjectFile(id, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
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
    setTimeout(function () {
        $("#datefromFile").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datetoFiler').datepicker('setStartDate', maxDate);
        });
        $("#datetoFile").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefromFile').datepicker('setEndDate', maxDate);
        });
    }, 200);
});
app.controller('projectTabFileAdd', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        FileUpload: '',
        FileCode: '',
        FileName: '',
        FileType: '',
        Tags: '',
        Category: '',
        Description: '',
        ProjectCode: ''
    }
    $scope.initLoad = function () {

        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initLoad();

    function validationSelect(data) {
        var mess = { Status: false, Title: "" };
        if (data.Category == "") {
            $scope.errorCategory = true;
            mess.Status = true;
        } else {
            $scope.errorCategory = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Category" && $scope.model.Category != "") {
            $scope.errorCategory = false;
        }
        //if (SelectType == "test" && $scope.model.test != "") {
        //    $scope.errortest = false;
        //}
    }
    $scope.errortest = false;


    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addfileform.validate() && validationSelect($scope.model).Status == false) {
            var msg = $rootScope.checkDatafile($scope.model.FileCode);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            $rootScope.modelFileAdd = $scope.model;
            var file = document.getElementById("File").files[0]
            if (file == null || file == undefined) {
                App.toastrError(caption.PROJECT_CURD_TAB_FILE_VALIDATE_CLICK_FILE);
            } else {
                var modalInstance = $uibModal.open({
                    templateUrl: ctxfolderMessage + '/messageConfirmAdd.html',

                    controller: function ($scope, $uibModalInstance) {
                        $scope.message = caption.PROJECT_CHECK_CLICK_BTN_ADD_TAB_FILE;
                        $scope.ok = function () {
                            $scope.model = $rootScope.modelFileAdd;
                            var file = $('#File').get(0).files[0];
                            var data = new FormData();
                            data.append("FileUpload", file);
                            dataservice.uploadFile(data, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                }
                                else {
                                    $scope.model.ProjectCode = $rootScope.ProjectCode;
                                    $scope.model.FileName = $('.inputFile').val().split('\\').pop();
                                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                                    $scope.model.Category = $scope.model.Category.Value;
                                    $scope.model.Tags = $scope.model.MemberCode;
                                    dataservice.insertProjectFile($scope.model, function (result) {
                                        if (result.Error) {
                                            App.toastrError(result.Title);
                                        } else {
                                            App.toastrSuccess(result.Title);
                                            //$scope.model.FileName = "";
                                            //$scope.model.Category = "";
                                            //$scope.model.Tags = "";
                                            //$scope.model.Description = "";
                                            $uibModalInstance.close();
                                        }
                                    });
                                }
                            })
                        };
                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                    },
                    size: '25',
                });
                modalInstance.result.then(function (d) {
                    $uibModalInstance.close();
                }, function () {
                });




                //$confirm({ text: 'Thêm tệp tin cho dự án này?', title: 'Xác nhận', cancel: ' Hủy ' })
                //    .then(function () {
                //        App.blockUI({
                //            target: "#contentMain",
                //            boxed: true,
                //            message: 'loading...'
                //        });
                //        var file = $('#File').get(0).files[0];
                //        var data = new FormData();
                //        data.append("FileUpload", file);
                //        dataservice.uploadFile(data, function (rs) {
                //            if (rs.Error) {
                //                App.toastrError(rs.Title);
                //            }
                //            else {
                //                $scope.model.ProjectCode = $rootScope.ProjectCode;
                //                $scope.model.FileName = $('.inputFile').val().split('\\').pop();
                //                $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                //                $scope.model.Category = $scope.model.Category.Value;
                //                $scope.model.Tags = $scope.model.MemberCode;
                //                dataservice.insertProjectFile($scope.model, function (result) {
                //                    if (result.Error) {
                //                        App.toastrError(result.Title);
                //                    } else {
                //                        App.toastrSuccess(result.Title);
                //                        //$scope.model.FileName = "";
                //                        //$scope.model.Category = "";
                //                        //$scope.model.Tags = "";
                //                        //$scope.model.Description = "";
                //                        $uibModalInstance.close();
                //                    }
                //                });
                //            }
                //        })
                //    });



            }
        };
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
        function myFunction() {
            var x = document.getElementById("mySelect").value;
            if (x != null) { $scope.errortest = false; }
        };

    }, 200);
});
app.controller('projectTabFileEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {};
    $scope.model.Member = '';

    $scope.initData = function () {

        dataservice.getFile(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                $scope.model.MemberCode = $scope.model.Tags;
                //$('.inputFile').val() = $scope.model.FileName ;
                angular.forEach($scope.Categories, function (value, key) {
                    if (value.Value == rs.Category) {
                        $scope.model.Category = value;
                        return;
                    }
                });
            }
        });
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initData();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };
        if (data.Category == "") {
            $scope.errorCategory = true;
            mess.Status = true;
        } else {
            $scope.errorCategory = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "Category" && $scope.model.Category != "") {
            $scope.errorCategory = false;
        }
        //if (SelectType == "test" && $scope.model.test != "") {
        //    $scope.errortest = false;
        //}
    }
    $scope.errortest = false;

    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addfileform.validate() && validationSelect($scope.model).Status == false) {
            //var file = document.getElementById("File").files[0]
            //if (file == null || file == undefined) {
            //    App.toastrError("Vui lòng chọn tệp tin");
            //} else {
            var msg = $rootScope.checkDatafile($scope.model.FileCode);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }

            $rootScope.modelFileAdd = $scope.model;

            var modalInstance = $uibModal.open({
                templateUrl: ctxfolderMessage + '/messageConfirmUpdate.html',

                controller: function ($scope, $uibModalInstance) {
                    $scope.message = "Chỉnh sửa tệp tin cho dự án này?";
                    $scope.ok = function () {
                        $scope.model = $rootScope.modelFileAdd;

                        var file = $('#File').get(0).files[0];
                        var data = new FormData();
                        if (file == null) {
                            $scope.model.ProjectCode = $rootScope.ProjectCode;
                            $scope.model.Category = $scope.model.Category.Value;
                            $scope.model.Tags = $scope.model.MemberCode;
                            dataservice.updateProjectFile($scope.model, function (result) {
                                if (result.Error) {
                                    App.toastrError(result.Title);
                                } else {
                                    App.toastrSuccess(result.Title);
                                    $uibModalInstance.close();

                                }
                            });
                        } else {
                            data.append("FileUpload", file);
                            dataservice.uploadFile(data, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                }
                                else {
                                    $scope.model.ProjectCode = $rootScope.ProjectCode;
                                    $scope.model.FileName = $('.inputFile').val().split('\\').pop();
                                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                                    $scope.model.Category = $scope.model.Category.Value;
                                    $scope.model.Tags = $scope.model.MemberCode;
                                    dataservice.updateProjectFile($scope.model, function (result) {
                                        if (result.Error) {
                                            App.toastrError(result.Title);
                                        } else {
                                            App.toastrSuccess(result.Title);
                                            $uibModalInstance.close();
                                        }
                                    });
                                }
                            })
                        }

                    };
                    $scope.cancel = function () {
                        $uibModalInstance.dismiss('cancel');
                    };
                },
                size: '25',
            });
            modalInstance.result.then(function (d) {
                $uibModalInstance.close();
            }, function () {
            });



            //$confirm({ text: 'Chỉnh sửa tệp tin cho dự án này?', title: 'Xác nhận', cancel: ' Hủy ' })
            //    .then(function () {
            //        App.blockUI({
            //            target: "#contentMain",
            //            boxed: true,
            //            message: 'loading...'
            //        });

            //        var file = $('#File').get(0).files[0];
            //        var data = new FormData();
            //        if (file == null) {
            //            $scope.model.ProjectCode = $rootScope.ProjectCode;
            //            $scope.model.Category = $scope.model.Category.Value;
            //            $scope.model.Tags = $scope.model.MemberCode;
            //            dataservice.updateProjectFile($scope.model, function (result) {
            //                if (result.Error) {
            //                    App.toastrError(result.Title);
            //                } else {
            //                    App.toastrSuccess(result.Title);
            //                    $uibModalInstance.close();

            //                }
            //            });
            //        } else {
            //            data.append("FileUpload", file);
            //            dataservice.uploadFile(data, function (rs) {
            //                if (rs.Error) {
            //                    App.toastrError(rs.Title);
            //                }
            //                else {
            //                    $scope.model.ProjectCode = $rootScope.ProjectCode;
            //                    $scope.model.FileName = $('.inputFile').val().split('\\').pop();
            //                    $scope.model.FileUrl = '/uploads/files/' + rs.Object;
            //                    $scope.model.Category = $scope.model.Category.Value;
            //                    $scope.model.Tags = $scope.model.MemberCode;
            //                    dataservice.updateProjectFile($scope.model, function (result) {
            //                        if (result.Error) {
            //                            App.toastrError(result.Title);
            //                        } else {
            //                            App.toastrSuccess(result.Title);
            //                            $uibModalInstance.close();
            //                        }
            //                    });
            //                }
            //            })
            //        }


            //    });
            ////}


        };
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});

app.controller('projectTabPayment', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        PayTitle: '',
        FromDate: '',
        ToDate: '',
        ProjectId: '',
        PaymentType: ''
    }

    //$scope.model = {
    //    ContractCode: ''
    //}
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableProjectTabPayment",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
                d.PayTitle = $scope.model.PayTitle;
                d.FromDate = $scope.model.FromDate;
                d.ToDate = $scope.model.ToDate;
                d.PaymentType = $scope.model.PaymentType;
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
            $compile(angular.element(row).find('input'))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        })
        .withOption('footerCallback', function (tfoot, data) {
            if (data.length > 0) {
                $scope.$apply(function () {
                    $scope.totalReceipts = 0;
                    $scope.totalPaymentSlip = 0;
                    angular.forEach(data, function (item, index) {
                        if (item.PayType == "True") {
                            $scope.totalReceipts = parseFloat($scope.totalReceipts) + parseFloat(item.MoneyTotal);
                        } else {
                            $scope.totalPaymentSlip = parseFloat($scope.totalPaymentSlip) + parseFloat(item.MoneyTotal);
                        }
                    });
                });
            }
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("_STT").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.ContractHeaderID] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractHeaderID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayCode').withTitle('{{ "PROJECT_CURD_TAB_PAYMENT_LIST_COL_PAYCODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayTitle').withTitle('{{ "PROJECT_CURD_TAB_PAYMENT_LIST_COL_PAYTITLE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayType').withTitle('{{ "PROJECT_CURD_TAB_PAYMENT_LIST_COL_PAYTYPE" | translate }}').renderWith(function (data, type) {
        if (data == "True") {
            return '<span class="text-success">Phiếu thu</span>';
        } else {
            return '<span class="text-danger">Phiếu thu</span>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('{{ "PROJECT_CURD_TAB_PAYMENT_LIST_COL_TOTAL" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('Người thu').renderWith(function (data, type) {
    //    return data != "" ? $filter('currency')(data, '', 0) : null;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('Người thanh toán').renderWith(function (data, type) {
    //    return data != "" ? $filter('currency')(data, '', 0) : null;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{ "PROJECT_CURD_TAB_PAYMENT_LIST_COL_CREATETIME" | translate }}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;

    }));
    vm.reloadData = reloadData;
    vm.dtInstance = {};

    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
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
    function callback(json) {

    }
    $scope.reload = function () {
        reloadData(true);
    }
    $scope.searchPayment = function () {
        reloadData(true);
    }
    //$scope.initLoad = function () {
    //    var userModel = {};
    //    var listdata = $('#tblDataContract').DataTable().data();
    //    for (var i = 0; i < listdata.length; i++) {
    //        if (listdata[i].id == $rootScope.ContractHeaderId) {
    //            userModel = listdata[i];
    //            break;
    //        }
    //    }
    //    $scope.model.ContractCode = userModel.code;
    //}
    //$scope.initLoad();

    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabPaymentAdd.html',
            controller: 'projectTabPaymentAdd',
            backdrop: 'static',
            size: '70'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    setTimeout(function () {
        $("#datefromPayment").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datetoPayment').datepicker('setStartDate', maxDate);
        });
        $("#datetoPayment").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#datefromPayment').datepicker('setEndDate', maxDate);
        });
    }, 200);
});
app.controller('projectTabPaymentAdd', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, $uibModalInstance, $filter, dataservice) {
    var vm = $scope;
    $scope.model = {
        PayCode: '',
        PayTitle: '',
        PayType: '',
        PayObjType: '',
        PayObjId: '',
        MoneyTotal: '',
        Currency: '',
        PayNote: '',
        PayOfWay: '',
        PayerId: '',
        ReceiperId: '',
        PayNextTime: '',
    }
    $scope.model1 = {
        Paid: false,
        PaymentCompleted: false,
        Total: ''
    }
    $scope.initLoad = function () {
        dataservice.getPaymentObjType(function (rs) {
            $scope.PaymentObjType = rs;

        })
        dataservice.getPaymentType(function (rs) {
            $scope.PaymentTypeData = rs;
        })
        dataservice.getUser(function (rs) {
            $scope.PaymentUserData = rs;
        })
        dataservice.getObjPayment(function (rs) {
            $scope.PaymentObjData = rs;
        })
        dataservice.getUnit(function (rs) {
            $scope.UnitData = rs;
        })
        dataservice.checkProject($rootScope.ProjectCode, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                if (rs.Object == '0') {
                    $("#MoneyTotal").prop('disabled', true);
                    $scope.model1.PaymentCompleted = true;
                    $scope.model1.Paid = true;
                    $scope.model1.Total = '';
                    $scope.model.MoneyTotal = '';
                } else {
                    $scope.model1.Paid = false;
                    $scope.model1.Total = rs.Object;
                }
            }
        });
    }
    $scope.initLoad();

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        $scope.model.PayObjType = "PAYMENT_PROJECT";
        $scope.model.PayObjId = $rootScope.ProjectCode;
        validationSelect($scope.model);
        if ($scope.addformPayment.validate() && !validationSelect($scope.model).Status) {
            if (parseFloat($scope.model.MoneyTotal) > parseFloat($scope.model1.Total)) {
                App.toastrError("Số tiền thanh toán không được lớn hơn số tiền còn lại");
            } else {
                var msg = $rootScope.checkDatapayment($scope.model);
                if (msg.Status) {
                    App.toastrError(msg.Title);
                    return;
                }


                $rootScope.modelSub = $scope.model;

                var modalInstance = $uibModal.open({
                    templateUrl: ctxfolderMessage + '/messageConfirmAdd.html',

                    controller: function ($scope, $uibModalInstance) {
                        $scope.message = "Thêm phiếu thu-chi này?";
                        $scope.ok = function () {
                            $scope.model = $rootScope.modelSub;

                            $scope.model.PayNextTime = new Date($scope.model.PayNextTime);
                            dataservice.insertProjectPayment($scope.model, function (rs) {
                                if (rs.Error) {
                                    App.toastrError(rs.Title);
                                } else {
                                    App.toastrSuccess(rs.Title);
                                    $uibModalInstance.close();
                                }
                                App.unblockUI("#contentMain");
                            });

                        };
                        $scope.cancel = function () {
                            $uibModalInstance.dismiss('cancel');
                        };
                    },
                    size: '25',
                });
                modalInstance.result.then(function (d) {
                    $uibModalInstance.close();
                }, function () {
                });


                //$confirm({ text: 'Thêm phiếu thu-chi này?', title: 'Xác nhận', cancel: ' Hủy ' })
                //    .then(function () {
                //        App.blockUI({
                //            target: "#contentMain",
                //            boxed: true,
                //            message: 'loading...'
                //        });
                //        $scope.model.PayNextTime = new Date($scope.model.PayNextTime);
                //        dataservice.insertProjectPayment($scope.model, function (rs) {
                //            if (rs.Error) {
                //                App.toastrError(rs.Title);
                //            } else {
                //                App.toastrSuccess(rs.Title);
                //                $uibModalInstance.close();
                //            }
                //            App.unblockUI("#contentMain");
                //        });
                //    });

            }
        }
    }

    //$scope.selectObj = function (obj) {
    //    if (obj.indexOf("Project")) {
    //        dataservice.getProject(function (rs) {
    //            $scope.ObjectName = rs;
    //        });
    //    }
    //}
    //$scope.selectObjName = function (obj) {
    //    dataservice.checkProject(obj, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        } else {
    //            if (rs.Object == '0') {
    //                $("#MoneyTotal").prop('disabled', true);
    //                $scope.model1.PaymentCompleted = true;
    //                $scope.model1.Paid = true;
    //                $scope.model1.Total = '';
    //                $scope.model.MoneyTotal = '';
    //            } else {
    //                $scope.model1.Paid = false;
    //                $scope.model1.Total = rs.Object;
    //            }
    //        }
    //    });
    //}
    $scope.selectpayment = function (obj) {
        if (obj.PayType == "Phiếu chi")
            return false;
        else
            return true;
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "PayType" && $scope.model.PayType != "") {
            $scope.errorPayType = false;
        }
        if (SelectType == "PayObjType" && $scope.model.PayObjType != "") {
            $scope.errorPayObjType = false;
        }
        if (SelectType == "PayObjId" && $scope.model.PayObjId != "") {
            $scope.errorPayObjId = false;
        }
        if (SelectType == "Currency" && $scope.model.Currency != "") {
            $scope.errorCurrency = false;
        }
        if (SelectType == "PayOfWay" && $scope.model.PayOfWay != "") {
            $scope.errorPayOfWay = false;
        }
        if (SelectType == "PayerId" && $scope.model.PayerId != "") {
            $scope.errorPayerId = false;
        }
        if (SelectType == "ReceiperId" && $scope.model.ReceiperId != "") {
            $scope.errorReceiperId = false;
        }
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" }
        //Check null loại phiếu
        if (data.PayType.Name == "" || data.PayType == null) {
            $scope.errorPayType = true;
            mess.Status = true;
        } else {
            $scope.errorPayType = false;
        }
        //Check null loại đối tượng
        if (data.PayObjType == "") {
            $scope.errorPayObjType = true;
            mess.Status = true;
        } else {
            $scope.errorPayObjType = false;
        }

        //Check null Tên đối tượng
        if (data.PayObjId == "") {
            $scope.errorPayObjId = true;
            mess.Status = true;
        } else {
            $scope.errorPayObjId = false;
        }

        //Check null Tên đối tượng
        if (data.Currency == "") {
            $scope.errorCurrency = true;
            mess.Status = true;
        } else {
            $scope.errorCurrency = false;
        }

        //Check null Tên đối tượng
        if (data.PayOfWay == "") {
            $scope.errorPayOfWay = true;
            mess.Status = true;
        } else {
            $scope.errorPayOfWay = false;
        }

        //Check null Tên đối tượng
        if (data.PayerId == "") {
            $scope.errorPayerId = true;
            mess.Status = true;
        } else {
            $scope.errorPayerId = false;
        }

        //Check null Tên đối tượng
        if (data.ReceiperId == "") {
            $scope.errorReceiperId = true;
            mess.Status = true;
        } else {
            $scope.errorReceiperId = false;
        }

        //Check null loại tiền
        return mess;
    };
    setTimeout(function () {
        $("#paymentNext").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        });

    }, 200);
});


app.controller('projectTabNote', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.model = {
        ProjectId: '',
        Title: '',
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableProjectNote",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
                d.Title = $scope.model.Title;
                d.Tags = $scope.model.Tags;
            },
            complete: function () {
                App.unblockUI("#contentMain");
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
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('_STT').withTitle('{{ "PROJECT_CURD_TAB_NOTE_LIST_COL_ADDRESS" | translate }}').renderWith(function (data, type) {
        return '<span  class="btn btn-success" style="height: 20px; font-size: 5; padding: 0">Tags</button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{ "PROJECT_CURD_TAB_NOTE_LIST_COL_TITLE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Name').withTitle('{{ "PROJECT_CURD_TAB_NOTE_LIST_COL_TAGS" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Note').withTitle('{{ "PROJECT_CURD_TAB_NOTE_LIST_COL_NOTE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{ "PROJECT_CURD_TAB_NOTE_LIST_COL_CREATETIME" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('{{ "COM_LIST_COL_ACTION" | translate }}').renderWith(function (data, type, full) {
        return '<button title="{{&quot;COM_BTN_EDIT&quot; | translate}}" ng-click="edit(' + full.Id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="{{&quot;COM_BTN_DELETE&quot; | translate}}" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }));
    vm.reloadData = reloadData;
    vm.dtInstance = {};
    function reloadData(resetPaging) {
        vm.dtInstance.reloadData(callback, resetPaging);
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
    function callback(json) {

    }
    $scope.search = function () {
        reloadData(true);
    }
    $scope.reload = function () {
        reloadData(true);
    }
    $scope.initLoad = function () {

        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initLoad();

    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabNoteAdd.html',
            controller: 'projectTabNoteAdd',
            backdrop: true,
            size: '50'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }
    $scope.edit = function (id) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/projectTabNoteEdit.html',
            controller: 'projectTabNoteEdit',
            backdrop: true,
            size: '50',
            resolve: {
                para: function () {
                    return id;
                }
            }
        });
        modalInstance.result.then(function (d) {
            reloadData();
        }, function () { });
    };
    $scope.delete = function (id) {

        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = "Bạn có chắc chắn muốn xóa ghi chú?";
                $scope.ok = function () {
                    dataservice.deleteprojectTabNote(id, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close();
                        }
                        App.unblockUI("#contentMain");
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



        //$confirm({ text: 'Bạn có chắc chắn muốn xóa ghi chú?', title: 'Xác nhận', cancel: ' Hủy ' })
        //    .then(function () {
        //        App.blockUI({
        //            target: "#contentMain",
        //            boxed: true,
        //            message: 'loading...'
        //        });
        //        dataservice.deleteprojectTabNote(id, function (result) {
        //            if (result.Error) {
        //                App.toastrError(result.Title);
        //            } else {
        //                App.toastrSuccess(result.Title);
        //                $scope.reload();
        //            }
        //            App.unblockUI("#contentMain");
        //        });
        //    });
    }
});
app.controller('projectTabNoteEdit', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter, para) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        Tags: '',
        Title: '',
        Note: '',
    };
    $scope.initData = function () {
        dataservice.getNote(para, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            } else {
                $scope.model = rs;
                $scope.model.MemberCode = $scope.model.Tags;
            }
        });
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initData();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };


        if (data.MemberCode == "" || data.MemberCode == null) {
            $scope.errorMemberCode = true;
            mess.Status = true;
        } else {
            $scope.errorMemberCode = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "MemberCode" && $scope.model.MemberCode != "") {
            $scope.errorMemberCode = false;
        }
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            $scope.model.ProjectCode = $rootScope.ProjectCode;
            dataservice.updateProjectTabNote($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
                App.unblockUI("#contentMain");
            });
        };
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('projectTabNoteAdd', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.model = {
        Position: '',
        ProjectCode: '',

    };
    $scope.initLoad = function () {

        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initLoad();
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };


        if (data.MemberCode == "" || data.MemberCode == null) {
            $scope.errorMemberCode = true;
            mess.Status = true;
        } else {
            $scope.errorMemberCode = false;

        }
        return mess;
    };

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "MemberCode" && $scope.model.MemberCode != "") {
            $scope.errorMemberCode = false;
        }
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            $scope.model.ProjectCode = $rootScope.ProjectCode;
            dataservice.insertProjectTabNote($scope.model, function (result) {
                if (result.Error) {
                    App.toastrError(result.Title);
                } else {
                    App.toastrSuccess(result.Title);
                    $uibModalInstance.close();
                }
                App.unblockUI("#contentMain");
            });
        };
    };

    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});


app.controller('card-job', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter, $confirm) {
    var vm = $scope;
    $scope.model = {
        ProjectId: '',
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;

    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Project/JTableCardJob",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contact-main",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ProjectId = $rootScope.ProjectId;
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
    vm.dtColumns.push(DTColumnBuilder.newColumn('Project').withTitle('{{ "PROJECT_CURD_TAB_CARDJOB_LIST_COL_PROJECTCODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardCode').withTitle('{{ "PROJECT_CURD_TAB_CARDJOB_LIST_COL_CARDCODE" | translate }}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardName').withTitle('{{ "PROJECT_CURD_TAB_CARDJOB_LIST_COL_CARDNAME" | translate }}').renderWith(function (data, type) {
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
            templateUrl: ctxfolder + '/contact_add.html',
            controller: 'contact_add',
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
            templateUrl: ctxfolder + '/contact_edit.html',
            controller: 'contact_edit',
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
        $confirm({ text: 'Bạn có chắc chắn xóa?', title: 'Xác nhận', cancel: ' Hủy ' })
            .then(function () {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
                dataservice.deleteContact(id, function (result) {
                    if (result.Error) {
                        App.toastrError(result.Title);
                    } else {
                        App.toastrSuccess(result.Title);
                        $scope.reload();
                    }
                    App.unblockUI("#contentMain");
                });
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
            ObjCode: $rootScope.ProjectId,
            CatObjCode: "PROJECT",
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

    $scope.initData = async function () {
        if (para === "") {
            console.log("Another");
            $scope.ParamNull = true;
        }
        else {
            $scope.ParamNull = false;
        }
        await dataservice.getLevels(function (rs) {
            $scope.CardLevels = rs;
        });
        await dataservice.getWorkType(function (rs) {
            $scope.WorkTypes = rs;
        });
        await dataservice.getCardDetail(para, function (rs) {
            console.log(rs);
            $scope.model = rs;
        });
        await dataservice.getAttachment(para, function (rs) {
            $scope.attachments = rs;
        });
        await dataservice.getCheckList(para, function (rs) {
            console.log(rs);
            $scope.checklists = rs;
            angular.forEach(rs, function (value, key) {
                dataservice.getCheckItem(value.ChkListCode, function (result) {
                    $scope.checkList[value.ChkListCode] = result;
                })
            })
        });
        await dataservice.getComment(para, function (rs) {
            $scope.comments = rs;
        });
        await dataservice.getCardDependency(para, function (rs) {
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
        await dataservice.getStatus(function (rs) {
            $scope.CardStatus = rs;
        });
        await dataservice.getBoards(function (rs) {
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