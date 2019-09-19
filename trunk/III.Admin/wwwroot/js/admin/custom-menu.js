/*=========================================================================================
  File Name: custom-menu.js
  Description: avtive menu,expand,collapse menu
  initialization and manipulations
  ----------------------------------------------------------------------------------------
  Item Name: Robust - Responsive Admin Theme
  Version: 1.2
  Author: GeeksLabs
  Author URL: http://www.themeforest.net/user/geekslabs
==========================================================================================*/
var configMenu = {
    init: function () {
        configMenu.activeNav();
        configMenu.expand();
        configMenu.navClick();
    },
    expand: function () {
        $('.main-menu-content').find('.navigation-header').addClass('open');
    },
    collapse: function () {
        var listMenu = $('.main-menu-content').find('.navigation-header');
    },
    activeNav: function () {
        if (document.location.pathname === '/Admin/CardJob') {
            $('#btnOpenTrello').removeAttr('href');
        } else {
            $("ul.navigation-main li a[href='" + document.location.pathname + "']").parents('li').addClass('active').addClass('hover').addClass('open');
        }
    },
    navClick: function () {
        $('.navigation-header').click(function (e) {
            if ($(this).hasClass("open")) {
                $(this).removeClass("open");
                $(this).nextUntil('.navigation-header').hide(200);
            } else {
                $(this).addClass("open");
                $(this).nextUntil('.navigation-header').show(200);
            }
            e.stopImmediatePropagation();
        });
    }
}

$(document).ready(function () {
    configMenu.init();
});




