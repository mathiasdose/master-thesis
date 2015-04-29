using System;
using System.Configuration;
using asp.net_mvc.App_Start;
using System.Web.Mvc;
using System.Web.Routing;

namespace asp.net_mvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            string[] allowedMethods = { "GET", "POST", "PUT", "DELETE" };
            var methodConstraints = new HttpMethodConstraint(allowedMethods);

            for (var i = 0; i < Math.Ceiling(StartUp.NumberOfRoutes/allowedMethods.Length); i++)
            {
                Route route;
                if (i == Math.Floor(StartUp.NumberOfRoutes/(allowedMethods.Length*2)))
                {
                    route = new Route("request-routing/hello/{id}/world", new MyRouteHandler())
                    {
                        Constraints = new RouteValueDictionary {{"httpMethod", methodConstraints}},
                        Defaults = new RouteValueDictionary {{"id", 1}}
                    };
                }
                else
                {
                    route = new Route("request-routing/" + Guid.NewGuid() + "/{id}/" + Guid.NewGuid(), 
                        new MyRouteHandler())
                    {
                        Constraints = new RouteValueDictionary { { "httpMethod", methodConstraints } },
                        Defaults = new RouteValueDictionary { { "id", 1 } }
                    };
                }
                
                routes.Add(route);
            }

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
