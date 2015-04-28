using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asp.net_mvc.App_Start
{
    public class MyHttpHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.StatusCode = 200;
        }
    }
}