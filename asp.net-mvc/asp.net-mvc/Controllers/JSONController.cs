using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace asp.net_mvc.Controllers
{
    public class JSONController : ApiController
    {
        public IHttpActionResult GetJSON()
        {
            return Ok(new {message = "Hello, World!"});
        }
    }
}
