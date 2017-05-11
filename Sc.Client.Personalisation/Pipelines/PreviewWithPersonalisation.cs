namespace Sc.Client.Personalisation.Pipelines
{
    using System.Linq;
    using System.Web;
    using Constants;
    using Sitecore.Analytics;
    using Sitecore.Mvc.Analytics.Pipelines.Response.CustomizeRendering;

    public class PreviewWithPersonalisation : Personalize
    {
        public override void Process(CustomizeRenderingArgs args)
        {
            global::Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
            if (args.IsCustomized)
            {
                return;
            }

            var showPersonalisedContent = HttpContext.Current != null && HttpContext.Current.Request.Cookies.AllKeys.Any(t => t.ToLower().StartsWith(PersonalisationConstants.RequestPrefix)); ;

            if (!showPersonalisedContent)
            {
                if (!Tracker.IsActive)
                {
                    return;
                }
            }
            this.Evaluate(args);
        }
    }
}