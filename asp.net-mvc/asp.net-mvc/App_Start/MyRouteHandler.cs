using System.Web;
using System.Web.Routing;

namespace asp.net_mvc.App_Start
{
    public class MyRouteHandler : IRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new MyHttpHandler();
        }
    }
}