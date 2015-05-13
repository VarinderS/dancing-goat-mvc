using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

using MvcDemo.Web.Infrastructure;

namespace MvcDemo.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var defaultCulture = CultureInfo.GetCultureInfo("en-US");

            var route = routes.MapRoute(
                                  name: "Default",
                                  url: "{culture}/{controller}/{action}/{id}",
                                  defaults: new { culture = defaultCulture.Name, controller = "Home", action = "Index", id = UrlParameter.Optional },
                                  constraints: new { culture = new SiteCultureConstraint("TestMvcDemo") }
                                );
            
            // Set the correct thread culture
            route.RouteHandler = new MultiCultureMvcRouteHandler(defaultCulture);

            // No usable route found, go to (404) Not found 
            routes.MapRoute(
                      name: "Error",
                      url: "{*url}",
                      defaults: new { controller = "Error", action = "NotFound" }
                   );
        }
    }
}
