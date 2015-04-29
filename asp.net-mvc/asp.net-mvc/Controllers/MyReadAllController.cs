using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace asp.net_mvc.Controllers
{
    public class MyReadAllController : ApiController
    {
        public IHttpActionResult GetMyData()
        {
            using (var db = new MyDbContainer())
            {
                var entities = db.world;
            }
            return Ok();
        }
    }
}
