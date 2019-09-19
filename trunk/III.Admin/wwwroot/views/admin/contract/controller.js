var ctxfolder = "/views/admin/contract";
var ctxfolderMessage = "/views/message-box";

var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', "ngJsTree", "treeGrid", "ui.select", "ngCookies", "pascalprecht.translate", 'dynamicNumber']);
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
        //listCommon
        getListCommon: function (callback) {
            $http.post('/Admin/Contract/GetListCommon').success(callback);
        },
        insert: function (data, callback) {
            $http.post('/Admin/Contract/InsertContract/', data).success(callback);
        },
        update: function (data, callback) {
            $http.post('/Admin/Contract/UpdateContract/', data).success(callback);
        },
        delete: function (data, callback) {
            $http.post('/Admin/Contract/DeleteContract/' + data).success(callback);
        },
        getSigner: function (callback) {
            $http.post('/Admin/Contract/JtreeSigner').success(callback);
        },
        getItemAdd: function (data, callback) {
            $http.get('/Admin/Contract/GetItemAdd?code=' + data).success(callback);
        },
        getItem: function (data, callback) {
            $http.get('/Admin/Contract/GetItem?contractCode=' + data, {
                beforeSend: function () {
                    App.blockUI({
                        target: "#contentMain",
                        boxed: true,
                        message: 'loading...'
                    });
                },
                complete: function () {
                    App.unblockUI("#contentMain");
                }
            }).success(callback);
        },
        getUser: function (callback) {
            $http.post('/Admin/Contract/GetUser').success(callback);
        },



        getTask: function (callback) {
            $http.post('/Admin/Contract/GetTask').success(callback);
        },
        insertTagPeople: function (data, callback) {
            $http.post('/Admin/Contract/InsertTagPeople', data).success(callback);
        },
        deleteTagPeople: function (data, callback) {
            $http.post('/Admin/Contract/DeleteTagPeople', data).success(callback);
        },

        getContractNote: function (data, callback) {
            $http.post('/Admin/Contract/GetContractNote', data).success(callback);
        },
        insertContractNote: function (data, callback) {
            $http.post('/Admin/Contract/InsertContractNote', data).success(callback);
        },
        updateContractNote: function (data, callback) {
            $http.post('/Admin/Contract/UpdateContractNote', data).success(callback);
        },
        deleteContractNote: function (data, callback) {
            $http.post('/Admin/Contract/DeleteContractNote', data).success(callback);
        },
        getUserlogin: function (callback) {
            $http.post('/Admin/Contract/GetUserlogin').success(callback);
        },


        deleteNotification: function (data, callback) {
            $http.post('/Admin/Contract/DeleteNotification', data).success(callback);
        },

        getContractDetail: function (data, callback) {
            $http.post('/Admin/Contract/GetContractDetail/' + data).success(callback);
        },
        insertContractDetail: function (data, callback) {
            $http.post('/Admin/Contract/InsertContractDetail/', data).success(callback);
        },
        updateContractDetail: function (data, callback) {
            $http.post('/Admin/Contract/UpdateContractDetail/', data).success(callback);
        },
        deleteContractDetail: function (data, callback) {
            $http.post('/Admin/Contract/DeleteContractDetail/' + data).success(callback);
        },

        insertContractFile: function (data, callback) {
            $http.post('/Admin/Contract/InsertContractFile/', data).success(callback);
        },
        getContractFile: function (data, callback) {
            $http.post('/Admin/Contract/GetContractFile/' + data).success(callback);
        },
        updateContractFile: function (data, callback) {
            $http.post('/Admin/Contract/UpdateContractFile/', data).success(callback);
        },
        deleteContractFile: function (data, callback) {
            $http.post('/Admin/Contract/DeleteContractFile/' + data).success(callback);
        },
        uploadFile: function (data, callback) {
            submitFormUpload('/Admin/Contract/UploadFile/', data, callback);
        },


        getContractAttr: function (data, callback) {
            $http.post('/Admin/Contract/GetContractAttr/' + data).success(callback);
        },
        insertContractAttr: function (data, callback) {
            $http.post('/Admin/Contract/InsertContractAttr/', data).success(callback);
        },
        updateContractAttr: function (data, callback) {
            $http.post('/Admin/Contract/UpdateContractAttr/', data).success(callback);
        },
        deleteContractAttr: function (data, callback) {
            $http.post('/Admin/Contract/DeleteContractAttr/' + data).success(callback);
        },


        getRepository: function (callback) {
            $http.post('/Admin/EDMSRepository/JtreeRepository').success(callback);
        },
        getCustomers: function (callback) {
            $http.post('/Admin/Contract/GetCustomers/').success(callback);
        },

        getCurrency: function (callback) {
            $http.post('/Admin/Contract/GetCurrency').success(callback);
        },
        getContractType: function (callback) {
            $http.post('/Admin/Contract/GetContractType').success(callback);
        },
        getMainService: function (callback) {
            $http.post('/Admin/Contract/GetMainService').success(callback);
        },
        getContractExtendGroup: function (callback) {
            $http.post('/Admin/Contract/GetContractExtendGroup').success(callback);
        },
        getContractStatus: function (callback) {
            $http.post('/Admin/Contract/GetContractStatus/').success(callback);
        },

        getContract: function (callback) {
            $http.post('/Admin/MaterialPaymentTicket/GetContract').success(callback);
        },
        checkContract: function (data, callback) {
            $http.get('/Admin/MaterialPaymentTicket/CheckContract?contractCode=' + data).success(callback);
        },
        getPaymentObjType: function (callback) {
            $http.post('/Admin/MaterialPaymentTicket/GetPaymentObjType').success(callback);
        },
        getUnit: function (callback) {
            $http.post('/Admin/MaterialPaymentTicket/GetUnit').success(callback);
        },
        getPaymentType: function (callback) {
            $http.post('/Admin/Contract/GetPaymentType').success(callback);
        },
        getObjPayment: function (callback) {
            $http.post('/Admin/Contract/GetObjPayment').success(callback);
        },
        insertContractPayment: function (data, callback) {
            $http.post('/Admin/MaterialPaymentTicket/Insert', data).success(callback);
        },


        //CardJob
        getBoards: function (callback) {
            $http.post('/Admin/CardJob/GetBoards/').success(callback);
        },

        addCardJob: function (data, callback) {
            $http.post('/Admin/Contract/AddCardJob/', data).success(callback);
        },

        getTeams: function (callback) {
            $http.post('/Admin/CardJob/GetTeams').success(callback);
        },
        getBoardByTeam: function (TeamCode, callback) {
            $http.post('/Admin/Contract/GetBoards/?TeamCode=' + TeamCode).success(callback);
        },
        getLists: function (BoardCode, callback) {
            $http.post('/Admin/Contract/GetLists/?BoardCode=' + BoardCode).success(callback);
        },
        getCards: function (ListCode, callback) {
            $http.post('/Admin/Contract/GetCards/?ListCode=' + ListCode).success(callback);
        },
        addCardRelative: function (data, callback) {
            $http.post('/Admin/Contract/AddCardRelative/', data).success(callback);
        },

        //getCards: function (data, callback) {
        //    $http.post('/CardJob/GetCards/?BoardCode=' + data).success(callback);
        //},
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


        add: function (data, callback) {
            $http.post('/Admin/UIYKienBanDoc/Insert/', data).success(callback);
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
        }
    };
});
app.controller('Ctrl_ESEIM', function ($scope, $rootScope, $cookies, $translate, dataservice , $filter) {
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
        $rootScope.checkData = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;
            var partternTelephone = /[0-9]/g;
            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.ContractCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace("{0}", caption.CONTRACT_CURD_LBL_CONTRACT_CODE), "<br/>");//"Mã hợp đồng không chứa ký tự đặc biệt hoặc khoảng trắng!"
            }
            if (!partternTelephone.test(data.Version) && data.Version != null) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.CONTRACT_VALIDATE_MUST_BE_NUMBER.replace("{0}", caption.CONTRACT_CURD_LBL_VERSION), "<br/>");//"Phiên bản phải là chữ số!"
            }
            return mess;
        }
        $rootScope.checkDatamore = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;

            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.AttrCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace("{0}", caption.CONTRACT_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_CODE), "<br/>");// "Mã thuộc tính không chứa ký tự đặc biệt hoặc khoảng trắng!"
            }

            return mess;
        }
        $rootScope.checkDatapayment = function (data) {
            var partternCode = /^[a-zA-Z0-9]+[^!@#$%^&*<>?\s]*$/g;

            var mess = { Status: false, Title: "" }
            if (!partternCode.test(data.PayCode)) {
                mess.Status = true;
                mess.Title = mess.Title.concat(" - ", caption.COM_VALIDATE_ITEM_CODE.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_LBL_PAY_CODE), "<br/>");//"Mã phiếu thu-chi không chứa ký tự đặc biệt hoặc khoảng trắng!"
            }

            return mess;
        }
        $rootScope.validationOptions = {
            rules: {
                ContractCode: {
                    required: true,
                    maxlength: 100,
                },
                Title: {
                    required: true,
                    maxlength: 255,
                },
                //CustomerCode: {
                //    required: true,

                //}
            },
            messages: {
                ContractCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_LBL_CONTRACT_CODE),//"Nhập mã hợp đồng",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_LBL_CONTRACT_CODE).replace("{1}", "100"),//"Mã hợp đồng không vượt quá 100 kí tự!",
                },
                Title: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_LBL_CONTRACT_NAME),//"Nhập tên hợp đồng",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_LBL_CONTRACT_CODE).replace("{1}", "255"),//"Tên hợp đồng không vượt quá 255 kí tự!",
                },
                //CustomerCode: {
                //    required: "Chọn khách hàng"
                //}
            }
        }

        $rootScope.validationOptionsDetail = {
            rules: {
                ItemCode: {
                    required: true,
                    maxlength: 100,
                },
                ItemName: {
                    required: true,
                    maxlength: 255,
                },
                Quatity: {
                    required: true,
                    maxlength: 18,
                },
                Cost: {
                    required: true,
                    maxlength: 18,
                }
            },
            messages: {
                ItemCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_ITEM_CODE),//"Mã chi tiết yêu cầu bắt buộc!",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_ITEM_CODE).replace("{1}", "100")//"Không vượt quá 100 kí tự"
                },
                ItemName: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_ITEM_NAME),//"Tên chi tiết yêu cầu bắt buộc!",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_ITEM_NAME).replace("{1}", "255")//"Không vượt quá 255 kí tự"
                },
                Quatity: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_QUATITY),//"Số lượng yêu cầu bắt buộc!",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_QUATITY).replace("{1}", "255")//"Số lượng không vượt quá 255 kí tự"
                },
                Cost: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_COST),//"Đơn giá yêu cầu bắt buộc!",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_DETAIL_CURD_LBL_COST).replace("{1}", "18")//"Đơn giák hông vượt quá 18 kí tự"
                },
            }
        }
        $rootScope.validationOptionsNote = {
            rules: {
                Title: {
                    required: true,
                },
                Tags: {
                    required: true,
                }
            },
            messages: {
                Title: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}",caption.CONTRACT_CURD_TAB_NOTE_CURD_TXT_TITLE),//"Tiêu đề yêu cầu bắt buộc",
                },
                Tags: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_NOTE_CURD_LBL_TAG),//"Tags yêu cầu bắt buộc",
                }
            }
        }
        $rootScope.validationOptionsAttr = {
            rules: {
                AttrCode: {
                    required: true,
                    maxlength: 255,
                },
                AttrValue: {
                    required: true,
                    maxlength: 255,
                }
            },
            messages: {
                AttrCode: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_CODE),//"Bắt buộc",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_CODE).replace("{1}", "255")//"Không vượt quá 255 kí tự"
                },
                AttrValue: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_VALUE),//"Bắt buộc",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_ATTRIBUTE_CURD_LBL_ATTR_VALUE).replace("{1}", "255")//"Không vượt quá 255 kí tự"
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
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_LBL_PAY_CODE),//"Mã phiếu yêu cầu bắt buộc",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_LBL_PAY_CODE).replace("{1}", "100")//"Không vượt quá 100 kí tự"
                },
                PayTitle: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_LBL_PAY_TITLE),//"Tên phiếu yêu cầu bắt buộc",
                    maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_LBL_PAY_TITLE).replace("{1}", "100")
                },
                MoneyTotal: {
                    required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_PAYMENT_CURD_TXT_MONEY_TOTAL)//"Tổng tiền yêu cầu bắt buộc"
                }
            }
        }
    });

    
    //$rootScope.Signers = [];
    //dataservice.getSigner(function (result) {
    //    if (!result.Error) {
    //        for (var i = 0; i < result.length; i++) {
    //            $rootScope.Signers.push(result[i]);
    //        }
    //    }
    //});

    //$rootScope.ContractType = [];
    //dataservice.getContractType(function (rs) {
    //    angular.forEach(rs, function (value, key) {
    //        $rootScope.ContractType.push(value.ValueSet);
    //    })
    //});

    //$rootScope.ContractStatus = [];
    //dataservice.getContractStatus(function (rs) {
    //    angular.forEach(rs, function (value, key) {
    //        $rootScope.ContractStatus.push(value.ValueSet);
    //    })
    //})

    //$rootScope.ContractService = [];
    //dataservice.getMainService(function (rs) {
    //    angular.forEach(rs, function (value, key) {
    //        $rootScope.ContractService.push(value.ValueSet);
    //    })
    //})

    //$rootScope.ContractUnit = [];
    //dataservice.getCurrency(function (rs) {
    //    $rootScope.ContractUnit = rs;
    //});

    //$rootScope.Group = [];
    //dataservice.getContractExtendGroup(function (rs) {
    //    $rootScope.Group = rs;
    //});

    dataservice.getCustomers(function (rs) {
        $rootScope.Customers = rs;
    })
    dataservice.getListCommon(function (rs) {
        $rootScope.ListCommon = rs;
    });
    $rootScope.PaymentType = [{
        Value: 0,
        Name: "Phiếu chi"
    }, {
        Value: 1,
        Name: "Phiếu thu"
    }]
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
});
app.config(function ($routeProvider, $validatorProvider, $translateProvider, $httpProvider) {
    $translateProvider.useUrlLoader('/Admin/Language/Translation');
    caption = $translateProvider.translations();

    $routeProvider
        .when('/', {
            templateUrl: ctxfolder + '/index.html',
            controller: 'index'
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
    $httpProvider.interceptors.push('interceptors');
});

app.controller('index', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        Key: '',
        FromDate: '',
        ToDate: '',
        ContractCode: '',
        Status: '',
        BudgetF: '',
        BudgetT: '',
        Signer: '',
        Currency: ''
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTable",
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
                d.ContractCode = $scope.model.ContractCode;
                d.Status = $scope.model.Status;
                d.BudgetF = $scope.model.BudgetF;
                d.BudgetT = $scope.model.BudgetT;
                d.Signer = $scope.model.Signer;
                d.Currency = $scope.model.Currency;
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
            $compile(angular.element(row))($scope);
            $compile(angular.element(row).attr('context-menu', 'contextMenu'))(contextScope);
        });
    vm.dtColumns = [];
    //vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().withOption('sWidth', '20px').renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.id] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{"CTR_CTR_MAN_CODE" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
    //    return data;
    //}));

    //vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('{{"CTR_CTR_MAN_NAME" | translate}}').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('duration').withTitle('{{"CTR_CTR_MAN_DURATION" | translate}}').renderWith(function (data, type) {
    //    return data;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('budget').withTitle('{{"CTR_CTR_MAN_BUDGET" | translate}}').renderWith(function (data, type) {
    //    return data != "" ? $filter('currency')(data, '', 0) : null;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('currency').withTitle('{{"CTR_CTR_MAN_CURRENCY" | translate}}').renderWith(function (data, type) {
    //    for (var i = 0; i < $rootScope.ListCommon.length; i++) {
    //        if ($rootScope.ListCommon[i].Code == data) {
    //            return $rootScope.ListCommon[i].Name;
    //            break;
    //        }
    //    }
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('tags').withTitle('{{"CTR_CTR_MAN_TAGS" | translate}}').withOption('sWidth', '20px').notSortable().renderWith(function (data, type, full) {
    //    return '<a ng-click="tag(' + full.id + ')" style="padding: 0px"><i class="fa fa-tags pt5" style="font-size: 20px;color:#5cb85c"></i></a>';
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('{{"CTR_CTR_MAN_NOTE" | translate}}').withOption('sClass', 'text-center').notSortable().renderWith(function (data, type, full) {
    //    return '<a ng-click="note(' + full.id + ')" style="padding: 0px"><i class="fa fa-file-text-o pt5" style="font-size: 20px ;color:#337ab7"></i></a>';
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('status').withTitle('{{"COM_STATUS" | translate}}Trạng thái').renderWith(function (data, type, full) {
    //    if (data == "CONTRACT_DEAVTIVE") {
    //        return '<span class="text-danger">{{"MSG_CONTRACT_DEAVTIVE" | translate}}</span>';
    //    } else if (data == "CONTRACT_ACTIVE") {
    //        return '<span class="text-success">{{"MSG_CONTRACT_ACTIVE" | translate}}</span>';
    //    }
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"COM_ACTION" | translate}}').renderWith(function (data, type, full, meta) {
    //    return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
    //        '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    //}));

        //vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().withOption('sWidth', '20px').renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.id] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{"CTR_CTR_MAN_CODE" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
    //    return data;
    //}));

     vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().withOption('sWidth', '20px').renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{"CONTRACT_LIST_COL_CODE" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('{{"CONTRACT_LIST_COL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('duration').withTitle('{{"CONTRACT_LIST_COL_DURATION" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('budget').withTitle('{{"CONTRACT_LIST_COL_BUDGET" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('currency').withTitle('{{"CONTRACT_LIST_COL_CURENCY" | translate}}').renderWith(function (data, type) {
        for (var i = 0; i < $rootScope.ListCommon.length; i++) {
            if ($rootScope.ListCommon[i].Code == data) {
                return $rootScope.ListCommon[i].Name;
                break;
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('tags').withTitle('{{"CONTRACT_LIST_COL_TAGS" | translate}}').withOption('sWidth', '20px').notSortable().renderWith(function (data, type, full) {
        return '<a ng-click="tag(' + full.id + ')" style="padding: 0px"><i class="fa fa-tags pt5" style="font-size: 20px;color:#5cb85c"></i></a>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('{{"CONTRACT_LIST_COL_NOTE" | translate}}').withOption('sClass', 'text-center').notSortable().renderWith(function (data, type, full) {
        return '<a ng-click="note(' + full.id + ')" style="padding: 0px"><i class="fa fa-file-text-o pt5" style="font-size: 20px ;color:#337ab7"></i></a>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('status').withTitle('{{"CONTRACT_LIST_COL_STATUS" | translate}}').renderWith(function (data, type, full) {
        if (data == "CONTRACT_DEAVTIVE") {
            return '<span class="text-danger">Không hoạt động</span>';
        } else if (data == "CONTRACT_ACTIVE") {
            return '<span class="text-success">Hoạt động</span>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"CONTRACT_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full, meta) {
        return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(85,168,253,0.45);" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px;-webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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


    $scope.reload = function () {
        reloadData(true);
    };
    $scope.reloadNoResetPage = function () {
        reloadData(false);
    };
    $scope.search = function () {
        reloadData(true);
    };
    $scope.tag = function (id) {
        var userModel = {};
        var listdata = $('#tblDataContract').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i].id == id) {
                userModel = listdata[i];
                break;
            }
        }
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contractPeopleTags.html',
            controller: 'contractPeopleTags',
            backdrop: true,
            size: '53',
            resolve: {
                para: function () {
                    return userModel.code;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    };
    $scope.note = function (id) {
        var userModel = {};
        var listdata = $('#tblDataContract').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i].id == id) {
                userModel = listdata[i];
                break;
            }
        }

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contractNote.html',
            controller: 'contractNote',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return userModel.code;
                }
            }
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    };
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.delete(id, function (result) {
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
            $scope.reloadNoResetPage();
        }, function () {
        });
    };
    $scope.edit = function (id) {
        var userModel = {};
        var listdata = $('#tblDataContract').DataTable().data();
        for (var i = 0; i < listdata.length; i++) {
            if (listdata[i].id == id) {
                userModel = listdata[i];
                break;
            }
        }
        dataservice.getItem(userModel.code, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                $rootScope.ContractCode = userModel.code;
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: ctxfolder + '/edit.html',
                    controller: 'edit',
                    backdrop: true,
                    size: '80',
                    resolve: {
                        para: function () {
                            return rs.Object;
                        }
                    }
                });
                modalInstance.result.then(function (d) {
                    $scope.reloadNoResetPage();
                }, function () { });
            }
        })
    };
    $scope.add = function () {
        $rootScope.ContractCode = '';
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/add.html',
            controller: 'add',
            backdrop: true,
            size: '80'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () { });
    };
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
    }, 50);
});

