var ctxfolder = "/views/admin/videoCall";
var app = angular.module('App_ESEIM', ["ui.bootstrap", "ngRoute", "ngValidate", "datatables", "datatables.bootstrap", "ngJsTree", "treeGrid", 'datatables.colvis', "ui.bootstrap.contextMenu", 'datatables.colreorder', 'angular-confirm', 'ui.select', "ngCookies", "pascalprecht.translate"]);

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
            //max: 'Max some message {0}'
        });
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

app.controller('index', function ($scope, $rootScope, $confirm, $compile, $uibModal, DTOptionsBuilder, DTColumnBuilder, DTInstances, dataservice, $filter) {
    var webSyncHandleUrl = 'https://websync.mapguide.vn/websync.ashx';
    fm.websync.client.enableMultiple = true;
    var clients = new fm.websync.client(webSyncHandleUrl);
    var testichat = document.getElementById('testichat');
    var cnt = 0;

    loadChat('Tutor', 'public', clients, testichat);
    getSubscribe(clients, 'public', testichat);
    var url = 'https://ikstudy.com:8000';

    var config = {
        innit: function () {
            $(".video_list").click(function () {
                $(".menu-tray").show();
                $(".attend-video-list").show();
                $(".attend-list").hide();
                $('.handle img').css("box-shadow", "0px -4px 0px #bababa");
                $('.handle hr').css("box-shadow", "0px -3px 0px #bababa");
            });
            $(".pop-chat").click(function () {
                $(".menu-tray").show("slide", { direction: "right" }, "slow");
            });
            $(".student_list").click(function () {
                $(".menu-tray").show();
                $(".attend-list").show();
                $(".attend-video-list").hide();
            }); 
            $(".status-selector").click(function () {
                if ($(this).hasClass('active')) {
                    $(this).removeClass('active');
                    $('.status-selector-bar').hide();
                } else {
                    $(this).addClass('active');
                    $('.status-selector-bar').show();
                }
            });    
        }
    }

    function initChat() {
        var socket = io.connect(url);

        $(".btn-student").dblclick(function () {
            var id = $(this).attr('data-id');
            var tab = getTab();
            var channels = [];

            $("#tab-chat li").each(function (i) {
                var room = $(this).attr('data-room');
                channels.push('/' + room);
            });
            if (id != tab && cnt < 1) {
                cnt = cnt + 1;
                $('.inbox-message').css("display", "none");

                var username = $(this).attr('data-name');
                var stanza = roomid = 'private' + id;
                var ul = document.createElement('ul');
                ul.id = "testichat" + id;
                ul.className = "inbox-message style-scrollbar";

                var img = document.createElement('img');
                img.id = "closePrivate" + id;
                img.className = "close-private";
                img.src = "assets/icons/icon_CLOSE.png";
                $('#chat').append(img);
                $('#chat').append(ul);

                var testichats = document.getElementById('testichat' + id);

                loadChat(username, roomid, clients, testichats);
                getSubscribe(clients, roomid, testichats);
                if (channels.length) unSubscribe(clients, channels);

                socket.emit('privatemessage', {
                    'id': id,
                    'roomid': roomid,
                    'studentid': id,
                    'studentname': username,
                    'room': stanza
                });

                $('.all-message').removeClass('active');
                $('.mess-private').removeClass('active');
                $('<li class="item-stt-message mess-private active" data-id="' + id + '" data-name="' + username + '" data-room="' + roomid + '"><p class="text-overfl">' + username + '</p></li>').insertAfter(".all-message");
            }
        });

        $('.prev-message').click(function () {
            var $prev = $('#tab-chat .active').prev();
            if ($prev.length) {
                $('#tab-chat').animate({
                    scrollLeft: $prev.position().left
                }, 'slow');
            }
        });

        $('.next-message').click(function () {
            var $next = $('#tab-chat .active').next();
            if ($next.length) {
                $('#tab-chat').animate({
                    scrollLeft: $next.position().left
                }, 'slow');
            }
        });

        socket.on('privatecreate', function (data) {
            var channels = [];

            $("#tab-chat li").each(function (i) {
                var room = $(this).attr('data-room');
                channels.push('/' + room);
            });

            $('.inbox-message').css("display", "none");

            var ul = document.createElement('ul');
            ul.id = "testichat" + data.studentid;
            ul.className = "inbox-message style-scrollbar";
            $('#chat').append(ul);
            stanza = data.roomid;

            var testichats = document.getElementById('testichat' + data.studentid);

            loadChat('Tutor', data.roomid, clients, testichats);
            getSubscribe(clients, data.roomid, testichats);
            if (channels.length) unSubscribe(clients, channels);

            $('.all-message').removeClass('active');
            $('.mess-private').removeClass('active');
            $('<li class="item-stt-message mess-private active" data-id="' + data.studentid + '" data-name="' + data.studentname + '" data-room="' + data.roomid + '"><p class="text-overfl">' + data.studentname + '</p></li>').insertAfter(".all-message");
        });
    }

    function initVideo() {
        var videoChat = document.getElementById('videoChat');
        var loading = document.getElementById('loading');
        var video = document.getElementById('video');
        var closeVideo = document.getElementById('closeVideo');
        var toggleAudioMute = document.getElementById('toggleAudioMute');
        var toggleVideoMute = document.getElementById('toggleVideoMute');
        var joinSessionButton = document.getElementById('catturacam');

        var app = new Video(testichat);
        var start = function (sessionId, statusVideo = false, statusAudio = true) {
            if (app.sessionId) {
                return;
            }

            if (sessionId.length != 6) {
                console.log('Session ID must be 6 digits long.');
                return;
            }

            app.sessionId = sessionId;

            // Switch the UI context.
            //location.hash = app.sessionId + '&screen=' + (captureScreenCheckbox.checked ? '1' : '0');
            videoChat.style.display = 'block';

            console.log('Joining session ' + app.sessionId + '.');
            //fm.log.info('Joining session ' + app.sessionId + '.');

            // Start the signalling client.
            app.startSignalling(function (error) {
                if (error != null) {
                    console.log(error);
                    stop();
                    return;
                }

                // Start the local media stream.
                app.startLocalMedia(video, false, statusVideo, statusAudio, function (error) {
                    if (error != null) {
                        console.log(error);
                        stop();
                        return;
                    }

                    // Update the UI context.
                    loading.style.display = 'none';
                    video.style.display = 'block';

                    // Enable the media controls.
                    //toggleAudioMute.removeAttribute('disabled');
                    //toggleVideoMute.removeAttribute('disabled');

                    // Start the conference.
                    app.startConference(function (error) {
                        if (error != null) {
                            console.log(error);
                            stop();
                            return;
                        }

                        // Enable the leave button.
                        //leaveButton.removeAttribute('disabled');

                        //fm.log.info('<span style="font-size: 1.5em;">' + app.sessionId + '</span>');
                        console.log('<span style="font-size: 1.5em;">' + app.sessionId + '</span>');
                    }, function () {
                        stop();
                    });
                });
            });
        };

        var stop = function () {
            if (!app.sessionId) {
                return;
            }

            // Disable the leave button.
            // leaveButton.setAttribute('disabled', 'disabled');

            console.log('Leaving session ' + app.sessionId + '.');
            //fm.log.info('Leaving session ' + app.sessionId + '.');

            app.sessionId = '';

            $('#catturacam').removeClass('active');

            app.stopConference(function (error) {
                if (error) {
                    fm.log.error(error);
                }

                // Disable the media controls.
                //toggleAudioMute.setAttribute('disabled', 'disabled');
                //toggleVideoMute.setAttribute('disabled', 'disabled');

                // Update the UI context.
                video.style.display = 'none';
                loading.style.display = 'block';

                app.stopLocalMedia(function (error) {
                    if (error) {
                        fm.log.error(error);
                    }

                    app.stopSignalling(function (error) {
                        if (error) {
                            fm.log.error(error);
                        }
                        // Switch the UI context.
                        //sessionSelector.style.display = 'block';
                        videoChat.style.display = 'none';
                        location.hash = '';
                    });
                });
            });
        };

        // Attach DOM events.
        fm.util.observe(joinSessionButton, 'click', function (evt) {
            if ($(this).hasClass('active')) {
                videoChat.style.display = 'none';
                $(this).removeClass('active');
                stop();
            } else {
                videoChat.style.display = 'block';
                $(this).addClass('active');
                $(".menu-tray").show("slide", { direction: "right" }, "slow");
                if ($('#toggleAudioMute').hasClass('active'))
                    statusAudio = true;
                else
                    statusAudio = false;

                if ($('#toggleVideoMute').hasClass('active'))
                    statusVideo = true;
                else
                    statusVideo = false;

                start('public', statusVideo, statusAudio);
            }
        });

        fm.util.observe(closeVideo, 'click', function (evt) {
            videoChat.style.display = 'none';
            $('#catturacam').removeClass('active');
            stop();
        });

        fm.util.observe(window, 'unload', function () {
            stop();
        });

        fm.util.observe(toggleVideoMute, 'click', function (evt) {
            if ($(this).hasClass('active')) {
                var muted = app.toggleVideoMute();
                $(this).children().attr('src', '../../../lib/chat/image/icon_Toggle_ALL_OFF.png');
                $(this).removeClass('active');
                videoChat.style.display = 'none';
                $('#catturacam').removeClass('active');
                stop();
            } else {
                $(this).children().attr('src', '../../../lib/chat/image/icon_Toggle_ALL_ON.png');
                $(this).addClass('active');
                videoChat.style.display = 'block';
                $(".menu-tray").show("slide", { direction: "right" }, "slow");
                if ($('#toggleVideoMute').hasClass('active'))
                    statusVideo = true;
                else
                    statusVideo = false;

                start('public', statusVideo, true);
            }

        });

        fm.util.observe(toggleAudioMute, 'click', function (evt) {
            if ($(this).hasClass('active')) {
                var muted = app.toggleAudioMute();
                $(this).children().attr('src', '../../../lib/chat/image/icon_Toggle_ALL_OFF.png');
                $(this).removeClass('active');
            } else {
                $(this).children().attr('src', '../../../lib/chat/image/icon_Toggle_ALL_ON.png');
                $(this).addClass('active');

                if ($('#toggleAudioMute').hasClass('active'))
                    statusAudio = true;
                else
                    statusAudio = false;
            }

        });
    }

    function loadChat(username, roomid, client, testichats) {
        var name = username;
        var rooms = roomid;
        var clients = client;
        var testichat = testichats;

        fm.util.addOnLoad(function () {

            //init object chat between users a room 
            var chat = {
                alias: 'Unknown',
                clientId: 0,
                channels: {
                    main: '/' + rooms
                },
                dom: {
                    chat: {
                        container: document.getElementById('chat'),
                        text: document.getElementById('scrivi'),
                        send: document.getElementById('btn-send'),
                        username: name,
                        roomid: rooms
                    }
                },
                util: {
                    start: function () {
                        //console.log(name + ':' + room);
                        chat.alias = name;
                        chat.clientId = rooms;
                        //chat.util.hide(chat.dom.prechat.container);
                        chat.util.show(chat.dom.chat.container);
                        chat.util.scroll();
                        chat.dom.chat.text.focus();
                    },
                    stopEvent: function (event) {
                        if (event.preventDefault) {
                            event.preventDefault();
                        } else {
                            event.returnValue = false;
                        }
                        if (event.stopPropagation) {
                            event.stopPropagation();
                        } else {
                            event.cancelBubble = true;
                        }
                    },
                    send: function () {
                        if (chat.util.isEmpty(chat.dom.chat.text)) {
                            chat.util.setInvalid(chat.dom.chat.text);
                        } else {
                            clients.publish({
                                retries: 0,
                                channel: '/' + rooms,
                                data: {
                                    alias: chat.alias,
                                    text: chat.dom.chat.text.value
                                },
                                onSuccess: function (args) {
                                    chat.util.clear(chat.dom.chat.text);
                                }
                            });
                        }
                    },
                    show: function (el) {
                        el.style.display = '';
                    },
                    hide: function (el) {
                        el.style.display = 'none';
                    },
                    clear: function (el) {
                        el.value = '';
                    },
                    observe: fm.util.observe,
                    isEnter: function (e) {
                        return (e.keyCode == 13);
                    },
                    isEmpty: function (el) {
                        return (el.value == '');
                    },
                    setInvalid: function (el) {
                        el.className = 'invalid';
                    },
                    clearLog: function () {
                        testichat.innerHTML = '';
                    },
                    logMessage: function (alias, text, me) {
                        var html = '<li';
                        if (me) {
                            html += ' class="item-message"';
                        } else {
                            html += ' class="item-message me"';
                        }
                        html += '><p class="name-sender">' + alias + ':</p><p class="content-mess">' + text + '</p></li>';
                        chat.util.log(html);
                    },
                    logSuccess: function (text) {
                        chat.util.log('<li class="item-message success"><p class="content-mess">' + text + '</p></li>');
                    },
                    logFailure: function (text) {
                        chat.util.log('<li class="item-message failure"><p class="content-mess">' + text + '</p></li>');
                    },
                    log: function (html) {
                        var div = document.createElement('div');
                        div.innerHTML = html;
                        testichat.appendChild(div);
                        chat.util.scroll();
                    },
                    scroll: function () {
                        testichat.scrollTop = testichat.scrollHeight;
                    }
                }
            };

            chat.util.observe(chat.dom.chat.send, 'click', function (e) {
                chat.util.start();
                chat.util.send();
            });

            chat.util.observe(chat.dom.chat.text, 'keydown', function (e) {
                if (chat.util.isEnter(e)) {
                    chat.util.start();
                    chat.util.send();
                    chat.util.stopEvent(e);
                }
            });

            client.setAutoDisconnect({
                synchronous: true
            });

            clients.connect({
                onSuccess: function (args) {
                    chat.clientId = args.clientId;
                    chat.util.clearLog();
                    //chat.util.logSuccess('Connected to WebSync.');
                    //chat.util.show(chat.dom.prechat.container);
                    chat.util.show(chat.dom.chat.container);
                },
                onFailure: function (args) {
                    //var username = args.getData().alias;
                    //var content = ''

                    //chat.util.logSuccess('Could not connect to WebSync.');
                }
            });
        });
    }

    function activeTab() {
        $(document).on('click', '.item-stt-message', function () {
            var id = $(this).attr('data-id');
            var channels = [];
            $('.item-stt-message').removeClass('active');
            $('.inbox-message').css("display", "none");
            $(this).addClass('active');
            $("#tab-chat li").each(function (i) {
                if (!$(this).hasClass('active')) {
                    var room = $(this).attr('data-room');
                    channels.push('/' + room);
                }
            });
            if (id == 0) {
                getSubscribe(clients, 'public', testichat);
                if (channels.length) unSubscribe(clients, channels);
                $('#testichat').css("display", "block");
            } else {
                var username = $(this).attr('data-name');
                var stanza = roomid = $(this).attr('data-room');
                var testichats = document.getElementById('testichat' + id);
                getSubscribe(clients, roomid, testichats);
                if (channels.length) unSubscribe(clients, channels);
                $('#testichat' + id).css("display", "block");
            }
        });
    }

    function getSubscribe(clients, roomid, testichat) {
        clients.subscribe({
            channel: '/' + roomid,
            onSuccess: function (args) {
                //chat.util.logSuccess('Content chat.');               
                var logs = args.getExtensionValue('logs');
                if (logs != null) {
                    for (var i = 0; i < logs.length; i++) {
                        logMessage(logs[i].alias, logs[i].text, false, testichat);
                    }
                }
            },
            onFailure: function (args) {
                //chat.util.logSuccess('Not connecting.');
            },
            onReceive: function (args) {
                var ch = args.getChannel();
                console.log(ch);
                logMessage(args.getData().alias, args.getData().text, args.getWasSentByMe(), testichat);
            }
        });
    }

    function unSubscribe(clients, channels) {
        clients.unsubscribe({
            channels: channels,
            onFailure: function (args) {
                alert(args.error);
            }
        });
    }

    function logMessage(alias, text, me, testichat) {
        var html = '<li';
        if (me) {
            html += ' class="item-message"';
        } else {
            html += ' class="item-message me"';
        }
        html += '><p class="name-sender">' + alias + ':</p><p class="content-mess">' + text + '</p></li>';
        var div = document.createElement('div');
        div.innerHTML = html;
        testichat.appendChild(div);
        testichat.scrollTop = testichat.scrollHeight;
    }

    function createCanvas() {
        $("#layers-body li").each(function (i) {
            $(this).addClass("item" + (i + 1));
            $(this).attr("data-cnt", (i + 1));
            $(this).find('span').text((i + 1));

            var canvas = cloneCanvas((i + 1));
            $('#panel').append(canvas);
        });
    }

    function getTab() {
        var tab = 0;
        $("#tab-chat li").each(function (i) {
            if ($(this).hasClass('active')) {
                tab = parseInt($(this).attr('data-id'));
            }
        });
        return tab;
    }


    $(function () {
        config.innit();
        //createCanvas();
        //initDraw();
        //addLayer();
        //deleteLayer();
        //clearState();
        initChat();
        activeTab();
        initVideo();
        //logMessage();
    });
});
