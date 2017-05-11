namespace Sc.Client.Personalisation.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Cors;
    using Models;
    using Sitecore.Data;
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.RenderRendering;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Services.Core;
    using Sitecore.Services.Infrastructure.Web.Http;

    [ServicesController("personalise/v1")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PersonalisationController : ServicesApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.ActionName("GetHtml")]
        public HttpResponseMessage GetHtml(string id)
        {
            var item = ID.Parse(id);
            var database = Sitecore.Context.Database;
            Sitecore.Context.Item = database.GetItem(item);
            var personalisedRenderingList = new List<PersonalisedRendering>();

            using (new PersonalisationContext())
            {
                var renderings = PageContext.Current.PageDefinition.Renderings.Where(IsPersonalisedRendering).ToList();

                foreach (Rendering current in renderings)
                {
                    using (StringWriter stringWriter = new StringWriter())
                    {
                        PipelineService.Get()
                            .RunPipeline<RenderRenderingArgs>("mvc.renderRendering",
                                new RenderRenderingArgs(current, stringWriter));

                        var personalisedRendering = new PersonalisedRendering()
                        {
                            Id = current.UniqueId.ToString(),
                            Html = stringWriter.ToString()
                        };

                        personalisedRenderingList.Add(personalisedRendering);
                    }
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, personalisedRenderingList);
        }

        private bool IsPersonalisedRendering(Rendering rendering)
        {
            return !rendering.RenderingType.Equals("layout", StringComparison.CurrentCultureIgnoreCase) &&
                   !string.IsNullOrEmpty(rendering.Properties["PersonlizationRules"]);
        }
    }
}