app.controller('add', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    $scope.model = {
        ContractCode: '',
        ContractType: '',
        Status: '',
        MainService: '',
        Currency: '',
        CusCode: '',
    }

    $scope.cancel = function () {
        $uibModalInstance.close();
    };

    $scope.chkContract = function () {
        if ($rootScope.ContractCode == '') {
            App.toastrError("Vui lòng tạo trước hợp đồng!");
        }
    }

    $scope.changleSelect = function (SelectType) {
        if (SelectType == "ContractType" && $scope.model.ContractType != "") {
            $scope.errorContractType = false;
        }
        if (SelectType == "Status" && $scope.model.Status != "") {
            $scope.errorStatus = false;
        }
        if (SelectType == "MainService" && $scope.model.MainService != "") {
            $scope.errorMainService = false;
        }
        if (SelectType == "Currency" && $scope.model.Currency != "") {
            $scope.errorCurrency = false;
        }
        if (SelectType == "CusCode" && $scope.model.CusCode != "") {
            $scope.errorCusCode = false;
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
            var modalInstance = $uibModal.open({
                templateUrl: ctxfolderMessage + '/messageConfirmAdd.html',
                resolve: {
                    para: function () {
                        return $scope.model;
                    }
                },
                controller: function ($scope, $uibModalInstance, para) {
                    $scope.message = caption.CONTRACT_VALIDATE_ADD_CONTRACT_CONFIRM;//"Bạn muốn thêm hợp đồng này ?";
                    $scope.ok = function () {
                        dataservice.insert(para, function (result) {
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
                $rootScope.ContractCode = $scope.model.ContractCode;
            }, function () {
            });
        }
    }

    $rootScope.amountbudget = function (amount) {
        $scope.model.Budget = amount;
    }

    function validationSelect(data) {
        var mess = { Status: false, Title: "" };

        if (data.ContractType == "") {
            $scope.errorContractType = true;
            mess.Status = true;
        } else {
            $scope.errorContractType = false;

        }
        if (data.Status == "") {
            $scope.errorStatus = true;
            mess.Status = true;
        } else {
            $scope.errorStatus = false;

        }
        if (data.MainService == "") {
            $scope.errorMainService = true;
            mess.Status = true;
        } else {
            $scope.errorMainService = false;

        }
        if (data.Currency == "") {
            $scope.errorCurrency = true;
            mess.Status = true;
        } else {
            $scope.errorCurrency = false;

        }
        if (data.CusCode == "") {
            $scope.errorCusCode = true;
            mess.Status = true;
        } else {
            $scope.errorCusCode = false;

        }
        return mess;
    };

    setTimeout(function () {
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        });
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('edit', function ($scope, $filter, $rootScope, $compile, $uibModal, $uibModalInstance, dataservice, para) {
    $scope.initData = function () {
        $scope.model = para;
        $scope.model.ContractDate = convertDate($scope.model.ContractDate);
        $scope.model.CreatedTime = convertDate($scope.model.CreatedTime);
        $scope.model.DeletedTime = convertDate($scope.model.DeletedTime);

    }
    $scope.initData();
    $scope.cancel = function () {
        $uibModalInstance.close();
    };
    $scope.submit = function () {
        validationSelect($scope.model);
        if ($scope.addform.validate() && validationSelect($scope.model).Status == false) {
            var msg = $rootScope.checkData($scope.model);
            if (msg.Status) {
                App.toastrError(msg.Title);
                return;
            }
            var modalInstance = $uibModal.open({
                templateUrl: ctxfolderMessage + '/messageConfirmUpdate.html',
                resolve: {
                    para: function () {
                        return $scope.model;
                    }
                },
                windowClass: "message-center",
                controller: function ($scope, $uibModalInstance, para) {
                    $scope.message = caption.CONTRACT_MSG_EDIT_CONFIRM.replace("{0}","");//"Bạn có chắc chắn muốn thay đổi ?";
                    $scope.ok = function () {
                        dataservice.update(para, function (result) {
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
            }, function () {
            });
        }
    }
    $scope.changleSelect = function (SelectType) {
        if (SelectType == "ContractType" && $scope.model.ContractType != "") {
            $scope.errorContractType = false;
        }
        if (SelectType == "Status" && $scope.model.Status != "") {
            $scope.errorStatus = false;
        }
        if (SelectType == "MainService" && $scope.model.MainService != "") {
            $scope.errorMainService = false;
        }
        if (SelectType == "Currency" && $scope.model.Currency != "") {
            $scope.errorCurrency = false;
        }
        if (SelectType == "CusCode" && $scope.model.CusCode != "") {
            $scope.errorCusCode = false;
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
    $rootScope.amountbudget = function (amount) {
        $scope.model.Budget = amount;
    }
    function convertDate(data) {
        var date = $filter('date')(new Date(data), 'dd/MM/yyyy');
        return date;
    }
    function validationSelect(data) {
        var mess = { Status: false, Title: "" };
        if (data.ContractType == "" || data.ContractType == null) {
            $scope.errorContractType = true;
            mess.Status = true;
        } else {
            $scope.errorContractType = false;

        }
        if (data.Status == "" || data.Status == null) {
            $scope.errorStatus = true;
            mess.Status = true;
        } else {
            $scope.errorStatus = false;

        }
        if (data.MainService == "" || data.MainService == null) {
            $scope.errorMainService = true;
            mess.Status = true;
        } else {
            $scope.errorMainService = false;

        }
        if (data.Currency == "" || data.Currency == null) {
            $scope.errorCurrency = true;
            mess.Status = true;
        } else {
            $scope.errorCurrency = false;

        }
        if (data.CusCode == "" || data.CusCode == null) {
            $scope.errorCusCode = true;
            mess.Status = true;
        } else {
            $scope.errorCusCode = false;

        }
        return mess;
    };
    function initDateTime() {
        $("#datefrom").datepicker({
            inline: false,
            autoclose: true,
            format: "dd/mm/yyyy",
            fontAwesome: true,
        });
    }
    setTimeout(function () {
        initDateTime();
        setModalMaxHeight('.modal');
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contractPeopleTags', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter, para) {
    var vm = $scope;
    //$scope.tests = true;
    $scope.model = {
        ContractPeople: '',
        Note: '',
        Task: '',

    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableTagPeople",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $scope.model.ContractCode;
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
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().withOption('sWidth', '20px').renderWith(function (data, type, full, meta) {
        $scope.selected[full.Id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.Id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Assigner').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_ASSIGNER" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('AssignerTime').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_ASSIGNER_TIME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ConfirmTime').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_CONFIRM_TIME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ValueSet').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_VALUE_SET" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Note').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_NOTE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('{{"CONTRACT_CURD_PEOPLE_TAG_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Xoá" ng-click="delete(' + full.Id + ')" style="width: 25px; height: 25px; padding: 0px; -webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.initLoad = function () {
        $scope.model.ContractCode = para;
        dataservice.getTask(function (rs) {
            $scope.listTask = rs;
        });
        dataservice.getUser(function (rs) {
            $scope.listUser = rs;
        });
    }
    $scope.initLoad();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
    $scope.submit = function () {
        if ($scope.model.ContractPeople == '' || $scope.model.ContractPeople.length == 0) {
            App.toastrError(caption.CONTRACT_CURD_PEOPLE_TAG_VALIDATE_PEOPLE);//Vui lòng chọn nhân viên được ủy quyền
        } else {
            dataservice.insertTagPeople($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                } else {
                    App.toastrSuccess(rs.Title);
                    $scope.reload();
                }
            });
        }
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.CONTRACT_MSG_DELETE_CONFIRM;//"Bạn có chắc chắn xóa người ủy quyền ?";
                windowClass: "message-center",
                    $scope.ok = function () {
                        dataservice.deleteTagPeople(id, function (result) {
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
});
app.controller('contractNote', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $uibModalInstance, para) {
    var vm = $scope;
    $scope.model = {
        Note: '',
        Tags: '',
        Title: '',
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractNote",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $scope.model.ContractCode;
            },
            complete: function () {
                App.unblockUI("#contentMain");
            }
        })
        .withPaginationType('full_numbers').withDOM("<'table-scrollable't>ip")
        .withDataProp('data').withDisplayLength(10)
        .withOption('order', [2, 'desc'])
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().withOption('sWidth', '20px').renderWith(function (data, type, full, meta) {
        $scope.selected[full.ContractNoteId] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractNoteId + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ContractCode').withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_CONTRACT_CODE" | translate}}').withOption('sWidth', '30px').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Note').withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_NOTE" | translate}}').renderWith(function (data, type, full) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Tags').withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_TAGS" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedBy').withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_CREATED_BY" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_CREATED_TIME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').notSortable().withTitle('{{"CONTRACT_CURD_NOTE_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Xoá" ng-click="delete(' + full.ContractNoteId + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    //$scope.initLoad = function () {
    //    dataservice.getUserlogin(function (rs) {
    //        $scope.UserName = rs;
    //    });
    //    $scope.model.ContractCode = para;
    //}
    //$scope.initLoad();
    //$scope.submit = function () {
    //    if ($scope.addformNote.validate()) {
    //        dataservice.insertContractNote($scope.model, function (rs) {
    //            if (rs.Error) {
    //                App.toastrError(rs.Title);
    //            } else {
    //                App.toastrSuccess(rs.Title);
    //                $scope.reload();
    //            }
    //        });
    //    }
    //}
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM//"Bạn có chắc chắn muốn xóa ?";
                windowClass: "message-center",
                    $scope.ok = function () {
                        dataservice.deleteContractNote(id, function (result) {
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
});

//tab
app.controller('contractTabDetail', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        ItemCode: '',
        ItemName: ''
    }
    //$scope.currentData = '';
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableDetail",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
                d.ItemCode = $scope.model.ItemCode;
                d.ItemName = $scope.model.ItemName;
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
    //vm.dtcolumns.push(dtcolumnbuilder.newcolumn("check").withtitle(titlehtml).notsortable().renderwith(function (data, type, full, meta) {
    //    $scope.selected[full.id] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleone(selected)"/><span></span></label>';
    //}).withoption('sclass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_ITEM_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_ITEM_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('quatity').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_QUANTITY" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('unit').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_UNIT" | translate}}').renderWith(function (data, type) {
        for (var i = 0; i < $rootScope.ListCommon.length; i++) {
            if ($rootScope.ListCommon[i].Code == data) {
                return $rootScope.ListCommon[i].Name;
                break;
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('cost').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_COST" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('note').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_NOTE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').withTitle('{{"CONTRACT_CURD_TAB_DETAIL_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
            templateUrl: ctxfolder + '/contractTabDetailAdd.html',
            controller: 'contractTabDetailAdd',
            backdrop: 'static',
            size: '35'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
            $rootScope.amountbudget(d);
        }, function () {
        });
    }

    $scope.edit = function (id) {
        dataservice.getContractDetail(id, function (rs) {
            if (rs.Error) {
                App.toastrError(ts.Title);
            }
            else {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: ctxfolder + '/contractTabDetailEdit.html',
                    controller: 'contractTabDetailEdit',
                    backdrop: 'static',
                    size: '35',
                    resolve: {
                        para: function () {
                            return rs.Object;
                        }
                    }
                });
                modalInstance.result.then(function (d) {
                    $scope.reload();
                    $rootScope.amountbudget(d);
                }, function () {
                });
            }
        })
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;//"Bạn có chắc chắn muốn xóa?";
                $scope.ok = function () {
                    dataservice.deleteContractDetail(id, function (result) {
                        if (result.Error) {
                            App.toastrError(result.Title);
                        } else {
                            App.toastrSuccess(result.Title);
                            $uibModalInstance.close(result.Object);
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
            $rootScope.amountbudget(d);
        }, function () {
        });
    }
});
app.controller('contractTabDetailAdd', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice) {
    $scope.model = {
        ContractCode: '',
        ItemCode: '',
        ItemName: '',
        Quatity: '',
        Cost: '',
        Note: ''
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.addformDetail.validate()) {
            $scope.model.ContractCode = $rootScope.ContractCode;
            dataservice.insertContractDetail($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(rs.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close(rs.Object);
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contractTabDetailEdit', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice, para) {
    $scope.init = function () {
        $scope.model = para;
    }
    $scope.init();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.editformDetail.validate()) {
            $scope.model.ContractCode = $rootScope.ContractCode;
            dataservice.updateContractDetail($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(ts.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close(rs.Object);
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});


app.controller('contractTabFile', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        Category: '',
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableFile",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
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
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractFileID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('name').withTitle('{{"CONTRACT_CURD_TAB_FILE_LIST_COL_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('createdTime').withTitle('{{"CONTRACT_CURD_TAB_FILE_LIST_COL_CREATED_TIME" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('categoryName').withTitle('{{"CONTRACT_CURD_TAB_FILE_LIST_COL_CATEGORY_NAME" | translate}}').renderWith(function (data, type, full) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('tags').withTitle('Tags').renderWith(function (data, type) {
    //    return data;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('description').withTitle('{{"CONTRACT_CURD_TAB_FILE_LIST_COL_DESCRIPTION" | translate}}').notSortable().renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').withTitle('{{"CONTRACT_CURD_TAB_FILE_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<a href="' + full.fileUrl + '" target="_blank" style="width: 25px; height: 25px; padding: 0px" title="Tải xuống - ' + full.name + '" class="btn btn-icon-only btn-circle btn-outline green " download><i class="fa fa-download pt5"></i></a>' +
            '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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

        //if (SelectType == "FileName" && ( $scope.searchform.validate())) {
        //    $scope.errorFileName = false;
        //}
    }

    $scope.add = function () {
        validationSelect($scope.model);
        if (validationSelect($scope.model).Status == false) {
            $confirm({
                text: caption.COM_ADD_FILE, title: caption.COM_BTN_CONFIRM, cancel: caption.COM_BTN_CANCEL })
                .then(function () {

                    var file = $('#FileInput').get(0).files[0];
                    var data = new FormData();
                    data.append("FileUpload", file);
                    dataservice.uploadFile(data, function (rs) {
                        if (rs.Error) {
                            App.toastrError(rs.Title);
                        }
                        else {
                            $scope.model.ContractCode = $rootScope.ContractCode;
                            $scope.model.FileName = $('.inputFile').val().split('\\').pop();
                            $scope.model.FileUrl = '/uploads/files/' + rs.Object;
                            $scope.model.Category = $scope.model.Category.Value;
                            dataservice.insertContractFile($scope.model, function (result) {
                                if (result.Error) {
                                    App.toastrError(result.Title);
                                } else {
                                    App.toastrSuccess(result.Title);
                                    $scope.model.FileName = "";
                                    $scope.model.Category = "";
                                    $scope.model.Tags = "";
                                    $scope.model.Description = "";
                                    $scope.reload();
                                }
                            });
                        }
                    })
                });

        }
    }

    $scope.edit = function (id) {
        dataservice.getContractFile(id, function (rs) {
            if (rs.Error) {
                App.toastrError(ts.Title);
            }
            else {
                //$scope.currentData = $scope.model;
                $scope.model = rs;
                angular.forEach($rootScope.Categories, function (value, key) {
                    if (value.Value == rs.Category) {
                        $scope.model.Category = value;
                        return;
                    }
                });
                $("#btnAddFile").addClass("hidden");
                $("#btnEditFile").removeClass("hidden");
            }
        })
    }

    $scope.save = function () {
        $confirm({ text: caption.CONTRACT_CURD_TAB_FILE_MSG_EDIT_FILE, title:caption.COM_BTN_CONFIRM, cancel: caption.COM_BTN_CANCEL })
            .then(function () {
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
                            //$scope.model = $scope.currentData;
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
                                    //$scope.model = $scope.currentData;
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
            });
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;
                $scope.ok = function () {
                    dataservice.deleteContractFile(id, function (result) {
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
        App.inputFile();
    }, 200);
});

app.controller('contractTabNote', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.model = {
        Title: '',
        Note: ''
    }
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractNote",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
                d.Title = $scope.model.Title;
                d.Note = $scope.model.Note;
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.ContractNoteId] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractNoteId + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('_STT').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_STT" | translate}}').renderWith(function (data, type) {
        return '<span  class="btn btn-success" style="height: 20px; font-size: 5; padding: 0">Tags</button>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_TITLE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Tags').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_TAGS" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Note').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_NOTE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_CREATED_TIME" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('{{"CONTRACT_CURD_TAB_NOTE_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Sửa" ng-click="edit(' + full.ContractNoteId + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.ContractNoteId + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contractTabNoteAdd.html',
            controller: 'contractTabNoteAdd',
            backdrop: 'static',
            size: '35'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    $scope.edit = function (id) {
        dataservice.getContractNote(id, function (rs) {
            if (rs.Error) {
                App.toastrError(ts.Title);
            }
            else {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: ctxfolder + '/contractTabNoteEdit.html',
                    controller: 'contractTabNoteEdit',
                    backdrop: 'static',
                    size: '35',
                    resolve: {
                        para: function () {
                            return rs.Object;
                        }
                    }
                });
                modalInstance.result.then(function (d) {
                    $scope.reload();
                }, function () {
                });
            }
        })
    }
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM;
                $scope.ok = function () {
                    dataservice.deleteContractNote(id, function (result) {
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
});
app.controller('contractTabNoteAdd', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice) {
    $scope.model = {
        ContractCode: '',
        Title: '',
        Tags: '',
        Note: ''
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.addformNote.validate()) {
            $scope.model.ContractCode = $rootScope.ContractCode;
            dataservice.insertContractNote($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(ts.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contractTabNoteEdit', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice, para) {
    $scope.init = function () {
        $scope.model = para;
    }
    $scope.init();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.editformNote.validate()) {
            dataservice.updateContractNote($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(ts.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});

app.controller('contractTabNotification', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    //$scope.model = {
    //    ContractCode: ''
    //}
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableNotification",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
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
            $compile(angular.element(row).contents())($scope);
        });
    vm.dtColumns = [];
    vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.NotifyID] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.NotifyID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('NotifyCode').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_NOTIFY_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_TITLE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Content').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_CONTENT" | translate}}').renderWith(function (data, type) {
        return data;
    }));

    vm.dtColumns.push(DTColumnBuilder.newColumn('ReceiverConfirm').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_RECEIVER_CONFIRM" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ConfirmTime').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_CONFIRM_TIME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('IsWarning').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_IS_WARNING" | translate}}').renderWith(function (data, type) {
        return data == "True" ? '<span class="text-danger"><i class="glyphicon glyphicon-exclamation-sign"></i> Cảnh báo</span> ' : '<span class="text-success">Bình thường</span>';
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('').withTitle('{{"CONTRACT_CURD_TAB_NOTIFICATION_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Xoá" ng-click="delete(' + full.NotifyID + ')" style="width: 25px; height: 25px; padding: 0px; -webkit-box-shadow: 0 2px 5px 0 rgba(230,60,95,0.45)" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
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

    //$scope.initLoad = function () {
    //    var userModel = {};
    //    var listdata = $('#tblDataContract').DataTable().data();
    //    for (var i = 0; i < listdata.length; i++) {
    //        if (listdata[i].id == $rootScope.ContractCode) {
    //            userModel = listdata[i];
    //            break;
    //        }
    //    }
    //    $scope.model.ContractCode = userModel.code;
    //}
    //$scope.initLoad();
    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;//"Bạn có chắc chắn muốn xóa thông báo ?";
                $scope.ok = function () {
                    dataservice.deleteNotification(id, function (rs) {
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

app.controller('contractTabAttribute', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        AttrCode: '',
        AttrValue: ''
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableAttribute",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
                d.AttrCode = $scope.model.AttrCode;
                d.AttrValue = $scope.model.AttrValue;
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
    vm.dtColumns.push(DTColumnBuilder.newColumn("check").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
        $scope.selected[full.id] = false;
        return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.id + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    }).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('code').withTitle('{{"CONTRACT_CURD_TAB_ATTRIBUTE_LIST_COL_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('value').withTitle('{{"CONTRACT_CURD_TAB_ATTRIBUTE_LIST_COL_VALUE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('attrGroup').withTitle('{{"CONTRACT_CURD_TAB_ATTRIBUTE_LIST_COL_ATTR_GROUP" | translate}}').renderWith(function (data, type) {
        for (var i = 0; i < $rootScope.ListCommon.length; i++) {
            if ($rootScope.ListCommon[i].Code == data) {
                return $rootScope.ListCommon[i].Name;
                break;
            }
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').withTitle('{{"CONTRACT_CURD_TAB_ATTRIBUTE_LIST_COL_ACTION" | translate}}').withOption('sWidth', '40px').renderWith(function (data, type, full) {
        return '<button title="Sửa" ng-click="edit(' + full.id + ')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>' +
            '<button title="Xoá" ng-click="delete(' + full.id + ')" style="width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline red"><i class="fa fa-trash"></i></button>';
    }).withOption('sClass', 'col50'));
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/contractTabAttributeAdd.html',
            controller: 'contractTabAttributeAdd',
            backdrop: 'static',
            size: '35'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }

    $scope.edit = function (id) {
        dataservice.getContractAttr(id, function (rs) {
            if (rs.Error) {
                App.toastrError(ts.Title);
            }
            else {
                var modalInstance = $uibModal.open({
                    animation: true,
                    templateUrl: ctxfolder + '/contractTabAttributeEdit.html',
                    controller: 'contractTabAttributeEdit',
                    backdrop: 'static',
                    size: '35',
                    resolve: {
                        para: function () {
                            return rs.Object;
                        }
                    }
                });
                modalInstance.result.then(function (d) {
                    $scope.reload();
                }, function () {
                });
            }
        })
    }

    $scope.delete = function (id) {
        var modalInstance = $uibModal.open({
            templateUrl: ctxfolderMessage + '/messageConfirmDeleted.html',
            windowClass: "message-center",
            controller: function ($scope, $uibModalInstance) {
                $scope.message = caption.COM_MSG_DELETE_CONFIRM_COM;//"Bạn có chắc chắn muốn xóa?";
                $scope.ok = function () {
                    dataservice.deleteContractAttr(id, function (result) {
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
});
app.controller('contractTabAttributeAdd', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice) {
    $scope.model = {
        ContractCode: '',
        AttrCode: '',
        AttrValue: '',
        AttrGroup: ''
    }
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.addformAttribute.validate()) {
            $scope.model.ContractCode = $rootScope.ContractCode;
            dataservice.insertContractAttr($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(ts.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close(rs.Object);
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});
app.controller('contractTabAttributeEdit', function ($scope, $rootScope, $uibModal, $uibModalInstance, $filter, dataservice, para) {
    $scope.init = function () {
        $scope.model = para;
    }
    $scope.init();
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        if ($scope.editformAttribute.validate()) {
            dataservice.updateContractAttr($scope.model, function (rs) {
                if (rs.Error) {
                    App.toastrError(ts.Title);
                }
                else {
                    App.toastrSuccess(rs.Title);
                    $uibModalInstance.close();
                }
            })
        }
    }
    setTimeout(function () {
        setModalDraggable('.modal-dialog');
    }, 200);
});

app.controller('contractTabOther', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractOther",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
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
        });
    vm.dtColumns = [];
    //vm.dtColumns.push(DTColumnBuilder.newColumn("Id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.ContractHeaderID] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractHeaderID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('ContractCode').withTitle('{{"CONTRACT_CURD_TAB_OTHER_LIST_COL_CONTRACT_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Title').withTitle('{{"CONTRACT_CURD_TAB_OTHER_LIST_COL_TITLE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedBy').withTitle('{{"CONTRACT_CURD_TAB_OTHER_LIST_COL_CREATED_BY" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"CONTRACT_CURD_TAB_OTHER_LIST_COL_CREATED_TIME" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('date')(new Date(data), 'dd/MM/yyyy') : null;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('Status').withTitle('{{"CONTRACT_CURD_TAB_OTHER_LIST_COL_STATUS" | translate}}').renderWith(function (data, type) {
        return data == "Có hiệu lực" ? '<span class="text-success">Có hiệu lực</span>' : '<span class="text-danger">Không hiệu lực</span>';
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/note_add.html',
            controller: 'note_add',
            backdrop: true,
            size: '90'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }
});
app.controller('contractTabMembers', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractMember",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
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
        });
    vm.dtColumns = [];
    //vm.dtColumns.push(DTColumnBuilder.newColumn("id").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.ContractHeaderID] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractHeaderID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('assigner').withTitle('{{"CONTRACT_CURD_TAB_MEMBER_LIST_COL_ASSIGNER" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('time').withTitle('{{"CONTRACT_CURD_TAB_MEMBER_LIST_COL_TIME" | translate}}Thời gian').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('assignee').withTitle('{{"CONTRACT_CURD_TAB_MEMBER_LIST_COL_ASSIGNEE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('task').withTitle('{{"CONTRACT_CURD_TAB_MEMBER_LIST_COL_TASK" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('Status').withTitle('Trạng thái').renderWith(function (data, type) {
    //    return data == "Có hiệu lực" ? '<span class="text-success">Có hiệu lực</span>' : '<span class="text-danger">Không hiệu lực</span>';
    //}));
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
    $scope.add = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: ctxfolder + '/note_add.html',
            controller: 'note_add',
            backdrop: true,
            size: '90'
        });
        modalInstance.result.then(function (d) {
            $scope.reload()
        }, function () { });
    }
});

//tab payment
app.controller('contractTabPayment', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    $scope.model = {
        FromTo: '',
        DateTo: '',
        PaymentName: '',
        PaymentType: '',
    }
    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractTabPayment",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
                d.FromTo = $scope.model.FromTo;
                d.DateTo = $scope.model.DateTo;
                d.PaymentName = $scope.model.PaymentName;
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
    //vm.dtColumns.push(DTColumnBuilder.newColumn("_STT").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.ContractHeaderID] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractHeaderID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', 'hidden'));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayCode').withTitle('{{"CONTRACT_CURD_TAB_PAYMENT_LIST_COL_PAY_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayTitle').withTitle('{{"CONTRACT_CURD_TAB_PAYMENT_LIST_COL_PAY_TITLE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('PayType').withTitle('{{"CONTRACT_CURD_TAB_PAYMENT_LIST_COL_PAY_TYPE" | translate}}Loại phiếu').renderWith(function (data, type) {
        if (data == "True") {
            return '<span class="text-success">Phiếu thu</span>';
        } else {
            return '<span class="text-danger">Phiếu thu</span>';
        }
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('{{"CONTRACT_CURD_TAB_PAYMENT_LIST_COL_MONEY_TOTAL" | translate}}').renderWith(function (data, type) {
        return data != "" ? $filter('currency')(data, '', 0) : null;
    }));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('Người thu').renderWith(function (data, type) {
    //    return data != "" ? $filter('currency')(data, '', 0) : null;
    //}));
    //vm.dtColumns.push(DTColumnBuilder.newColumn('MoneyTotal').withTitle('Người thanh toán').renderWith(function (data, type) {
    //    return data != "" ? $filter('currency')(data, '', 0) : null;
    //}));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CreatedTime').withTitle('{{"CONTRACT_CURD_TAB_PAYMENT_LIST_COL_CREATED_TIME" | translate}}').renderWith(function (data, type) {
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
    //$scope.initLoad = function () {
    //    var userModel = {};
    //    var listdata = $('#tblDataContract').DataTable().data();
    //    for (var i = 0; i < listdata.length; i++) {
    //        if (listdata[i].id == $rootScope.ContractCode) {
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
            templateUrl: ctxfolder + '/contractTabPaymentAdd.html',
            controller: 'contractTabPaymentAdd',
            backdrop: 'static',
            size: '70'
        });
        modalInstance.result.then(function (d) {
            $scope.reload();
        }, function () {
        });
    }
    setTimeout(function () {
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
    }, 200);
});
app.controller('contractTabPaymentAdd', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, $uibModalInstance, $filter, dataservice) {
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
    }
    $scope.initLoad();

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.submit = function () {
        validationSelect($scope.model);
        $scope.model.PayNextTime = new Date($scope.model.PayNextTime);
        if ($scope.addformPayment.validate() && !validationSelect($scope.model).Status) {
            if (parseFloat($scope.model.MoneyTotal) > parseFloat($scope.model1.Total)) {
                App.toastrError(caption.CONTRACT_CURD_TAB_PAYMENT_MSG_MONEY_TOTAL);//Số tiền thanh toán không được lớn hơn số tiền còn lại
            } else {
                var msg = $rootScope.checkDatapayment($scope.model);
                if (msg.Status) {
                    App.toastrError(msg.Title);
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
                        $scope.message = caption.CONTRACT_CURD_TAB_PAYMENT_MSG_ADD_PAYMENT;//Bạn muốn thêm phiếu thu-chi này
                        $scope.ok = function () {
                            dataservice.insertContractPayment(para, function (rs) {
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
                    $uibModalInstance.close();
                }, function () {
                });
            }
        }
    }

    $scope.selectObj = function (obj) {
        if (obj.indexOf("Contract")) {
            dataservice.getContract(function (rs) {
                $scope.ObjectName = rs;
            });
        }
    }
    $scope.selectObjName = function (obj) {
        dataservice.checkContract(obj, function (rs) {
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

//card
app.controller('contractTabCardJob', function ($scope, $rootScope, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var vm = $scope;
    $scope.selected = [];
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    //$scope.model = {
    //    ContractCode: ''
    //};
    $scope.relative = [
        { Code: "MAIN", Name: "Main" },
        { Code: "SUP", Name: "Support" }
    ];



    var titleHtml = '<label class="mt-checkbox"><input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)"/><span></span></label>';

    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withOption('ajax', {
            url: "/Admin/Contract/JTableContractCardjob",
            beforeSend: function (jqXHR, settings) {
                App.blockUI({
                    target: "#contentMain",
                    boxed: true,
                    message: 'loading...'
                });
            },
            type: 'POST',
            data: function (d) {
                d.ContractCode = $rootScope.ContractCode;
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
    //vm.dtColumns.push(DTColumnBuilder.newColumn("_STT").withTitle(titleHtml).notSortable().renderWith(function (data, type, full, meta) {
    //    $scope.selected[full.ContractHeaderID] = false;
    //    return '<label class="mt-checkbox"><input type="checkbox" ng-model="selected[' + full.ContractHeaderID + ']" ng-click="toggleOne(selected)"/><span></span></label>';
    //}).withOption('sClass', 'hidden'));

    vm.dtColumns.push(DTColumnBuilder.newColumn('Contract').withTitle('{{"CONTRACT_CURD_TAB_CARD_JOB_LIST_COL_CONTRACT" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardCode').withTitle('{{"CONTRACT_CURD_TAB_CARD_JOB_LIST_COL_CARD_CODE" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('CardName').withTitle('{{"CONTRACT_CURD_TAB_CARD_JOB_LIST_COL_CARD_NAME" | translate}}').renderWith(function (data, type) {
        return data;
    }));
    vm.dtColumns.push(DTColumnBuilder.newColumn('action').withTitle('{{"CONTRACT_CURD_TAB_CARD_JOB_LIST_COL_ACTION" | translate}}').renderWith(function (data, type, full) {
        return '<button title="Sửa thẻ" ng-click="edit(\'' + full.CardCode + '\')" style = "width: 25px; height: 25px; padding: 0px" class="btn btn-icon-only btn-circle btn-outline blue"><i class="fa fa-edit"></i></button>';
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
    };
    $scope.initLoad = function () {
        dataservice.getTeams(function (rs) {
            $scope.Teams = rs;
        });
    };
    $scope.initLoad();

    $scope.add = function () {
        if ($scope.model.Card === undefined) {
            App.toastrError(caption.CONTRACT_CURD_TAB_CARD_JOB_MSG_CARD);//Chọn thẻ!
            return;
        }
        if ($scope.model.Relative === undefined) {
            App.toastrError(caption.CONTRACT_CURD_TAB_CARD_JOB_MSG_RELATIVE); //Chọn mối quan hệ
            return;
        }
        var data = {
            ObjCode: $rootScope.ContractCode,
            CatObjCode: "CONTRACT",
            CardCode: $scope.model.Card,
            Relative: $scope.model.Relative
        };
        dataservice.addCardRelative(data, function (rs) {
            if (rs.Error) {
                App.toastrError(rs.Title);
            }
            else {
                App.toastrSuccess(rs.Title);
                $scope.reload();
            }
        });
    };
    $scope.edit = function (cardCode) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: '/views/admin/cardJob/edit-tag.html',
            controller: 'edit-card',
            backdrop: true,
            size: '60',
            resolve: {
                para: function () {
                    return cardCode;
                }
            }
        });
        modalInstance.result.then(function (d) {
            //$scope.reload();
        }, function () { });
    }
    $scope.teamSelect = function (teamCode) {
        dataservice.getBoardByTeam(teamCode, function (rs) {
            $scope.Boards = rs;
        });
    };
    $scope.boardSelect = function (boardCode) {
        dataservice.getLists(boardCode, function (rs) {
            $scope.Lists = rs;
        });
    };
    $scope.listSelect = function (listCode) {
        dataservice.getCards(listCode, function (rs) {
            $scope.Cards = rs;
        });
    };

});
app.controller('add-cardjob', function ($scope, $rootScope, $compile, $uibModal, $uibModalInstance, DTColumnBuilder, DTInstances, dataservice, $filter) {
    //$scope.model = {
    //    ContractCode: ''
    //};

    //$scope.cancel = function () {
    //    $uibModalInstance.close();
    //}
    //$scope.relative = [
    //    { Code: "MAIN", Name: "Main" },
    //    { Code: "SUP", Name: "Support" }
    //];

    //$scope.add = function () {
    //    if ($scope.model.List === undefined) {
    //        App.toastrError("Chọn thẻ!");
    //        return;
    //    }
    //    if ($scope.model.Relative === undefined) {
    //        App.toastrError("Chọn mối quan hệ!");
    //        return;
    //    }
    //    var data = {
    //        ObjCode: $rootScope.ContractHeaderId,
    //        CatObjCode: "CONTRACT",
    //        ListCode: $scope.model.List,
    //        CardName: $scope.model.CardName,
    //        Relative: $scope.model.Relative
    //    };
    //    dataservice.addCardJob(data, function (rs) {
    //        if (rs.Error) {
    //            App.toastrError(rs.Title);
    //        }
    //        else {
    //            App.toastrSuccess(rs.Title);
    //            $scope.reload();
    //        }
    //    });
    //};

    //$scope.initLoad = function () {
    //    dataservice.getTeams(function (rs) {
    //        $scope.Teams = rs;
    //    });
    //};
    //$scope.initLoad();

    //$scope.teamSelect = function (teamCode) {
    //    dataservice.getBoardByTeam(teamCode, function (rs) {
    //        $scope.Boards = rs;
    //    });
    //};
    //$scope.boardSelect = function (boardCode) {
    //    dataservice.getLists(boardCode, function (rs) {
    //        $scope.Lists = rs;
    //    });
    //};
    //$scope.listSelect = function (listCode) {
    //    dataservice.getCards(listCode, function (rs) {
    //        $scope.Cards = rs;
    //    });
    //};

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
            ObjCode: $rootScope.ContractCode,
            CatObjCode: "CONTRACT",
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
            App.toastrError(caption.CONTRACT_CURD_TAB_MEMBER_MSG_DEFAULT);//Chưa cài đặt mặt định!
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
                required: caption.COM_ERR_REQUIRED.replace("{0}", caption.CONTRACT_CURD_TAB_NOTE_CURD_LBL_TITLE),//'Nhập tiêu đề!',
                maxlength: caption.COM_ERR_EXCEED_CHARACTERS.replace("{0}", "").replace("{1}", "255")//'Cho phép tối đa 255 ký tự!'
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
            App.toastrError(caption.COM_MSG_CHOSE_FILE)//"Vui lòng chọn tệp tin");
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