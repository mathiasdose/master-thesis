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

            for (var i = 0; i < int.Parse(ConfigurationManager.AppSettings["NumberOfRoutes"]); i++)
            {
                routes.Add(new Route(
                    "JSON" + i, new MyRouteHandler()));
            }

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
