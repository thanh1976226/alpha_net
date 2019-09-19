/*=========================================================================================
  File Name: site.js
  Description: config message,scroll top
  initialization and manipulations
  ----------------------------------------------------------------------------------------
==========================================================================================*/

var config = {
    init: function () {
        setInterval(config.countEventToday(), 300000);
        config.loadMessage();
        config.scrollToTop();
        config.clickScrollToTop();
        config.countEventToday();
    },
    loadMessage: function () {
        App.inputFile = function () {
            $(".input-file").before(
                function () {
                    if (!$(this).prev().hasClass('input-ghost')) {
                        var element = $("<input type='file' id='File' class='input-ghost' style='display: none'>");
                        element.attr("name", $(this).attr("name"));
                        element.change(function () {
                            element.next(element).find('.inputFile').val((element.val().split('\\').pop()));
                        });
                        $(this).find("button.btn-choose").click(function () {
                            element.click();
                        });
                        $(this).find('input').css("cursor", "pointer");
                        $(this).find('input').mousedown(function () {
                            $(this).parents('.input-file').prev().click();
                            return false;
                        });
                        return element;
                    }
                }
            );
        }

        App.toastrSuccess = function (msg) {
            toastr.clear();
            toastr['success'](msg);
        };

        App.toastrInfo = function (msg) {
            toastr.clear();
            toastr['info'](msg);
        };

        App.toastrWarning = function (msg) {
            toastr.clear();
            toastr['warning'](msg);
        };

        App.toastrError = function (msg) {
            toastr.clear();
            toastr['error'](msg);
        };
    },
    clickScrollToTop: function () {
        $(".go2top").click(function (o) {
            o.preventDefault(),
                $("html, body").animate({
                    scrollTop: 0
                }, 600)
        })
    },
    scrollToTop: function () {
        var o = $(window).scrollTop();
        o > 100 ? $(".go2top").show() : $(".go2top").hide()
    },
    countEventToday: function () {
        $.ajax({
            url: '/Admin/DispatchesWeekWorkingSchedule/GetEventToday/',
            type: 'POST',
            dataType: 'JSON',
            success: function (data, xhr) {
                if (data == 0) {
                    $("#notificationWeek").addClass('hidden');
                } else {
                    $("#notificationWeek").text(data);
                }
            }
        });
    }
}

jQuery(document).ready(function () {
    config.init();
    if (toastr != undefined) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "1000",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
    }
});

$(window).scroll(function () {
    config.scrollToTop();
});

$(window).resize(function () {
    if ($('.modal.in').length != 0) {
        setModalMaxHeight($('.modal.in'));
    }
});

function setModalDraggable(element) {
    //$(element).draggable({
    //    handle: ".modal-header"
    //});
    $(element).draggable({
        "handle": ".modal-header"
    });
    $(".modal-header").css({
        'cursor': 'pointer'
    });

    //$(element).children(".modal-dialog").draggable();
}

function setModalMaxHeight(element) {
    this.$element = $(element);
    //$(element).children(".modal-dialog").draggable();
    this.$content = this.$element.find('.modal-content');
    var borderWidth = this.$content.outerHeight() - this.$content.innerHeight();
    var dialogMargin = $(window).width() < 768 ? 20 : 60;
    var contentHeight = $(window).height() - (dialogMargin + borderWidth);
    var headerHeight = this.$element.find('.modal-header').outerHeight() || 0;
    var footerHeight = this.$element.find('.modal-footer').outerHeight() || 0;
    var maxHeight = contentHeight - (headerHeight + footerHeight) + 30;

    this.$content.css({
        'overflow': 'hidden'
    });
    this.$element
        .find('.modal-body').css({
            'max-height': maxHeight,
            'overflow-y': 'auto'
        });
    this.$element
        .find('.modal-body').addClass("scrollbar-lg");
}




