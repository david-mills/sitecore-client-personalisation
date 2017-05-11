(function ($) {
    getPersonalisedUrl: function() {
        return "personalised-isstudent=1";
    }
    var url = "/sitecore/api/ssc/personalise/v1/110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9/gethtml?sc_lang=en-gb";
    url = url + "&" + getPersonalisedUrl();
    var personalisedComponents = $('.personalised');
    if (personalisedComponents.length > 0) {
        $.ajax({
            dataType: "json",
            url: url,
            timeout: 3000,
            success: function (data) {
                if (data) {
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        var selector = ".personalised[data-personalised='" + item.Id + "'] :nth-child(2)";
                        var component = $(selector);
                        if (component.length > 0) {
                            component.replaceWith(item.Html);
                        }
                    }
                }
            },
            complete: function () {
                personalisedComponents.removeClass('personalised');
            }
        });
    }
})($, provider, personalisedConfig);