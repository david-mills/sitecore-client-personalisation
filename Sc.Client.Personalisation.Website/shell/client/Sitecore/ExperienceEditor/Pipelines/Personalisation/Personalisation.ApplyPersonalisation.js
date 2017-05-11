define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js", "/sitecore/shell/client/Sitecore/ExperienceEditor/Commands/CookieHelper.js"], function (Sitecore, ExperienceEditor, Cookies) {
    return {
        priority: 2,
        execute: function (context) {
            

            Cookies.clearCookies('asos-pers');
            if (context.currentContext.personalisation.indexOf('=') > 0) {
                var pairs = context.currentContext.personalisation.split('&');
                for (var i = 0; i < pairs.length; i++) {
                    var cookie = pairs[i].split('=');
                    if (cookie.length === 2) {
                        Cookies.setCookie(cookie[0], cookie[1], 1);
                    }
                }
            }
            // TODO: Check modified flag
            window.parent.location.reload();
        }
    };
});