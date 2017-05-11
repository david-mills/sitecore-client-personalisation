namespace Sc.Client.Personalisation.Pipelines
{
    using System;
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;

    public class PersonalisationWrapper : IDisposable
    {
        private readonly RenderRenderingArgs _args;

        public PersonalisationWrapper(RenderRenderingArgs args, Guid id)
        {
            _args = args;
            _args.Writer.Write(string.Format("<div class=\"personalised\" data-personalised=\"{0}\"><div class=\"personalised-overlay\"></div>", id.ToString().ToLower()));
        }

        public void Dispose()
        {
            _args.Writer.Write("</div>");
            _args.Writer.Flush();
        }
    }
}