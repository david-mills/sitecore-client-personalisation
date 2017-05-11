define(["sitecore", "/sitecore/shell/client/Sitecore/ExperienceEditor/Commands/CookieHelper.js"],
    function(Sitecore, cookieHelper) {
        Sitecore.Commands.PersonalisationClearer =
        {
            canExecute: function(context) {
                return true;
            },

            execute: function (context) {
                cookieHelper.clearCookies('asos-pers');
                window.parent.location.reload();
            }
        }
    });
    