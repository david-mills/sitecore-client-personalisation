namespace Sc.Client.Personalisation.Controllers
{
    using System;
    using System.Web;
    using System.Web.Routing;
    using Sitecore;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Mvc.Common;
    using Sitecore.Mvc.Configuration;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Sites;
    using PageContext = Sitecore.Mvc.Presentation.PageContext;

    public class PersonalisationContext : IDisposable
    {
        private readonly IDisposable _context;
        private readonly TrackerSwitcher _tracker;
        private readonly IDisposable _placeholder;

        public PersonalisationContext() : this("dummy")
        {
        }

        public PersonalisationContext(string placeholder)
        {
            PageContext pageContext = this.CreatePageContext(new System.Web.HttpContextWrapper(HttpContext.Current));
            _context = ContextService.Get().Push(pageContext);
            var tracker = new NullTracker(new StandardSession())
            {
                IsActive = true
            };
            Context.Site.SetDisplayMode(DisplayMode.Normal, DisplayModeDuration.Temporary);
            _tracker = new TrackerSwitcher(tracker);
            _placeholder = ContextService.Get().Push(new PlaceholderContext(placeholder));
        }

        public void Dispose()
        {
            _context.Dispose();
            _tracker.Dispose();
            _placeholder.Dispose();
        }

        private PageContext CreatePageContext(System.Web.HttpContextWrapper context)
        {
            PageContext pageContext = new PageContext();
            RouteData routeData = CreateRouteData();
            pageContext.RequestContext = this.CreateRequestContext(context, routeData);
            return pageContext;
        }

        private RequestContext CreateRequestContext(System.Web.HttpContextWrapper httpContextWrapper,
            RouteData routeData)
        {
            return new RequestContext
            {
                HttpContext = httpContextWrapper,
                RouteData = routeData
            };
        }

        private static RouteData CreateRouteData()
        {
            RouteData routeData = new RouteData();
            routeData.Values["scLanguage"] = global::Sitecore.Context.Language.Name;
            routeData.Values["controller"] = MvcSettings.SitecoreControllerName;
            return routeData;
        }
    }
}