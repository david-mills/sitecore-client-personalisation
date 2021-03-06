﻿define(function () {
    return {
        setCookie: function(cname, cvalue, exdays) {
            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        },

        getCookie: function (cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        },

        clearCookies: function (prefix) {
            var cookies = this.getCookieNames();
            for (var i = 0; i < cookies.length; i++) {
                if (cookies[i].indexOf(prefix) >= 0) {
                    this.setCookie(cookies[i], "", -1);
                }
            }
        },
        getCookieNames: function () {
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                ca[i] = c.substring(0, c.indexOf('='));
            }
            return ca;
        }
    }
});