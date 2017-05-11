define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    
    return ExperienceEditor.PipelinesUtil.generateDialogCallProcessor({
        url: function (context) { return "/sitecore/shell/Applications/InPagePreview.aspx?fo=" + context.currentContext.itemId + "&sc_content=" + context.currentContext.database + "&sc_lang=" + context.currentContext.language; },
        features: "dialogHeight: 400px;dialogWidth: 400px;",
        onSuccess: function (context, value) {
            context.currentContext.InPagePreview = { url: value };
        }
    });
});