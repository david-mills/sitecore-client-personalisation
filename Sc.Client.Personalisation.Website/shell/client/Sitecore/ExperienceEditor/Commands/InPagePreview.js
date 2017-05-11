define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.InPagePreview =
    {
        canExecute: function (context) {
            return false;//context.app.canExecute("ExperienceEditor.Personalisation.Commands.IsTargetedContentPageCommand", context.currentContext);
        },

        execute: function (context) {
            ExperienceEditor.modifiedHandling(true,
                function (isOk) {
                    ExperienceEditor.PipelinesUtil.executePipeline(context.app.InPagePreviewPipeline,
                        function () {
                            ExperienceEditor.PipelinesUtil.executeProcessors(Sitecore.Pipelines.InPagePreview, context);
                        });
                });
        }
    };
});