define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js", "/sitecore/shell/client/Sitecore/ExperienceEditor/Commands/CookieHelper.js"], function (Sitecore, ExperienceEditor, Cookies) {
    Sitecore.Commands.ApplyPersonalisation =
    {
        canExecute: function (context) {

            var cookies = Cookies.getCookieNames();
            var isPressed = false;
            for (var i = 0; i < cookies.length; i++) {
                if (cookies[i].indexOf('asos-pers') >= 0) {
                    isPressed = true;
                }
            }
            context.button.set(
            {
                isPressed: isPressed
            });

            return true;
        },

        execute: function (context) {
            ExperienceEditor.modifiedHandling(true,
                function (isOk) {
                    ExperienceEditor.PipelinesUtil.executePipeline(context.app.PersonalisationPipeline,
                        function () {
                            ExperienceEditor.PipelinesUtil.executeProcessors(Sitecore.Pipelines.Personalisation, context);
                        });
                });
        }
    };
});