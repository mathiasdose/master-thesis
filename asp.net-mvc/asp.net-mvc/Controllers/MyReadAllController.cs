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
            var entities = new List<world>();
            using (var db = new MyDbContainer())
            {
                entities = db.world.ToList();
            }
            return Ok();
        }
    }
}
