define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js", "/sitecore/shell/client/Sitecore/ExperienceEditor/Commands/CookieHelper.js"], function (Sitecore, ExperienceEditor, Cookies) {
    return {
        priority: 2,
        execute: function (context) {
            

            Cookies.clearCookies('asos-previewurl');
            if (context.currentContext.InPagePreview && context.currentContext.InPagePreview.url) {
                var url = context.currentContext.InPagePreview.url;
                if (url != "-1") {
                    Cookies.setCookie('asos-previewurl', context.currentContext.InPagePreview.url, 1);
                }
            }
            window.parent.location.reload();
        }
    };
});