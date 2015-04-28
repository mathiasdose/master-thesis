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
            var worlds = new List<world>();
            using (var db = new MyDbContainer())
            {
                if (bool.Parse(ConfigurationManager.AppSettings["TestMode"]))
                    worlds = db.world.ToList();
                else
                {
                    var entities = db.world;
                }
            }

            if (bool.Parse(ConfigurationManager.AppSettings["TestMode"]))
                return Ok(worlds);

            return Ok();
        }
    }
}
