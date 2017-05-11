namespace Sc.Client.Personalisation.Pipelines
{
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;

    public class AddPersonalisedWrapper : MvcPipelineProcessor<RenderRenderingArgs>
    {
        public override void Process(RenderRenderingArgs args)
        {
            if (args.Rendered)
            {
                return;
            }
            if (Sitecore.Context.PageMode.IsNormal && !string.IsNullOrEmpty(args.Rendering.Properties["PersonlizationRules"]))
            {
                if (!args.PageContext.RequestContext.HttpContext.Request.Url.ToString().Contains("/sitecore/api/ssc/"))
                {
                    args.Disposables.Insert(0, new PersonalisationWrapper(args, args.Rendering.UniqueId));
                }
            }
        }
    }
}